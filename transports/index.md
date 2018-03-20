---
title: Transports
reviewed: 2016-10-26
redirects:
 - nservicebus/nservicebus-and-websphere-sonic
 - nservicebus/transports
related:
 - samples/custom-transport
---

NServiceBus is built on top of existing queuing technologies. In NServiceBus the choice of queuing technology is referred to as a "Transport".


## Types of transports

NServiceBus transports can be divided into several categories.


### Bus transports

Bus transports are inherently distributed. Each endpoint instance might potentially connect to a different node of the bus. Messages are routed transparently between the nodes but physical routing layer needs to contain information to which bus node particular endpoint is connected to.

Bus transports include:

 * [MSMQ](/transports/msmq)
 * [Learning Transport](/transports/learning/)


### Broker transports

Broker transports are inherently centralized. Even if there are multiple servers, they act as a single logical entity that hosts all the queues (and/or topics).

Broker transports include:

 * [Azure Service Bus](/transports/azure-service-bus/)
 * [Azure Storage Queues](/transports/azure-storage-queues/)
 * [SQL Server](/transports/sql/)
 * [RabbitMQ](/transports/rabbitmq/)


### Unicast-only transports

Unicast-only transports do not have a notion of topic; only queues. Because of that they allow only point-to-point communication. Sending message to multiple receivers (e.g. publishing an event) is composed of multiple transport-level  sends. Unicast-only transports require a subscription storage to be configured.

Unicast-only transports include:

 * [Azure Storage Queues](/transports/azure-storage-queues/)
 * [MSMQ](/transports/msmq/)
 * [SQL Server](/transports/sql/)
 * [Amazon SQS](/transports/sqs/)

### Multicast-enabled transports

Multicast-enabled transports have some notion of a topic or a similar concept that allows to send a message once and have it received by multiple clients. These transports do not require a subscription storage.

Multicast-enabled transports include:

 * [Learning Transport](/transports/learning/)
 * [Azure Service Bus](/transports/azure-service-bus/)
 * [RabbitMQ](/transports/rabbitmq/)


## Community run transports

There are several community run transports that can be seen on the full list of [Extensions](/components#transports).


## WebSphereMQ

WebSphereMQ Transport for NServiceBus is not supported by Particular Software at this time. The code is available [on GitHub](https://github.com/ParticularLabs/NServiceBus.WebSphereMQ) as-is, for legacy, community use and reference. [Contact support](https://particular.net/contactus) for licensing and support details.
