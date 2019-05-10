---
title: Replying to a Message
summary: Answer a message using the Reply method on IMessageHandlerContext/IBus.
component: Core
reviewed: 2018-10-11
redirects:
- nservicebus/how-do-i-reply-to-a-message
---

The simplest way to reply to a message is using the `Reply` method:

snippet: ReplyingMessageHandler

Only use a reply when implementing the Request-Response pattern (also called the Full Duplex pattern). In this pattern the originator of the message should expect a response message and have a handler for it. For examples and more information see the [Full Duplex Sample](/samples/fullduplex/) and the article [Callbacks](/nservicebus/messaging/callbacks.md).

When using the Publish-Subscribe pattern, an endpoint handling an event shouldn't use `Reply`. This is because the publisher might not expect a reply and has no message handler for the replied message.

The reply address is controlled by the sender of the message replying to. See [how to influence the reply behavior when sending messages](send-a-message.md#influencing-the-reply-behavior).

partial: influence

## Use immediate dispatch for responses

In some cases when doing request/response it is required to reply with a response indicating a non transient failure. If an  exception is thrown that reply will not be send if that exception is not handled.

Often exceptions need to be propagated to ensure data modifications by performing a transactional rollback. The exception should not be handled to prevent the exception reaching the pipeline as then transactions will be committed and any buffered message still be send.

NOTE: When no message has been send or published or any state change been written immediate dispatch is not required is the handled exception is not rethrown.


```cs
try
{
}
catch (NonTransientFailureException ex)
{
    var replyOptions = new ReplyOptions();
    replyOptions.RequireImmediateDispatch();
    await context.Reply(new MyResponseIndicatingFailureMessage, replyOptions);
    throw;
}
```

In the code above the exception is handled and then rethrown after a the reply indicating failure. This reply is send with immediate dispatch or else this message will not be dispatched due to the exception resulting in transactions to be rolled back.

NOTE: This is also required with transaction mode Receive Only or None as sends and published are buffered until all handlers have been succesfully invoked and then actually transmitted to the message infrastructure.

Often such non-transient errors do not need to be retried. A [custom recoverability policy](/nservicebus/recoverability/custom-recoverability-policy.md) can be configured to prevent retrying non-transient errors.



