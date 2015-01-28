---
title: How to Handle Responses on the Client Side
summary: The client (or sending process) has its own queue. When messages arrive in the queue, they are handled by a message handler.
tags: []
---

To handle responses on the client, the client (or the sending process) must have its own queue and cannot be configured as a SendOnly endpoint. When messages arrive in this queue, they are handled just like on the server by a message handler:


```C#
public class MyHandler : IHandleMessages<MyMessage>
{
     public void Handle( MyMessage message )
     {
          // do something in the client process
     }
}
```

You will probably need to access other objects in the client process in this handler. Use [dependency injection](containers.md) , declaring a property or constructor argument of the necessary type in your message handler. Then, register the relevant object in the container (most likely as a singleton).

## Handling responses in the context of the request

When sending a message/request, you can register a callback that is invoked when a response arrives. This callback is invoked before any message handler as it is registered like this:


```C#
bus.Send(messageInstance).Register(asyncCallback, state);
```

DANGER: If the server process returns multiple responses, NServiceBus cannot know which response message will be the last. To prevent memory leaks, the callback is invoked only for the first response. Callbacks won't survive a crash as they are held in memory, so they are less suitable for server-side development where fault-tolerance is required. In those cases, [sagas are preferred](sagas-in-nservicebus.md).

To trigger a callback, you need to return an `enum` or `int` value.

<!-- import TriggerCallback -->

## Handling callbacks in the context of web application

Callbacks can be used to provide an acknowledgement for successfully dispatched command, but should not be used instead of message handlers. Message handlers should handle asynchronous communication using [Full Duplex](/samples/fullduplex). Request/response with callback is possible, through discurraged and reason is outlined above. 

To access response message through callback, the following code can be used
<!-- import CallbackToAccessMessageRegistration -->

