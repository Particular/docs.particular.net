---
title: Azure Queue Storage Transport
summary: Using Azure Queue Storage as a message transport
component: ASQ
related:
 - samples/azure/storage-queues
 - samples/azure/native-integration-asq
reviewed: 2025-11-20
redirects:
 - nservicebus/azure-storage-queues
---

Azure Queue Storage is a service hosted on the Azure platform, used for storing large numbers of messages. The messages can be accessed from anywhere in the world via authenticated calls using HTTP or HTTPS.

Azure Queue Storage is designed for very large cloud networks or hybrid networks, providing a highly reliable and inexpensive queuing service. A single message can be up to 64 KB in size and a queue can hold millions of messages, up to the total capacity limit of the storage account (200 TB). Furthermore, it is capable to emulate local transactions using its queue Peek-Lock mechanism.

The main disadvantages of this service is latency introduced by remoteness and the fact that it supports only HTTP-based communication.

> [!NOTE]
> Microsoft's service is named Azure Queue Storage. Earlier versions of this documentation called it Azure Storage Queues, which is also reflected in the NuGet package name `NServiceBus.Transport.AzureStorageQueues`.

include: azure-transports

## Transport at a glance

|Feature                    |   |
|:---                       |---
|Transactions |None, ReceiveOnly (Message visibility timeout)
|Pub/Sub                    |Native (Requires Storage Table)
|Timeouts                   |Native (Requires Storage Table)
|Large message bodies       |Data bus
|Scale-out             |Competing consumer
|Scripted Deployment        |Not supported
|Installers                 |Mandatory
|Native integration         |[Supported](native-integration.md)
|Case Sensitive             |No
|Aspire integration         |No

## Configuring the endpoint

To use Azure Queue Storage as the underlying transport configure it as follows:

snippet: AzureStorageQueueTransportWithAzure

Then set up appropriate [connection strings](/transports/azure-storage-queues/configuration.md#configuration-parameters-connectionstring).

#if-version ASQN [11, )

include: v7-usetransport-shim-api

#end-if

> [!NOTE]
> When using Azure Queue Storage transport, a serializer must be configured explicitly [by the `UseSerialization` API](/nservicebus/serialization/).
