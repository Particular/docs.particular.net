using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
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
        using (var webSocketData = new MemoryStream(e.RawData))
        {
            var envelope = JsonSerializer.Deserialize<ChannelEnvelope>(webSocketData);

            //using (var channelData = new MemoryStream(envelope.Data))
            //{
            var args = new DataReceivedOnChannelEventArgs
            {
                //Data = channelData,
                Headers = envelope.Headers
            };
            dataReceivedOnChannel(args, CancellationToken.None);
            // }
        }
    }
}

class ChannelEnvelope
{
    public IDictionary<string, string> Headers { get; set; }

    //public byte[] Data { get; set; }
}