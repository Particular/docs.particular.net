namespace CoreAll.Msmq.NativeSend
{
    using System.Collections.Generic;
    using System.IO;
    using System.Messaging;
    using System.Text;
    using System.Transactions;
    using System.Xml.Serialization;

    public static class NativeSend
    {

        static void Usage()
        {
            #region msmq-nativesend-usage

            SendMessage(
                queuePath: @"MachineName\private$\QueueName",
                messageBody: "{\"Property\":\"PropertyValue\"}",
                headers: new List<HeaderInfo>
                {
                    new HeaderInfo
                    {
                        Key = "NServiceBus.EnclosedMessageTypes",
                        Value = "MyNamespace.MyMessage"
                    }
                });

            #endregion
        }

        #region msmq-nativesend

        public static void SendMessage(string queuePath, string messageBody, List<HeaderInfo> headers)
        {
            using (var scope = new TransactionScope())
            {
                using (var queue = new MessageQueue(queuePath))
                using (var message = new Message())
                {
                    var bytes = Encoding.UTF8.GetBytes(messageBody);
                    message.BodyStream = new MemoryStream(bytes);
                    message.Extension = CreateHeaders(headers);
                    queue.Send(message, MessageQueueTransactionType.Automatic);
                }
                scope.Complete();
            }
        }

        public static byte[] CreateHeaders(List<HeaderInfo> headerInfos)
        {
            var serializer = new XmlSerializer(typeof(List<HeaderInfo>));
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, headerInfos);
                return stream.ToArray();
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