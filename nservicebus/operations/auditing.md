---
title: Auditing Messages
summary: Configure where to send messages and it provides built-in message auditing for every endpoint.
component: Core
reviewed: 2019-07-22
related:
 - nservicebus/operations
 - nservicebus/messaging/headers
 - nservicebus/messaging/discard-old-messages
redirects:
 - nservicebus/auditing-with-nservicebus
---

NOTE: Auditing is required for optimum functionality of the platform tools. ServicePulse can work without auditing enabled but will show incorrect retry states on messages, and ServiceInsight will only show failures instead of complete conversations.

The distributed nature of parallel, message-driven systems makes them more difficult to debug than their sequential, synchronous and centralized counterparts. For these reasons, NServiceBus provides built-in message auditing for every endpoint. Configure NServiceBus to audit and it will capture a copy of every received message and forward it to a specified audit queue.

NOTE: By default, auditing is not enabled and must be configured for each endpoint where auditing is required.

It is recommended to specify a central auditing queue for all related endpoints (i.e. endpoints that belong to the same system). This gives an overview of the entire system in one place and can help see how messages correlate. One can look at the audit queue as a central record of everything that happened in the system. A central audit queue is also required by the Particular Service Platform and especially [ServiceControl](/servicecontrol), which consumes messages from these auditing queues. For more information, see [ServicePulse documentation](/servicepulse/).

## Performance impact

Enabling auditing has an impact on storage and network resources.

### Message throughput

Enabling auditing on all endpoints results in doubling the global message throughput. That can sometimes be troublesome in high message volume environments.

In such environments it might make sense to only enable auditing on relevant endpoints that need auditing.

### Queue storage capacity

Auditing succesfully processed message can result in storing huge amounts of message data. First in the audit queue, second in any application that will store these messages. If the audit storage logic stops processing audit messages the audit queue size can grow very fast. Messaging infrastructure has a size limit on the amount of data that can be stored in a queue. If this storage limit is reached messages can no longer be processed in all the endpoints that have auditing enabled.

Make sure the size limit is increased to allow for scheduled and unscheduled downtime.

### Audit store capacity

Perform capacity planning on the store where messages will be written.

When using ServiceControl is it advised read [ServiceControl capacity planning](/servicecontrol/capacity-and-planning.md).


## Message headers

The audited message is enriched with additional headers, which contain information related to processing the message:

 * Processing start and end times.
 * Processing host id and name.
 * Processing machine address.
 * Processing endpoint.


## Handling Audit messages

Audit messages can be handled in a variety of ways: Save them in a database, do custom logging, etc. It is important not to leave the messages in the audit queue however, as most queuing technologies have upper-bound limits on their queue sizes and depth. By not processing these messages, the limits of the underlying queue technology may be reached.


## Configuring auditing

partial: configuration


## Audit configuration options

There two settings that control auditing:


### Queue Name

The queue name to forward audit messages to


### OverrideTimeToBeReceived

To force a [TimeToBeReceived](/nservicebus/messaging/discard-old-messages.md) on audit messages by setting `OverrideTimeToBeReceived` use the configuration syntax below.

Note that while the phrasing is "forwarding a message" in the implementation it is actually "cloning and sending a new message". This is important when considering TimeToBeReceived since the time taken to receive and process the original message is not part of the TimeToBeReceived of the new audit message. In effect the audit message receives the full time allotment of whatever TimeToBeReceived is used.

{{Warning: MSMQ forces the same TimeToBeReceived on all messages in a transaction. Therefore, OverrideTimeToBeReceived is not supported when using the [MSMQ Transport](/transports/msmq/). If OverrideTimeToBeReceived is detected when using MSMQ an exception will be thrown with the following text:

```
Setting a custom OverrideTimeToBeReceived for audits is not supported on transactional MSMQ
```
}}


#### Default Value

If no OverrideTimeToBeReceived is defined then:

**Versions 5 and below**: TimeToBeReceived of the original message will be used.

**Versions 6 and above**: No TimeToBeReceived will be set.


## Filtering audit messages

When auditing is enabled, all messages are audited by default. To control which message types are audited, see the [audit filter sample](/samples/pipeline/audit-filtering/).

### Filtering individual properties

Auditing works by sending an exact copy of the received message to the audit queue, so filtering out individual properties is not supported.

For sensitive properties, e.g. credit card numbers or passwords, use [message property encryption](/nservicebus/security/property-encryption.md). For large properties, consider [streaming](/samples/pipeline/stream-properties/) them instead to avoid including the actual payload in the audited message.


partial: additional-data