---
title: NServiceBus and WebSphere/Sonic
summary: Swap out the pluggable MSMQ transport to get NServiceBus API on top of WebSphere and Sonic.
originalUrl: http://www.particular.net/articles/nservicebus-and-websphere-sonic
tags: []
createdDate: 2013-05-21T18:35:21Z
modifiedDate: 2013-07-29T14:02:20Z
authors: []
reviewers: []
contributors: []
---

WebSphere and Sonic are enterprise-grade middleware, robust, performant. No argument there.

But to the average .NET developer, exposed .NET API is horribly complex, looks more like Java, and does not take advantage of the strong typing provided by generics or lambdas. The developer-friendly NServiceBus API can be used on top of WebSphere and Sonic by swapping out the pluggable MSMQ transport implementation, giving you the best of both worlds. Here is an [NServiceBus adapter for WebSphere](http://code.google.com/p/nservicebuswmq/) .

Unit testing
------------

One of the benefits of working with the NServiceBus API is greatly simplified unit testing. By abstracting the transport technology and introducing strongly-typed message classes, NServiceBus lets you fully test message handling logic and long-running processes, knowing that integration with the messaging layer is handled for you.

Here's an example of the kind of service layer tests that NServiceBus enables:


```txt
public class TestHandler
{
    [Test]
    public void Run()
    {
        Test.Initialize();

        Test.Handler<MyHandler>()
            .ExpectReply<ResponseMessage>(m => m.String == "hello")
            .OnMessage<RequestMessage>(m => m.String = "hello");
    }

    public class MyHandler :
        IHandleMessages<RequestMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(RequestMessage message)
        {
            var reply = new ResponseMessage
                        {
                            String = message.String
                        };
            Bus.Reply(reply);
        }
    }

    public class ResponseMessage : IMessage
    {
        public string String { get; set; }
    }

    public class RequestMessage : IMessage
    {
        public string String { get; set; }
    }

}

```

 Another benefit when working with NServiceBus is its generic host. Similar to application servers on the Java platform, the NServiceBus Host provides standardized installation and manageability as well as a lightweight sandbox where all infrastructure components automatically switch to in-memory mode for greatest productivity when you want to focus on business logic.

Download
--------

Why not [download](http://particular.net/downloads) NServiceBus and give it a quick evaluation?

Get the best of both worlds: enterprise-grade middleware AND highly productive developer APIs.

Next steps
----------

Learn more about these features of NServiceBus:

-   [NServiceBus Host](the-nservicebus-host.md)
-   [Unit Testing](unit-testing.md)


