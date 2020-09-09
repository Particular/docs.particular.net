---
title: AWS Lambda with Simple Queue Service
component: SQSLambda
summary: Hosting NServiceBus endpoints with AWS Lambda triggered by Simple Queue Service
related:
 - samples/previews/aws-lambda/sqs
reviewed: 2020-08-24
---

Host NServiceBus endpoints with [AWS Lambda](https://aws.amazon.com/lambda/) using the [Simple Queue Service](https://aws.amazon.com/sqs/) as a trigger.

## Basic usage

Setting up an NServiceBus endpoint with AWS Lambda requires instantiating an `AwsLambdaSQSEndpoint` instance and calling the `Process` method from within an AWS Lambda definition.

### AwsLambdaSQSEndpoint creation

The endpoint should be configured and instantiated as a `static` field, making sure that it's created only once when the lambda is first called:

snippet: endpoint-creation

Since the initial cost of starting an `AwsLambdaSQSEndpoint` endpoint can be high, it is recommended to [configure the lambda's concurrency](https://docs.aws.amazon.com/lambda/latest/dg/configuration-concurrency.html) to ensure that cold starts are minimized.

### AWS Lambda definition

The `AwsLambdaSQSEndpoint.Process` method should be invoked inside the function handler:

snippet: function-definition

### Queue creation

Transport installers are not supported. The creation of the required queues can be scripted via the [CLI](/transports/sqs/operations-scripting.md#create-resources).

## Configuration

### Routing

[Command routing](/nservicebus/messaging/routing.md#command-routing) for AWS Lambda endpoint can be specified with the following API: 

snippet: configure-routing

### Diagnostics

[NServiceBus startup diagnostics](/nservicebus/hosting/startup-diagnostics.md) are disabled by default when using AWS Lambdas. Diagnostics can be written to the logs via the following snippet:

snippet: custom-diagnostics

### Delayed Retries

[Delayed retries](/nservicebus/recoverability/configure-delayed-retries.md) are disabled by default when using AWS Lambdas. Delayed retries can be configured using the following snippet:

snippet: delayed-retries

If the time increase is expected to be greater than [15 minutes](/transports/sqs/delayed-delivery.md#enable-unrestricted-delayed-delivery), `UnrestrictedDurationDelayedDelivery` must be enabled on the endpoint:

snippet: unrestricted-delayed-delivery

Note: Automatic creation of the required queues for unrestricted delayed delivery is not supported. The creation of the required queues can be scripted via the [CLI](/transports/sqs/delayed-delivery.md#enable-unrestricted-delayed-delivery-manual-fifo-queue-creation).

### Error queue

Continuously failing messages are moved to the error queue that must be defined with the following API:

snippet: configure-error-queue

It is posible to configure AWS Lambda endpoint to never move failing messages to the error queue with:

snippet: configure-dont-move-to-error

### Serializer

snippet: custom-serializer

### Licenses

License information can be specified using Environment Variables so that license details can be updated at runtime.

snippet: load-license-file

Updating the environment variable can be done by [configuring the environment variables](https://docs.aws.amazon.com/lambda/latest/dg/configuration-envvars.html) for the lambda.

## Supported features

The `AwsLambdaSQSEndpoint` class supports the full featureset of NServiceBus, including:

* Outbox
* Sagas
* Delayed Delivery
* Recoverability
* Publish / Subscribe

A [persister](/persistence) is required to use some of these features.

### Transactionality

As the `AwsLambdaSQSEndpoint` uses the `SqsTransport`, it only supports Receive Only, or Disabled transaction modes.
