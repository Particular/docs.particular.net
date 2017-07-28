using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Threading.Tasks;
using System.IO;
using System.Net;

#region OwinToMsmq

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

public class OwinToMsmq
{
    string queuePath;

    public OwinToMsmq(string queuePath)
    {
        this.queuePath = queuePath;
    }

    public Func<AppFunc, AppFunc> Middleware()
    {
        return x => Invoke;
    }

    async Task Invoke(IDictionary<string, object> environment)
    {
        using (var memoryStream = await RequestAsStream(environment)
            .ConfigureAwait(false))
        using (var queue = new MessageQueue(queuePath))
        using (var message = new Message())
        {
            message.BodyStream = memoryStream;
            var requestHeaders = (IDictionary<string, string[]>) environment["owin.RequestHeaders"];
            var messageType = requestHeaders["MessageType"].Single();
            message.Extension = MsmqHeaderSerializer.CreateHeaders(messageType);
            queue.Send(message, MessageQueueTransactionType.Single);
        }
        environment["owin.ResponseStatusCode"] = (int)HttpStatusCode.Accepted;
    }

    async Task<Stream> RequestAsStream(IDictionary<string, object> environment)
    {
        var memoryStream = new MemoryStream();
        using (var requestStream = (Stream)environment["owin.RequestBody"])
        {
            await requestStream.CopyToAsync(memoryStream)
                .ConfigureAwait(false);
        }
        return memoryStream;
    }
}

#endregion