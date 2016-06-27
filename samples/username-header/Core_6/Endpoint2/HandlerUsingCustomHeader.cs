using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region handler-using-custom-header
public class HandlerUsingCustomHeader : IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger("HandlerUsingCustomHeader");

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var usernameFromCustomHeader = context.MessageHeaders["UserName"];
        log.Info($"Username extracted from custom message header: {usernameFromCustomHeader}");
        return Task.FromResult(0);
    }

}
#endregion