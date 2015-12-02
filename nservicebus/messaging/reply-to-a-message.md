---
title: Replying to a Message
summary: Answer a message using the Reply operation.
tags: []
redirects:
- nservicebus/how-do-i-reply-to-a-message
---

In order to reply to a message you should use the `Reply` operation like this:

snippet:Replies-Basic

Underneath the `Reply` operation invokes `Send` so you can reply directly using `Send`:

snippet:Replies-ViaSend

NOTE: In V6 the `Reply` operation is only available in context of handling a message. In previous versions it was defined on the `IBus` interface which made it available from every place in the code base though the behavior of `Reply` was not defined outside of a message handler.

You should only use a reply when you implement the Request-Response pattern (also called the Full Duplex pattern). In this pattern the originator of the message should expect a response message and have a handler for it. See the [Full Duplex Sample](/samples/fullduplex/) provided with the install and the article [How to Handle Responses on the Client Side](/nservicebus/messaging/handling-responses-on-the-client-side.md) for examples and more information.

When using the Publish-Subscribe pattern an endpoint handling an event shouldn't use the `Reply` operation. This is because the publisher will not be expecting any reply and should not have a handler for it.

