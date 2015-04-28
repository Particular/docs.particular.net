using System;
using Shared;
using NServiceBus;

// Bootstrapper that facilitates testing by publishing an event every time Enter is pressed
public class PublishEvent : IWantToRunWhenBusStartsAndStops
{
    public IBus Bus { get; set; }

    public void Start()
    {
        #region publisher-loop
        Console.WriteLine("Press Enter to publish the SomethingHappened Event");
        while (Console.ReadLine() != null)
        {
            Console.WriteLine("Event published");
            Bus.Publish(new SomethingHappened());
        }
        #endregion
    }

    public void Stop()
    {
    }
}