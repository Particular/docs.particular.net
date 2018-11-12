---
title: Transports
summary: An overview of the transports available in NServiceBus
reviewed: 2018-11-01
redirects:
 - nservicebus/nservicebus-and-websphere-sonic
 - nservicebus/transports
related:
 - transports/selecting
 - transports/types
 - samples/custom-transport
---

NServiceBus contains an abstraction for underlying queueing technologies. An implementation of that abstraction for a given queueing technology is known as a "transport".

The transport abstraction enables business to build systems with the Particular Service Platform using existing queueing technologies. Sometimes the choice of queueing technology may be constrained be constrained by the business. At other times the best queueing technology can be selected according to requirements. The transport abstraction allows more time to be spent delivering features and less time to be spent dealing with the details of a specific queueing technology.

The documentation related to transports describes the differences between them, and contains guidelines for deciding which transport may best suit a specific scenario. For each transport, the documentation describes how to make optimal usage of NServiceBus in combination with that transport. This includes how to deal with concurrency, transactions, scaling, and more.

## Supported transports

- [Learning](/transports/learning/)
- [MSMQ](/transports/msmq)
- [Azure Service Bus (current)](/transports/azure-service-bus-netstandard/)
- [Azure Service Bus (legacy)](/transports/azure-service-bus/)
- [Azure Storage Queues](/transports/azure-storage-queues/)
- [SQL Server](/transports/sql/)
- [RabbitMQ](/transports/rabbitmq/)
- [Amazon SQS](/transports/sqs/)

There is also a WebSphereMQ transport but it is not supported by Particular Software at this time. The code is available [on GitHub](https://github.com/ParticularLabs/NServiceBus.WebSphereMQ) as-is, for legacy, community use and reference. [Contact Particular Software](https://particular.net/contactus) for licensing.

## Community-maintained transports

There are several community-maintained transports which can be found in the full list of [extensions](/components#transports).


## How to select a transport

Initially, it's challenging to decide which queueing technology may be best for a specific scenario. See the [guide to selecting a transport](/transports/transports-selecting) for help in making that decision.
