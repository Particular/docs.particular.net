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

1. Message lock renewal operation is initiated by the client code, not the broker. If the request to renew the lock fails after all the built into the SDK retries, the lock won't be re-acquired, and the message will become unlocked and available for re-processing by compteting consumers. Lock renewal should be treated as best-effort and not as a guaranteed operation.
1. Message lock renewal applies to the currently processed message **only**. Prefetched messages that are not handled within the `LockDuration` time will lose their lock, indicated with a `LockLostException` in the log when they are attempted to be completed by the transport. To prevent the exception with lock renewal, prefetching should be adjusted to reduce number of prefetched files. Alternatively, endpoint's concurrency can be inclreased.

## Code walk-through

This contains a single executable project, `Endpoint` that sends a `LongProcessingMessage` message to itself for processing that exceeds the maximum lock duration time allowed on the endpoint's input queue.

### Lock renewal feature

Lock renewal is enabled by the `LockRenewalFeature` that is configured to be enabled by default.

snippet: LockRenewalFeature

The Azure Service Bus transports sets the `LockDuration` to 5 minutes by default and that's the value provided to the feture as the default `LockDuration` to use. In addition to that, a `TimeSpan` indicating at what point in time before the lock expires to attempt lock renewal. The default value for the feature is set to 10 seconds. Both values can be overriden and configured using `EndpointConfiguration` API.

snippet: override-lock-renewal-configuration

In this sample, the `LockDuration` of the queue is modified to be 30 seconds and lock renewal will take place 5 seconds before lock expires.

### Lock renewal behavior

The `LockRenewalFeature` uses the two settings and registers the pipeline behavior `LockRenewalBehavior`, providing it with the `TimeSpan` to use for lock renewal. With the endpoint configured for `LockDuration` of 30 seconds and renwal 5 seconds before lock expires, the lock tocket will be renewed every (`LockDuration` - `ExecuteRenewalBefore`) or 25 seconds.

Every incoming message going through the pipeline will be processed by this behavior. Using [native message access](/transports/azure-service-bus/native-message-access.md), the behavior can get access to the message's lock token, required for lock renewal.

snippet: native-message-access

To extend the lock, the call has to be using the same Azure Service Bus connection object and the queue path used to receive the incoming message. The transport exposes this information via `TransportTransaction` and can be accessed in the following manner:

snippet: get-connection-and-path

With connection object and queue path, an Azure ServiceBus can be created to renew lock using the message's lock token obtained earlier. This is done in a background task, running in an infinite loop until the cancellation token passed in is signaled as cancelled.

snippet: renewal-background-task

The behavior executes the rest of the pipeline and when execution is completed, cancels the background task.

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