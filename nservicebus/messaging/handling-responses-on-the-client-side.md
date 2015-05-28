---
title: Handling Responses on the Client Side
summary: The client (or sending process) has its own queue. When messages arrive in the queue, they are handled by a message handler.
tags: []
redirects:
- nservicebus/how-do-i-handle-responses-on-the-client-side
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

## Handling responses in the context of the request

When sending a message/request, you can register a callback that is invoked when a response arrives. This callback is invoked before any message handler as it is registered like this:

```C#
bus.Send(messageInstance).Register(asyncCallback, state);
```

To trigger a callback, you need to return an `enum` or `int` value.

<!-- import TriggerCallback -->

To access the response message through callbacks, the following code can be used
<!-- import CallbackToAccessMessageRegistration -->

DANGER: If the server process returns multiple responses, NServiceBus cannot know which response message will be the last. To prevent memory leaks, the callback is invoked only for the first response. Callbacks won't survive a process restart (common scenarios are a crash or an IIS recycle) as they are held in memory, so they are less suitable for server-side development where fault-tolerance is required. In those cases, [sagas are preferred](/nservicebus/sagas/).

## When should you use callbacks?

Due to the fact that callbacks won't survive restarts, use callbacks when the data returned is **not business critical and data loss is acceptable**. Otherwise, use [request/response](/samples/fullduplex) with a message handler for the reply messages.

When using callbacks in a ASP.NET Web/MVC/Web API, the NServiceBus callbacks can be used in combination with the async support in Asp.Net to avoid blocking the web server thread and allowing processing of other requests. When response is recieved, it is handled and returned to the client side. Web clients will still be blocked while waiting for response. This scenario is common when migrating from traditional blocking request/response to messaging.
