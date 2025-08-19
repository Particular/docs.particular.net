---
title: Azure messaging
summary: Describes the Azure messaging options available for the Particular Service Platform
reviewed: 2025-07-18
callsToAction: ['solution-architect', 'poc-help']
---

Azure offers [several messaging services](https://learn.microsoft.com/en-us/azure/service-bus-messaging/compare-messaging-services), each built for specific purposes. The Particular Service Platform supports [Azure Service Bus and Azure Storage Queues](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-azure-and-service-bus-queues-compared-contrasted). Additionally the Particular Platform supports using [Azure SQL Database](https://azure.microsoft.com/en-us/products/azure-sql/database) tables as message queues.

## Azure Service Bus

[Azure Service Bus](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messaging-overview) is a fully-fledged enterprise messaging service with support for queuing, publish/subscribe, and more advanced integration patterns. It is designed to integrate applications or application components that may span multiple communication protocols, data contracts, trust domains, or network environments.

Azure Service Bus has two pricing tiers: [Standard and Premium](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-premium-messaging). The Premium tier provides hardware-level isolation, which means each customer's workload runs on dedicated hardware. Premium tier uses _messaging units_ for scaling and pricing and guarantees predictable throughput and performance. The Standard tier offers pay-as-you-go pricing with quota-based throughput and non-guaranteed latency. Microsoft recommends running production systems on the Premium tier.

:heavy_plus_sign: Pros:

- Supports transactional message operations across multiple queues and topics (cross-entity transactions)
- Up to 100 MB message size on the Premium tier
- Scripted infrastructure deployment using the [NServiceBus ASB CLI](/transports/azure-service-bus/operational-scripting.md)
- Supports the AMQP 1.0 protocol over TCP and WebSockets

:heavy_minus_sign: Cons:

- Some features are only available on the Premium tier
- The maximum message size is 256 KB on the Standard tier
- Cross-entity transactions are limited to 100 messages
- No cross-namespace communication
- Emulator for local development and testing [does not have required features](/transports/azure-service-bus/#transport-at-a-glance) to work with the Particular Service Platform.

[**Try the Azure Service Bus Tutorial →**](https://learn.microsoft.com/en-us/azure/service-bus-messaging/build-message-driven-apps-nservicebus?tabs=Sender)

### When to use Azure Service Bus

Azure Service Bus should be considered the default messaging choice for Azure. Alternatives should be considered only when the message size limitations are not sufficient or if a more portable queueing technology is required.

## Azure Queue Storage

[Azure Queue Storage](https://learn.microsoft.com/en-us/azure/storage/queues/storage-queues-introduction) offers a simple, low-cost, and minimal messaging service based on the Azure Storage infrastructure.

:heavy_plus_sign: Pros:

- Queues can contain up to 200 TB of messages
- Cheaper than Azure Service Bus
- Azure Storage Emulator for local development and testing

:heavy_minus_sign: Cons:

- The maximum message size is 64 KB
- Higher latency due to HTTP communication
- No cross-entity transactions
- The maximum number of requests (e.g. message delivery) per storage account is 20,000 per second (2,000 per second for single queues)

[**Try the Azure Storage Queues sample →**](/samples/azure/storage-queues/)

### When to use Azure Storage Queues

Azure Service Bus is recommended as the default messaging choice for Azure. Consider Azure Storage Queues when a managed message queuing technology is required for simple messaging needs or when Azure Service Bus doesn't meet critical requirements.

## SQL transport

SQL transport is an NServiceBus feature that can use existing SQL Server databases as feature-complete message queues.

:heavy_plus_sign: Pros:

- Runs on infrastructure which often already exists
- Strong transaction integration with business data operations
- Runs on cloud-hosted and on-premises SQL Server-compatible data stores (including SQL Server Express for development, testing, and CI)
- Arbitrary message sizes
- Allows for [exactly-once processing](https://particular.net/blog/what-does-idempotent-mean) if business data and message data are in the same database
- Ease of backup and recovery as business data and messages are backed up in the same database

:heavy_minus_sign: Cons:

- Not an actual message queue
- More expensive and laborious to scale
- Impacts overall database performance
- Lower message throughput compared to specialized message queuing technologies

[**Try the SQL transport sample →**](/samples/sqltransport/simple/)

### When to use SQL transport

Consider using SQL transport if an existing application already uses a SQL Server-compatible data store and requires high transactional consistency between message and business data operations, but very high message throughput or performance is not a priority. SQL transport can be a good stepping-stone when introducing messaging into an existing system without the introduction of new infrastructure.

## Other services

Azure offers other messaging services focused on asynchronous communication. These services are not directly supported by the Particular Service Platform, however they might be combined in combination with supported message queueing systems to handle [data distribution](/architecture/data-distribution.md) needs by more specialized technologies.

- [Azure Event Grid](https://learn.microsoft.com/en-us/azure/event-grid/overview) is an integration focused messaging system using the publish-subscribe model. Azure Event Grid can ingest and distribute events provided by applications, other Azure Services or IoT devices using HTTP protocols (MQTT support in public preview). By default, Event Grid uses a [push-model](https://learn.microsoft.com/en-us/azure/event-grid/push-delivery-overview) to forward events to all subscribers.
- [Azure Event Hubs](https://learn.microsoft.com/en-us/azure/event-hubs/event-hubs-about) is a data streaming platform specialized in large-scale event ingestion and data distribution. While data streaming platforms like Azure Event Hubs or Apache Kafka use shared or similar terminologies, they differ significantly from traditional message queuing systems and are designed to solve a different set of problems.   [Read more about the differences between event streaming and message queues](https://particular.net/blog/lets-talk-about-kafka).

## Additional resources

- [Storage queues and Service Bus queues—compared and contrasted](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-azure-and-service-bus-queues-compared-contrasted)
- [Azure Service Bus quotas](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quotas)
- [Scalability and performance targets for Queue Storage](https://learn.microsoft.com/en-us/azure/storage/queues/scalability-targets)
- [Events, Data Points, and Messages—Choosing the right Azure messaging service](https://azure.microsoft.com/en-us/blog/events-data-points-and-messages-choosing-the-right-azure-messaging-service-for-your-data/)
