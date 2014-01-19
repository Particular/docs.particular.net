---
title: How to Handle a Message?
summary: Write a class to handle messages in NServiceBus.
originalUrl: http://www.particular.net/articles/how-do-i-handle-a-message
tags: []
createdDate: 2013-05-22T05:12:13Z
modifiedDate: 2013-12-10T07:04:22Z
authors: []
reviewers: []
contributors: []
---

To handle a message, write a class that implements IMessageHandler<t> where T is the message type:


```C#
public class H1 : IMessageHandler<MyMessage>
{
     public void Handle(MyMessage message)
     {
        //do something relevant with the message
     }
}
```


<span style="font-size: 14px;">To handle messages of all types:</span>

1.  Set up the unobtrusive message configuration to designate which
    classes are messages. This example uses a namespace match.
2.  Create a handler of type Object. This handler will be executed for
    all messages that are delivered to the queue for this endpoint.


```C#
//Since this class is setup to handle type Object, every message arriving in the queue will trigger it.
public class GenericMessageHandler : IHandleMessages<Object>
    {
        public static readonly ILog Logger = LogManager.GetLogger(typeof(GenericMessageHandler));
        
        public void Handle(Object message)
        { 
        Logger.Info(string.Format("I just received a message of type {0}.", message.GetType().Name));
        Console.WriteLine("*********************************************************************************");
        }
    }
```

 If you are using the Request-Response or Full Duplex pattern, your handler will probably do the work it needs to do, such as updating a database or calling a web service, then creating and sending a response message. See [How to Reply to a Message](how-do-i-reply-to-a-message.md).

If you are handling a message in a publish and subscribe scenario, see
[How to Publish/Subscribe to a Message](how-to-pub/sub-with-NServiceBus.md).

