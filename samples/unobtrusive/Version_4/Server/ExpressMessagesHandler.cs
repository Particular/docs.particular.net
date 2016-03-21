using Messages;
using NServiceBus;
using NServiceBus.Logging;

public class ExpressMessagesHandler : IHandleMessages<RequestExpress>
{
    static ILog log = LogManager.GetLogger(typeof(ExpressMessagesHandler));

    public void Handle(RequestExpress message)
    {
        log.InfoFormat("Message [{0}] received, id: [{1}]", message.GetType(), message.RequestId);
    }
}