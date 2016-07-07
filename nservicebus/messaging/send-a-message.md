---
title: Sending messages
redirects:
 - nservicebus/how-do-i-send-a-message
related:
 - nservicebus/messaging/message-owner
 - nservicebus/messaging/messages-as-interfaces
 - nservicebus/messaging/routing
---

NServiceBus supports sending different types of messages (see [Messages, Events, and Commands](messages-events-commands.md)) to any endpoint. Messages can be sent either when the bus is started or as part of handling another message. When a message arrives at an endpoint, it goes through a [pipeline of processing steps](/nservicebus/pipeline/).


## Outside a message handler

In some cases, messages that need to be sent may not be related to an incoming message. Some examples are:

 * Sending a command when a HTML form is submitted in an ASP.NET application.
 * Publishing an event when the user clicks a button on a GUI application (see [Publish and Handle an Event](publish-subscribe/publish-handle-event.md)).

To send a message when the bus is started:

snippet:BasicSend

Note: In Versions 5 and below, `IBus` is automatically registered in the configured dependency injection container. In Versions 6 and above, `IBus` has been deprecated and replaced with `IEndpointInstance` for sending messages outside the pipeline. `IEndpointInstance` is not registered by default.


## Inside the incoming message processing pipeline

In some cases, messages might need to be sent as part of handling a message such as, inside a regular message handler, a saga, or in some advanced cases as part of extending the message handling pipeline. In this scenario, all the send operations are linked to the incoming message. The send operations take part in the same transaction scope as that of the message handler, thereby ensuring that the send operations rollback if the handling of the message fails at any stage of the message processing pipeline.

To send a message from inside a message handler:

snippet:SendFromHandler

The message can also be an interface rather than a concrete class:

snippet:BasicSendInterface

Note: In Versions 5 and below, the operations are available on the `IBus` which can be accessed using constructor or property injection. In Versions 6 and above, the message handlers have access to the `IMessageHandlerContext` parameter, which can be used to dispatch messages on the bus.


## Overriding the default routing

The `SendOptions` object can be used to override the default routing.

Using the destination address:

snippet:BasicSendSetDestination

Using the ID of the target instance:

snippet:BasicSendSpecificInstance


## Sending to *self*

Sending a message to the same endpoint, i.e. Sending to *self* can be done in two ways.

An endpoint can send a message to any of its instances:

snippet:BasicSendToAnyInstance

Or, it can request a message to be routed to itself, i.e. the same instance.
NOTE: This option is only possible when endpoint instance ID has been specified 

snippet:BasicSendToThisInstance


## Influencing the reply behavior

The sender of the message can also control how the reply messages are received. When a receiving endpoint replies to a message, by default the reply message will be routed to any instance of the sending endpoint.

To explicitly control the reply message to be dispatched to a particular instance:

snippet:BasicSendReplyToThisInstance

To send the reply message to any instance of the endpoint:

snippet:BasicSendReplyToAnyInstance

The sender can also request the reply to be routed to a specific destination address

snippet:BasicSendReplyToDestination


## Dispatching a message immediately

While its usually best to let NServiceBus [handle all exceptions](/nservicebus/errors), there are some scenarios where messages might need to be sent regardless of if the message handler succeeds or not. For example, sending a reply notifying that there was a problem with processing the message.

To request immediate dispatch use the following syntax:

snippet:RequestImmediateDispatch

WARNING: By specifying immediate dispatch, outgoing messages will not be [batched](/nservicebus/messaging/batched-dispatch.md) or enlisted in the current receive transaction even if the transport has support for it.

Suppressing the ambient transaction to have the outgoing message sent immediately is also possible:

snippet:RequestImmediateDispatchUsingScope

WARNING: Suppressing transaction scopes only works for MSMQ and SQL transports in DTC mode. Other transports or disabled DTC may result in unexpected behavior. In Version 6 and above, use the explicit immediate dispatch API instead.
