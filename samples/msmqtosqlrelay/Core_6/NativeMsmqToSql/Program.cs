using System.Text;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.MsmqToSqlRelay.NativeMsmqToSql";
        #region receive-from-msmq-using-native-messaging
        // The address of the queue that will be receiving messages from other MSMQ publishers
        string queuePath = @".\private$\MsmqToSqlRelay";

        //Create message queue object
        MessageQueue addressOfMsmqBridge = new MessageQueue(queuePath, QueueAccessMode.SendAndReceive);
        addressOfMsmqBridge.MessageReadPropertyFilter.SetAll(); // for the sample's sake. Might need to fine tune the exact filters we need.

        // Add an event handler for the ReceiveCompleted event.
        addressOfMsmqBridge.ReceiveCompleted += MsmqBridgeOnReceiveCompleted;

        // Begin the asynchronous receive operation.
        addressOfMsmqBridge.BeginReceive();
        #endregion

        Console.WriteLine("Watching MSMQ: {0} for messages. Received messages will be sent to the SqlRelay", queuePath);
        Console.WriteLine("Press any key to quit.");
        ConsoleKeyInfo key = Console.ReadKey();
    }

    static void MsmqBridgeOnReceiveCompleted(object sender, ReceiveCompletedEventArgs receiveCompletedEventArgs)
    {
        string sqlConnectionStr = @"Data Source =.\SQLEXPRESS; Initial Catalog = PersistenceForSqlTransport; Integrated Security = True";
        string sqlRelayEndpointName = "SqlRelay";

        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
        {

            // Connect to the queue.
            MessageQueue mq = (MessageQueue)sender;

            // End the asynchronous receive operation.
            Message msg = mq.EndReceive(receiveCompletedEventArgs.AsyncResult);

            //Validate the message
            if (msg != null)
            {
                Console.WriteLine("Received a message in MSMQ -- Processing ...");
                #region read-message-and-push-to-sql
                byte[] messageBody = ConvertStreamToByteArray(msg.BodyStream);

                // Serialize to dictionary
                Dictionary<string, string> headers = ExtractHeaders(Encoding.UTF8.GetString(msg.Extension).TrimEnd('\0'));

                // If this queue is going to be receiving messages from endpoints older than v4.0, then
                // set the header["NServiceBus.MessageId"] to be a deterministic guid based on the Id
                // as Sql transport expects a Guid for the MessageId and not an Id in the MSMQ format.

                // Have the necessary raw information from queue - Therefore write it to Sql.
                Console.WriteLine("Forwarding message to SQLRelay endpoint");
                SendMessageToSql(sqlConnectionStr, sqlRelayEndpointName, messageBody, headers);
                #endregion

            }
            // Restart the asynchronous receive operation.
            mq.BeginReceive();

            //Commit transaction
            scope.Complete();

        }
    }

    static Dictionary<string, string> ExtractHeaders(string headerString)
    {
        // Deserialize the XML stream from the Extension property on the MSMQ to a dictionary of key-value pairs
        XmlSerializer headerSerializer = new XmlSerializer(typeof(List<HeaderInfo>));
        object headers;
        using (StringReader stream = new StringReader(headerString))
        using (XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings
        {
            CheckCharacters = false
        }))
        {
            headers = headerSerializer.Deserialize(reader);
        }
        return ((List<HeaderInfo>) headers).Where(pair => pair.Key != null).ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    static void SendMessageToSql(string connectionString, string queue, byte[] messageBody, Dictionary<string, string> headers)
    {
        string insertSql =
            $@"INSERT INTO [{queue
                }] (
        [Id],
        [Recoverable],
        [Headers],
        [Body])
    VALUES (
        @Id,
        @Recoverable,
        @Headers,
        @Body)";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(insertSql, connection))
            {
                SqlParameterCollection parameters = command.Parameters;
                command.CommandType = CommandType.Text;
                parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                string serializeHeaders = Newtonsoft.Json.JsonConvert.SerializeObject(headers);
                parameters.Add("Headers", SqlDbType.VarChar).Value = serializeHeaders;
                parameters.Add("Body", SqlDbType.VarBinary).Value = messageBody;
                parameters.Add("Recoverable", SqlDbType.Bit).Value = true;
                command.ExecuteNonQuery();
            }
        }

    }

    static byte[] ConvertStreamToByteArray(Stream input)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            input.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}

