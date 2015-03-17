---
title: Sending a Message
summary: Describes how to send a message, or instantiate and send all messages at once.
tags: []
redirects:
- nservicebus/how-do-i-send-a-message
---

To send a message, use the `Send` method on the `IBus` interface, passing as the argument the instance of the message to deliver:


```C#
bus.Send( messageInstance );
```

 Or instantiate and send all messages at once:

```C#
bus.Send<IMyMessage>( m => { m.Prop1 = v1; m.Prop2 = v2; });
```




