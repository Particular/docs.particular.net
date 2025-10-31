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

It is common for systems running on Azure Storage Queues to depend on a single storage account. However, there is a potential for throttling issues once the maximum number of concurrent requests to the storage account is reached. Multiple storage accounts can be used to overcome this. 

![Scale out with multiple storage accounts](azure03.png "width=500")

To determine whether your system may benefit from scaling out to multiple storage accounts, refer to the the Scale targets table in the Azure [Scalability and performance targets for Queue Storage](https://learn.microsoft.com/en-us/azure/storage/queues/scalability-targets) article, which define when throttling starts to occur.

For additional guidance on considerations when developing a system using Azure Storage Queues, see the article on [Performance and scalability checklist for Queue Storage](https://learn.microsoft.com/en-us/azure/storage/queues/storage-performance-checklist).

> [!NOTE]
> Use real Azure storage accounts. Do not use Azure storage emulator as it only supports a single fixed account named devstoreaccount1.".

> [!NOTE]
> There are limits to how much using multiple storage accounts increase throughput. Consider using [scale units as a comprehensive scaling strategy](https://learn.microsoft.com/en-us/azure/well-architected/performance-efficiency/scale-partition#choose-a-scaling-strategy) to address higher throughput and reliability needs.

## NServiceBus routing with multiple storage accounts

The preferred way to route when using multiple accounts is to register endpoints with their associated storage accounts.

partial: registered-endpoint

NServiceBus also allows specifying destination addresses using the `<endpoint>@<physicallocation>` notation when messages are dispatched. In this notation, the `physicallocation` element represents the location where the endpoint's infrastructure is hosted, such as a storage account.

Using this notation, it is possible to route messages to any endpoint hosted in any storage account.

partial: routing-send-options-full-connectionstring

partial: aliases
