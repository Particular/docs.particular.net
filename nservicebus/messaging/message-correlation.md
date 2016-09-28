---
title: Message correlation
summary: Explains the concept of a message correlation and the use of the correlation ID header
component: Core
reviewed: 2016-08-29
related:
 - nservicebus/messaging/headers
---

Message correlation connects instances of request messages with instances of response messages. Each message has a `Message Id` and the `Correlation Id` is a reference back to the specific message instance that caused this message to be sent back as a reply.

This is a pattern from the [Enterprise Integration Patterns book](http://www.enterpriseintegrationpatterns.com/patterns/messaging/CorrelationIdentifier.html).

One example usage within NServiceBus is to find out which callback to invoke should the user have requested a callback when the request message was sent out.

From a message payload perspective the `Correlation Id` is transmitted as a [message header](/nservicebus/messaging/headers.md) that defaults to the same value as the message ID for new messages but automatically gets set to the `Message Id` of the incoming message when doing a `Reply`.

NServiceBus handles this automatically. Should full control be needed over the `Correlation Id` for integration purposes use the following code when sending the message

snippet:custom-correlationid



### Correlation vs conversations

Correlation allows requests to be matched with responses. It also enables the tracking of message flows that span more than one message exchange. First message that is sent in a new flow is assigned a unique `Conversation Id` that is then propagated to all the messages that are subsequently sent, thus forming a _conversation_. Each message that is sent within a conversation also has a `RelatedTo` value that identifies the originating message that caused it to be sent. `Conversation Id` and `RelatedTo` fields allow [ServiceInsight](/serviceinsight/#flow-diagram) to reliably reconstruct the entire message flow.

Similar to the `Correlation Id`, the `Conversation Id` and `RelatedTo` are transmitted in `NServiceBus.ConversationId` and `NServiceBus.RelatedTo` message headers.
