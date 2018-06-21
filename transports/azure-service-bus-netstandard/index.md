---
title: Azure Service Bus .Net Standard Transport
component: ASBS
tags:
 - Azure
reviewed: 2018-06-21
---

## Azure Service Bus .Net Standard Transport

The Azure Service Bus .NET Standard transport (ASBS) leverages the .NET Standard [Microsoft.Azure.ServiceBus](https://www.nuget.org/packages/Microsoft.Azure.ServiceBus/) client SDK.

[Azure Service Bus (ASB)](https://azure.microsoft.com/en-us/services/service-bus/) is a messaging service hosted on the Azure platform, that allows for exchanging messages between various applications in a loosely coupled fashion. The service offers guaranteed message delivery, and support a range of standard protocols (REST, AMQP, WS*) and APIs such as native pub/sub, delayed delivery, and more.

## Configuring an endpoint

To use Azure Service Bus .Net Standard transport as the underlying transport:

snippet: azure-service-bus-for-dotnet-standard

NOTE: The Azure Service Bus .NET Standard transport requires a connection string to connect to a namespace.
