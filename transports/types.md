---
title: Transport types
summary: A list of all transports offered by the Particular Service Platform
component: Core
reviewed: 2025-08-13
---

NServiceBus transports can be divided into several categories:

- Federated transports
- Broker transports
- Unicast-only transports
- Multicast-enabled transports

A description of each appears in the corresponding category below.

## Federated transports

Federated transports are inherently distributed. Each endpoint instance may connect to a different node of the queuing technology. Messages are routed transparently between the nodes, but the physical routing layer must contain information about which node a particular endpoint is connected to.

Federated transports include:

- [Learning](/transports/learning/)
- [MSMQ](/transports/msmq/)

## Broker transports

Broker transports are inherently centralized. Even if there are multiple servers, there is a single logical instance that hosts all the queues (and/or topics/exchanges).

Broker transports include:

- [Azure Service Bus](/transports/azure-service-bus/)
- [Azure Storage Queues](/transports/azure-storage-queues/)
- [SQL Server](/transports/sql/)
- [RabbitMQ](/transports/rabbitmq/)

## Unicast-only transports

Unicast-only transports do not have the notion of topics, exchanges, or similar concepts; only queues. Because of this, they allow only point-to-point communication. Sending a message to multiple receivers (e.g., publishing an event) involves multiple transport-level sends. Unicast-only transports require subscription storage via [NServiceBus persistence](/persistence).

Unicast-only transports include:

- [MSMQ](/transports/msmq/)
- [SQL Server version 4 and below](/transports/sql/)
- [Amazon SQS version 4 and below](/transports/sqs/)
- [Azure Storage Queues version 9 and below](/transports/azure-storage-queues/)

## Multicast-enabled transports

Multicast-enabled transports have some notion of topics, exchanges, or similar concepts, which allow sending a message once and having it received by multiple clients. These transports do not require subscription storage.

Multicast-enabled transports include:

- [Learning](/transports/learning/)
- [Azure Service Bus](/transports/azure-service-bus/)
- [RabbitMQ](/transports/rabbitmq/)
- [SQL Server version 5 and above](/transports/sql/)
- [Amazon SQS version 5 and above](/transports/sqs/)
- [Azure Storage Queues version 10 and above](/transports/azure-storage-queues/)