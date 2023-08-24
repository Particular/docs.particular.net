---
title: Azure messaging
summary:
reviewed: 2023-07-18
callsToAction: ['solution-architect', 'poc-help']
---

Azure offers [several messaging services](https://learn.microsoft.com/en-us/azure/service-bus-messaging/compare-messaging-services), each built for specific purposes. The Particular Service Platform supports [Azure Service Bus and Azure Queue Storage](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-azure-and-service-bus-queues-compared-contrasted).

### Azure Service Bus

[Azure Service Bus](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messaging-overview) supports queuing, publish/subscribe, and more advanced integration patterns. It is designed to integrate applications or application components that may span multiple communication protocols, data contracts, trust domains, or network environments.

Azure Service Bus has two pricing tiers: [Standard and Premium](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-premium-messaging). The Premium tier provides hardware level isolation, which means each customer's workload runs on dedicated hardware. Premium tier uses _messaging units_ for pricing. The Standard tier offers pay-as-you-go pricing with less predictable latency and throughput. Microsoft recommends running production systems on the Premium tier.

:heavy_plus_sign: Pros:

- Up to 100 MB message size on the Premium tier
- Supports transactions across multiple queues
- Scripted infrastructure deployment using the [NServiceBus ASB CLI](/transports/azure-service-bus/operational-scripting.md)
- Supports the AMQP 1.0 protocol
- Supports cross-entity transactions

:heavy_minus_sign: Cons:

- Some features are only available on the Premium tier
- The maximum message size is 256 KB on the Standard tier
- The maximum queue size is 80 GB on the Standard tier
- There is no on-premises equivalent.

[**Try the Azure Service Bus Tutorial →**](https://learn.microsoft.com/en-us/azure/service-bus-messaging/build-message-driven-apps-nservicebus?tabs=Sender)

#### When to use Azure Service Bus

Azure Service Bus may be considered the default messaging choice for Azure. Alternatives should be considered only when the message size limitations are not sufficient or if a more portable queueing technology is required.

### Azure Queue Storage

[Azure Queue Storage](https://learn.microsoft.com/en-us/azure/storage/queues/storage-queues-introduction) offer a simple, low-cost, and easy-to-use messaging service based on the Azure Storage infrastructure.

:heavy_plus_sign: Pros:

- Queues can contain up to 200 TB of messages
- Cheaper than Azure Service Bus
- Part of Azure Storage
- The Azure Storage Emulator may be on-premises for development, testing, and CI

:heavy_minus_sign: Cons:

- The maximum message size is 64 KB
- Higher latency due to HTTP communication
- No cross-entity transactions
- The maximum number of requests (e.g. message delivery) per storage account is 20,000 per second (2,000 per second for single queues)

[**Try the Azure Storage Queues sample →**](/samples/azure/storage-queues/)

#### When to use Azure Storage Queues

Consider Azure Storage Queues when a managed message queuing technology is required but Azure Service Bus doesn't meet requirements or is too expensive.

### SQL transport

SQL transport is an NServiceBus feature that can use existing SQL Server-compatible databases as feature-complete message queues.

:heavy_plus_sign: Pros:

- Runs on cloud hosted and on-premises SQL Server-compatible data stores (including SQL Server Express for development, testing, and CI)
- Arbitrary message sizes
- Runs on infrastructure which often already exists
- Strong transaction integration with business data operations

:heavy_minus_sign: Cons:

- More expensive and laborious to scale
- Impacts overall database performance
- Lower message throughput compared to specialized message queuing technologies

[**Try the SQL transport sample →**](/samples/azure/storage-queues/)

#### When to use SQL transport

Consider SQL transport if an existing application already uses a SQL Server-compatible data store and limited amount of messaging is being introduced. SQL transport can be a good stepping-stone when introducing messaging into an existing system without the introduction of new infrastructure.

## Other services

Azure offers other messaging services focused on asynchronous communication. These services are not directly support by the Particular Platform, however they might be combined in combination with supported message queueing systems to handle [data distribution](/nservicebus/concepts/data-distribution.md) needs by more specialized technologies.

* [Azure Event Grid](https://learn.microsoft.com/en-us/azure/event-grid/overview) is an integration focused messaging system using the publish-subscribe model. Azure Event Grid can ingest and distribute events provided by applications, other Azure Services or IoT devices using HTTP protocols (MQTT support in public preview). By default, Event Grid uses a [push-model](https://learn.microsoft.com/en-us/azure/event-grid/push-delivery-overview) to forward events to all subscribers.
* [Azure Event Hubs](https://learn.microsoft.com/en-us/azure/event-hubs/event-hubs-about) is a data streaming platform specialized in large-scale event ingestion and data distribution. While data streaming platforms like Azure Event Hubs or Apache Kafka use shared or similar terminologies, they differ significantly from traditional message queuing systems and are designed to solve a different set of problems.   [Read more about the differences between event streaming and message queues](https://particular.net/blog/lets-talk-about-kafka).


## Additional Resources

- [Storage queues and Service Bus queues - compared and contrasted](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-azure-and-service-bus-queues-compared-contrasted)
- [Azure Service Bus quotas](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quotas)
- [Scalability and performance targets for Queue Storage](https://learn.microsoft.com/en-us/azure/storage/queues/scalability-targets)
- [Events, Data Points, and Messages – Choosing the right Azure messaging service](https://azure.microsoft.com/en-us/blog/events-data-points-and-messages-choosing-the-right-azure-messaging-service-for-your-data/)
