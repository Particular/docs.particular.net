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

From version 4 onwards these settings are configured via a transport connection string (named `NServiceBus/Transport` for all transports).

<!-- include MsmqTransportConnectionString -->

Before V4 some of these properties could be set via `MsmqMessageQueueConfig` configuration section

<!-- include MsmqTransportConnectionStringV4 -->

The connectionCache setting as well as ability to use non-trnasactional queues were not available prior to V4.

### Failure handling & throttling

NServiceBus is designed in such a way that a user does not have to care about exception handling. All the heavy lifting is done by the framework via a [two-level retries mechanism](how-do-i-handle-exceptions.md)

From V4 onwards the configuration for this mechanism is implemented in the `TransportConfig` section:

<!-- include TransportConfig -->

Some of these settings uses to be coupled to MSMQ becaouse they existed on `MsmqTransportConfig` configuation section before V4:

<!-- include MsmqTransportConfigV3 -->

 * In V3 the `ErrorQueue` (the queue where messages that fail a configured number of times) settings can be set both via the new `MessageForwardingInCaseOfFaultConfig ` section and the old `MsmqTransportConfig` section.
 * In V3 the `MaxRetries` as well as the throttling  (`NumberOfWorkerThreads`) settings can be set only via `MsmqTransportConfig` section.

### NServiceBus V3

The configuration section defines properties of the MSMQ transport. Read background on [MSMQ](msmq-information.md).

Example of `MsmqTransportConfig`:

```XML
<MessageForwardingInCaseOfFaultConfig ErrorQueue="error"/>
```

### ErrorQueue

Beginning with NServiceBus V3, use the configuration section to declare an error queue:

```XML
<section name="MessageForwardingInCaseOfFaultConfig" 
 type="NServiceBus.Config.MessageForwardingInCaseOfFaultConfig, NServiceBus.Core" />
```

To define the value:

```XML
<MsmqTransportConfig ErrorQueue="error" NumberOfWorkerThreads="1" MaxRetries="5"/>
```

The `ErrorQueue` in `MsmqTransportConfig` is for compatibility with earlier versions.

The `ErrorQueue` defines the name of the queue to which messages are transferred if they cannot be processed successfully. This may be a queue on the local machine or on a remote machine, in which case the value should be based on the template `queueName@remoteMachineName` where `queueName` is the name of the error queue (often "error") and
"remoteMachineName" is the name of the remote machine on which the error queue resides.

If no error queue is defined, NServiceBus fails to start with the exception: "Could not find backup configuration section 'MsmqTransportConfig' in order to locate the error queue."

Read more about [messages whose processing fails](how-do-i-handle-exceptions.md).

### NumberOfWorkerThreads

This property dictates the number of threads that receive messages from the input queue. This property has no impact on the number of threads that can use the bus to send or publish messages.

### MaxRetries

This property is related to the `ErrorQueue` property, defining the number of times to retry a message whose processing fails before it is moved to the error queue.

Default value: 5.

## NServiceBus V4: Changes to MsmqTransportConfig

The MsmqTransportConfig configuration section became obsolete in V4.0. Use the TransportConfig section instead:


```XML
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="MessageForwardingInCaseOfFaultConfig" type="NServiceBus.Config.MessageForwardingInCaseOfFaultConfig, NServiceBus.Core" />
    <section name="TransportConfig" type="NServiceBus.Config.TransportConfig, NServiceBus.Core"/>
  </configSections>

  <MessageForwardingInCaseOfFaultConfig ErrorQueue="error"/>
  <TransportConfig MaximumConcurrencyLevel="5" MaxRetries="2" MaximumMessageThroughputPerSecond="0"/>
 
</configuration>
```

**MaximumConcurrencyLevel** - The same as the `NumberOfWorkerThreads` property in `MsmqTransportConfig`.

**MaximumMessageThroughputPerSecond**  - Sets a limit on how quickly messages can be processed between all threads. Use a value of 0 to have no throughput limit. 

**MaxRetries** - Sets the First Level Retries (FLR) value that defines the number of times to retry a message whose processing fails before it is moved to the error queue, or, if configured, before the Second Level Retries (SLR) engine kicks in. The default value is 5.



