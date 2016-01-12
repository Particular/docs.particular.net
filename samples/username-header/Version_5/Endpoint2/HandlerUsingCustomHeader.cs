using NServiceBus;
using NServiceBus.Logging;

#region handler-using-custom-header
public class HandlerUsingCustomHeader : IHandleMessages<MyMessage>
{
    static ILog logger = LogManager.GetLogger("HandlerUsingCustomHeader");
    IBus bus;

    public HandlerUsingCustomHeader(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(MyMessage message)
    {
        string usernameFromCustomHeader = bus.CurrentMessageContext.Headers["UserName"];
        logger.Info("Username extracted from custom message header: " + usernameFromCustomHeader);
    }
}
#endregion