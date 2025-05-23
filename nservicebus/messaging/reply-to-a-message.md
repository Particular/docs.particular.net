---
title: Replying to a Message
summary: Answer a message using the Reply method on IMessageHandlerContext/IBus.
component: Core
reviewed: 2025-05-19
redirects:
- nservicebus/how-do-i-reply-to-a-message
---

The simplest way to reply to a message is using the `Reply` method:

snippet: ReplyingMessageHandler

Only use a reply when implementing the Request-Response pattern (also called the Full Duplex pattern). In this pattern, the originator of the message should expect a response message and have a handler for it. For examples and more information, see the [Full Duplex Sample](/samples/fullduplex/) and the article [Callbacks](/nservicebus/messaging/callbacks.md).

When using the Publish-Subscribe pattern, an endpoint handling an event shouldn't use `Reply`. This is because the publisher might not expect a reply and has no message handler for the replied message.

The reply address is controlled by the sender of the message being replied to. See [how to influence the reply behavior when sending messages](send-a-message.md#influencing-the-reply-behavior).

## Influencing the reply behavior

A sender of a reply can influence how the requester will behave when continuing the conversation (replying to a reply). It can request a reply to go to itself (not any other instance of the same endpoint)

snippet: BasicReplyReplyToThisInstance

or explicitly to any instance of the endpoint (which overrides the *public reply address* setting)

snippet: BasicReplyReplyToAnyInstance

It can also request the reply to be routed to a specified transport address.

snippet: BasicReplyReplyToDestination

> [!NOTE]
> Replies participate in the handler transaction and are not sent if the message rolls back. If the code requires a response, whether or not message processing succeeds, use [immediate dispatch](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately) on the reply options. Make sure exceptions are rethrown to rollback any transactions and use a [custom recoverability policy](/nservicebus/recoverability/custom-recoverability-policy.md) to not retry non-transient errors.



