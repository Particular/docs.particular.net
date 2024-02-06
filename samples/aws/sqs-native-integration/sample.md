---
title: AmazonSQS transport native integration sample
reviewed: 2024-02-06
component: Sqs
related:
- transports/sqs
redirects:
- samples/sqs/native-integration
---

This sample demonstrates how to enable an NServiceBus endpoint to receive messages sent by a native (i.e. non-NServiceBus-based) implementation.

In this sample, an external system sends a message to an SQS queue using the Amazon SQS SDK.

snippet: NativeMessage

Attributes are included to demonstrate how to access those from handlers or behaviors in the pipeline.

snippet: SendingANativeMessage

On the receiving end, an NServiceBus endpoint is listening to the queue and has a handler in place to handle messages of type `SomeNativeMessage`.

NOTE: For the message to be successfully deserialized by NServiceBus, the sender must include the full name of the message class in the `$type` special attribute recognized by the Newtonsoft JSON serializer.

snippet: NativeMessage

The serializer must be configured to handle this annotation:

snippet: SerializerConfig

NOTE: Custom serializers are also supported

First, the message will be intercepted in the incoming logical message context as there is a behavior in place:

snippet: BehaviorAccessingNativeMessage

The code to register the above behavior is:

snippet: RegisterBehaviorInPipeline

Next, the handler is invoked. The handler code can also access the native message and its attributes.

snippet: HandlerAccessingNativeMessage