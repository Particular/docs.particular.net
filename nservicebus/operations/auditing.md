---
title: Auditing Messages
summary: Configure where to send messages and it provides built-in message auditing for every endpoint.
component: Core
reviewed: 2017-10-06
tags:
 - Audit
related:
 - nservicebus/operations
 - nservicebus/messaging/headers
 - nservicebus/messaging/discard-old-messages
redirects:
 - nservicebus/auditing-with-nservicebus
---

The distributed nature of parallel, message-driven systems makes them more difficult to debug than their sequential, synchronous and centralized counterparts. For these reasons, NServiceBus provides built-in message auditing for every endpoint. Configure NServiceBus to audit and it will capture a copy of every received message and forward it to a specified audit queue.

NOTE: By default, auditing is not enabled and must be configured for each endpoint where auditing is required.

It is recommended to specify a central auditing queue for all related endpoints (i.e. endpoints that belong to the same system). This gives an overview of the entire system in one place and can help see how messages correlate. One can look at the audit queue as a central record of everything that is happening in the system. A central audit queue is also required by the Particular Service Platform and especially [ServiceControl](/servicecontrol), which consumes messages from these auditing queues. For more information, see [ServicePulse documentation](/servicepulse/).


## Message headers

The audited message is enriched with additional headers, which contain information related to processing the message:

 * Processing start and end times.
 * Processing host id and name.
 * Processing machine address.
 * Processing endpoint.


## Handling Audit messages

Audit messages can be handled in a variety of ways: Save them in a database, do custom logging, etc. It is important not to leave the messages in the audit queue however, as most queuing technologies have upper-bound limits on their queue sizes and depth. By not processing these messages, the limits of the underlying queue technology may be reached.


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


## Configuring auditing

partial: configuration

## Filtering audit messages

By default all messages are audited, to control what message types that gets audited see our [audit filter sample](/nservicebus/audit-filter/).

### Filtering out individual properties

Auditing works by sending an exact copy of the received message to the audit queue so filtering out individual propeties is not supported.

Property filtering requirements usually comes from properties either containing sensitive information like credit card numbers and passwords or them being large in size like videos and images.

For sensitive properties we recommend property [encryption](/nservicebus/security/property-encryption) and for large properties we recommend to [stream them instead](/samples/pipeline/stream-properties/) to avoid including the actual payload in the audited message.
