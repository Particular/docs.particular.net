---
title: Native delayed delivery
component: ASQ
versions: '[7.1,]'
tags:
- Azure
- Cloud
- Delayed delivery
- ASQ
- Azure Storage Queues
reviewed: 2016-10-21
---

[Delayed delivery](/nservicebus/messaging/delayed-delivery) enables sending messages that are delivered at a later time. 
This mechanism is supported by the NServiceBus core and is part of the [persistence](/nservicebus/persistence). Native delayed delivery provided for Azure Storage Queues overrides this behavior and instead of delegating the delayed delivery to the persistence, stores messages directly in the Azure Storage Tables of the account, that is configured for the transport. This eliminates a lot of overhead and results in lowered number of storage transactions, lowering both the costs and the time needed to register and dispatch delayed messages. Additionally, in Scale Out scenarios, native delayed delivery ensures that only one timeout poller will be running at the same time. This greatly lowers the possibility of duplicated messages.

## Configuration

Configuration of the native delayed delivery requires just a single step

snippet:AzureStorageQueueUseNativeDelivery

## Simple table storage

Native delayed delivery stores deferred messages in a table passed as the parameter to the configuration table. Messages are partitioned by their delivery date and time (hour precision). Once a message is dispatched, the entity is removed from the table.

## Single timeout poller

The component responsible for dispatching messages is called a timeout poller. It's important to ensure that only one poller is active at the same time for a scaled out endpoint. Native delayed delivery does it by creating a blob container and acquiring a lease. As only one lease can be acquired for a container at the same time, only the poller that successfully acquired it proceeds with dispatching messages.