---
title: Discarding Old Messages
summary: Automatically discard messages if they have not been processed within a given period of time.
reviewed: 2025-09-25
component: Core
related:
 - nservicebus/operations/auditing
redirects:
 - nservicebus/how-do-i-discard-old-messages
---

Messages sent through the Particular Service Platform can be configured with a Time-To-Be-Received (TTBR) value. TTBR instructs the platform to automatically discard a message if it is not processed within a specified period, freeing up system resources and preventing outdated messages from being handled.

**Benefits of using TTBR:**
- Reduces processing of stale or irrelevant messages.
- Frees up resources in high-volume environments.
- Ensures only timely, valuable messages are processed.

TTBR applies only to messages that have not been handled. A message is considered handled if it has been successfully processed, moved to the error queue, or audited. The TTBR value is removed from handled messages to prevent message loss. You can inspect the original TTBR value in the `NServiceBus.TimeToBeReceived` [header](/nservicebus/messaging/headers.md).

If a message has no value after a certain period, specify a TTBR to allow it to be discarded.

## How to Set Time-To-Be-Received

You can specify a TTBR interval for a message in the following ways:

### Using an Attribute

Apply an attribute to the message class to set TTBR:

snippet: DiscardingOldMessagesWithAnAttribute

### Using a Custom Convention

Define a custom convention in code to set TTBR for specific messages:

snippet: DiscardingOldMessagesWithCode

## Clock synchronization considerations

When using TTBR, clock synchronization issues ([clock skew](https://en.wikipedia.org/wiki/Clock_skew)) between sender and receiver can cause premature message discard. If the receiver's clock is ahead of the sender's, messages may be rejected immediately as stale.

> [!INFO]
> **Frequently synchronize time between hosts**
> Ensure the endpoint hosts share the same clock source. These clocks should be very frequently synchronized to ensure reliable timestamps.

> [!WARNING]
> **Add clock skew safety margin for TTBR values under 1 minute**
> With opposing 30-second drifts per system, total skew reaches 60 seconds. Add safety margin.
> 
> Formula: TTBR = intended_lifetime + (2 × max_expected_drift)
> Example: 90s lifetime + 30s max drift = 150 seconds TTBR

## Discarding messages at startup

In certain situations, it may be required that messages in the incoming queue should not be processed after restarting the endpoint. This usually applies to development and test environments, but may also be appropriate for messages containing information that gets outdated or otherwise unneeded, e.g. change notifications, readings from sensors in IoT apps, etc.

> [!WARNING]
> It's not recommended to discard messages at startup in a production environment because it may lead to subtle message loss situations that can be hard to diagnose.

To discard all existing messages in the incoming queue at startup:

snippet: PurgeMessagesAtStartup

## Caveats

Time-To-Be-Received relies on the transport infrastructure to discard expired messages. As a result, runtime behavior is highly affected by the implementation in the different transports.

partial: msmq

### RabbitMQ transport

RabbitMQ checks the TTBR value, but only for message at the front of the queue. Expired messages are not removed from the queue, and their disk space is not reclaimed until they reach the front of the queue. Using TTBR as a disk-saving measure on RabbitMQ is recommended only when all messages in the queue use the same TTBR value. Otherwise, messages in front of the queue may prevent other stale messages from being cleaned.

### Azure transports

The Azure transports evaluate the TTBR for a message only when the message is requested by the client. Expired messages are not removed from the queue and their disk space will not be reclaimed until they reach the front of the queue and a consumer tries to read them. Using TTBR as a storage saving measure on the Azure transports is not a good choice for queues with long-lived messages like audit and forward.

### SQL transport

The SQL Server transport runs a periodic task that removes expired messages from the queue. The task is first executed when the endpoint starts and is subsequently scheduled to execute 5 minutes after the previous run when the task has been completed. Expired messages are not received from the queue and their disk space will be reclaimed when the periodic task executes.