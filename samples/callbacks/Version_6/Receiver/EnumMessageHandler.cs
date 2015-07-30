using System;
using NServiceBus;

#region EnumMessageHandler
public class EnumMessageHandler : IHandleMessages<EnumMessage>
{
    IBus bus;

    public EnumMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(EnumMessage message)
    {
        Console.WriteLine("Message received, Returning");
        bus.Reply(Status.OK);
    }
}


#endregion