---
title: Sending messages
reviewed: 2020-06-18
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

* Sending a command when an HTML form is submitted in an ASP.NET application.
* Publishing an event when the user clicks a button on a GUI application (see [Publish and Handle an Event](publish-subscribe/publish-handle-event.md)).

To send a message directly from the endpoint:

snippet: BasicSend

Unit testing the process of sending a message is supported by [the `NServiceBus.Testing` library](/nservicebus/testing/#testing-message-session-operations).

## Inside the incoming message processing pipeline

Messages often must be sent as part of handling an incoming message. When running in a [transaction mode](/transports/transactions.md) that supports it, these send operations take part in the same transaction as that of the message handler, thereby ensuring that the send operation rolls back if the handling of the message fails at any stage of the message processing pipeline.

To send a message from inside a message handler:

snippet: SendFromHandler

partial: batch-sends-performance

partial: override-default-routing

partial: sending-to-self

partial: influence-reply

## Dispatching a message immediately

While it's usually best to let NServiceBus [handle all exceptions](/nservicebus/recoverability/), there are some scenarios where messages might need to be sent regardless of whether the message handler succeeds or not, for example, to send a reply notifying that there was a problem with processing the message.

partial: immediate-dispatch
