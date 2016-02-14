---
title: Sending a Message
summary: Describes how to send a message
tags: []
redirects:
- nservicebus/how-do-i-send-a-message
---

Message sending involves using Send operation that takes an argument of a message instance to be delivered. The details differ slightly between version of NServiceBus, as shown in following snippets.

Here's how a message is sent by directly using the instance of the endpoint

snippet:BasicSend

And here's how it is done from the message handler

snippet:SendFromHandler

In both cases you can use an interface rather than a concrete class for a message:

snippet:BasicSendInterface


## Overriding the default routing

The `SendOptions` object can be used to override the default routing, either by specifying the raw destination address

snippet:BasicSendSetDestination

or the ID of the target instance

snippet:BasicSendSpecificInstance


## Sending to *self*

Sending to *self* comes in two flavors. First, endpoint can send a message to any instance of the same endpoint

snippet:BasicSendToAnyInstance

Second, it can request a message to be routed to itself (same instance). This option is only possible when endpoint instance ID has been specified

snippet:BasicSendToThisInstance


## Influencing the reply behavior

A sender of a message can influence how the receiver will behave when replying to that message by attaching the *reply to* header (by default the reply is routed to any instance of the requester endpoint). The sender can request a reply to go to itself (not any other instance of the same endpoint)

snippet:BasicSendReplyToThisInstance

or explicitly to any instance of the endpoint (which overrides the *public reply address* setting)

snippet:BasicSendReplyToAnyInstance

The sender can also request the reply to be routed to a specified raw address

snippet:BasicSendReplyToDestination


## Immediate Dispatch

While its usually best to let NServiceBus [handle exceptions for you](/nservicebus/errors), there are some scenarios where you want to send messages out even though the incoming message is rolled back. One example would be sending a reply notifying that there was an issue processing the message.

In order to request immediate dispatch you can use the following syntax.

snippet:RequestImmediateDispatch

NOTE: By specifying immediate dispatch, outgoing messages are not [batched](/nservicebus/messaging/batched-dispatch.md) or enlisted in the current receive transaction even if the transport has support for it.


### Suppressing the transaction scope

Version 6 and below allows you to suppress the ambient transaction in order to have the outgoing message sent immediately.

snippet:RequestImmediateDispatchUsingScope

The issue with this approach is that it only works for transports that enlists the receive operation in a transaction scope. Currently this would be MSMQ and SqlServer in DTC mode. When using any other transport, or disable the DTC, this no longer works and the outgoing message might be rolled back together with the incoming message.

For this reason this method has been deprecated. It is recommended to switch to the explicit API mentioned above.
