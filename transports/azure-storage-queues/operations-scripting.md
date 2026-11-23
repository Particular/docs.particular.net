---
title: Azure Storage Queues Transport Scripting
summary: Azure CLI scripts to create and delete queues, manage subscriptions, and create delayed delivery storage for the Azure Storage Queues transport.
component: ASQ
reviewed: 2026-11-23
related:
 - nservicebus/operations
redirects:
 - nservicebus/azure-storage-queues/operations-scripting
---

The following are example scripts to facilitate operations against the Azure Storage Queues transport.

## Requirements

These scripts require the Azure command line (Azure CLI) to be installed. For more information, refer to the [Azure CLI installation guide](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli).

To run the scripts against the Azure Storage Queues transport, a valid connection string to the Azure Storage account must be present. For additional information, refer to the document on how to [create a storage account](https://learn.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal).

For easier access, the connection string can be stored as an environment variable. The scripts on this page use PowerShell syntax.

```ps
$storageAccount = "<storage-account-name>"
$resourceGroup = "<resource-group-name>"

# get the connection string
$connectionString = az storage account show-connection-string -n $storageAccount -g $resourceGroup --query connectionString -o tsv

# set the connection string as an environment variable
$env:AZURE_STORAGE_CONNECTION_STRING = $connectionString
```

## Create Queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.

> [!NOTE]
> The Azure Storage Queues transport will fail to start if a queue name is longer than 63 characters. Refer to the [sanitization document](/transports/azure-storage-queues/sanitization.md) for more details.

```ps
# check if a queue exists
az storage queue exists -n "endpointname"

# create a queue
az storage queue create -n "endpointname"

# create the error queue
az storage queue create -n "error"
```

## Delete Queues

```ps
az storage queue delete -n "endpointname"
```

## Publish/Subscribe

The Azure Storage Queues transport implements [native publish/subscribe](/transports/azure-storage-queues/native-publish-subscribe.md) (pub/sub).


### Create the subscription routing table

The transport stores subscriptions in a subscription routing table that is shared by all endpoints and holds subscription information for each event type. The default table name is `subscriptions`. If the name is overridden with the `SubscriptionTableName` setting, use that name in the scripts below.

```ps
az storage table create -n "subscriptions"
```

### Create a subscription

When the subscriber endpoint subscribes to an event, an entity is created in the subscription routing table. When the publisher endpoint publishes an event, the subscription routing table is queried to find all of the subscribing endpoints.

The entity uses the full type name of the event as both `PartitionKey` and `Topic`, the subscriber endpoint name as both `RowKey` and `Endpoint`, and the subscriber's input queue as `Address`.

```ps
az storage entity insert --entity PartitionKey=MyEvent.Full.Type.Name RowKey=SubscriberEndpointName Address=subscriberendpointname Endpoint=SubscriberEndpointName Topic=MyEvent.Full.Type.Name --if-exists fail --table-name subscriptions
```

### Unsubscribe

To unsubscribe, delete the entity from the subscription routing table.

```ps
az storage entity delete --partition-key MyEvent.Full.Type.Name --row-key SubscriberEndpointName --table-name subscriptions --if-match "*"
```

## Delayed delivery

When delayed delivery is enabled at an endpoint, the transport creates a storage table to store the delayed messages.

By default, the storage table name is constructed using a naming scheme that starts with the word `delays` followed by a SHA-1 hash of the endpoint's name.

```ps
az storage table create -n "delays11f9578d05e6f7bb58a3cdd00107e9f4e3882671"
```

To ensure a single copy of delayed messages is dispatched by any endpoint instance, a blob container is used for leasing access to the delayed messages table.

The blob container name uses the same naming scheme as the storage table.

```ps
az storage container create -n "delays11f9578d05e6f7bb58a3cdd00107e9f4e3882671" --public-access off
```

Delayed messages storage table and container names can be overridden with a custom name. For more information, see [Azure Storage Queues Delayed Delivery](/transports/azure-storage-queues/delayed-delivery.md#how-it-works-overriding-tablecontainer-name).

