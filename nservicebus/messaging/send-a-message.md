---
title: Sending messages
reviewed: 2017-03-17
component: Core
redirects:
 - nservicebus/how-do-i-send-a-message
 - nservicebus/containers/injecting-ibus
related:
 - nservicebus/messaging/routing
 - nservicebus/messaging/messages-as-interfaces
---

NServiceBus supports sending different types of messages (see [Messages, Events, and Commands](messages-events-commands.md)) to any endpoint. Messages can be sent either when the bus is started or as part of handling another message. When a message arrives at an endpoint, it goes through a [pipeline of processing steps](/nservicebus/pipeline/).


## Outside a message handler

In some cases, messages that need to be sent may not be related to an incoming message. Some examples are:

 * Sending a command when a HTML form is submitted in an ASP.NET application.
 * Publishing an event when the user clicks a button on a GUI application (see [Publish and Handle an Event](publish-subscribe/publish-handle-event.md)).

To send a message when the bus is started:

snippet: BasicSend

partial: batch-sends-performance


## Inside the incoming message processing pipeline

In some cases, messages might need to be sent as part of handling a message such as, inside a regular message handler, a saga, or in some advanced cases as part of extending the message handling pipeline. In this scenario, all the send operations are linked to the incoming message. The send operations take part in the same transaction scope as that of the message handler, thereby ensuring that the send operations rollback if the handling of the message fails at any stage of the message processing pipeline.

To send a message from inside a message handler:

snippet: SendFromHandler

The message can also be an interface rather than a concrete class:

snippet: BasicSendInterface

Note: In Versions 5 and below, the operations are available on the `IBus` which can be accessed using constructor or property injection. In Versions 6 and above, the message handlers have access to the `IMessageHandlerContext` parameter, which can be used to dispatch messages on the bus.


partial: override-default-routing


partial: sending-to-self


partial: influence-reply


## Dispatching a message immediately

While it's usually best to let NServiceBus [handle all exceptions](/nservicebus/recoverability/), there are some scenarios where messages might need to be sent regardless of whether the message handler succeeds or not. For example, it might be desirable to send a reply notifying that there was a problem with processing the message.

partial: immediate-dispatch
