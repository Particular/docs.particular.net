---
title: Metrics
summary: Measuring the performance and health of an endpoint.
reviewed: 2017-04-04
component: Metrics
related:
 - nservicebus/operations
related:
 - samples/metrics
---

The `NServiceBus.Metrics` package can be used to collect information and measure endpoint health and performance. When a system is broken down into multiple processes, each with its own queue, then bottlenecks may be identified by tracking:
 - the number of messages in each queue
 - the incoming rate of messages for each queue
 - the processing rate for each queue.

## Enabling NServiceBus.Metrics

snippet: Metrics-Enable

partial: reporting

## Metrics captured

NServiceBus and ServiceControl capture a number of different metrics about a running endpoint including the processing time, "critical time", and number of messages in each queue.

### Processing time

Processing time is the time it takes for an endpoint to process a single message.

### Number of messages pulled from queue

This metric measures the number of messages that the endpoint reads from it's input queue regardless of whether message processing succeeds or fails. 

### Number of messages successfully processed

This metric measures the number of messages that the endpoint successfully processes. In order for a message to be counted by this metric, all handlers must have executed without throwing an exception.

### Number of message processing failures

This metric measures the number of messages that the endpoint has failed to process. In order for a message to be counted by this metric, at least one handler must have thrown an exception.

### Critical time

Critical time is the time between when a message is sent and when it is fully processed. It is a combination of:
- Network send time: The time a message spends on the network before arriving in the destination queue
- Queue wait time: The time a message spends in the destination queue before being picked up and processed
- Processing time: The time it takes for the destination endpoint to process the message

### Queue length

This metric tracks estimated number of messages in the input queue of an endpoint.

A _link_ is a communication channel between a sender of the message and its receiver. Each link is uniquely identified by some combination of destination address, message assembly, and the [host identifier](/nservicebus/hosting/override-hostid.md#host-identifier) of the sender. The exact composition of link identifiers depends on the transport properties and type of message being sent.

Each sender maintains a monotonic counter of messages sent over each of its outgoing links and transmits the value of this counter to the receiver in a message header. The receiver tracks the counter value for the last message received over each link. This allows both communicating endpoints to track how many messages were sent and received over each link.

ServiceControl collects these metrics for all links and estimates the length of the input queue for each receiver based on how many messages were sent in total over all incoming links and how many of those messages have already been received.

#### Example

The system consists of two endpoints, Sales and Shipping. Sales send messages to Shipping to notify it about some business events. The Sales endpoint is scaled out and deployed to two machines, `1` and `2`. Consider the following values reported to ServiceControl:

| Link ID                        | Max sent counter | Max received counter | Messages in queue from this link |
|--------------------------------|:----------------:|:--------------------:|:--------------------------------:|
| `Sales@1->Shipping`             | 20               | 17                   | 3                                |
| `Sales@2->Shipping`             | 33               | 31                   | 2                                |


Based on the data above, ServiceControl can estimate the following values of queue length for `Shipping` endpoints:

| Endpoint | Queue length terms  | Calculated queue length |
|----------|:-------------------:|:-----------------------:|
| Shipping    | 3 + 2               | 5                       |
