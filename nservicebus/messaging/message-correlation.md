---
title: Message correlation
summary: Explains the concept of a message correlation and the use of the correlation id header
---

Message correlation is the act of connecting instance of request messages with instances response messages. Since all messages have a `Message Id` the `Correlation Id` is just a reference back to the specific message instance that caused this message to be sent back as a reply.

This is a well known pattern from the [Enterprise Integration Patterns book](http://www.enterpriseintegrationpatterns.com/patterns/messaging/CorrelationIdentifier.html)

One example usage is that it allows NServiceBus to find out which callback to invoke should the user have requested a callback when the request message was sent out.

From an message payload perspective the `Correlation Id` is just an ordinary [message header](/nservicebus/messaging/headers.md) that defaults to the same value as the message id for new messages but automatically gets set to the `Message Id` of the incoming message when you call `bus.Reply`.

NServiceBus handles this automatically for you but should you need full control over the `Correlation Id` for integration purposes you can use the following code when sending your message

snippet:custom-correlationid
