---
title: Azure Service Bus Transport (Legacy)
component: ASB
redirects:
 - nservicebus/using-azure-servicebus-as-transport-in-nservicebus
 - nservicebus/azure/azure-servicebus-transport
 - nservicebus/azure-service-bus
related:
 - samples/azure-service-bus-netstandard/asbs-asb-side-by-side
reviewed: 2020-06-08
---

include: legacy-asb-warning

[Azure Service Bus (ASB)](https://azure.microsoft.com/en-us/services/service-bus/) is a messaging service hosted on the Azure platform, that allows for exchanging messages between various applications in a loosely coupled fashion. ASB Queues offer <a href="https://en.wikipedia.org/wiki/FIFO_(computing_and_electronics)">"First In, First Out" (FIFO)</a> guaranteed message delivery, and support a range of standard protocols (REST, AMQP, WS*) and APIs (to put messages on and pull messages off the queue). ASB Topics deliver messages to multiple subscribers and facilitate use of the fan-out pattern to deliver messages to downstream systems.

NServiceBus is an abstraction over ASB. It takes advantage of ASB's built-in features, such as message batching and deferred messages. It also provides a higher-level, convenient API for programmers on top of ASB.

Note: Publish/Subscribe and Timeouts (including message deferral) are supported natively by the ASB transport and do not require NServiceBus persistence.

 * The main advantage of ASB is that it offers a highly reliable and low latency remote messaging infrastructure. A single message can be up to 256 KB in size (1 MB for Premium), and a queue can store many messages at once, up to 5 GB size in total. Furthermore, it is capable of emulating local transactions using its queue [peek-lock mechanism](https://docs.microsoft.com/en-us/rest/api/servicebus/peek-lock-message-non-destructive-read).
 * The main disadvantage of ASB is its dependency on TCP (for low latency), which may require opening outbound ports on the firewall. Additionally, in some systems the price for the service (at the per message level) may be significant.


include: azure-transports

## Transport at a glance

|Feature                    |   |  
|:---                       |---
|Transactions |None, ReceiveOnly, SendsWithAtomicReceive
|Pub/Sub                    |Native
|Timeouts                   |Native
|Large message bodies       |via higher tier (e.g. Premium) or data bus
|Scale-out             |Competing consumer
|Scripted Deployment        | Not supported
|Installers                 |Optional


## Enabling the Transport

When creating the namespace at the Azure portal, choose Standard or Premium Messaging Tier for Azure Service Bus.

Then at configuration time set ASB as the transport:

snippet: AzureServiceBusTransportWithAzure


## Setting the Connection String

For more details on setting up connection strings and securing them, refer to the [Configuration Connection Strings](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-how-to-use-topics-subscriptions) and the [Securing Credentials](/transports/azure-service-bus/legacy/securing-connection-strings.md) articles.

To set the connection string use the following:

partial: code-connection


### Via App.Config

snippet: AzureServiceBusConnectionStringFromAppConfig
