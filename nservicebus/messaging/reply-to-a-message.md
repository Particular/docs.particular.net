---
title: Replying to a Message
summary: Answer a message using the Reply method on IBus.
tags: []
redirects:
- nservicebus/how-do-i-reply-to-a-message
---

The simplest way to reply to a message is using the `Reply` method provided by the bus, like this:

snippet: ReplyingMessageHandler

You should only use a reply when you implement the Request-Response pattern (also called the Full Duplex pattern). In this pattern the originator of the message should expect a response message and have a handler for it. For examples and more information see the [Full Duplex Sample](/samples/fullduplex/) and the article [How to Handle Responses on the Client Side](/nservicebus/messaging/handling-responses-on-the-client-side.md).

When using the Publish-Subscribe pattern an endpoint handling an event shouldn't use `Reply`. This is because the publisher will not be expecting any reply and should not have a handler for it.

## Influencing the reply behavior

A sender of a reply can influence how the requester will behave when continuing the conversation (replying to a reply). It can request a reply to go to itself (not any other instance of the same endpoint)

snippet:BasicReplyReplyToThisInstance

or explicitly to any instance of the endpoint (which overrides the *public reply address* setting)

snippet:BasicReplyReplyToAnyInstance

It can also request the reply to be routed to a specified raw address instead

snippet:BasicReplyReplyToDestination
