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
reviewed: 2026-05-05
---

The Azure Service Bus transport leverages the [Azure.Messaging.ServiceBus](https://www.nuget.org/packages/Azure.Messaging.ServiceBus/) client library for .NET.

[Azure Service Bus](https://azure.microsoft.com/en-us/services/service-bus/) is a messaging service hosted on the Azure platform that allows for exchanging messages between various applications in a loosely coupled fashion. The service offers guaranteed message delivery and supports a range of standard protocols (e.g. REST, AMQP, WS*) and APIs such as native pub/sub, delayed delivery, and more.

## Transport at a glance

|Feature                    |   |
|:---                       |---
|Transactions |None, ReceiveOnly, SendsAtomicWithReceive
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

## Azure Service Bus Emulator

The Azure Service Bus transport works with the [Azure Service Bus emulator](https://learn.microsoft.com/en-us/azure/service-bus-messaging/overview-emulator). To use the emulator, your [connection string](https://learn.microsoft.com/en-us/azure/service-bus-messaging/test-locally-with-service-bus-emulator?tabs=automated-script#choosing-the-right-connection-string) must include `UseDevelopmentEmulator=true;`

### Known Emulator Limitations

The emulator is only suitable for small systems, since it contains limitations on connections and queue size. For anything larger, a full Azure Service Bus instance will be required.

The current [known limitations](https://learn.microsoft.com/en-us/azure/service-bus-messaging/overview-emulator#known-limitations) that affect functionality with the Particular Service Platform are:

- A limit of 10 connections. Each NServiceBus endpoint will use a connection for sending and a connection for receiving, effectively using 2 connections for a full endpoint.
- `MaxDeliveryCount` is fixed at 10.

