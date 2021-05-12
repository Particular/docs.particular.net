---
title: Native Integration with Azure Service Bus
summary: How to integrate NServiceBus endpoints with non-NServiceBus endpoints on Azure Service Bus.
component: ASBS
related:
 - samples/azure-service-bus-netstandard/native-integration
reviewed: 2021-05-12
---

This document provides guidance on how to integrate NServiceBus endpoints with non-NServiceBus endpoints by sharing an Azure Service Bus (ASB) namespace as a mutual communication channel.


### Attention points

The following points must be taken into account when integrating

1. Both the ASB SDK and the transport make assumptions about the format of exchanged `Message` instances.

1. The transport assumes a specific layout of ASB entities; any non-NServiceBus endpoint is expected to use the correct entities for each purpose. In general, the following rule applies: queues are for sending, and topic are for publishing. To learn more about the layouts of the built in topology, refer to [Azure Service Bus Transport Topology](/transports/azure-service-bus/topology.md).

1. By default, the transport creates its own entities when they don't exist in the namespace. But non-NServiceBus endpoints may require manual creation of entities. Refer to the [Azure Service Bus documentation](https://docs.microsoft.com/en-us/azure/service-bus-messaging/) for more information on available ASB SDKs and tools to perform these tasks.

1. The native message must allow NServiceBus to [detect the message type either via the headers or the message payload](/nservicebus/messaging/message-type-detection.md).


### See it in action

[This sample](/samples/azure-service-bus-netstandard/native-integration/) shows how to use the native integration capabilities of the Azure Service Bus transport between an NServiceBus endpoint and a regular .NET application.