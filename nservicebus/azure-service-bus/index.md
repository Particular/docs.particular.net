---
title: Azure Service Bus Transport
summary: Using Azure Service Bus as transport
tags:
- Azure
- Cloud
redirects:
 - nservicebus/using-azure-servicebus-as-transport-in-nservicebus
 - nservicebus/azure/azure-servicebus-transport
related:
 - samples/azure/azure-service-bus
---

[Azure Service Bus (ASB)](https://azure.microsoft.com/en-us/services/service-bus/) is a messaging service hosted on the Azure platform, that allows for exchanging messages between various applications in a loosely coupled fashion. ASB Queues offer ["First In, First Out" (FIFO)](https://en.wikipedia.org/wiki/FIFO_(computing_and_electronics)) guaranteed message delivery, and support a range of standard protocols (REST, AMQP, WS*) and APIs (to put/pull messages on/off the queue). ASB Topics deliver messages to multiple subscribers and allow to use fan-out pattern to deliver messages to downstream systems.

NServiceBus is an abstraction over ASB. It takes advantage of ASB's built-in features, such as message batching and deferred messages. It also provides higher-level, convenient API for programmers on top of ASB.

Note: Publish/Subscribe and Timeouts (including message deferral) are supported natively by the ASB transport and do not require NServiceBus persistence.

 * The main advantage of ASB is that it offers a highly reliable and low latency remote messaging infrastructure. A single message can be up to 256 KB in size, and a queue can store many messages at once, up to 5 GB size in total. Furthermore, it is capable of emulating local transactions using its queue [peek-lock mechanism](https://msdn.microsoft.com/en-us/library/azure/hh780722.aspx). 
 * The main disadvantage of ASB is its dependency on TCP (for low latency), which may require opening outbound ports on the firewall. Additionally, in some systems the price for the service may get high ($1 per million messages).

NOTE: As part of the Azure support for NServiceBus, one can choose between two transports provided by the Azure platform: Azure Storage Queues and Azure Service Bus. Each of these two options has separate features, capabilities, and usage characteristics. A detailed comparison and discussion of when to select which is beyond the scope of this document. To help decide which option best suits the application's needs, review the Azure article [Azure Queues and Azure Service Bus Queues - Compared and Contrasted](https://azure.microsoft.com/en-us/documentation/articles/service-bus-azure-and-service-bus-queues-compared-contrasted/).


## Enabling the Transport

When creating the namespace at the Azure portal, choose Standard or Premium Messaging Tier for Azure Service Bus. 

In the solution reference `NServiceBus.Azure.Transports.WindowsAzureServiceBus` NuGet package.

```
PM> Install-Package NServiceBus.Azure.Transports.WindowsAzureServiceBus
```

Then use the Configuration API to set up NServiceBus, by specifying `.UseTransport<AzureServiceBus>()` to override the default transport:

snippet:AzureServiceBusTransportWithAzure


## Setting the Connection String

The default way to set the connection string is using `.ConnectionString()` configuration API or the .NET provided `connectionStrings` configuration section in an `app.config` or a `web.config` file, with the name `NServicebus\Transport`:

snippet:setting_asb_connection_string

or

snippet:AzureServiceBusConnectionStringFromAppConfig

For more details on setting up connection strings and securing them, refer to the [Configuration Connection Strings](https://azure.microsoft.com/en-us/documentation/articles/service-bus-dotnet-how-to-use-topics-subscriptions/#set-up-a-service-bus-connection-string) and the [Securing Credentials](/nservicebus/azure-service-bus/securing-connection-strings.md) articles.