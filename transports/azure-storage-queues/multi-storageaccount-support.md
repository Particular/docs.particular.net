---
title: Multiple storage accounts
summary: Use multiple Azure storage accounts for scale out
component: ASQ
reviewed: 2016-10-26
tags:
- Azure
- Performance
redirects:
 - nservicebus/using-multiple-azure-storage-accounts-for-scaleout
 - nservicebus/azure/using-multiple-azure-storage-accounts-for-scaleout
 - nservicebus/azure-storage-queues/multi-storageaccount-support
related:
 - nservicebus/operations
 - transports/azure-storage-queues/configuration
---

Endpoints running on Azure Storage Queues transport using a single storage account is subject to potential throttling once the maximum number of messages is written to the storage account. To overcome this limitation, use multiple storage accounts. To better understand scale out options with storage accounts, it is necessary understand Azure storage account scalability and performance.


## Azure Storage Scalability and Performance

All messages in a queue are accessed via a single queue partition. A single queue is targeted to process up to 2,000 messages per second. Scalability targets for storage accounts can vary based on region with up to 20,000 messages per second (throughput achieved using an object size of 1KB). This is subject to change and should be periodically verified using [Azure Storage Scalability and Performance Targets](https://docs.microsoft.com/en-us/azure/storage/storage-scalability-targets).

When the number of messages exceeds this quota, storage service responds with an [HTTP 503 Server Busy message](https://docs.microsoft.com/en-us/azure/media-services/media-services-encoding-error-codes). This message indicates that the platform is throttling the queue. If a single storage account is unable to handle an application's request rate, an application could also leverage several different storage accounts using a storage account per endpoint. This ensures application scalability without choking a single storage account. This also allows discrete control over queue processing, based on the sensitivity and priority of the messages that are handled by different endpoints. High priority endpoints could have more workers dedicated to them than low priority endpoints.

NOTE: Using multiple storage accounts is currently NOT compatible with ServiceControl. A [ServiceControl transport adapter](/servicecontrol/transport-adapter/), or multiple installations of ServiceControl, is required in order to leverage both.

## Scaling Out

A typical implementation uses a single storage account to send and receive messages. All endpoints are configured to receive and send messages using the same storage account.

![Single storage account](azure01.png "width=500")

When the number of instances with endpoints are increased, all endpoints continue reading and writing to the same storage account. Once the limit of 2,000 message/sec per queue or 20,000 message/sec per storage account is reached, Azure throttles the message throughput.

![Single storage account with scaled out endpoints](azure02.png "width=500")

While an endpoint can only read from a single Azure storage account, it can send messages to multiple storage accounts. Configure this by specifying a connection string when message mapping. Each endpoint will have its own storage account to overcome the Azure storage account throughput limitation of 20,000 messages/sec.

Example: Endpoint 1 sends messages to Endpoint 2. Endpoint 1 defines message mapping with a connection string associated with the Endpoint 2 Azure storage account. The same idea applies to Endpoint 1 sending messages to Endpoint 2.

Message mapping for Endpoint 1:

```xml
<MessageEndpointMappings>
  <add Messages="Contracts"
       Namespace="Contracts.Commands.ForEndpoint2"
       Endpoint="Endpoint2@connection_string_for_endpoint_2" />
</MessageEndpointMappings>
```

Message mapping for Endpoint 2:

```xml
<MessageEndpointMappings>
  <add Messages="Contracts"
       Namespace="Contracts.Commands.ForEndpoint1"
       Endpoint="Endpoint1@connection_string_for_endpoint_1" />
</MessageEndpointMappings>
```

Each endpoint uses its own Azure storage account, thereby increasing message throughput.


![Scale out with multiple storage accounts](azure03.png "width=500")


partial: aliases


## Scale Units

Scaleout works to a certain extent, but it cannot be applied infinitely while expecting throughput to increase accordingly. Only so much throughput from a single resource or group of resources grouped together is possible.

Suitable techniques in the cloud include resource partitioning and use of scale units. A scale unit is a set of resources with well determined throughput, where adding more resources to this unit does not result in increased throughput. When the scale unit is determined, to improve throughput, create more scale units. Scale units do not share resources.

An example of a partitioned application with a different number of deployed scale units is an application deployed in various regions.

![Scale units](azure04.png "width=500")

NOTE: Use real Azure storage accounts because the Azure storage emulator only supports a single fixed account named `devstoreaccount1`.
