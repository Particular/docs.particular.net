using System;
using NServiceBus;

#region ObjectMessageHandler
public class ObjectMessageHandler : IHandleMessages<ObjectMessage>
{
    IBus bus;

    public ObjectMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(ObjectMessage message)
    {
        Console.WriteLine("Message received, Returning");
        bus.Reply(new ObjectResponseMessage
        {
            Property = "PropertyValue"
        });
    }
}


#endregion