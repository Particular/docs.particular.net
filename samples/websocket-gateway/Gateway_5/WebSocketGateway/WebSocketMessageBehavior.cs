using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Gateway;
using WebSocketSharp;
using WebSocketSharp.Server;

class WebSocketMessageBehavior :
    WebSocketBehavior
{
    Func<DataReceivedOnChannelEventArgs, CancellationToken, Task> dataReceivedOnChannel;

    public WebSocketMessageBehavior(Func<DataReceivedOnChannelEventArgs, CancellationToken, Task> dataReceivedOnChannel)
    {
        this.dataReceivedOnChannel = dataReceivedOnChannel;
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        using (var ms = new MemoryStream(e.RawData))
        {
            var formatter = new BinaryFormatter();
            var headers = (IDictionary<string, string>) formatter.Deserialize(ms);

            var args = new DataReceivedOnChannelEventArgs
            {
                Data = ms,
                Headers = headers
            };
            dataReceivedOnChannel(args, CancellationToken.None);
        }
    }
}