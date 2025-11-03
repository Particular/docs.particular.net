---
title: Multiple storage accounts with Azure Storage Queues
summary: Use multiple Azure storage accounts for scale out
component: ASQ
reviewed: 2025-10-30
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

### Aliases instead of connection strings

To avoid connection strings leaking, aliases are always used, using an empty string as the default.
Therefore, when multiple accounts are used, an alias has to be registered for each storage account.

To enable sending from `account_A` to `account_B`, the following configuration needs to be applied in the `account_A` endpoint:

snippet: AzureStorageQueueUseMultipleAccountAliasesInsteadOfConnectionStrings1

Aliases can be provided for both the endpoint's connection strings as well as other accounts' connection strings. This enables using the `@` notation for destination addresses like `queue_name@accountAlias`.

snippet: storage_account_routing_send_options_alias

> [!NOTE]
> The examples above use different values for the default account aliases. Using the same name, such as `default`, to represent different storage accounts in different endpoints is highly discouraged as it introduces ambiguity in resolving addresses like `queue@default` and may cause issues when e.g. replying. In that case an address is interpreted as a reply address, the name `default` will point to a different connection string.

> [!NOTE]
> This feature is currently NOT compatible with ServiceControl. A [ServiceControl transport adapter](/servicecontrol/transport-adapter.md) is required in order to leverage both.

### Using registered endpoints

In order to route message to endpoints without having to specify the destination at all times, it is also possible to register the endpoint for a given command type, assembly or namespace

snippet: storage_account_routing_registered_endpoint

Once the endpoint is registered no send options need to be specified.

snippet: storage_account_routing_send_registered_endpoint

#### Publishers

Similar to sending to an endpoint, the transport can also be configured to subscribe to events published by endpoints in another storage account, using:

snippet: storage_account_routing_registered_publisher

### Using send options

NServiceBus also allows specifying destination addresses using the `<endpoint>@<physicallocation>` notation when messages are dispatched. In this notation, the `physicallocation` element represents the location where the endpoint's infrastructure is hosted, such as a storage account alias.

Using this notation, it is possible to route messages to any endpoint hosted in any storage account.

The use of send options enables routing messages to any endpoint hosted in another storage account by specifying the storage account using the `@` notation.
The `@` notation is used to point to a connection string represented by a specified alias.

snippet: storage_account_routing_send_options_alias

