using Shared;
using NServiceBus;
using NServiceBus.Logging;

#region msmqsubscriber-handler
public class SomethingHappenedHandler : IHandleMessages<SomethingHappened>
{
    static ILog log = LogManager.GetLogger<SomethingHappenedHandler>();

    public void Handle(SomethingHappened message)
    {
        log.Info("MSMQ Subscriber has now received the event: SomethingHappened");
    }
}
#endregion