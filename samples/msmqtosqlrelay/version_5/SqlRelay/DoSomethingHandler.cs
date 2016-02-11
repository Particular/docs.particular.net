using System;
using NServiceBus;
using Shared;

#region sqlrelay-handler
class SomethingHappenedHandler : IHandleMessages<SomethingHappened>
{
    IBus bus;

    public SomethingHappenedHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(SomethingHappened message)
    {
        Console.WriteLine("Sql Relay has now received this event from the MSMQ. Publishing this event for downstream SQLSubscribers ");

        // relay this event to other interested SQL subscribers
        bus.Publish(message);
    }
}
#endregion