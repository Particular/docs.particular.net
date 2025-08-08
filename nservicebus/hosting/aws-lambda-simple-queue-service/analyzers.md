---
title: Roslyn analyzers for AWS Lambda
summary: Details of the Roslyn analyzers that promote code quality in AWS Lambda.
component: SQSLambda
versions: '[1.1,)'
reviewed: 2025-08-08
---

Starting in version 1.1, the AWS Lambda host package comes with [Roslyn analyzers](https://docs.microsoft.com/en-us/visualstudio/code-quality/roslyn-analyzers-overview) that analyze the NServiceBus code to prevent the use of API calls that are not applicable in a serverless environment.

## Endpoint configuration API

### PurgeOnStartup is not supported

* **Rule ID**: NSBLAM001
* **Severity**:Error
* **Example message**: AWS Lambda endpoints do not support PurgeOnStartup.

### LimitMessageProcessingTo is not supported

* **Rule ID**: NSBLAM002
* **Severity**:Error
* **Example message**: Concurrency-related settings can be configured with the Lambda API.

### DefineCriticalErrorAction is not supported

* **Rule ID**: NSBLAM003
* **Severity**:Error
* **Example message**: AWS Lambda endpoints do not control the application lifecycle and should not define behavior in the case of critical errors.

### SetDiagnosticsPath is not supported

* **Rule ID**: NSBLAM004
* **Severity**:Error
* **Example message**: AWS Lambda endpoints should not write diagnostics to the local file system. Use CustomDiagnosticsWriter to write diagnostics to another location.

### MakeInstanceUniquelyAddressable is not supported

* **Rule ID**: NSBLAM005
* **Severity**:Error
* **Example message**: AWS Lambda endpoints have unpredictable lifecycles and should not be uniquely addressable.

### UseTransport is not supported

* **Rule ID**: NSBLAM006
* **Severity**:Warning
* **Example message**: The package configures Amazon SQS transport by default. Use AwsLambdaSQSEndpointConfiguration.Transport to access the transport configuration.

### OverrideLocalAddress is not supported

* **Rule ID**: NSBLAM007
* **Severity**:Error
* **Example message**: The NServiceBus endpoint address in AWS Lambda is determined by Lambda event source mapping.

## Options API

### RouteReplyToThisInstance is not supported

* **Rule ID**: NSBLAM008
* **Severity**:Error
* **Example message**: AWS Lambda instances cannot be directly addressed as they have a highly volatile lifetime. Use 'RouteToThisEndpoint' instead.

### RouteToThisInstance is not supported

* **Rule ID**: NSBLAM009
* **Severity**:Error
* **Example message**: AWS Lambda instances cannot be directly addressed as they have a highly volatile lifetime. Use 'RouteToThisEndpoint' instead.

## Transport Settings API

### TransportTransactionMode is not supported

* **Rule ID**: NSBLAM010
* **Severity**:Error
* **Example message**: Transport TransactionMode is controlled by the AWS Lambda trigger and cannot be configured via the NServiceBus transport configuration API when using AWS Lambda.
