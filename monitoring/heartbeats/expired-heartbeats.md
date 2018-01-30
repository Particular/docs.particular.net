---
title: Expired heartbeat messages
summary:
reviewed: 2018-01-26
component: Heartbeats
versions: 'Heartbeats:*'
---


Heartbeat messages have a time to be received (TTBR) set based on the time to live (TTL) value.  If ServiceControl does not consume the heartbeat messages before the TTBR expires then those messages may be discarded. Transports like MSMQ and Azure Service Bus support dead letter queues (DLQ) and the expired heartbeat messages can be explicitly configured to be forwarded to the DLQ instead of being discarded.


### MSMQ

Although NServiceBus configures the use of DLQ by default, messages that are defined with TTBR will not be automatically forwarded to the DLQ and will be discarded. Configuration can be specified to [override this behavior](/transports/msmq/dead-letter-queues.md#enabling-dlq-for-messages-with-ttbr) so that these messages can be forwarded to the DLQ.

WARNING: When using NServiceBus Versions 6.1 or below, messages will be forwarded to the DLQ even if TTBR is set on the messages.  To avoid this behavior, DLQ can be disabled by configuring the [MSMQ connection strings](/transports/msmq/connection-strings.md).  The heartbeat messages will be forwarded to the DLQ when ServiceControl is either stopped or very busy. In this case, the dead letter queue needs to be monitored and cleaned up.


### Azure Service Bus

Forwarding messages with expired TTL to DLQ is a configuration that needs to be set on the destination queue which is the ServiceControl queue for the heartbeat messages. In order to forward the heartbeat messages after the TTBR expiration to DLQ, ServiceControl needs to be [explicitly configured](/transports/azure-service-bus/configuration/azureservicebusqueueconfig.md) by specifying `EnableDeadLetteringOnMessageExpiration`.
