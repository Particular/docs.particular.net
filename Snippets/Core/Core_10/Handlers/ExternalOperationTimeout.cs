namespace Core.Handlers;

using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

#region HandlerExternalOperationTimeout

public class ExternalOperationTimeoutHandler(RemoteFileServer remoteFileServer) :
    IHandleMessages<MyMessage>
{
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        using var timeoutTokenSource = CancellationTokenSource.CreateLinkedTokenSource(context.CancellationToken);
        timeoutTokenSource.CancelAfter(TimeSpan.FromSeconds(30));

        await remoteFileServer.Download(message.Data, timeoutTokenSource.Token);
    }
}

#endregion

public class RemoteFileServer
{
    public Task Download(string path, CancellationToken cancellationToken) => Task.CompletedTask;
}
