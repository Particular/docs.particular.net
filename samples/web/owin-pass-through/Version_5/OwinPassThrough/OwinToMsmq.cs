using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Threading.Tasks;

namespace OwinPassThrough
{

    #region OwinToMsmq

    using System.IO;
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class OwinToMsmq
    {
        string queuePath;

        public OwinToMsmq(string queuePath)
        {
            this.queuePath = queuePath;
        }

        public Func<AppFunc, AppFunc> Middleware()
        {
            return x => (AppFunc) Invoke;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            using (Stream memoryStream = await OwinToMsmqStreamHelper.RequestAsStream(environment))
            using (MessageQueue queue = new MessageQueue(queuePath))
            using (Message message = new Message())
            {
                message.BodyStream = memoryStream;
                IDictionary<string, string[]> requestHeaders = (IDictionary<string, string[]>) environment["owin.RequestHeaders"];
                string messageType = requestHeaders["MessageType"].Single();
                message.Extension = MsmqHeaderSerializer.CreateHeaders(messageType);
                queue.Send(message, MessageQueueTransactionType.Single);
            }
        }
    }

    #endregion
}