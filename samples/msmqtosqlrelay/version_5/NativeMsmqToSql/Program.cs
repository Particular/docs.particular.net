using System.Text;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Transactions;
using System.Xml;

class Program
{
    static void Main()
    {
        ManualResetEvent signal = new ManualResetEvent(false);

        #region receive-from-msmq-using-native-messaging
        // The address of the queue that will be receiving messages from other MSMQ publishers
        var queuePath = @".\private$\MsmqToSqlRelay";

        //Create message queue object
        MessageQueue addressOfMsmqBridge = new MessageQueue(queuePath, QueueAccessMode.SendAndReceive);
        addressOfMsmqBridge.MessageReadPropertyFilter.SetAll(); // for the sample's sake. Might need to fine tune the exact filters we need. 

        // Add an event handler for the ReceiveCompleted event.
        addressOfMsmqBridge.ReceiveCompleted += MsmqBridgeOnReceiveCompleted;

        // Begin the asynchronous receive operation.
        addressOfMsmqBridge.BeginReceive();
        #endregion

        Console.WriteLine("Watching MSMQ: {0} for messages. Received messages will be sent to the SqlRelay", queuePath);
        signal.WaitOne();
    }


    private static void MsmqBridgeOnReceiveCompleted(object sender, ReceiveCompletedEventArgs receiveCompletedEventArgs)
    {
        var sqlConnectionStr = @"Data Source =.\SQLEXPRESS; Initial Catalog = PersistenceForSqlTransport; Integrated Security = True";

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
                var messageBody = ConvertStreamToByteArray(msg.BodyStream);

                // Serialize to dictionary
                var headers = ExtractHeaders(Encoding.UTF8.GetString(msg.Extension).TrimEnd('\0'));

                // If this queue is going to be receiving messages from endpoints older than v4.0, then
                // set the header["NServiceBus.MessageId"] to be a deterministic guid based on the Id
                // as Sql transport expects a Guid for the MessageId and not an Id in the MSMQ format.

                // Have the necessary raw information from queue - Therefore write it to Sql.
                Console.WriteLine("Forwarding message to SQLRelay endpoint");
                SendMessageToSql(sqlConnectionStr, "SqlRelay", messageBody, headers);
                #endregion

            }
            // Restart the asynchronous receive operation.
            mq.BeginReceive();

            //Commit transaction
            scope.Complete();

        }
    }

    private static Dictionary<string, string> ExtractHeaders(string headerString)
    {
        // Deserialize the XML stream from the Extension property on the MSMQ to a dictionary of key-value pairs
        var headerSerializer = new System.Xml.Serialization.XmlSerializer(typeof(List<HeaderInfo>));
        object headers;
        using (var stream = new StringReader(headerString))
        {
            using (var reader = XmlReader.Create(stream, new XmlReaderSettings
            {
                CheckCharacters = false
            }))
            {
                headers = headerSerializer.Deserialize(reader);
            }
        }

        return ((List<HeaderInfo>)headers).Where(pair => pair.Key != null).ToDictionary(pair => pair.Key, pair => pair.Value);

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

    static byte[] ConvertStreamToByteArray(System.IO.Stream input)
    {
        byte[] buffer = new byte[16 * 1024];

        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
        {
            int chunk;

            while ((chunk = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, chunk);
            }

            return ms.ToArray();
        }
    }
}

[Serializable]
public class HeaderInfo
{
    /// <summary>
    /// The key used to lookup the value in the header collection.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// The value stored under the key in the header collection.
    /// </summary>
    public string Value { get; set; }
}

