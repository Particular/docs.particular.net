---
title: Azure Service Bus .NET Standard Transport
component: ASBS
tags:
 - Azure
related:
 - samples/azure-service-bus-netstandard
reviewed: 2018-07-23
---

## Azure Service Bus .NET Standard transport

The Azure Service Bus .NET Standard transport (ASBS) leverages the .NET Standard [Microsoft.Azure.ServiceBus](https://www.nuget.org/packages/Microsoft.Azure.ServiceBus/) client SDK.

[Azure Service Bus (ASB)](https://azure.microsoft.com/en-us/services/service-bus/) is a messaging service hosted on the Azure platform that allows for exchanging messages between various applications in a loosely coupled fashion. The service offers guaranteed message delivery and supports a range of standard protocols (e.g. REST, AMQP, WS*) and APIs such as native pub/sub, delayed delivery, and more.

## Configuring an endpoint

To use Azure Service Bus .NET Standard transport as the underlying transport:

snippet: azure-service-bus-for-dotnet-standard

NOTE: The Azure Service Bus .NET Standard transport requires a connection string to connect to a namespace.
