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

[Delayed delivery](/nservicebus/messaging/delayed-delivery) allows deferring the actual sending of a message with a scheduled delivery in the future.
This mechanism is supported by the NServiceBus and is part of the [persistence](/nservicebus/persistence). Native delayed delivery provided for Azure Storage Queues replaces this mechanism. Instead of delegating the delayed delivery to the persistence, it stores messages directly in the Azure Storage Tables of the account, that is configured for the transport. This eliminates a lot of overhead and results in a smaller number of storage transactions, lowering both the costs and the time needed to register and dispatch delayed messages. 

## Configuration

Configuration of the native delayed delivery requires just a single step

snippet:AzureStorageQueueUseNativeDelivery

## Azure tables

Native delayed delivery stores deferred messages in a table passed as the parameter to the configuring method. Messages are partitioned by their delivery date and time (hour precision). Once a message is dispatched, the entity is removed from the table.

## Scale Out and Azure Object Storage

It is possible that to increase the throughput of an endpoint it will be scaled out to multiple instances. To lower the possibility of duplicates, only one instance should be dispatching delayed messages. This is done by creating a blob container and acquiring a lease on it. As only one lease can be acquired for a container at the same time, only the endpoint that successfully acquired it, is able to proceed with dispatching delayed messages.