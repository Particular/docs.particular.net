---
title: Metric definitions
summary: Measuring the performance and health of an endpoint.
reviewed: 2018-01-12
redirects:
  - nservicebus/operations/metrics
---

Gathering metrics is important to know how a system works and if it works properly. When a system is broken down into multiple processes, each with its own queue, gathering and reporting metrics becomes vital. Below, there's a list of metrics that are captured and reported by NServiceBus.

Note: Depending on the version of the reporting package and the way metrics are gathered, the set of available metrics may vary.

## Metrics captured

NServiceBus and ServiceControl capture a number of different metrics about a running endpoint including the processing time, the number of messages in each queue (differentiating between those pulled from the queue, those processed successfully, and those which failed processing), as well as "critical time".


### Processing time

Processing time is the time it takes for an endpoint to invoke all handlers and sagas for a single incoming message.

Note: Processing time does not include the time to store the outbox operation, transmit outgoing messages to the transport, fetch the incoming message, and complete the incoming message (i.e. commit the transport transaction or acknowledge the message).


### Number of messages pulled from queue

This metric measures the number of messages that the endpoint reads from it's input queue regardless of whether message processing succeeds or fails.


### Number of messages successfully processed

This metric measures the number of messages that the endpoint successfully processes. In order for a message to be counted by this metric, all handlers must have executed without throwing an exception.


### Number of message processing failures

This metric measures the number of messages that the endpoint has failed to process. In order for a message to be counted by this metric, at least one handler must have thrown an exception.


### Critical time

Critical time is the time between when a message is sent and when it is fully processed. It is a combination of:

 * Network send time: The time a message spends on the network before arriving in the destination queue
 * Queue wait time: The time a message spends in the destination queue before being picked up and processed
 * Processing time: The time it takes for the destination endpoint to process the message

Note: Critical time does not include the time to store the outbox operation, transmit messages to the transport, and complete the incoming message (i.e. commit the transport transaction or acknowledge).

### Retries

This metric measures the number of [retries](/nservicebus/recoverability) scheduled by the endpoint (immediate or delayed).

### Queue length

This metric tracks the number of messages in the input queue of an endpoint. 
