---
title: Native Integration with Azure Service Bus
summary: How to integrate NServiceBus endpoints with non-NServiceBus endpoints on Azure Service Bus.
component: ASB
reviewed: 2021-05-12
related:
 - nservicebus/operations
redirects:
 - nservicebus/azure-service-bus/native-integration
 - transports/azure-service-bus/native-integration
---

include: legacy-asb-warning

This document provides guidance on how to integrate NServiceBus endpoints with non-NServiceBus endpoints by sharing an Azure Service Bus (ASB) namespace as mutual communication channel.


### Attention points

The following points need to be taken into account when integrating

1. Both the ASB SDK and the transport make assumptions about the format of exchanged `BrokeredMessage` instances. Refer to [Brokered Message Creation](brokered-message-creation.md) to learn about these assumptions and how to align the sending and receiving endpoints at the wire level.

1. The transport assumes a specific layout of ASB entities depending on the selected topology; any non-NServiceBus endpoint is expected to use the correct entities for each purpose. In general, the following rule applies: queues are for sending, and topics are for publishing. To learn more about the layouts of the built in topologies, refer to [Azure Service Bus Transport Topologies](/transports/azure-service-bus/legacy/topologies.md).

1. By default, the transport creates its own entities when they don't exist in the namespace. But non-NServiceBus endpoints may require manual creation of entities. Refer to the [Azure Service Bus documentation](https://docs.microsoft.com/en-us/azure/service-bus-messaging/) for more information on available ASB SDKs and tools to perform these tasks.

1. The native message must allow NServiceBus to [detect the message type either via the headers or the message payload](/nservicebus/messaging/message-type-detection.md).