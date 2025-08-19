---
title: AmazonSQS transport native integration sample
reviewed: 2025-07-25
component: Sqs
related:
- transports/sqs
redirects:
- samples/sqs/native-integration
---

This sample demonstrates how to enable an NServiceBus endpoint to receive messages sent by a native (i.e. non-NServiceBus-based) implementation.

downloadbutton

## AWS setup

### Security and access configuration

Add the [AWS Access Key ID and AWS Secret Access Key](https://docs.aws.amazon.com/general/latest/gr/aws-sec-cred-types.html#access-keys-and-secret-access-keys) to the following environment variables:

* Access Key ID in `AWS_ACCESS_KEY_ID`
* Secret Access Key in `AWS_SECRET_ACCESS_KEY`
* Default Region in `AWS_REGION`

See also [AWS Account Identifiers](https://docs.aws.amazon.com/general/latest/gr/acct-identifiers.html), [Managing Access Keys for an AWS Account](https://docs.aws.amazon.com/general/latest/gr/managing-aws-access-keys.html), and [IAM Security Credentials](https://console.aws.amazon.com/iam/home#/security_credential).

See also [AWS Regions](https://docs.aws.amazon.com/general/latest/gr/rande.html) for a list of available regions.

### SQS

Several [Amazon SQS](https://aws.amazon.com/sqs/) queues are required to run this sample. These will be created at start-up via the [installer mechanism](/nservicebus/operations/installers.md) of NServiceBus. The queues can be seen in the [SQS management UI](https://console.aws.amazon.com/sqs/home).

* `Samples-Sqs-SimpleReceiver`: The main message processing queue.
* `Samples-Sqs-SimpleReceiver-delay.fifo`: Queue used for [delayed retries](/nservicebus/recoverability/#delayed-retries).
* `error`: Queue used for [error handling](/nservicebus/recoverability/configure-error-handling.md).

### Code walk-through

In this sample, an external system sends a message to an SQS queue using the Amazon SQS .NET SDK.

snippet: NativeMessage

Attributes are included to demonstrate how to access those from handlers or behaviors in the pipeline.

snippet: SendingANativeMessage

On the receiving end, an NServiceBus endpoint is listening to the queue and has a handler in place to handle messages of type `SomeNativeMessage`.

> [!NOTE]
> For the message to be successfully deserialized by NServiceBus, the sender must include the full name of the message class in the `$type` special attribute recognized by the Newtonsoft JSON serializer.

snippet: NativeMessage

The serializer must be configured to handle this annotation:

snippet: SerializerConfig

> [!NOTE]
> Custom serializers are also supported

First, the message will be intercepted in the incoming logical message context as there is a behavior in place:

snippet: BehaviorAccessingNativeMessage

The code to register the above behavior is:

snippet: RegisterBehaviorInPipeline

Next, the handler is invoked. The handler code can also access the native message and its attributes.

snippet: HandlerAccessingNativeMessage
