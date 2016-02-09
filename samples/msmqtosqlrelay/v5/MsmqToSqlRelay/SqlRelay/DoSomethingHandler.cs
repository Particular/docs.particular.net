using System;
using NServiceBus;
using Shared;

#region sqlrelay-handler
class SomethingHappenedHandler : IHandleMessages<SomethingHappened>
{
    public IBus Bus { get; set; }
    public void Handle(SomethingHappened message)
    {
        Console.WriteLine("Sql Relay has now received this event from the MSMQ. Publishing this event for downstream SQLSubscribers ");

        // You can now relay this event to other interested SQL subscribers
        Bus.Publish(message);
    }
}
#endregion