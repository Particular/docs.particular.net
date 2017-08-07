namespace SqsAll.ErrorQueue
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Messaging;
    using System.Text;
    using System.Transactions;
    using System.Xml.Serialization;
    using SqsAll.QueueCreation;
    using Sqs_All;

    public static class ErrorQueue
    {

        static void Usage()
        {
            #region sqs-return-to-source-queue-usage

            ReturnMessageToSourceQueue(
                errorQueueName: "error",
                msmqMessageId: "c390a6fb-4fb5-46da-927d-a156f75739eb");

            #endregion
        }

        #region sqs-return-to-source-queue

        public static void ReturnMessageToSourceQueue(string errorQueueName, string msmqMessageId)
        {
            var path = QueueNameHelper.GetSqsQueueName($"{errorQueueName}");
            using (var client = ClientFactory.CreateSqsClient())
            {
                // TODO
            }
            var errorQueue = new MessageQueue(path);
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
                    var transactionType = MessageQueueTransactionType.Automatic;
                    var message = errorQueue.ReceiveById(msmqMessageId, TimeSpan.FromSeconds(5), transactionType);
                    var fullPath = ReadFailedQueueHeader(message);
                    using (var failedQueue = new MessageQueue(fullPath))
                    {
                        failedQueue.Send(message, transactionType);
                    }
                    scope.Complete();
                }
            }
        }

        static string ReadFailedQueueHeader(Message message)
        {
            var headers = ExtractHeaders(message);
            var header = headers.Single(x => x.Key == "NServiceBus.FailedQ").Value;
            var queueName = header.Split('@')[0];
            var machineName = header.Split('@')[1];
            return $@"{machineName}\private$\{queueName}";
        }

        public static List<HeaderInfo> ExtractHeaders(Message message)
        {
            var serializer = new XmlSerializer(typeof(List<HeaderInfo>));
            var extension = Encoding.UTF8.GetString(message.Extension);
            using (var stringReader = new StringReader(extension))
            {
                return (List<HeaderInfo>) serializer.Deserialize(stringReader);
            }
        }

        public class HeaderInfo
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        #endregion
    }

}