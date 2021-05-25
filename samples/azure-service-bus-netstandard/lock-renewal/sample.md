---
title: Azure Service Bus lock renewal
summary: Long message processing with Azure Service Bus
reviewed: 2021-05-18
component: ASBS
related:
- transports/azure-service-bus
- samples/azure-service-bus-netstandard/azure-service-bus-long-running
---


## Prerequisites

include: asb-connectionstring-xplat

<!-- include documentation from https://docs.particular.net/transports/azure-service-bus/legacy/message-lock-renewal -->

## Important information about lock renewal

1. The transport must use the default [`SendsAtomicWithReceive`](/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive) transaction mode for the sample to work.
1. When using lock renewal with the [outbox feature](/nservicebus/outbox/), the transport transaction mode has to be **explicitly** set to [`SendsAtomicWithReceive`](/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive) in the endpoint configuration code.
1. For a lock to be extended for longer than 10 minutes, the value of [`TransactionManager.MaxTimeout`](https://docs.microsoft.com/en-us/dotnet/api/system.transactions.transactionmanager.maximumtimeout) must be changed to the maximum time allowed to process a message. This is a machine wide setting and should be treated carefully.
1. Message lock renewal is initiated by client code, not the broker. If the request to renew the lock fails after all the SDK built-in retries, the lock won't be renewed, and the message will become unlocked and available for processing by competing consumers. Lock renewal should be treated as best-effort and not as a guaranteed operation.
1. Message lock renewal applies to **only** the message currently being processed. Prefetched messages that are not handled within the `LockDuration` time will lose their lock, indicated by a `LockLostException` in the log when the transport attempts to complete them. The number of prefetched messages may be adjusted, which may help to prevent exceptions with lock renewal. Alternatively, the endpoint's concurrency limit may be raised to increase the rate of message processing, but this may also increase resource contention.

## Code walk-through

The sample contains a single executable project, `LockRenewal`, that sends a `LongProcessingMessage` message to itself, and the time taken to process that message exceeds the maximum lock duration on the endpoint's input queue.

### Lock renewal feature

Lock renewal is enabled by the `LockRenewalFeature`, which is configured to be enabled by default.

snippet: LockRenewalFeature

The Azure Service Bus transport sets `LockDuration` to 5 minutes by default, so the default `LockDuration` for the feature has the same value. `ExecuteRenewalBefore` is a `TimeSpan` specifying how soon to attempt lock renewal before the lock expires. The default is 10 seconds. Both settings may be overridden using the `EndpointConfiguration` API.

snippet: override-lock-renewal-configuration

In the sample, `LockDuration` is set to 30 seconds, and `ExecuteRenewalBefore` is set to 5 seconds.

### Lock renewal behavior

The `LockRenewalFeature` uses the two settings to register the `LockRenewalBehavior` pipeline behavior. With `LockDuration` set to 30 seconds and `ExecuteRenewalBefore` set to 5 seconds, the lock will be renewed every 25 seconds (`LockDuration` - `ExecuteRenewalBefore`).

The behavior processes every incoming message and uses [native message access](/transports/azure-service-bus/native-message-access.md) to get the message's lock token, which is required for lock renewal.

snippet: native-message-access

The request to renew the lock must use the same Azure Service Bus connection object and queue path used to receive the incoming message. These items are available in the `TransportTransaction`:

snippet: get-connection-and-path

With the lock token, connection object, and queue path, an Azure Service Bus `MessageReceiver` object may be created to renew the lock. This is done in a background task, running in an infinite loop until the specified cancellation token is signaled as canceled.

snippet: renewal-background-task

The behavior executes the rest of the pipeline and cancels the background task when execution is completed.

snippet: processing-and-cancellation

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

## Overriding the value of TransactionManager.MaxTimeout

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
