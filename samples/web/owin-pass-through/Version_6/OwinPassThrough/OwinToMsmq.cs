using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Threading.Tasks;
using System.IO;

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
        return _ => Invoke;
    }

    async Task Invoke(IDictionary<string, object> environment)
    {
        using (Stream memoryStream = await RequestAsStream(environment))
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

    static async Task<Stream> RequestAsStream(IDictionary<string, object> environment)
    {
        MemoryStream memoryStream = new MemoryStream();
        using (Stream requestStream = (Stream)environment["owin.RequestBody"])
        {
            await requestStream.CopyToAsync(memoryStream).ConfigureAwait(false);
        }
        return memoryStream;
    }
}

#endregion