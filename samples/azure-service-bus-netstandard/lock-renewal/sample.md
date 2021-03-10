---
title: Azure Service Bus Lock Renewal Sample
summary: Long message processing with Azure Service Bus
reviewed: 2021-02-22
component: ASBS
related:
- transports/azure-service-bus
- samples/azure/azure-service-bus-long-running
---


## Prerequisites

include: asb-connectionstring-xplat

<!-- include documentation from https://docs.particular.net/transports/azure-service-bus/legacy/message-lock-renewal -->

## Important information about lock renewal

1. The transport must use the default [`SendsAtomitWithReceive`](/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive) transport transaction mode for the sample to work.
1. For a lock to be extended for longer than 10 minutes, the value of [`TransactionManager.MaxTimeout`](https://docs.microsoft.com/en-us/dotnet/api/system.transactions.transactionmanager.maximumtimeout) must be changed to the maximum time a message will be processed. This is a machine wide setting and should be treated carefuly.
1. Message lock renewal operation is initiated by the client code, not the broker. If the request to renew the lock fails after all the SDK built-in retries, the lock won't be re-acquired, and the message will become unlocked and available for re-processing by competing consumers. Lock renewal should be treated as best-effort and not as a guaranteed operation.
1. Message lock renewal applies to the currently processed message **only**. Prefetched messages that are not handled within the `LockDuration` time will lose their lock, indicated with a `LockLostException` in the log when they are attempted to be completed by the transport. Prefetching can be adjusted to reduce the number of prefetched messages, which can help to prevent exceptions with lock renewal. Alternatively, the endpoint's concurrency can be increased to speed up the processing of messages due to the increased concurrency.

## Code walk-through

This contains a single executable project, `LockRenewal` that sends a `LongProcessingMessage` message to itself for processing that exceeds the maximum lock duration allowed on the endpoint's input queue.

### Lock renewal feature

Lock renewal is enabled by the `LockRenewalFeature` that is configured to be enabled by default.

snippet: LockRenewalFeature

The Azure Service Bus transport sets the `LockDuration` to 5 minutes by default, and that's the value provided to the feature as the default `LockDuration`. In addition to that, a `TimeSpan` indicating at what point in time before the lock expires to attempt lock renewal. The default value for the feature is set to 10 seconds. Both values can be overridden and configured using the `EndpointConfiguration` API.

snippet: override-lock-renewal-configuration

In this sample, the `LockDuration` of the queue is modified to be 30 seconds, and lock renewal will take place 5 seconds before the lock expires.

### Lock renewal behavior

The `LockRenewalFeature` uses the two settings and registers the pipeline behavior `LockRenewalBehavior`, providing it with the `TimeSpan` to use for the lock renewal. With the endpoint configured for `LockDuration` of 30 seconds and renewal of 5 seconds before the lock expires, the lock token will be renewed every (`LockDuration` - `ExecuteRenewalBefore`) or 25 seconds.

This behavior will process every incoming message going through the pipeline. Using [native message access](/transports/azure-service-bus/native-message-access.md), the behavior can get access to the message's lock token, required for lock renewal.

snippet: native-message-access

The call to extend the lock must be using the same Azure Service Bus connection object and the queue path used to receive the incoming message. The transport exposes this information via `TransportTransaction` and can be accessed in the following manner:

snippet: get-connection-and-path

With connection object and queue path, an Azure ServiceBus can be created to renew the lock using the message's lock token obtained earlier. This is done in a background task, running in an infinite loop until the cancellation token passed in is signaled as canceled.

snippet: renewal-background-task

The behavior executes the rest of the pipeline and cancels the background task when execution is completed.

snippet: processing-and-cancellation

### Long-running handler

The handler is emulating long-running processing by delaying the processing.

snippet: handler-processing

In this example, the handler will be running for 45 seconds, exceeding the `LockDuration` of the input queue set to 30 seconds.

### Running the sample

Running the sample will produce a similir to the following output:

```
Press any key to exit
INFO  LockRenewalBehavior Incoming message ID: 940e9a1f-fd8e-4e48-96b7-4604a544d8f2
INFO  LockRenewalBehavior Lock will be renewed in 00:00:25
INFO  LongProcessingMessageHandler --- Received a message with processing duration of 00:00:45
INFO  LockRenewalBehavior Lock renewed till 2021-02-22 05:47:40 UTC / 2021-02-21 22:47:40 local
INFO  LockRenewalBehavior Lock will be renewed in 00:00:25
INFO  LongProcessingMessageHandler --- Processing completed
INFO  LockRenewalBehavior Cancelling renewal task for incoming message ID: 940e9a1f-fd8e-4e48-96b7-4604a544d8f2
INFO  LockRenewalBehavior Lock renewal task for incoming message ID: 940e9a1f-fd8e-4e48-96b7-4604a544d8f2 was cancelled.
```

A message processed for 45 seconds will have its lock renewed once, successfully finishing the processing exceeding `LockDuration` of 30 seconds.

## Overriding the value of TransactionManager.MaxTimeout

### .NET Framework

The setting can be modified in a machine-level configuration file:

```
<system.transactions>
  <machineSettings maxTimeout="01:00:00" />
</system.transactions>
```

or via reflection:

snippet: override-transaction-manager-timeout-net-framework

### .NET Core

The setting can be modified using reflection:

snippet: override-transaction-manager-timeout-net-core