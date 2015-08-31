using System;
using Messages;
using NServiceBus;

public class DeferredMessageHandler : IHandleMessages<DeferredMessage>
{
    public void Handle(DeferredMessage message)
    {
        Console.WriteLine("{0} - {1}", DateTime.Now.ToLongTimeString(), "Deferred message processed");
    }
}