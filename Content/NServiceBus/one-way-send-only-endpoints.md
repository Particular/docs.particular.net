---
title: One Way/Send Only Endpoints
summary: Use "Send only mode" for endpoints whose only purpose is sending messages
tags:
- SendOnly
---

The equivalent to the [one way bus in Rhino Service Bus](http://ayende.com/blog/140289/setting-up-a-rhino-service-bus-application-part-ii-one-way-bus) is what NServiceBus calls "Send only mode". You would use this for endpoints whose only purpose is sending messages, such as websites. This is the code for starting an endpoint in send only mode.


In Version 4.0:


```C#
Configure.Serialization.Xml();
var bus = Configure.With()
    .DefaultBuilder()
    .UseTransport<Msmq>()
    .UnicastBus()
    .SendOnly();
bus.Send(new TestMessage());

```




In Version 3.0:


```C#
var bus = Configure.With()
    .DefaultBuilder()
    .XmlSerializer()
    .MsmqTransport()
    .UnicastBus()
    .SendOnly();
bus.Send(new TestMessage());
```



The only configuration when running in this mode is the destination of the messages you're sending. You can configure it [inline or through configuration](how-do-i-specify-to-which-destination-a-message-will-be-sent.md)
.

