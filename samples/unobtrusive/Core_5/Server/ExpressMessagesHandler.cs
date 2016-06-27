using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class ExpressMessagesHandler : IHandleMessages<RequestExpress>
{
    static ILog log = LogManager.GetLogger<ExpressMessagesHandler>();

    public void Handle(RequestExpress message)
    {
        log.Info($"Message [{message.GetType()}] received, id: [{message.RequestId}]");
    }
}