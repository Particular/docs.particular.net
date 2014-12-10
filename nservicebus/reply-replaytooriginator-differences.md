---
title: Reply and ReplyToOriginator differences.
summary: Document the different behaviors of the Reply and ReplyToOriginator methods of the IBus instance.
tags: []
---

When dealing with the [request/response pattern](how-do-i-reply-to-a-message.md) we are used to utilize the `Reply` method exposed by the `IBus` interface.

When using the request/response pattern in a Saga relying on the `Reply` method can lead to some unexpected behaviors.

The following diagram details a scenario where two sagas and an integration endpoint utilize the request/response pattern to communicate. In red are highlighted the possible unexpected behaviors.

![Sample sequence diagram](reply-replaytooriginator-differences.png)

The reason why a call to `Bus.Reply(res2)` sends a message to `Endpoint3` is that it is invoked in the context of handling the `res1` message, and the return address of `res1` is `Endpoint3`.

Calling `ReplyToOriginator` makes it clear to NServiceBus that you want the message to be delivered to the endpoint that was the originator of the saga.