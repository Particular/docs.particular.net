---
title: Native Integration
summary: Native integration with Azure Service Bus
component: ASB
reviewed: 2017-05-05
tags:
- Azure
related:
 - nservicebus/operations
redirects:
 - nservicebus/azure-service-bus/native-integration
---

This document provides guidance on how to integrate NServiceBus endpoints with non-NServiceBus endpoints by sharing an Azure Service Bus (ASB) namespace as mutual communication channel.


### Attention points

The following points need to be taken into account when integrating

 1. Both the ASB SDK and the transport have some assumptions about the format of exchanged `BrokeredMessage` instances. Refer to [Brokered Message Creation](brokered-message-creation.md) to learn more about these assumptions and how to align the sending and receiving endpoints at the wire level.
 1. The transport assumes a specific layout of ASB entities depending on the selected topology; any non-NServiceBus endpoint is expected to use the correct entities for each purpose. In general, the following rule applies: queues are for sending, and topics are for publishing. To learn more about the layouts of the built in topologies, refer to [Azure Service Bus Transport Topologies](/transports/azure-service-bus/topologies/).
 1. By default, the transport creates its own entities when they don't exist in the namespace yet. But non-NServiceBus endpoints may require manual creation of entities. Refer to [the operational scripting guide](operational-scripting.md) for more information on available ASB SDK's and tools to perform these tasks.
 1. Also, refer to [this guide](operational-scripting.md) to learn more about the native ASB SDK's to send and receive messages.


### See it in action

[The following sample](/samples/azure/native-integration-asb/) shows how to use the native integration capabilities of the Azure Service Bus transport between an NServiceBus endpoint and a regular .NET application.
