---
title: Expired heartbeat messages
summary: NServiceBus endpoints send heartbeat messages with TTBR; expired messages are discarded if unconsumed
reviewed: 2026-06-29
component: Heartbeats
versions: 'Heartbeats:*'
---

Heartbeat messages have a [time to be received (TTBR)](/nservicebus/messaging/discard-old-messages.md) set based on the [time to live (TTL)](/monitoring/heartbeats/install-plugin.md#time-to-live-ttl) value, which defaults to four times the heartbeat interval. If ServiceControl does not consume the heartbeat messages before the TTBR expires then those messages may be discarded. Transports like MSMQ and Azure Service Bus support dead letter queues (DLQ) and the expired heartbeat messages can be explicitly configured to be forwarded to the DLQ instead of being discarded.

### MSMQ

Although NServiceBus configures the use of DLQ by default, messages that are defined with TTBR will not be automatically forwarded to the DLQ and will be discarded. Configuration can be specified to [override this behavior](/transports/msmq/dead-letter-queues.md#enabling-dlq-for-messages-with-ttbr) so that these messages can be forwarded to the DLQ.

### Azure Service Bus

Forwarding messages with expired TTL to DLQ is a configuration that needs to be set on the destination queue, which is the ServiceControl queue for the heartbeat messages. In order to forward the heartbeat messages after the TTBR expiration to DLQ, ServiceControl needs to be explicitly configured by specifying `EnableDeadLetteringOnMessageExpiration`.
