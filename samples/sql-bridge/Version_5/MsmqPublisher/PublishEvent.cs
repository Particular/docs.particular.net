using System;
using Shared;
using NServiceBus;

// Bootstrapper that facilitates testing by publishing an event every time Enter is pressed
public class PublishEvent : IWantToRunWhenBusStartsAndStops
{
    IBus bus;

    public PublishEvent(IBus bus)
    {
        this.bus = bus;
    }

    public void Start()
    {
        #region publisher-loop
        Console.WriteLine("Press Enter to publish the SomethingHappened Event");
        while (Console.ReadLine() != null)
        {
            Console.WriteLine("Event published");
            bus.Publish(new SomethingHappened());
        }
        #endregion
    }

    public void Stop()
    {
    }
}