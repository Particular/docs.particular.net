---
title: Sending a Message
summary: Describes how to send a message
tags: []
redirects:
- nservicebus/how-do-i-send-a-message
---

Prior to V6 sending a message involved using the `Send` method on the `IBus` interface, passing as the argument the instance of the message to deliver. In V6 the equivalent of the `IBus` is the `IBusContext` family of interfaces.

Here's how a message is sent by directly using the instance of the endpoint

snippet:BasicSend

And here's how it is done from the message handler

snippet:SendFromHandler

In both cases you can use an interface rather than a concrete class for a message:

snippet:BasicSendInterface

## Immediate Dispatch

While its usually best to let NServiceBus [handle exceptions for you](/nservicebus/errors), there are some scenarios where you want to send messages out even though the incoming message is rolled back. One example would be sending a reply notifying that there was an issue processing the message. 

In order to request immediate dispatch you can use the following syntax.

snippet:RequestImmediateDispatch

NOTE: By specifying immediate dispatch, outgoing messages are not [batched](/nservicebus/messaging/batched-dispatch.md) or enlisted in the current receive transaction even if the transport has support for it.

### Suppressing the transaction scope

Version 6 and below allows you to suppress the ambient transaction in order to have the outgoing message sent immediately.

snippet:RequestImmediateDispatchUsingScope

The issue with this approach is that it only works for transports that enlists the receive operation in a transaction scope. Currently this would be MSMQ and SqlServer in DTC mode. Should you use any other transport or disable the DTC this no longer works and the outgoing message might be rolled back together with the incoming message. 

For this reason we've decided to deprecate this method and recommend users switch to the explicit API mentioned above.



