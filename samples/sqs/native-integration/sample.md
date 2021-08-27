---
title: Native integration with AmazonSQS Transport
reviewed: 2021-01-19
component: Sqs
related:
- transports/sqs
---

This sample demonstrates how to enable an NServiceBus endpoint to receive messages sent by a native (i.e. non-NServiceBus-based) implementation.

In this sample, an external system sends a message to an SQS queue using the Amazon SQS SDK. In order for NServiceBus to be able to consume this message, a `MessageTypeFullName` message attribute must be present. Other attributes are also included to demonstrate how to access those from handlers or behaviors in the pipeline.

snippet: SendingANativeMessage

On the receiving end, an NServiceBus endpoint is listening to the queue and has a handler in place to handle messages of type `SomeNativeMessage`.

First, the message will be intercepted in the incoming logical message context as there is a behavior in place:

snippet: BehaviorAccessingNativeMessage

The code to register the above behavior is:

snippet: RegisterBehaviorInPipeline

Next, the handler is invoked. The handler code can also access the native message and its attributes.

Note: The message attribute `MessageTypeFullName` might not be available anymore in the `MessageAttributes` collection in recoverability scenarios. Instead, it will be part of the `Headers` collection.

snippet: HandlerAccessingNativeMessage

## Replies

To enable the NServiceBus endpoint to [reply](/nservicebus/messaging/reply-to-a-message.md) back to the native endpoint a reply to address must be provided. The sample demonstrates how the native endpoint uses the attribute `ReplyToAddress` to provide the address where replies should be routed to. The receiver then uses a behavior to transfer that value to the [`NServiceBus.ReplyToAddress` header](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-replytoaddress) that NServiceBus is using for routing replies:

snippet: BehaviorPopulatingNativeReplyToAddress

After running the sample use the AWS management console to view the replies sent back to the queue `my-native-endpoint`.
