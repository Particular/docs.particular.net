---
title: Metric definitions
summary: List of metrics captured by NServiceBus and their definitions
reviewed: 2025-07-18
redirects:
  - nservicebus/operations/metrics
---

Gathering metrics is important to know how a system works and if it works properly. When a system is broken down into multiple processes, each with its own queue, gathering and reporting metrics becomes vital. Below is a list of metrics that are captured and reported by NServiceBus.

> [!NOTE]
> Depending on the version of the reporting package and the way metrics are gathered, the set of available metrics may vary.

## Metrics captured

NServiceBus and ServiceControl capture a number of different metrics about a running endpoint including the processing time, the number of messages in each queue (differentiating between those pulled from the queue, those processed successfully, and those that  failed processing), as well as "critical time".

### Handler time

Handler time is the amount of time it takes for each handler to perform the business logic. It is recorded for each handler separately. It includes serialization of outgoing messages. Handler time does not include any database operations managed by NServiceBus.

### Processing time

Processing time is the amount of time it takes for an endpoint to **successfully** process an incoming message. It includes:

- the execution of all handlers and sagas for the incoming message
- the execution of the incoming message processing pipeline, which includes deserialization and, where applicable, user-defined pipeline behaviors and saga loading and saving time
- storing the outbox operations (if outbox is enabled)

Processing failures are not included in the processing time metric.

> [!NOTE]
> Processing time does not include fetching the incoming message, transmitting outgoing messages to the transport, and completing the incoming message (i.e. commmitting the transport transaction or acknowledging).

### Number of messages pulled from queue

This metric measures the total number of messages that the endpoint reads from its input queue regardless of whether message processing succeeds or fails.

### Number of messages successfully processed

This metric measures the total number of messages that the endpoint has successfully processed. In order for a message to be counted by this metric, all handlers must have executed without throwing an exception.

### Number of message processing failures

This metric measures the total number of messages that the endpoint has failed to process. In order for a message to be counted by this metric, at least one handler must have thrown an exception.

### Critical time

Critical time is the amount of time between when a message is sent and when it is fully processed. It is a sum of the amounts of time it takes to perform all of the following :

- Committing the sender outbox transaction (if outbox is enabled)
- Network send time: the amount of time that a message spends on the network before arriving in the destination queue
- Queue wait time: the amount of time that a message spends in the destination queue before being picked up and processed
- Processing time: the amount of time that it takes for the destination endpoint to process the message
- Outgoing messages dispatch time: The amount of time that it takes to transmit outgoing messages to the transport

Critical time does _not_ include:

- The amount of time to complete the incoming message (i.e. commit the transport transaction or acknowledge)
- The amount of time that a delayed message is held by a timeout mechanism (NServiceBus version 7.7 and above)

> [!NOTE]
> Due to the fact that the critical time is calculated based on timestamps taken on two different machines (the sender and the receiver of a message), it is affected by the [clock drift problem](https://en.wikipedia.org/wiki/Clock_drift). In cases where the clocks of the machines differ significantly, the critical time may be reported as a negative value. Use well-known clock synchronization solutions, such as NTP, to mitigate the issue.

### Retries

This metric measures the number of [retries](/nservicebus/recoverability) scheduled by the endpoint (immediate or delayed).

### Immediate retries

This metric measures the number of [immediate retries](/nservicebus/recoverability/#immediate-retries) scheduled by the endpoint.

### Delayed retries

This metric measures the number of [delayed retries](/nservicebus/recoverability/#delayed-retries) scheduled by the endpoint.

### Moved to error queue

This metric measures the number of [messages moved to the error queue](/nservicebus/recoverability/#fault-handling).

### Queue length

This metric tracks the number of messages in the main input queue of the endpoint.

### Transactional Session metrics

There are metrics that measure various spects of the [transactioanl session operations](/nservicebus/transactional-session/#how-it-works):

- Commit duration - how long it took to commit the transaction and (optionally) dispatch the control message
- Dispatch druation - how long it took to dispatch the control message
- Attempts - how many times the mechanism attempted to process the control message for a given transaction
- Transit time - how long it took between dispatching the control message and starting to process it again

> [!NOTE]
> The queue length metric is measured centrally by the [ServiceControl Monitoring instance](/servicecontrol/monitoring-instances) for all transports except MSMQ, which uses a [custom plugin installed at the endpoint](/monitoring/metrics/msmq-queue-length.md). As a result, the NServiceBus.Metrics package does not contain a probe for this metric.
