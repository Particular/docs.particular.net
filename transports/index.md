---
title: Transports
summary: An overview of the transports available in NServiceBus
reviewed: 2025-08-13
redirects:
 - nservicebus/nservicebus-and-websphere-sonic
 - nservicebus/transports
---

NServiceBus provides an abstraction for underlying queuing technologies. An implementation of that abstraction for a given queuing technology is known as a "transport".

This abstraction enables businesses to build systems with the Particular Service Platform using existing queuing technologies. Sometimes, the choice of queuing technology may be constrained by business needs. At other times, the best queuing technology can be selected according to the application's requirements. The transport abstraction allows teams to spend more time delivering business features and less time dealing with the details of a specific queuing technology.

The transport documentation describes the differences between transports and provides guidelines for deciding which transport may best suit a specific scenario. For each transport, the documentation explains how to make optimal use of NServiceBus in combination with that transport, including how to handle concurrency, transactions, scaling, and more.

## How to select a transport

Initially, it can be challenging to decide which queuing technology is best for a specific scenario. See the [guide to selecting a transport](selecting.md) for help in making that decision.

## Supported transports

- [Azure Service Bus](/transports/azure-service-bus/)
- [Azure Storage Queues](/transports/azure-storage-queues/)
- [Amazon SQS](/transports/sqs/)
- [RabbitMQ](/transports/rabbitmq/)
- [SQL Server](/transports/sql/)
- [PostgreSQL](/transports/postgresql/)
- [MSMQ](/transports/msmq)
- [Learning](/transports/learning/)
