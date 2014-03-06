---
title: How to Handle a Message?
summary: Write a class to handle messages in NServiceBus.
tags: []
---

To handle a message, write a class that implements `IMessageHandler<T>` where `T` is the message type:

```C#
public class H1 : IMessageHandler<MyMessage>
{
     public void Handle(MyMessage message)
     {
        //do something relevant with the message
     }
}
```

To handle messages of all types:

1.  Set up the unobtrusive message configuration to designate which classes are messages. This example uses a namespace match.
2.  Create a handler of type `Object`. This handler will be executed for all messages that are delivered to the queue for this endpoint.

Since this class is setup to handle type `Object`, every message arriving in the queue will trigger it.

```C#
public class GenericMessageHandler : IHandleMessages<Object>
{
    static ILog Logger = LogManager.GetLogger(typeof(GenericMessageHandler));
    
    public void Handle(Object message)
    { 
        Logger.Info(string.Format("I just received a message of type {0}.", message.GetType().Name));
        Console.WriteLine("*********************************************************************************");
    }
}
```

If you are using the Request-Response or Full Duplex pattern, your handler will probably do the work it needs to do, such as updating a database or calling a web service, then creating and sending a response message. See [How to Reply to a Message](how-do-i-reply-to-a-message.md).

If you are handling a message in a publish and subscribe scenario, see [How to Publish/Subscribe to a Message](how-to-pub-sub-with-NServiceBus).
