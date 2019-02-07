using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region message-handlers

class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyMessageHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var authContext = context.Extensions.Get<MyAuthContext>();

        log.Info($"Handler got auth context {authContext}.");

        return Task.CompletedTask;
    }
}

#endregion