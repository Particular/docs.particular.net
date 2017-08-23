using System;
using System.Threading.Tasks;
using NServiceBus.Gateway;
using WebSocketSharp.Server;

namespace WebSocketGateway
{
    #region WebSockectGateway-ChannelReceiver
    public class WebSocketChannelReceiver : IChannelReceiver
    {
        public void Start(string address, int maxConcurrency, Func<DataReceivedOnChannelArgs, Task> dataReceivedOnChannel)
        {
            var uri = new Uri(address);
            server = new WebSocketServer(uri.GetLeftPart(UriPartial.Authority));

            server.AddWebSocketService(uri.AbsolutePath, () => new WebSocketMessageBehavior(dataReceivedOnChannel));
            server.Start();
        }

        public Task Stop()
        {
            server.Stop();
            return Task.CompletedTask;
        }

        private WebSocketServer server;
    }
    #endregion
}