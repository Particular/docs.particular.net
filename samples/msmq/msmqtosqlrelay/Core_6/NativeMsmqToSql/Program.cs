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
using Newtonsoft.Json;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.MsmqToSqlRelay.NativeMsmqToSql";

        #region receive-from-msmq-using-native-messaging

        // The address of the queue that will be receiving messages from other MSMQ publishers
        var queuePath = @".\private$\MsmqToSqlRelay";

        // Create message queue object
        var addressOfMsmqBridge = new MessageQueue(queuePath, QueueAccessMode.SendAndReceive);
        // for the sample's sake. Might need to fine tune the exact filters required.
        addressOfMsmqBridge.MessageReadPropertyFilter.SetAll();

        // Add an event handler for the ReceiveCompleted event.
        addressOfMsmqBridge.ReceiveCompleted += MsmqBridgeOnReceiveCompleted;

        // Begin the asynchronous receive operation.
        addressOfMsmqBridge.BeginReceive();

        #endregion

        Console.WriteLine($"Watching MSMQ: {queuePath} for messages. Received messages will be sent to the SqlRelay");
        Console.WriteLine("Press any key to quit.");
        var key = Console.ReadKey();
    }

    static void MsmqBridgeOnReceiveCompleted(object sender, ReceiveCompletedEventArgs receiveCompletedEventArgs)
    {
        var sqlConnectionStr = @"Data Source =.\SqlExpress; Initial Catalog = PersistenceForSqlTransport; Integrated Security = True";
        var sqlRelayEndpointName = "SqlRelay";

        using (var scope = new TransactionScope(TransactionScopeOption.Required))
        {
            // Connect to the queue.
            var messageQueue = (MessageQueue) sender;

            // End the asynchronous receive operation.
            var message = messageQueue.EndReceive(receiveCompletedEventArgs.AsyncResult);

            // Validate the message
            if (message != null)
            {
                Console.WriteLine("Received a message in MSMQ - Processing");

                #region read-message-and-push-to-sql

                var messageBody = ConvertStreamToByteArray(message.BodyStream);

                // Serialize to dictionary
                var headerString = Encoding.UTF8.GetString(message.Extension)
                    .TrimEnd('\0');
                var headers = ExtractHeaders(headerString);

                // If this queue is going to be receiving messages from endpoints
                // older than v4.0, then set the header["NServiceBus.MessageId"]
                // to be a deterministic guid based on the Id as Sql transport
                // expects a Guid for the MessageId and not an Id in the MSMQ format.

                // Have the necessary raw information from queue
                // Therefore write it to Sql.
                Console.WriteLine("Forwarding message to SQLRelay endpoint");
                SendMessageToSql(sqlConnectionStr, sqlRelayEndpointName, messageBody, headers);

                #endregion

            }
            // Restart the asynchronous receive operation.
            messageQueue.BeginReceive();

            // Commit transaction
            scope.Complete();
        }
    }

    static Dictionary<string, string> ExtractHeaders(string headerString)
    {
        // Deserialize the XML stream from the Extension property on the MSMQ
        // to a dictionary of key-value pairs
        var headerSerializer = new XmlSerializer(typeof(List<HeaderInfo>));
        var readerSettings = new XmlReaderSettings
        {
            CheckCharacters = false
        };
        using (var stream = new StringReader(headerString))
        using (var reader = XmlReader.Create(stream, readerSettings))
        {
            var headers = (List<HeaderInfo>) headerSerializer.Deserialize(reader);
            return headers
                .Where(pair => pair.Key != null)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }

    static void SendMessageToSql(string connectionString, string queue, byte[] messageBody, Dictionary<string, string> headers)
    {
        var insertSql =$@"
        insert into [{queue}] (
            Id,
            Recoverable,
            Headers,
            Body)
        values (
            @Id,
            @Recoverable,
            @Headers,
            @Body)";

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(insertSql, connection))
            {
                var parameters = command.Parameters;
                parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                var serializeHeaders = JsonConvert.SerializeObject(headers);
                parameters.Add("Headers", SqlDbType.VarChar).Value = serializeHeaders;
                parameters.Add("Body", SqlDbType.VarBinary).Value = messageBody;
                parameters.Add("Recoverable", SqlDbType.Bit).Value = true;
                command.ExecuteNonQuery();
            }
        }

    }

    static byte[] ConvertStreamToByteArray(Stream input)
    {
        using (var memoryStream = new MemoryStream())
        {
            input.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}