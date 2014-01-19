---
title: How to Send a Message?
summary: Describes how to send a message, or instantiate and send all messages at once.
originalUrl: http://www.particular.net/articles/how-do-i-send-a-message
tags: []
createdDate: 2013-05-22T05:03:38Z
modifiedDate: 2013-11-25T13:48:37Z
authors: []
reviewers: []
contributors: []
---

To send a message, use the Send method on the IBus interface, passing as the argument the instance of the message to deliver:


```C#
busInstance.Send( messageInstance );
```

 Or instantiate and send all messages at once:


```C#
busInstance.Send<IMyMessage>( m => { m.Prop1 = v1; m.Prop2 = v2; });
```




