---
title: Transports
summary: An overview of the transports available in NServiceBus
reviewed: 2023-11-07
redirects:
 - nservicebus/nservicebus-and-websphere-sonic
 - nservicebus/transports
---

NServiceBus contains an abstraction for underlying queuing technologies. An implementation of that abstraction for a given queuing technology is known as a "transport".

The transport abstraction enables businesses to build systems with the Particular Service Platform using existing queuing technologies. Sometimes the choice of queuing technology may be constrained by the needs of the business. At other times the best queuing technology can be selected according to the requirements of the application. The transport abstraction allows teams to spend more time delivering business features and less time dealing with the details of a specific queuing technology.

The transport documentation describes differences between transports and contains guidelines for deciding which transport may best suit a specific scenario. For each transport, the documentation describes how to make optimal usage of NServiceBus in combination with that transport. This includes how to deal with concurrency, transactions, scaling, and more.

## How to select a transport

Initially, it's challenging to decide which queuing technology may be best for a specific scenario. See the [guide to selecting a transport](selecting.md) for help in making that decision.

## Supported transports

- [Azure Service Bus](/transports/azure-service-bus/)
- [Azure Storage Queues](/transports/azure-storage-queues/)
- [Amazon SQS](/transports/sqs/)
- [RabbitMQ](/transports/rabbitmq/)
- [SQL Server](/transports/sql/)
- [PostgreSQL](/transports/postgresql/)
- [MSMQ](/transports/msmq)
- [Learning](/transports/learning/)
