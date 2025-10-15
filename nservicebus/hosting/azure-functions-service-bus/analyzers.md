---
title: Roslyn analyzers for Azure Functions
summary: Details of the Roslyn analyzers that promote code quality in Azure Functions.
component: ASBFunctionsWorker
versions: '[4.2,)'
reviewed: 2025-08-05
---

Starting in version 4.2, the Azure Functions host package comes with [Roslyn analyzers](https://docs.microsoft.com/en-us/visualstudio/code-quality/roslyn-analyzers-overview) that analyze the NServiceBus code to prevent the use of API calls that are not applicable in a serverless environment.

## Endpoint configuration API

### PurgeOnStartup is not supported

* **Rule ID**: NSBWFUNC004
* **Severity**:Error
* **Example message**: Azure Functions endpoints do not support PurgeOnStartup.

### LimitMessageProcessingTo is not supported

* **Rule ID**: NSBWFUNC005
* **Severity**:Error
* **Example message**: Concurrency-related settings are controlled via the Azure Function host.json configuration file.

### DefineCriticalErrorAction is not supported

* **Rule ID**: NSBWFUNC006
* **Severity**:Error
* **Example message**: Azure Functions endpoints do not control the application lifecycle and should not define behavior in the case of critical errors.

### SetDiagnosticsPath is not supported

* **Rule ID**: NSBWFUNC007
* **Severity**:Error
* **Example message**: Azure Functions endpoints should not write diagnostics to the local file system. Use CustomDiagnosticsWriter to write diagnostics to another location.

### MakeInstanceUniquelyAddressable is not supported

* **Rule ID**: NSBWFUNC008
* **Severity**:Error
* **Example message**: Azure Functions endpoints have unpredictable lifecycles and should not be uniquely addressable.

### UseTransport is not supported

* **Rule ID**: NSBWFUNC009
* **Severity**:Warning
* **Example message**: The package configures Azure Service Bus transport by default. Use ServiceBusTriggeredEndpointConfiguration.Transport to access the transport configuration.

### OverrideLocalAddress is not supported

* **Rule ID**: NSBWFUNC010
* **Severity**:Error
* **Example message**: The NServiceBus endpoint address in Azure Functions is determined by the ServiceBusTrigger attribute.

### Default logging with LogDiagnostics will log to the built-in Azure Functions logs

* **Rule ID**: NSBWFUNC018
* **Severity**: Information
* **Example message**: In Azure Functions, console output to the built-in logs is not persisted and may result in the loss of the diagnostic information. Consider using 'AdvancedConfiguration.CustomDiagnosticsWriter' for more control over diagnostics output.

## Options API

### RouteReplyToThisInstance is not supported

* **Rule ID**: NSBWFUNC011
* **Severity**:Error
* **Example message**: Azure Functions instances cannot be directly addressed as they have a highly volatile lifetime. Use 'RouteToThisEndpoint' instead.

### RouteToThisInstance is not supported

* **Rule ID**: NSBWFUNC012
* **Severity**:Error
* **Example message**: Azure Functions instances cannot be directly addressed as they have a highly volatile lifetime. Use 'RouteToThisEndpoint' instead.

## Transport Settings API

### TransportTransactionMode is not supported

* **Rule ID**: NSBWFUNC013
* **Severity**:Error
* **Example message**: Transport TransactionMode is controlled by the Azure Service Bus trigger and cannot be configured via the NServiceBus transport configuration API when using Azure Functions.

### MaxAutoLockRenewalDuration is not supported

* **Rule ID**: NSBWFUNC014
* **Severity**:Error
* **Example message**: Azure Functions endpoints do not control the message receiver and cannot decide the lock renewal duration.

### PrefetchCount is not supported

* **Rule ID**: NSBWFUNC015
* **Severity**:Error
* **Example message**: Message prefetching is controlled by the Azure Service Bus trigger and cannot be configured via the NServiceBus transport configuration API when using Azure Functions.

### PrefetchMultiplier is not supported

* **Rule ID**: NSBWFUNC016
* **Severity**:Error
* **Example message**: Message prefetching is controlled by the Azure Service Bus trigger and cannot be configured via the NServiceBus transport configuration API when using Azure Functions.

### TimeToWaitBeforeTriggeringCircuitBreaker is not supported

* **Rule ID**: NSBWFUNC017
* **Severity**:Error
* **Example message**: Azure Functions endpoints do not control the message receiver and cannot access circuit breaker settings.
