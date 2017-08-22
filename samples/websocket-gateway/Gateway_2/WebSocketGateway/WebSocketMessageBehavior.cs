using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using NServiceBus.Gateway;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebSocketGateway
{
    class WebSocketMessageBehavior : WebSocketBehavior
    {
        private readonly Func<DataReceivedOnChannelArgs, Task> dataReceivedOnChannel;

        public WebSocketMessageBehavior(Func<DataReceivedOnChannelArgs, Task> dataReceivedOnChannel)
        {
            this.dataReceivedOnChannel = dataReceivedOnChannel;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            using (var ms = new MemoryStream(e.RawData))
            {
                var formatter = new BinaryFormatter();
                var headers = (IDictionary<string, string>)formatter.Deserialize(ms);

                dataReceivedOnChannel(new DataReceivedOnChannelArgs
                {
                    Data = ms,
                    Headers = headers
                });
            }
        }
    }
}