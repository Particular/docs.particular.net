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

Warning: the queue length metric is experimental and  the calculated value for the metric can substantially differ from the actual value under [certain scenarios](#metrics-captured-queue-length-known-limitations).

This metric tracks the **estimated** number of messages in the input queue of an endpoint. The reason why this value needs to be estimated is that when scaling out certain queuing environments, under high load, the number of messages actually in a given input queue would not reflect all the messages *in transit* to that queue, thus reporting much lower values. For this reason, a different approach is used - one based on *links* rather than queues.

A _link_ is a communication channel between a sender of the message and its receiver. Each link is uniquely identified by some combination of destination address, message assembly, and the [host identifier](/nservicebus/hosting/override-hostid.md#host-identifier) of the sender. The exact composition of link identifiers depends on the transport properties and type of message being sent.

Each sender maintains a monotonic counter of messages sent over each of its outgoing links and transmits the value of this counter to the receiver in a message header. The receiver tracks the counter value for the last message received over each link. This allows both communicating endpoints to track how many messages were sent and received over each link.

ServiceControl collects these metrics for all links and estimates the length of the input queue for each receiver based on how many messages were sent in total over all incoming links and how many of those messages have already been received.

#### Known limitations
As noted before, the current implementation might produce estimates which significantly differ from the actual queue length value. This might happen in the following scenarios:
 * Queue length for [satellite queues](/nservicebus/satellites) like `.timeouts`, `.timeoutdispatcher`, `.retries` and custom satellite queues are not reported.
 * [Sender Side Distribution](/transports/msmq/sender-side-distribution.md) with non-identical receiver instances, e.g. some are fast and some are slow
 * High error rate scenarios in which significant number of messages are scheduled for [delayed retrying](/nservicebus/recoverability/#delayed-retries) or moved to the [error](/nservicebus/recoverability/#fault-handling) queue
 * In [Distributor](/transports/msmq/distributor/)-based deployments there is no queue length metric provided for the distributor node, only for the workers
 * After restarting any component the estimated queue length value can be off until all messages sent before restart are consumed
 *  In [Service Fabric Stateful Services with Partition Affine Routing](/samples/azure/azure-service-fabric-routing/) the queue length cannot be determined. Individual partitions can have a different backlog of messages e.g. one partition has more messages to be processed then another or is faster than another.
 * Does not work with dynamically subscribe/unsubscribe, assumption is that subscription routes are static.

#### Example

The system consists of two endpoints, Sales and Shipping. Sales send messages to Shipping to notify it about some business events. The Sales endpoint is scaled out and deployed to two machines, `1` and `2`. Consider the following values reported to ServiceControl:

| Link ID                        | Max sent counter | Max received counter | Messages in queue from this link |
|--------------------------------|:----------------:|:--------------------:|:--------------------------------:|
| `Sales@1->Shipping`            | 20               | 17                   | 3                                |
| `Sales@2->Shipping`            | 33               | 31                   | 2                                |


Based on the data above, ServiceControl can estimate the following values of queue length for `Shipping` endpoints:

| Endpoint | Queue length terms  | Calculated queue length |
|----------|:-------------------:|:-----------------------:|
| Shipping | 3 + 2               | 5                       |
