---
title: MSMQ transport
summary: 'Explains the mechanics of MSMQ transport, its configuration options and various other configuration settings that were at some point coupled to this transport'
tags: 
- Transports
- MSMQ
---

## MSMQ

Historically MSMQ is the first transport supported by NServiceBus. In version 5 it still is by far the most commonly used one. Because of these and also the fact that MSMQ client libraries are included in .NET Base Class Library (`System.Messaging` assembly), MSMQ transport is built into the core of NServiceBus.

### Receiving algorithm

Because of the way MSMQ API has been designed i.e. polling receive that throws an exception when timeout is reached the receive algorithm is more complex than for other polling-driven transports (such as [SQLServer](SqlServer/Configuration.md)).

The main loops starts by subscribing to `PeekCompleted` event and calling the `BeginPeek` method. When a message arrives the event is raised by the MSMQ client API. The handler for this event starts a new receiving task and waits till this new task has completed its `Receive` call. After that is calls `BeginPeek` again to wait for more messages. 

## Configuration

Because of historic reasons, the configuration for MSMQ transport has been coupled to general bus configuration in the previous versions of NServiceBus.

### MSMQ-specific

Following settings are purely related to the MSMQ:

 * UseDeadLetterQueue (default: true)
 * UseJournalQueue (default: false)
 * UseConnectionCache (default: true)
 * UseTransactionalQueues (default: true)

From version 4 onwards these settings are configured via a transport connection string (named `NServiceBus/Transport` for all transports). Before V4 some of these properties could be set via `MsmqMessageQueueConfig` configuration section while other (namely the connectionCache and the ability to use non-transactional queues) were not available prior to V4.

<!-- include MessageQueueConfiguration-.config -->

### Failure handling & throttling

NServiceBus is designed in such a way that a user does not have to care about exception handling. All the heavy lifting is done by the framework via a [two-level retries mechanism](how-do-i-handle-exceptions.md).

From V4 onwards the configuration for this mechanism is implemented in the `TransportConfig` section. 

<!-- include TransportConfig-.config -->

 * MaximumMessageThroughputPerSecond (default: 0) sets a limit on how quickly messages can be processed between all threads. Use a value of 0 to have no throughput limit.
 * MaximumConcurrencyLevel defines the maximum number of threads concurrently processing messages at any given point in time
 * MaxRetries (default: 5) defines how many times a message is tried to be processed before is is moved to the *error queue* or passed to the [Second-Level Retries, SLR](how-do-i-handle-exceptions.md) mechanism.
 * ErrorQueue (default: error) sets the name of the queue where poison messages are sent to (including messages that failed *MaxRerties* number of times with SLR disabled and messages which cannot be processed at all, e.g. having unparsable or missing headers)

In V3 some of these setting were available via `MsqmTransportConfig` section with following 

 * In V3 the `ErrorQueue` (the queue where messages that fail a configured number of times) settings can be set both via the new `MessageForwardingInCaseOfFaultConfig ` section and the old `MsmqTransportConfig` section.
 * In V3 the `MaxRetries` as well as the throttling  (`NumberOfWorkerThreads`) settings can be set only via `MsmqTransportConfig` section.

