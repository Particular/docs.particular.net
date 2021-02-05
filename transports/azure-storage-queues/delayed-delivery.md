---
title: Azure Storage Queues Delayed Delivery
component: ASQ
versions: '[7,)'
related:
- persistence
- persistence/azure-table
- persistence/azure-table/performance-tuning
reviewed: 2019-10-01
---


In Versions 7.4 and above, the Azure Storage Queues transport no longer relies on the [timeout manager](/nservicebus/messaging/timeout-manager.md) to provide [delayed delivery](/nservicebus/messaging/delayed-delivery.md). Instead, the transport uses the configured storage to provide delayed delivery without needing an external persister.


## How it works

When an endpoint is started, the transport creates a storage table to store the delayed messages. To ensure a single copy of delayed messages is dispatched by any endpoint instance, a blob container is used for leasing access to the delayed messages table.

By default, the storage table and blob container names are constructed using a naming scheme that starts with the word `delays` followed by SHA-1 hash of the endpoint's name. For example, `delays2fd4e1c67a2d28fced849ee1bb76e7391b93eb12` where `2fd4e1c67a2d28fced849ee1bb76e7391b93eb12` is a SHA-1 hash of an endpoint name.


### Overriding table/container name

Delayed messages table and container names can be overridden with a custom name:

snippet: delayed-delivery-override-name

partial: disabling

NOTE: When making use of the table name override, make sure the table is unique per endpoint and not shared across multiple endpoints.

## Backwards compatibility

When upgrading to a version of the transport that supports delayed delivery natively, it is safe to run with both native-delay and non-native-delay endpoints at the same time. Endpoints supporting native delayed delivery can send delayed messages to endpoints that are not yet aware of the native delay infrastructure. These endpoints can continue to receive delayed messages from non-native endpoints as well.


partial: disable-timeout-manager