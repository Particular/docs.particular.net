---
title: ASQ Transport Scripting
summary: Example scripts to facilitate operational actions against the Azure Storage Queue Transport.
component: ASQ
reviewed: 2023-05-28
related:
 - nservicebus/operations
redirects:
 - nservicebus/asq/operations-scripting
 - transports/asq/operations-scripting
---

The following are example scripts to facilitate operations against the Azure Storage Queue Transport.

## Requirements

These scripts require the Azure command line (Azure CLI) to be installed. For more information refer to the [Azure CLI installation guide](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli).

For the scripts to be run against the Azure Storage Queue transport, a  valid connectionstring to the Azure Storage account must be present. For more information refer to the document on how to [create a storage account](https://learn.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal) 

For easier access, the connection string can be stored as an environment variable.

```
#get the connection string, 
$connectionString=az storage account show-connection-string -n $storageAccount -g $resourceGroup --query connectionString -o tsv

#set conn string as env variable
$env:AZURE_STORAGE_CONNECTION_STRING = $connectionString
```

## Create Queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.

Note: The Azure Storage Queues Transport will fail to start if a queue name is longer than 63 characters. Refer to the [sanitization document](/transports/azure-storage-queues/sanitization.md) for more details.

```
#check if queue exists
az storage queue exists -n $queueName

#create a queue
az storage queue create -n $queueName
```

## Delete Queues

```
#delete a queue
az storage queue delete -n $queueName
```

## Publish/Subscribe
Azure Storage Queue transport implements the publish/subscribe (pub/sub) pattern. Consider a scenario where there are two endpoints 
 - "sample-pubsub-publisher" : This endpoint publishes an event
 - "sample-pubsub-subscriber" : This endpoint subscribes to an event 



### Create queues for publisher and subscriber endpoints and the error queue
```
az storage queue create -n "sample-pubsub-publisher"
az storage queue create -n "sample-pubsub-subscriber"
az storage queue create -n "error"
```

### Create the subscription routing table
In a pub/sub pattern, the transport creates a dedicated subscription routing table shared by all endpoints, which holds subscription information for each event type.

```
#create table
az storage table create -n "subscriptions"
```

### Create a subscription
When the "sample-pubsub-subscriber" endpoint subscribes to an event ( say "OrderReceived"), an entry is created in the subscription routing table.
When the "sample-pubsub-publisher"  endpoint publishes an event, the subscription routing table is queried to find all of the subscribing endpoints.

```
#create entity in the table
az storage entity insert --entity PartitionKey=OrderReceived RowKey=Sample-PubSub-Subscriber  Address=sample-pubsub-subscriber Endpoint=Sample-PubSub-Subscriber Topic=OrderReceived --if-exists fail --table-name subscriptions
```

### Delayed delivery
When delayed delivery is enabled at an endpoint, the transport creates a storage table to store the delayed messages. 
```
#create delayed delivery table
az storage table create -n "delayssamplepubsubpublisher" 
az storage table create -n "delayssamplepubsubsubscriber" 


```
To ensure a single copy of delayed messages is dispatched by any endpoint instance, a blob container is used for leasing access to the delayed messages table.
For more infomation see [Azure Storage Queues Delayed Delivery](/transports/azure-storage-queues/delayed-delivery.md)

```
#create container
az storage container create -n "delayssamplepubsubpublisher" --public-access off
az storage container create -n "delayssamplepubsubsubscriber" --public-access off

#acquire lease
az storage container lease acquire --container-name delayssamplepubsubpublisher
az storage container lease acquire --container-name delayssamplepubsubsubscriber

```
## Unsubscribe

In order to unsubscribe, delete the entity from the subscriptions table

```
az storage entity delete --partition-key  OrderReceived  --row-key  Sample-PubSub-Subscriber  --table-name subscriptions  --if-match *
```