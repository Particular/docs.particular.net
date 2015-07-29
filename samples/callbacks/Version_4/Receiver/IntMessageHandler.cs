using System;
using NServiceBus;

#region IntMessageHandler
public class IntMessageHandler : IHandleMessages<IntMessage>
{
    IBus bus;

    public IntMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(IntMessage message)
    {
        Console.WriteLine("Message received, Returning");
        bus.Return(10);
    }
}


#endregion