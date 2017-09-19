---
title: Azure Storage Queues Delayed Delivery
component: ASQ
versions: '[7,)'
tags:
- Azure
related:
- persistence
- persistence/azure-storage
- persistence/azure-storage/performance-tuning
reviewed: 2017-07-17
---


In Versions 7.4 and above, the Azure Storage Queues transport no longer relies on the [timeout manager](/nservicebus/messaging/timeout-manager.md) to implement [delayed delivery](/nservicebus/messaging/delayed-delivery.md). Instead, the transport creates infrastructure for queuing which can delay messages using native Storage features. Storage account provided for queuing is used to implement delayed delivery.


## How it works

When an endpoint is started, the transport creates a storage table and a storage container to provide the necessary infrastructure to support delayed messages. When a message needs to be delayed, it will be stored by the transport in a storage table. To ensure a single copy of delayed messages are dispatched by any endpoint instance, a storage container is used for leasing access to the delayed messages table.

By default, storage table and storage container names are constructed using naming schema that starts with the word `delays` followed by SHA-1 hash of the endpoint's name. For example, `delays2fd4e1c67a2d28fced849ee1bb76e7391b93eb12` where `2fd4e1c67a2d28fced849ee1bb76e7391b93eb12` is a SHA-1 hash of an endpoint name. 


### Overriding table/container name

Delayed messages table and container names can be overridden with a custom name:

snippet: delayed-delivery-override-name


### Disabling delayed delivery

Delayed delivery can be turned off to disable unnecessary Azure Storage table polling. Delayed delivery should **not** be turned off if any of the following features are required:

- Deferred messages
- Saga timeouts
- Delayed retries

snippet: delayed-delivery-disabled


## Backwards compatibility

When upgrading to a version of the transport that supports delayed delivery natively, it is safe to operate a combination of native-delay and non-native-delay endpoints at the same time. Endpoints supporting native delayed delivery can send delayed messages to endpoints that are not yet aware of the native delay infrastructure. These endpoints can continue to receive delayed messages from non-native endpoints as well.


### Disabling the timeout manager

To assist with the upgrade process, the timeout manager is still enabled by default, so any delayed messages already stored in the endpoint's persistence database before the upgrade will be sent when their timeouts expire. Any delayed messages sent after the upgrade will be sent through the delay infrastructure even though the timeout manager is enabled.

Once an endpoint has no more delayed messages in its persistence database, there is no more need for the timeout manager. It can be disabled by calling:

snippet: delayed-delivery-disable-timeoutmanager

At this point, the `-timeouts` and `-timeoutsdispatcher` queues for the endpoint can be deleted from the storage account. In addition, the endpoint no longer requires timeout persistence, so those storage tables can be removed from the persistence database as well.