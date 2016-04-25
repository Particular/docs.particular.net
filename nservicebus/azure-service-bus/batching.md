---
title: Azure Service Bus Transport Batching
summary: Batching with Azure Service Bus and how it works
component: ASB
tags:
- Cloud
- Azure
- Transports
reviewed: 2016-04-26
---

## Batching store access

To increase entities throughput, Azure Service Bus batches messages prior to writing to its internal store. Write message operations are delayed by 20ms and that helps to increase throughput. Read operations are not affected by this setting. By default batching store access is enabled. To change the default, set `EnableBatching` on [queues](/nservicebus/azure-service-bus/configuration/configuration.md#Queues), [topics](/nservicebus/azure-service-bus/configuration/configuration.md#Topics), and [subscriptions](/nservicebus/azure-service-bus/configuration/configuration.md#Subscriptions).

For low throughput scenarios with required low latency, batching store operations should be disabled.


## Client-side batching

Client-side batching allows to delay sending messages to a queue or a topic for a short period of time. Within that period of time, if additional messages where sent to the same queue or topics, messages will be transmitted in a single batch. This setting affects `BatchFlushInterval` of the Azure Service Bus and configured using [`BatchFlushInterval`](/nservicebus/azure-service-bus/configuration/configuration.md#Messaging-Factories). `BatchFlushInterval` is enabled by default.


## Batching messages sent from a handler

Azure Service Bus optimizes multiple message sends by batching send operations into a single operation. For example, when a handler is sending multiple messages, Azure Service Bus will batch those and send in as few operations as it can. Size of a batch cannot exceed the maximum size on a `BrokeredMessage`. The transport allows to configure what the maximum message size should be used. To configure message maximum size, use [`MaximumMessageSizeInKilobytes`](/nservicebus/azure-service-bus/configuration/configuration.md#Message-Senders) setting of the Message Senders.

NOTE: `BrokeredMessage` size is different between [tiers](https://azure.microsoft.com/en-us/documentation/articles/service-bus-premium-messaging/) of Azure Service Bus. 

When batching messages sent from a handler, the underlying implementation of batching serializes messages. Serialized batch is usually bigger than the original messages combined together. To ensure successful send of the batch, it has to be under the `BrokeredMessage` size. To estimate if a batch is not exceeding the maximum size, `MessageSizePaddingPercentage` is used. The default is set to 5%. To change percentage value, use [`MessageSizePaddingPercentage`](/nservicebus/azure-service-bus/configuration/configuration.md#Message-Senders) configuration of the Message Senders.