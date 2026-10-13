namespace Core7.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;

    public class ExternalOperationTimeoutHandler :
        IHandleMessages<MyMessage>
    {
        RemoteFileServer remoteFileServer;

        public ExternalOperationTimeoutHandler(RemoteFileServer remoteFileServer)
        {
            this.remoteFileServer = remoteFileServer;
        }

        #region HandlerExternalOperationTimeout

        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            using (var timeoutTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
            {
                await remoteFileServer.Download(message.Data, timeoutTokenSource.Token);
            }
        }

        #endregion
    }

    public class RemoteFileServer
    {
        public Task Download(string path, CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
