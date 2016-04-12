---
title: Azure Service Bus
summary: Using Azure ServiceBus as transport
tags:
- Azure
- Cloud
redirects:
 - nservicebus/using-azure-servicebus-as-transport-in-nservicebus
 - nservicebus/azure/azure-servicebus-transport
related:
 - samples/azure/azure-service-bus
---

In some environments it is not possible or recommended to rely heavily on distributed transactions to ensure reliability and consistency. Therefore in environments such as very large cloud networks or hybrid networks using MSMQ is not the best idea. In those scenarios a good alternative is Azure Service Bus.

NOTE: As part of the Azure support for NServiceBus, one can choose between two transports provided by the Azure platform Azure Storage Queues and Azure Service Bus. Each of these two options has separate features, capabilities, and usage characteristics. A detailed comparison and discussion of when to select which is beyond the scope of this document. To help decide which option best suits the application's needs, review the Azure article [Azure Queues and Azure Service Bus Queues - Compared and Contrasted](https://azure.microsoft.com/en-us/documentation/articles/service-bus-azure-and-service-bus-queues-compared-contrasted/).

[Azure Service Bus (ASB)](https://azure.microsoft.com/en-us/services/service-bus/) is a messaging service (broker) that sits between applications, allowing them to exchange messages in a loosely coupled fashion for improved scale and resiliency. Service Bus Queues offer simple first in, first out guaranteed message delivery and support a range of standard protocols (REST, AMQP, WS*) and APIs (to put/pull messages on/off the queue). Service Bus Topics deliver messages to multiple subscribers and easily use fan-out pattern to deliver messages to downstream systems.

 * The main advantage of ASB is that it offers a highly reliable and (relatively) low latency remote messaging infrastructure. A single message can be up to 256 KB in size, and a queue can keep many messages at once, up to 5 GB size in total. Furthermore it is capable of emulating local transactions using its queue peek-lock mechanism. NServiceBus is an abstraction over ASB. It takes advantage of ASB's built-in features, such as message deduplication and deferred messages, and provides higher-level, convenient API for programmers on top of ASB.
 * The main disadvantage of ASB is its dependency on TCP (for low latency), which may require opening some outbound ports on the firewall. Additionally, in some systems the price may get high ($1 per million messages).

Note: Publish/Subscribe and Timeouts (including message deferral) are supported natively by the ASB transport and do not use persistence.


## Enabling the Transport

Firstly, choose Standard or Premium Messaging Tier for Azure Service Bus when creating the namespace at Azure portal.

Secondly, reference `NServiceBus.Azure.Transports.WindowsAzureServiceBus` NuGet package.

```
PM> Install-Package NServiceBus.Azure.Transports.WindowsAzureServiceBus
```

Then use the Configuration API to set up NServiceBus, by specifying `.UseTransport<T>()` to override the default transport:

snippet:AzureServiceBusTransportWithAzure


## Setting the Connection String

The default way to set the connection string is using the .NET provided `connectionStrings` configuration section in app.config or web.config, with the name `NServicebus\Transport`:

snippet:AzureServiceBusConnectionStringFromAppConfig

For more details refer to [Configuration Connection Strings](https://azure.microsoft.com/en-us/documentation/articles/service-bus-dotnet-how-to-use-topics-subscriptions/#set-up-a-service-bus-connection-string) document.