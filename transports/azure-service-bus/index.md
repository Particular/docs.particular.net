---
title: Azure Service Bus Transport
component: ASBS
related:
 - samples/azure-service-bus-netstandard/send-reply
 - samples/azure-service-bus-netstandard/native-integration
 - samples/azure-service-bus-netstandard/native-integration-pub-sub
 - samples/azure-service-bus-netstandard/options
 - samples/azure-service-bus-netstandard/send-receive-with-nservicebus
 - samples/azure-service-bus-netstandard/topology-migration
reviewed: 2024-05-17
---

The Azure Service Bus transport leverages the [Azure.Messaging.ServiceBus](https://www.nuget.org/packages/Azure.Messaging.ServiceBus/) client library for .NET.

[Azure Service Bus](https://azure.microsoft.com/en-us/services/service-bus/) is a messaging service hosted on the Azure platform that allows for exchanging messages between various applications in a loosely coupled fashion. The service offers guaranteed message delivery and supports a range of standard protocols (e.g. REST, AMQP, WS*) and APIs such as native pub/sub, delayed delivery, and more.

## Transport at a glance

|Feature                    |   |
|:---                       |---
|Transactions |None, ReceiveOnly, SendsWithAtomicReceive
|Pub/Sub                    |Native
|Timeouts                   |Native
|Large message bodies       | with Premium tier or data bus
|Scale-out             |Competing consumer
|Scripted Deployment        |Supported using `NServiceBus.Transport.AzureServiceBus.CommandLine`
|Installers                 |Optional
|Native integration         |[Supported](native-integration.md)
|Case Sensitive             |No

> [!NOTE]
> The Azure Service Bus transport only supports the Standard and Premium tiers of the Microsoft Azure Service Bus service. Premium tier is recommended for production environments.
>
> The Azure Service Bus transport is not compatible with the [Azure Service Bus emulator](https://learn.microsoft.com/en-us/azure/service-bus-messaging/overview-emulator) because the emulator [doesn't support on-the-fly management operations](https://learn.microsoft.com/en-us/azure/service-bus-messaging/overview-emulator#known-limitations) through the client-side SDK, which prevents operations like creating queues or subscribing to events.
