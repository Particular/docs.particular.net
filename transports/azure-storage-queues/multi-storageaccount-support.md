---
title: Multiple storage accounts with Azure Storage Queues
summary: Use multiple Azure storage accounts for scale out
component: ASQ
reviewed: 2025-10-28
redirects:
 - nservicebus/using-multiple-azure-storage-accounts-for-scaleout
 - nservicebus/azure/using-multiple-azure-storage-accounts-for-scaleout
 - nservicebus/azure-storage-queues/multi-storageaccount-support
related:
 - nservicebus/operations
 - transports/azure-storage-queues/configuration
---

> [!IMPORTANT]
> Using multiple storage accounts is currently NOT compatible with ServiceControl. It is necessary to use [ServiceControl transport adapter](/servicecontrol/transport-adapter.md) or multiple installations of ServiceControl for monitoring in such situation.

It is common for systems running on Azure Storage Queues to depend on a single storage account. However, there is a potential for throttling issues once the maximum number of concurrent requests to the storage account is reached. Multiple storage accounts can be used to overcome this limitation. To determine whether your system may benefit from scaling out to multiple storage accounts, refer to the targets referenced in the Azure [Scalability and performance targets for Queue Storage](https://learn.microsoft.com/en-us/azure/storage/queues/scalability-targets) article, which define when throttling starts to occur.

For additional guidance on considerations when developing a system using Azure Storage Queues, see the article on [Performance and scalability checklist for Queue Storage](https://learn.microsoft.com/en-us/azure/storage/queues/storage-performance-checklist).

## Scaling Out

All endpoints are configured to receive and send messages using the same storage account.

![Single storage account](azure01.png "width=500")

When the number of instances of endpoints is increased, all endpoints continue reading and writing to the same storage account. Once the limit of 2,000 message/sec per queue or 20,000 message/sec per storage account is reached, Azure storage service throttles messages throughput.

![Single storage account with scaled out endpoints](azure02.png "width=500")

While an endpoint can only read from a single Azure storage account, it can send messages to multiple storage accounts. This way one can set up a solution using multiple storage accounts where each endpoint uses its own Azure storage account, thereby increasing message throughput.

![Scale out with multiple storage accounts](azure03.png "width=500")


## Scale Units

Scaleout and splitting endpoints over multiple storage accounts work to a certain extent, but it cannot be applied infinitely while expecting throughput to increase linearly. Each resource and group of resources has certain throughput limitations.

A suitable technique to overcome this problem includes resource partitioning and usage of scale units. A scale unit is a set of resources with well determined throughput, where adding more resources to this unit does not result in increased throughput. When the scale unit is determined, to improve throughput more scale units can be created. Scale units do not share resources.

An example of a partitioned application with a different number of deployed scale units is an application deployed in various regions.

![Scale units](azure04.png "width=500")

> [!NOTE]
> Use real Azure storage accounts. Do not use Azure storage emulator as it only supports a single fixed account named devstoreaccount1.".


## Cross namespace routing

NServiceBus allows to specify destination addresses using an `"endpoint@physicallocation"` when messages are dispatched, in various places such as the [Send](/nservicebus/messaging/send-a-message.md) and [Routing](/nservicebus/messaging/routing.md) API or the `MessageEndpointMappings`. In this notation the `physicallocation` section represents the location where the endpoint's infrastructure is hosted, such as a storage account.

Using this notation it is possible to route messages to any endpoint hosted in any storage account.

partial: routing-send-options-full-connectionstring

partial: aliases

partial: registered-endpoint
