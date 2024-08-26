---
title: Auditing Messages
summary: Configure where to send messages and it provides built-in message auditing for every endpoint.
component: Core
reviewed: 2021-08-20
related:
 - nservicebus/operations
 - nservicebus/messaging/headers
 - nservicebus/messaging/discard-old-messages
redirects:
 - nservicebus/auditing-with-nservicebus
---

> [!NOTE]
> Auditing is required for optimum functionality of the platform tools. ServicePulse can work without auditing enabled but will show incorrect retry states on messages, and ServiceInsight will only show failures instead of complete conversations.

The distributed nature of parallel, message-driven systems makes them more difficult to debug than their sequential, synchronous and centralized counterparts. For these reasons, NServiceBus provides built-in message auditing for every endpoint. Configure NServiceBus to audit and it will capture a copy of every received message and forward it to a specified audit queue.

> [!NOTE]
> By default, auditing is not enabled and must be configured for each endpoint where auditing is required.

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

There two settings that control auditing:

### Queue Name

The queue name to forward audit messages.

### Time-to-be-received

The [Time-to-be-received (TTBR)](/nservicebus/messaging/discard-old-messages.md) for audit messages can be overriden.

> [!NOTE]
> What happens to messages for which the TTBR expires is different. Most transports will **not** auto deleted expired message in the backlog of the queue and purge when dequeued and are received by the client. More details at [discarding old messages behavior](/nservicebus/messaging/discard-old-messages.md#behavior)

#### Default Value

#if-version [, 6.0)

Until NServiceBus version 6.0 the TTBR of the original message will be used.

#end-if

#if-version [6.0, )

Audit message since NServiceBus 6.x by default will have no TTBR set.

#end-if

#### Override TimeToBeReceived

The TimeToBeReceived (TTBR) on audit messages can be set using the following configuration syntax:

snippet: audit-ttbr-override

Note that while the phrasing is "forwarding a message" in the implementation it is actually "cloning and sending a new message". This is important when considering TimeToBeReceived since the time taken to receive and process the original message is not part of the TimeToBeReceived of the new audit message. In effect the audit message receives the full time allotment of whatever TimeToBeReceived is used.


## Filtering audit messages

When auditing is enabled, all messages are audited by default. To control which message types are audited, see the [audit filter sample](/samples/pipeline/audit-filtering/).

### Filtering individual properties

Auditing works by sending an exact copy of the received message to the audit queue, so filtering out individual properties is not supported.

For sensitive properties, e.g. credit card numbers or passwords, use [message property encryption](/nservicebus/security/property-encryption.md). For large properties, consider the [data bus feature](/nservicebus/messaging/databus/) to avoid including the actual payload in the audited message.

## Additional audit information

Additional information can be added to audit messages using a [custom behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) as shown in the following snippet. The additional data will be contained in the audit message headers.

snippet: AddAuditData

> [!NOTE]
> Message headers count towards the message size and the additional audit information has to honor the transport's message header limitations.

partial: custom-audit-action
