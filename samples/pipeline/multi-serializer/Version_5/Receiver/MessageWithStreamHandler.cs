using System;
using NServiceBus;

public class MessageHandler :
    IHandleMessages<MessageWithJson>,
    IHandleMessages<MessageWithXml>
{
    public void Handle(MessageWithJson message)
    {
        Console.WriteLine("Received JSON message with property '{0}'", message.SomeProperty);
    }
    public void Handle(MessageWithXml message)
    {
        Console.WriteLine("Received Xml message with property '{0}'", message.SomeProperty);
    }
}