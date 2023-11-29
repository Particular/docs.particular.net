---
title: Azure Service Bus lock renewal
summary: Long message processing with Azure Service Bus
reviewed: 2021-05-18
component: ASBS
related:
- transports/azure-service-bus
- samples/azure-service-bus-netstandard/azure-service-bus-long-running
---

WARNING: In version 3 and higher of the Azure Service Bus transport, [lock renewal is built into the transport](/transports/azure-service-bus/configuration.md#lock-renewal), and custom lock renewal as shown in this sample is no longer required.

## Prerequisites

include: asb-connectionstring-xplat

## Important information about lock renewal

1. For a lock to be extended for longer than 10 minutes, the value of [`TransactionManager.MaxTimeout`](https://docs.microsoft.com/en-us/dotnet/api/system.transactions.transactionmanager.maximumtimeout) must be changed to the maximum time allowed to process a message. This is a machine wide setting and should be treated carefully.
1. The client initiates message lock renewal, not the broker. If the request to renew the lock fails after all the SDK built-in retries (.e.g, due to connection loss), the lock won't be renewed, and the message will be made available for processing by competing consumers. Lock renewal should be treated as a best effort, not as a guaranteed operation.
1. Message lock renewal applies to **only** the message currently being processed. Prefetched messages that are not handled within the `LockDuration` time will lose their lock, indicated by a `LockLostException` in the log when the transport attempts to complete them. The number of prefetched messages may be adjusted, which may help to prevent exceptions with lock renewal. Alternatively, the endpoint's concurrency limit may be raised to increase the rate of message processing, but this may also increase resource contention.

## Code walk-through

The sample contains a single executable project, `LockRenewal`, that sends a `LongProcessingMessage` message to itself, and the time taken to process that message exceeds the maximum lock duration on the endpoint's input queue.

### Configuration

Lock renewal is enabled by the `LockRenewalFeature`, which is configured to be enabled by default.

The feature does not require any additional configuration.

### Behavior

The lock will be renewed 10 seconds before the `LockUntil` value from the message. By default the lock will thus be renewed after 4m50s.

The sample overrides the queue lock duration to 30 seconds for demo purposes which means in the sample the lock is renewed every 20s (30s-10s). The buffer value is hardcoded in the sample.

The behavior processes every incoming message and uses [native message access](/transports/azure-service-bus/native-message-access.md) to get the message's lock token, which is required for lock renewal.

snippet: native-message-access

To renew the lock, get the following object from the context:

snippet: get-connection-and-path

A background task is started that will renew the lock until the completion of message processing signals the cancellation token.

snippet: renewal-background-task

The behavior executes the rest of the pipeline and cancels the background task when execution is completed.

snippet: processing-and-cancellation

### Prefetching

Disable prefetching if the majority of messages in the queue result in long processing durations. Otherwise, the lock for prefetched messages could expire before processing even starts.

### Long-running handler

The handler emulates long-running processing with a delay of 45 seconds, which exceeds the `LockDuration` of the input queue, which is set to 30 seconds.

snippet: handler-processing

### Running the sample

Running the sample produces output similar to the following:

```text
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

Message processing takes 45 seconds, and the `LockDuration` is 30 seconds, so the message will have its lock renewed once, and processing will finish successfully.

## Overriding the TransactionManager.MaxTimeout value

### .NET Framework

The setting can be modified using a machine level-configuration file:

```xml
<system.transactions>
  <machineSettings maxTimeout="01:00:00" />
</system.transactions>
```

or using reflection:

snippet: override-transaction-manager-timeout-net-framework

### .NET Core

The setting can be modified using reflection:

snippet: override-transaction-manager-timeout-net-core
