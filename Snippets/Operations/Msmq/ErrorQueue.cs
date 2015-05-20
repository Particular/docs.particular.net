namespace Operations.Msmq
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Messaging;
    using System.Text;
    using System.Transactions;
    using System.Xml.Serialization;

    //using System.Transactions.dll
    public static class ErrorQueue
    {

        public static void ReturnMessageToSourceQueue(string machineName, string queueName, string msmqMessageId)
        {
            using (var errorQueue = new MessageQueue(machineName + "\\private$\\" + queueName))
            {
                var messageReadPropertyFilter = new MessagePropertyFilter
                {
                    Body = true,
                    TimeToBeReceived = true,
                    Recoverable = true,
                    Id = true,
                    ResponseQueue = true,
                    CorrelationId = true,
                    Extension = true,
                    AppSpecific = true,
                    LookupId = true,
                };
                errorQueue.MessageReadPropertyFilter = messageReadPropertyFilter;
                using (var scope = new TransactionScope())
                {
                    var message = errorQueue.ReceiveById(msmqMessageId, TimeSpan.FromSeconds(5), MessageQueueTransactionType.Automatic);
                    string fullPath = ReadFailedQueueHeader(message);
                    using (var failedQueue = new MessageQueue(fullPath))
                    {
                        failedQueue.Send(message, MessageQueueTransactionType.Automatic);
                    }
                    scope.Complete();
                }
            }
        }

        static string ReadFailedQueueHeader(Message message)
        {
            var headers = ExtractHeaders(message);
            string header = headers.Single(x => x.Key == "NServiceBus.FailedQ").Value;
            string queueName = header.Split('@')[0];
            string machineName = header.Split('@')[1];
            return machineName + "\\private$\\" + queueName;
        }

        public static List<HeaderInfo> ExtractHeaders(Message message)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<HeaderInfo>));
            var extension = Encoding.UTF8.GetString(message.Extension);
            using (var stringReader = new StringReader(extension))
            {
                return (List<HeaderInfo>)serializer.Deserialize(stringReader);
            }
        }
    }

    public class HeaderInfo
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}