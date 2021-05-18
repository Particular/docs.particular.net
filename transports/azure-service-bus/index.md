---
title: Azure Service Bus Transport
component: ASBS
related:
 - samples/azure-service-bus-netstandard/send-reply
 - samples/azure-service-bus-netstandard/native-integration
 - samples/azure-service-bus-netstandard/lock-renewal
 - samples/azure-service-bus-netstandard/native-integration-pub-sub
reviewed: 2021-05-18
---

The Azure Service Bus transport leverages the .NET Standard [Microsoft.Azure.ServiceBus](https://www.nuget.org/packages/Microsoft.Azure.ServiceBus/) client SDK.

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

NOTE: The Azure Service Bus transport only supports Standard and Premium tiers of the Microsoft Azure Service Bus service. Premium tier is recommended for production environments.

## Configuring an endpoint

To use Azure Service Bus as the underlying transport:

snippet: azure-service-bus-for-dotnet-standard

NOTE: The Azure Service Bus transport requires a connection string to connect to a namespace.
