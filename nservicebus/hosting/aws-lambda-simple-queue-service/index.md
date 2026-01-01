---
title: AWS Lambda with Simple Queue Service
component: SQSLambda
summary: Hosting NServiceBus endpoints with AWS Lambda triggered by Simple Queue Service
related:
 - samples/aws/lambda-sqs
 - samples/aws/lambda-sqs-annotations
reviewed: 2026-01-02
redirects:
 - previews/aws-lambda-simple-queue-service
---

Host NServiceBus endpoints with [AWS Lambda](https://aws.amazon.com/lambda/) using the [Simple Queue Service](https://aws.amazon.com/sqs/) as a trigger.

## Basic usage

An NServiceBus endpoint is hosted in AWS Lambda by creating an `AwsLambdaSQSEndpoint` instance and calling the `Process` method from within an AWS Lambda definition.

### `AwsLambdaSQSEndpoint` creation

partial: endpoint-creation

Since the cost of starting an `AwsLambdaSQSEndpoint` endpoint can be high, it is recommended to [configure the lambda's concurrency](https://docs.aws.amazon.com/lambda/latest/dg/configuration-concurrency.html) to minimize cold starts.

### Calling the `Process` method

The `IAwsLambdaSQSEndpoint.Process` method is invoked inside the function handler:

snippet: aws-function-definition

### Queue creation

Transport installers are not supported. The creation of the required queues may be scripted using the [CLI](/transports/sqs/operations-scripting.md#create-resources).

## Configuration

The configuration API exposes NServiceBus configuration options to allow customization; however, not all options will [apply to execution within AWS Lambda](./analyzers.md).

partial: serializer

### Routing

Specifying [command routing](/nservicebus/messaging/routing.md#command-routing) for an AWS Lambda endpoint:

snippet: aws-configure-routing

### Diagnostics

[NServiceBus startup diagnostics](/nservicebus/hosting/startup-diagnostics.md) are disabled by default when using AWS Lambda. Diagnostics may be enabled as follows:

snippet: aws-custom-diagnostics

partial: delayed-delivery

### Error handling

Messages that fail all retries are [moved to the error queue](/nservicebus/recoverability/configure-error-handling.md#configure-the-error-queue-address). Alternatively, to enable the use of [AWS Lambda error handling](https://docs.aws.amazon.com/lambda/latest/dg/invocation-retries.html), the endpoint can be configured to not move messages to the error queue:

snippet: aws-configure-dont-move-to-error

### Licenses

The license is provided via the `NSERVICEBUS_LICENSE` environment variable, which can be set via the Function settings in the [Lambda console](https://docs.aws.amazon.com/lambda/latest/dg/configuration-envvars.html).

## Native integration

It is sometimes useful to access the native Amazon SQS message from behaviors and handlers. In addition to `SqsTransport`'s support for [Amazon SQS Native Integration](/transports/sqs/native-integration.md), `AwsLambdaSQSEndpoint` also provides access to an instance of the native Lambda message type `SQSEvent.SQSMessage` from the message processing context.

snippet: native-lambda-sqs-message

## Supported features

The `AwsLambdaSQSEndpoint` class supports the full feature set of NServiceBus including:

* [Outbox](/nservicebus/outbox/)
* [Sagas](/nservicebus/sagas/)
* [Delayed Delivery](/nservicebus/messaging/delayed-delivery.md)
* [Recoverability](/nservicebus/recoverability/)
* [Publish / Subscribe](/nservicebus/messaging/publish-subscribe/)

[Persistence](/persistence) is required to use some of these features.

### Transactions

The `AwsLambdaSQSEndpoint` uses `SqsTransport`, which limits transaction support to the Receive Only and Disabled [transaction modes](/transports/transactions.md). Distributed transactions and atomic send-and-receive semantics are not available with this transport.
