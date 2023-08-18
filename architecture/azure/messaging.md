---
title: Azure Messaging Options
summary:
reviewed: 2023-07-18
---

Azure offers [several messaging services](https://learn.microsoft.com/en-us/azure/service-bus-messaging/compare-messaging-services), each built for specific purposes. The Particular Service Platform supports Azure Service Bus and Azure Queue Storage.

### Azure Service Bus

Azure Service Bus supports queuing, publish/subscribe, and more advanced integration patterns. They're designed to integrate applications or application components that may span multiple communication protocols, data contracts, trust domains, or network environments.

Azure Service Bus has two pricing tiers: Standard and Premium. Premium tier provides hardware level isolation to run customer workload in isolation. Premium tier is using _messaging units_ for pricing. Standard tier offers pay-as-you-go pricing with less predictable latency and throughput behaviors. Azure recommends to run production systems on the Premium tier. [Read more about Azure Service Bus tiers](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-premium-messaging).

<https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-azure-and-service-bus-queues-compared-contrasted>

:heavy_plus_sign: Pros:

- Up to 100 MB message size on Premium tier
- Supports transactions across multiple queues
- Script infrastructure deployment using the [NServiceBus ASB CLI](/transports/azure-service-bus/operational-scripting.md)
- Supports AMQP 1.0 protocol
- Supports cross-entity transactions

:heavy_minus_sign: Cons:

- Some features require Premium tier of Azure Service Bus
- Maximum message size is 256 KB on Standard tier
- Maximum queue size of 80 GB on Standard tier
- No ability to run Azure Service Bus outside the Azure Platform.

[Try the Azure Service Bus Tutorial](https://learn.microsoft.com/en-us/azure/service-bus-messaging/build-message-driven-apps-nservicebus?tabs=Sender)

#### When to use Azure Service Bus

Consider Azure Service Bus as the default messaging option on Azure. Alternatives should be considered when the message size limitations are not sufficient or if a more portable queueing technology is required.

### Azure Storage Queues

Azure Storage Queues offer a simple, low-cost, and easy-to-use messaging service based on the Azure Storage infrastructure.

:heavy_plus_sign: Pros:

- Queues can contain up to 200 TB of messages
- Cheaper than Azure Service Bus
- Part of Azure Storage
- Azure Storage Emulator allows local usage of Azure Storage Queues for development and testing

:heavy_minus_sign: Cons:

- Maximum message size is 64 KB
- Higher service latency due to HTTP-based communication
- No cross-entity transactions
- Maximum request rate per storage account: 20,000 messages (up to 2,000 message per second for single queues)

[Try the Azure Storage Queues sample](/samples/azure/storage-queues/)

#### When to use Azure Storage Queues

Consider Azure Storage Queues when a managed message queuing technology is required and Azure Service Bus doesn't meet the requirements or is considered too expensive.

#### References

- [Scalability and performance targets for Queue Storage](https://learn.microsoft.com/en-us/azure/storage/queues/scalability-targets)

### SQL Transport

SQL transport is an NServiceBus feature that can use existing MSSQL databases as feature complete message queues.

:heavy_plus_sign: Pros:

- Runs on cloud hosted and on-premises MSSQL Servers (including SQL Server Express for testing and development)
- Can handle arbitrary message sizes
- Can run on already existing infrastructure
- Strong transaction integration with business data operations

:heavy_minus_sign: Cons:

- More expensive and laborious to scale
- Impacts overall database performance
- Lower message throughput compared to specialized message queuing technologies

#### When to use SQL Transport

Consider SQL Transport if an existing application already using MS SQL Server is freshly adopting messaging in very limited capacity.

### Other messaging services

Azure offers additional message-oriented services like Azure Event Hubs and Azure Event Grid. While not directly supported by NServiceBus, these services have their own specialized use-cases:

- Azure Event Grid can be used to subscribe to discrete events from other Azure Services or 3rd party publishers.
- Azure Event Hubs is a data ingestion and streaming services focused on processing large amounts of messages per second.

Refer to the [Microsoft documentation](https://learn.microsoft.com/en-us/azure/service-bus-messaging/compare-messaging-services) for further information about these services.
