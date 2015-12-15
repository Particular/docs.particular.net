---
title: Handling Responses on the Client Side
summary: The client (or sending process) has its own queue. When messages arrive in the queue, they are handled by a message handler.
tags: []
redirects:
- nservicebus/how-do-i-handle-responses-on-the-client-side
related:
- samples/callbacks
---

To handle responses on the client, the client (or the sending process) must have its own queue and cannot be configured as a SendOnly endpoint. When messages arrive in this queue, they are handled just like on the server by a message handler:


```C#
public class MyHandler : IHandleMessages<MyMessage>
{
     public void Handle(MyMessage message)
     {
          // do something in the client process
     }
}
```


## Handling responses in the context of a message being sent

When sending a message, you can register a callback that is invoked when a response arrives. 

DANGER: If the server process returns multiple responses, NServiceBus cannot know which response message will be the last. To prevent memory leaks, the callback is invoked only for the first response. Callbacks won't survive a process restart (common scenarios are a crash or an IIS recycle) as they are held in memory, so they are less suitable for server-side development where fault-tolerance is required. In those cases, [sagas are preferred](/nservicebus/sagas/).


## Prerequisites for callback functionality

In NServiceBus Version 5 and lower callbacks are built into the core NuGet.

In NServiceBus Version 6 and higher callbacks are shipped as `NServiceBus.Callbacks` NuGet package. 


## Using Callbacks 

The callback functionality can be split into three categories based on the type of information being used; integers, enums and objects. Each of these categories involves two parts; send+callback and the response.


### Int 

The integer response scenario allows any integer value to be returned in a strong typed manner.

#### Send and Callback

snippet:IntCallback


#### Response

snippet:IntCallbackResponse


### Enum 

The enum response scenario allows any enum value to be returned in a strong typed manner.


#### Send and Callback

snippet:EnumCallback


#### Response

snippet:EnumCallbackResponse


### Object 

The Object response scenario allows an object instance to be returned. 


#### The Response message

This feature leverages the message Reply mechanism of the bus and hence the response need to be a message.

snippet:CallbackResponseMessage


#### Send and Callback

snippet:ObjectCallback

Note: In Version 3 if no handler exists for a received message then NServiceBus will throw an exception. As such for this scenario to operate a fake message handler is needed on the callback side.

snippet:FakeObjectCallbackHandler


#### Response

snippet:ObjectCallbackResponse


## Cancellation 

This API was added in the externalized Callbacks feature.

The asynchronous callback can be canceled by registering a `CancellationToken` provided by a `CancellationTokenSource`. The token needs to be registered on the `SendOptions` as shown below.

snippet:CancelCallback


## When should you use callbacks?

WARNING: Using callbacks in `IHandleMessages<T>` classes can cause deadlocks and/or other unexpected behavior, so **do not call the callback apis from inside a `Handle` method in an `IHandleMessages<T>` class**.

Due to the fact that callbacks won't survive restarts, use callbacks when the data returned is **not business critical and data loss is acceptable**. Otherwise, use [request/response](/samples/fullduplex) with a message handler for the reply messages.

When using callbacks in a ASP.NET Web/MVC/Web API, the NServiceBus callbacks can be used in combination with the async support in Asp.Net to avoid blocking the web server thread and allowing processing of other requests. When response is received, it is handled and returned to the client side. Web clients will still be blocked while waiting for response. This scenario is common when migrating from traditional blocking request/response to messaging.
