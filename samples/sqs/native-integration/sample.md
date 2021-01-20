---
title: Native integration with AmazonSQS Transport
reviewed: 2021-01-19
component: Sqs
related:
- transports/sqs
---

This sample demonstrates how to enable an NServiceBus endpoint to receive messages sent by a native implementation.

First of all, an external system sends a message to an SQS queue using the Amazon SQS SDK.
The main requirement for NServiceBus to be able to consume this message is the presence of the `MessageTypeFullName` message attribute.
Some random attributes are aded to demonstrate how to access those from handlers or behaviors in the pipeline.

snippet: SendingANativeMessage

On the receiving end, an NServiceBus endpoint is listening to the queue and has a handler in place to handle messages of type `SomeNativeMessage`.

First, it will be intercepted in the incoming logical message context as there is a behavior in place:

snippet: BehaviorAccessingNativeMessage

To register this behavior:

snippet: RegisterBehaviorInPipeline

Then the handler is invoked. The handler code can also access the native message and its attributes.

Note: The message attribute `MessageTypeFullName` will not be available anymore in the MessageAttributes collection. Instead it is now part of the Headers collection.

snippet: HandlerAccessingNativeMessage

