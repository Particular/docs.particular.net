using System;
using NServiceBus;



public class Handler :
    IHandleMessages<EnumMessage>,
    IHandleMessages<IntMessage>,
    IHandleMessages<ObjectMessage>
{
    IBus bus;

    public Handler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(EnumMessage message)
    {
        string format = string.Format("Received EnumMessage. Property:'{0}'", message.Property);
        Console.WriteLine(format);
        bus.Reply(Status.Ok);
    }
    public void Handle(IntMessage message)
    {
        string format = string.Format("Received IntMessage. Property:'{0}'", message.Property);
        Console.WriteLine(format);
        bus.Reply(10);
    }

    public void Handle(ObjectMessage message)
    {
        string format = string.Format("Received ObjectMessage. Property:'{0}'", message.Property);
        Console.WriteLine(format);

        bus.Reply(new ReplyMessage
        {
            Property = string.Format("Handler Received '{0}'", message.Property)
        });
    }
}