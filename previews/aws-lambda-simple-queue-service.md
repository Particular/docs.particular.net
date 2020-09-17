---
title: AWS Lambda with Simple Queue Service
component: SQSLambda
summary: Hosting NServiceBus endpoints with AWS Lambda triggered by Simple Queue Service
related:
 - samples/previews/aws-lambda/sqs
reviewed: 2020-09-08
---

Host NServiceBus endpoints with [AWS Lambda](https://aws.amazon.com/lambda/) using the [Simple Queue Service](https://aws.amazon.com/sqs/) as a trigger.

## Basic usage

An NServiceBus endpoint is hosted in AWS Lambda by creating an `AwsLambdaSQSEndpoint` instance and calling the `Process` method from within an AWS Lambda definition.

### `AwsLambdaSQSEndpoint` creation

The endpoint should be instantiated only once, when the lambda is first called, and assigned to a `static` field:

snippet: endpoint-creation

Since the cost of starting an `AwsLambdaSQSEndpoint` endpoint can be high, it is recommended to [configure the lambda's concurrency](https://docs.aws.amazon.com/lambda/latest/dg/configuration-concurrency.html) to minimize cold starts.

### Calling the `Process` method

The `AwsLambdaSQSEndpoint.Process` method is invoked inside the function handler:

snippet: function-definition

### Queue creation

Transport installers are not supported. The creation of the required queues may be scripted using the [CLI](/transports/sqs/operations-scripting.md#create-resources).

## Configuration

### Routing

Specifying [command routing](/nservicebus/messaging/routing.md#command-routing) for an AWS Lambda endpoint:

snippet: configure-routing

### Diagnostics

[NServiceBus startup diagnostics](/nservicebus/hosting/startup-diagnostics.md) are disabled by default when using AWS Lambda. Diagnostics may be enabled as follows:

snippet: custom-diagnostics

### Delayed Retries

[Delayed retries](/nservicebus/recoverability/configure-delayed-retries.md) are disabled by default when using AWS Lambdas. Delayed retries may be enabled as follows:

snippet: delayed-retries

If the accumulated time increase is expected to be greater than [15 minutes](/transports/sqs/delayed-delivery.md#enable-unrestricted-delayed-delivery), `UnrestrictedDurationDelayedDelivery` must be enabled:

snippet: unrestricted-delayed-delivery

Note: Automatic creation of the required queues for unrestricted delayed delivery is not supported. The creation of the required queues may be scripted using the [CLI](/transports/sqs/delayed-delivery.md#enable-unrestricted-delayed-delivery-manual-fifo-queue-creation).

### Error queue

Messages which fail all retries are moved to the error queue, which must be defined as follows:

snippet: configure-error-queue

Alternatively, the endpoint may be configured to never move failing messages to the error queue as follows:

snippet: configure-dont-move-to-error

### Serializer

There is no default [serializer](/nservicebus/serialization), so one must be configured. For example:

snippet: custom-serializer

### Licenses

The license is provided via the `NSERVICEBUS_LICENSE` environment variable, which can be set via the Function settings in the [Lambda console](https://docs.aws.amazon.com/lambda/latest/dg/configuration-envvars.html).

## Supported features

The `AwsLambdaSQSEndpoint` class supports the full feature set of NServiceBus including:

* Outbox
* Sagas
* Delayed Delivery
* Recoverability
* Publish / Subscribe

[Persistence](/persistence) is required to use some of these features.

### Transactions

As the `AwsLambdaSQSEndpoint` uses the `SqsTransport`, it only supports the Receive Only and Disabled [transaction modes](/transports/transactions.md).
