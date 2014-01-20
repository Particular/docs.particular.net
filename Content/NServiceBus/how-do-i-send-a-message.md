---
title: How to Send a Message?
summary: Describes how to send a message, or instantiate and send all messages at once.
originalUrl: http://www.particular.net/articles/how-do-i-send-a-message
tags: []
---

To send a message, use the Send method on the IBus interface, passing as the argument the instance of the message to deliver:


```C#
busInstance.Send( messageInstance );
```

 Or instantiate and send all messages at once:


```C#
busInstance.Send<IMyMessage>( m => { m.Prop1 = v1; m.Prop2 = v2; });
```




