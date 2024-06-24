---
title: Azure Storage Queues Transport
summary: Using Azure Storage Queues as a message transport
component: ASQ
related:
 - samples/azure/storage-queues
 - samples/azure/native-integration-asq
reviewed: 2024-02-01
redirects:
 - nservicebus/azure-storage-queues
---

Azure Storage Queues is a service hosted on the Azure platform, used for storing large numbers of messages. The messages can be accessed from anywhere in the world via authenticated calls using HTTP or HTTPS.

Azure Storage Queues are designed for very large cloud networks or hybrid networks, providing a highly reliable and inexpensive queuing service. A single message can be up to 64 KB in size and a queue can hold millions of messages, up to the total capacity limit of the storage account (200 TB). Furthermore, it is capable to emulate local transactions using its queue Peek-Lock mechanism.

The main disadvantages of this service is latency introduced by remoteness and the fact that it supports only HTTP-based communication.

include: azure-transports

## Transport at a glance

| Feature                    |                                                   |
|:---------------------------|---------------------------------------------------|
| Transactions               | None, ReceiveOnly (Message visibility timeout)    |
| Pub/Sub                    | Native (Requires Storage Table)                   |
| Timeouts                   | Native (Requires Storage Table)                   |
| Large message bodies       | Data bus                                          |
| Scale-out                  | Competing consumer                                |
| Scripted Deployment        | Not supported                                     |
| Installers                 | Mandatory                                         |
| Native integration         | [Supported](native-integration.md)                |
| time-to-be-received (TTBR) | Deleted after expiration, TTL depends per version |

## Configuring the endpoint

partial: endpointconfig

> [!NOTE]
> When using Azure Storage Queues transport, a serializer must be configured explicitly [by the `UseSerialization` API](/nservicebus/serialization/).

## Time-to-be-received

The term [Azure Storage queues uses for time-to-be-received is time-to-live (TTL)](https://learn.microsoft.com/en-us/rest/api/storageservices/put-message#uri-parameters)

Azure Storage Queues will automatically delete the message from the queue once the TTL value expires.

#if-version ASQN [8.1,)

Since Azure Storage Queues transport version 8.1.0 it will use a default TTL of 30 days

#end-if

#if-version ASQ [,9.0)

Before Azure Storage Queues transport version 8.1.0 it will use the Azure Storage Queues default TTL which is 7 days.

Since Azure Storage Queues transport version 8.1.0 it will use a default TTL of 30 days

#end-if
