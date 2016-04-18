using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region handler-using-custom-header
public class HandlerUsingCustomHeader : IHandleMessages<MyMessage>
{
    static ILog logger = LogManager.GetLogger("HandlerUsingCustomHeader");

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        string usernameFromCustomHeader = context.MessageHeaders["UserName"];
        logger.Info($"Username extracted from custom message header: {usernameFromCustomHeader}");
        return Task.FromResult(0);
    }

}
#endregion