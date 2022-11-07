---
title: Azure Service Bus lock renewal
summary: Long message processing with Azure Service Bus
reviewed: 2021-05-18
component: ASBS
related:
- transports/azure-service-bus
- samples/azure-service-bus-netstandard/azure-service-bus-long-running
---

Note: Starting from version 3 of the Azure Service Bus transport [lock renewal is built-into the transport](/transports/azure-service-bus/configuration.md#lock-renewal) and custom lock renewal is no longer required. This sample will be removed in future versions of the transport.

## Prerequisites

include: asb-connectionstring-xplat

## Important information about lock renewal

partial: transactionmode

partial: maxtimeoutinformation

1. Message lock renewal is initiated by client code, not the broker. If the request to renew the lock fails after all the SDK built-in retries, the lock won't be renewed, and the message will become unlocked and available for processing by competing consumers. Lock renewal should be treated as best-effort and not as a guaranteed operation.
1. Message lock renewal applies to **only** the message currently being processed. Prefetched messages that are not handled within the `LockDuration` time will lose their lock, indicated by a `LockLostException` in the log when the transport attempts to complete them. The number of prefetched messages may be adjusted, which may help to prevent exceptions with lock renewal. Alternatively, the endpoint's concurrency limit may be raised to increase the rate of message processing, but this may also increase resource contention.

## Code walk-through

The sample contains a single executable project, `LockRenewal`, that sends a `LongProcessingMessage` message to itself, and the time taken to process that message exceeds the maximum lock duration on the endpoint's input queue.

### Configuration

Lock renewal is enabled by the `LockRenewalFeature`, which is configured to be enabled by default.

partial: walkthrough

The behavior processes every incoming message and uses [native message access](/transports/azure-service-bus/native-message-access.md) to get the message's lock token, which is required for lock renewal.

snippet: native-message-access

Obtain the means to renew the lock:

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

partial: maxtimeout