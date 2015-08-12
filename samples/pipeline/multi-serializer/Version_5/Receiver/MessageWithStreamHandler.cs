using System;
using NServiceBus;

public class MessageHandler :
    IHandleMessages<MessageWithJson>,
    IHandleMessages<MessageWithBinary>
{
    public void Handle(MessageWithJson message)
    {
        Console.WriteLine("Received JSON message with property '{0}'", message.SomeProperty);
    }
    public void Handle(MessageWithBinary message)
    {
        Console.WriteLine("Received Binary message with property '{0}'", message.SomeProperty);
    }
}