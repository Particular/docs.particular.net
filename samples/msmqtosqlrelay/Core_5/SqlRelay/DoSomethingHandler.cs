using NServiceBus;
using NServiceBus.Logging;
using Shared;

#region sqlrelay-handler
class SomethingHappenedHandler :
    IHandleMessages<SomethingHappened>
{
    static ILog log = LogManager.GetLogger<SomethingHappenedHandler>();
    IBus bus;

    public SomethingHappenedHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(SomethingHappened message)
    {
        log.Info("Sql Relay has now received this event from the MSMQ. Publishing this event for downstream SQLSubscribers");
        // relay this event to other interested SQL subscribers
        bus.Publish(message);
    }
}
#endregion