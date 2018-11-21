---
title: Transports
summary: An overview of the transports available in NServiceBus
reviewed: 2018-11-01
redirects:
 - nservicebus/nservicebus-and-websphere-sonic
 - nservicebus/transports
related:
 - transports/transports-deciding
 - transports/transports-types
 - samples/custom-transport
---

NServiceBus is an abstraction built on top of existing queuing technologies. These queuing technologies are referred to as a "Transport" with NServiceBus.

The documentation related to transports will help with a better understanding of what the differences are between the transports and decide which one best suits a specific scenario. And after a transport has been decided, how to make optimal usage of NServiceBus in combination with that transport. Including how to deal with concurrency, transactions, scaling, etc.

## Different Transports

The different transports that NServiceBus supports are

- [Learning Transport](/transports/learning/)
- [MSMQ](/transports/msmq)
 * [Azure Service Bus](/transports/azure-service-bus-netstandard/)
 * [Azure Service Bus [Legacy]](/transports/azure-service-bus/)
- [Azure Storage Queues](/transports/azure-storage-queues/)
- [SQL Server](/transports/sql/)
- [RabbitMQ](/transports/rabbitmq/)
- [Amazon SQS](/transports/sqs/)

### Community-maintained transports

There are several community-maintained transports which can be found in the full list of [extensions](/components#transports).


#### WebSphereMQ

WebSphereMQ Transport for NServiceBus is not supported by Particular Software at this time. The code is available [on GitHub](https://github.com/ParticularLabs/NServiceBus.WebSphereMQ) as-is, for legacy, community use and reference. [Contact support](https://particular.net/contactus) for licensing and support details.

## How to select a transport

When new to message queuing, it can be a challenge to decide which technology is the right one for a specific scenario. For this reason there is [comprehensive documentation](/transports/transports-selecting) to make an educated decision.