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

NServiceBus supports sending different types of messages (see [Messages, Events, and Commands](messages-events-commands.md)) to any endpoint. Messages can be sent either directly from the endpoint or as part of handling an incoming message. When a message arrives at an endpoint, it goes through a [pipeline of processing steps](/nservicebus/pipeline/).


## Outside a message handler

In some cases, messages that need to be sent may not be related to an incoming message. Some examples are:

 * Sending a command when a HTML form is submitted in an ASP.NET application.
 * Publishing an event when the user clicks a button on a GUI application (see [Publish and Handle an Event](publish-subscribe/publish-handle-event.md)).

To send a message directly from the endpoint:

snippet: BasicSend

partial: batch-sends-performance


## Inside the incoming message processing pipeline

Messages often need to be sent as part of handling an incoming message. When running in a [transaction mode](/nservicebus/transports/transactions.md) that supports it, these send operations take part in the same transaction as that of the message handler, thereby ensuring that the send operations roll back if the handling of the message fails at any stage of the message processing pipeline.

To send a message from inside a message handler:

snippet: SendFromHandler

The message can also be an interface rather than a concrete class:

snippet: BasicSendInterface

partial:imessagessession-warning


partial: override-default-routing


partial: sending-to-self


partial: influence-reply


## Dispatching a message immediately

While it's usually best to let NServiceBus [handle all exceptions](/nservicebus/recoverability/), there are some scenarios where messages might need to be sent regardless of whether the message handler succeeds or not. For example, it might be desirable to send a reply notifying that there was a problem with processing the message.

partial: immediate-dispatch
