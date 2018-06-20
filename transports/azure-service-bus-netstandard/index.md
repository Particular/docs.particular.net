---
title: Azure Service Bus .Net Standard Transport
component: ASBS
tags:
 - Azure
reviewed: 2018-06-19
---

## Azure Service Bus .Net Standard Transport

The Azure Service Bus .Net Standard transport, ASBS in short, is the replacement of to the [Azure Service Bus transport](transports/azure-service-bus/) which leverages the .Net Standard compatible [Microsoft.Azure.ServiceBus](https://www.nuget.org/packages/Microsoft.Azure.ServiceBus/) client SDK.

[Azure Service Bus (ASB)](https://azure.microsoft.com/en-us/services/service-bus/) is a messaging service hosted on the Azure platform, that allows for exchanging messages between various applications in a loosely coupled fashion. ASB Queues offer <a href="https://en.wikipedia.org/wiki/FIFO_(computing_and_electronics)">"First In, First Out" (FIFO)</a> guaranteed message delivery, and support a range of standard protocols (REST, AMQP, WS*) and APIs (to put messages on and pull messages off the queue). ASB Topics deliver messages to multiple subscribers and facilitate use of the fan-out pattern to deliver messages to downstream systems.

## Configuring an endpoint

To use Azure Service Bus .Net Standard transport as the underlying transport:

snippet: asbs-config-basic

NOTE: The Azure Service Bus .Net Standard transport requires a connection string to connect to a namespace.
