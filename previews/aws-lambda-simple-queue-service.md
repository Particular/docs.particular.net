---
title: AWS Lambda with Simple Queue Service
component: SQSLambda
summary: Hosting NServiceBus endpoints with AWS Lambda triggered by Simple Queue Service
related:
 - samples/previews/aws-lambda/sqs
reviewed: 2020-08-24
---

Host NServiceBus endpoints with [AWS Lambda](https://aws.amazon.com/lambda/) using [Simple Queue Service](https://aws.amazon.com/sqs/) as a trigger.

## Basic usage

Setting up AWS Lambda endpoint requires instantiating an `AwsLambdaSQSEndpoint` instance and calling the `Process` method from within an AWS Lambda definition.

### AwsLambdaSQSEndpoint creation

The endpoint should be configured and instantiated as a `static` field, making sure that it's created only once when the lambda is first called:

snippet: endpoint-creation

Since the initial cost of starting an `AwsLambdaSQSEndpoint` endpoint can be high, it is recommended to [configure the lambda's concurrency](https://docs.aws.amazon.com/lambda/latest/dg/configuration-concurrency.html) to ensure that cold starts are minimized.

### AWS Lambda definition

`AwsLambdaSQSEndpoint.Process` should be invoked inside the function handler:

snippet: function-definition

## Configuration

### Routing

[Command routing](/nservicebus/messaging/routing.md#command-routing) for AWS Lambda endpoint can be specified with the following API: 

snippet: configure-routing

### Diagnostics

[NServiceBus startup diagnostics](/nservicebus/hosting/startup-diagnostics.md) are disabled by default when using AWS Lambdas. Diagnostics can be written to the logs via the following snippet:

snippet: custom-diagnostics

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