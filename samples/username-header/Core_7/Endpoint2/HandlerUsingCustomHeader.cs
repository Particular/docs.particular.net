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
        var headers = context.MessageHeaders;
        var usernameFromHeader = headers["UserName"];
        log.Info($"Username extracted from header: {usernameFromHeader}");
        return Task.CompletedTask;
    }
}

#endregion