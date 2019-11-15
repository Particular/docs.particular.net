using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region handler-using-custom-header

public class HandlerUsingCustomHeader :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger("HandlerUsingCustomHeader");

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var usernameFromThread = Thread.CurrentPrincipal.Identity.Name;
        log.Info($"Username extracted from Thread.CurrentPrincipal: {usernameFromThread}");
        return Task.CompletedTask;
    }
}

#endregion