using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Threading.Tasks;
using System.Transactions;

namespace OwinPassThrough
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    #region OwinToMsmq

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
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var queue = new MessageQueue(queuePath))
                using (var message = new Message())
                using (var memoryStream = await RequestAsMemoryStream(environment))
                {
                    message.BodyStream = memoryStream;
                    var requestHeaders = (IDictionary<string, string[]>) environment["owin.RequestHeaders"];
                    var messageType = requestHeaders["MessageType"].Single();
                    message.Extension = MsmqHeaderSerializer.CreateHeaders(messageType);
                    queue.Send(message, MessageQueueTransactionType.Automatic);
                }
                scope.Complete();
            }
        }

        async Task<MemoryStream> RequestAsMemoryStream(IDictionary<string, object> environment)
        {
            var memoryStream = new MemoryStream();
            using (var requestStream = (Stream) environment["owin.RequestBody"])
            {
                await requestStream.CopyToAsync(memoryStream);
            }
            return memoryStream;
        }

    }

    #endregion
}