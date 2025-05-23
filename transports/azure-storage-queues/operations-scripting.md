---
title: ASQ Transport Scripting
summary: Example scripts to facilitate operational actions against the Azure Storage Queue Transport.
component: ASQ
reviewed: 2025-05-14
related:
 - nservicebus/operations
redirects:
 - nservicebus/azure-storage-queues/operations-scripting
---

The following are example scripts to facilitate operations against the Azure Storage Queue Transport.

## Requirements

These scripts require the Azure command line (Azure CLI) to be installed. For more information refer to the [Azure CLI installation guide](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli).

For the scripts to be run against the Azure Storage Queue transport, a  valid connection string to the Azure Storage account must be present. For additional information, refer to the document on how to [create a storage account](https://learn.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal)

For easier access, the connection string can be stored as an environment variable.

```
#get the connection string,
$connectionString=az storage account show-connection-string -n $storageAccount -g $resourceGroup --query connectionString -o tsv

#set conn string as env variable
$env:AZURE_STORAGE_CONNECTION_STRING = $connectionString
```

## Create Queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.

> [!NOTE]
> The Azure Storage Queues Transport will fail to start if a queue name is longer than 63 characters. Refer to the [sanitization document](/transports/azure-storage-queues/sanitization.md) for more details.

```
#check if queue exists
az storage queue exists -n "endpointname"

#create a queue
az storage queue create -n "endpointname"

#create a error queue
az storage queue create -n "error"
```

## Delete Queues

```
az storage queue delete -n "endpointname"
```

## Publish/Subscribe

Azure Storage Queue transport implements the publish/subscribe (pub/sub) pattern.


#### Create the subscription routing table

In a pub/sub pattern, the transport creates a dedicated subscription routing table shared by all endpoints, which holds subscription information for each event type.

```
az storage table create -n "subscriptions"
```

#### Create a subscription

When the subscriber endpoint subscribes to an event, an entity is created in the subscription routing table. When the publisher endpoint publishes an event, the subscription routing table is queried to find all of the subscribing endpoints.

```
az storage entity insert --entity PartitionKey=MyEvent.Full.Type.Name RowKey=SubscriberEndpointName  Address=subscriber Endpoint=SubscriberEndpointName  Topic=MyEvent.Full.Type.Name --if-exists fail --table-name subscriptions
```

## Unsubscribe

To unsubscribe, delete the entity from the subscriptions table

```
az storage entity delete --partition-key  MyEvent.Full.Type.Name  --row-key  Subscriber  --table-name subscriptions  --if-match *
```

## Delayed delivery

When delayed delivery is enabled at an endpoint, the transport creates a storage table to store the delayed messages.

By default, the storage table name is constructed using a naming scheme that starts with the word delays followed by SHA-1 hash of the endpoint's name.

```
az storage table create -n "delays11f9578d05e6f7bb58a3cdd00107e9f4e3882671"
```

To ensure a single copy of delayed messages is dispatched by any endpoint instance, a blob container is used for leasing access to the delayed messages table.

Similar to  the storage table, the blob container names are also constructed using a naming scheme that starts with the word delays followed by SHA-1 hash of the endpoint's name.

```
az storage container create -n "delays11f9578d05e6f7bb58a3cdd00107e9f4e3882671" --public-access off
```

Delayed messages storage table and container names can be overridden with a custom name. For more information see [Azure Storage Queues Delayed Delivery](/transports/azure-storage-queues/delayed-delivery.md#how-it-works-overriding-tablecontainer-name)

