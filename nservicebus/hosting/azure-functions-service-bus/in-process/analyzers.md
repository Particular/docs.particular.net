---
title: Roslyn analyzers for Azure Functions
summary: Details of the Roslyn analyzers that promote code quality in Azure Functions.
component: ASBFunctions
versions: '[4.3,)'
reviewed: 2025-08-18
---

Starting in version 4.3, [Roslyn analyzers](https://docs.microsoft.com/en-us/visualstudio/code-quality/roslyn-analyzers-overview) are packaged with the Azure Functions host package that analyze the NServiceBus code to prevent the use of API calls that are not applicable in a serverless environment.

## Endpoint configuration API

### PurgeOnStartup is not supported

* **Rule ID**: NSBFUNC003
* **Severity**:Error
* **Example message**: Azure Functions endpoints do not support PurgeOnStartup.

### LimitMessageProcessingTo is not supported

* **Rule ID**: NSBFUNC004
* **Severity**:Error
* **Example message**: Concurrency-related settings are controlled via the Azure Function host.json configuration file.

### DefineCriticalErrorAction is not supported

* **Rule ID**: NSBFUNC005
* **Severity**:Error
* **Example message**: Azure Functions endpoints do not control the application lifecycle and should not define behavior in the case of critical errors.

### SetDiagnosticsPath is not supported

* **Rule ID**: NSBFUNC006
* **Severity**:Error
* **Example message**: Azure Functions endpoints should not write diagnostics to the local file system. Use CustomDiagnosticsWriter to write diagnostics to another location.

### MakeInstanceUniquelyAddressable is not supported

* **Rule ID**: NSBFUNC007
* **Severity**:Error
* **Example message**: Azure Functions endpoints have unpredictable lifecycles and should not be uniquely addressable.

### UseTransport is not supported

* **Rule ID**: NSBFUNC008
* **Severity**:Warning
* **Example message**: The package configures Azure Service Bus transport by default. Use ServiceBusTriggeredEndpointConfiguration.Transport to access the transport configuration.

### OverrideLocalAddress is not supported

* **Rule ID**: NSBFUNC009
* **Severity**:Error
* **Example message**: The NServiceBus endpoint address in Azure Functions is determined by the ServiceBusTrigger attribute.

## Options API

### RouteReplyToThisInstance is not supported

* **Rule ID**: NSBFUNC010
* **Severity**:Error
* **Example message**: Azure Functions instances cannot be directly addressed as they have a highly volatile lifetime. Use 'RouteToThisEndpoint' instead.

### RouteToThisInstance is not supported

* **Rule ID**: NSBFUNC011
* **Severity**:Error
* **Example message**: Azure Functions instances cannot be directly addressed as they have a highly volatile lifetime. Use 'RouteToThisEndpoint' instead.

## Transport Settings API

### TransportTransactionMode is not supported

* **Rule ID**: NSBFUNC012
* **Severity**:Error
* **Example message**: Transport TransactionMode is controlled by the Azure Service Bus trigger and cannot be configured via the NServiceBus transport configuration API when using Azure Functions.

### MaxAutoLockRenewalDuration is not supported

* **Rule ID**: NSBFUNC013
* **Severity**:Error
* **Example message**: Azure Functions endpoints do not control the message receiver and cannot decide the lock renewal duration.

### PrefetchCount is not supported

* **Rule ID**: NSBFUNC014
* **Severity**:Error
* **Example message**: Message prefetching is controlled by the Azure Service Bus trigger and cannot be configured via the NServiceBus transport configuration API when using Azure Functions.

### PrefetchMultiplier is not supported

* **Rule ID**: NSBFUNC015
* **Severity**:Error
* **Example message**: Message prefetching is controlled by the Azure Service Bus trigger and cannot be configured via the NServiceBus transport configuration API when using Azure Functions

### TimeToWaitBeforeTriggeringCircuitBreaker is not supported

* **Rule ID**: NSBFUNC016
* **Severity**:Error
* **Example message**: Azure Functions endpoints do not control the message receiver and cannot access circuit breaker settings.

### EntityMaximumSize is not supported

* **Rule ID**: NSBFUNC017
* **Severity**:Error
* **Example message**: Azure Functions endpoints do not support automatic queue creation.

### EnablePartitioning is not supported

* **Rule ID**: NSBFUNC018
* **Severity**:Error
* **Example message**: Azure Functions endpoints do not support automatic queue creation.
