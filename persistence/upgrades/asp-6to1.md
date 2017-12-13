---
title: Upgrade from NServiceBus Azure Version 6
summary: Instructions on how to migrate from NServiceBus.Azure Storage Persistence Version 6 to NServiceBus.Persistence.AzureStorage Version 1.
reviewed: 2016-06-23
component: ASP
related:
 - persistence/azure-storage
 - persistence/upgrades/asp-saga-deduplication
redirects:
 - nservicebus/upgrades/asp-6to1
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

WARNING: Upgrades from NServiceBus.Azure Versions 6.2.3 and below will need to apply the [saga de-duplication patch](/persistence/upgrades/asp-saga-deduplication.md) followed by applying the [saga index patch](/persistence/upgrades/asp-saga-pruning.md) before completing the remainder of these upgrade steps.


## Changing Nuget Packages and namespaces

One of the most visible changes for this persister was moving it from the NServiceBus.Azure package to the NServiceBus.Persistence.AzureStorage package. This change in packaging has reset the version number to Version 1.

Upgrading to the new package requires removing the [NServiceBus.Azure NuGet package](https://www.nuget.org/packages/NServiceBus.Azure/) from the project and adding the [NServiceBus.Persistence.AzureStorage NuGet package](https://www.nuget.org/packages/NServiceBus.Persistence.AzureStorage/).

Once the packages have been changed, any use of the `NServiceBus.Azure` namespace needs to be replaced with `NServiceBus.Persistence.AzureStorage`.


## New Configuration API

In Versions 6 and below the Azure Storage Persistence was configured using XML configuration sections called `AzureSubscriptionStorageConfig`, `AzureSagaPersisterConfig`, and `AzureTimeoutPersisterConfig`. These XML configuration sections have been obsoleted in favor of a more granular, code based configuration API.

For example, the following Xml:

snippet: AzurePersistenceFromAppConfig

changes to this code configuration:

snippet: AzurePersistenceSubscriptionsAllConnectionsCustomization

The new configuration APIs are accessible through extension methods on the `UsePersistence<AzureStoragePersistence, StorageType.Sagas>()`, `UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>()`, and `UsePersistence<AzureStoragePersistence, StorageType.Timeouts>()` extension points in the endpoint configuration. See [Azure Storage Persistence Code Configuration](/persistence/azure-storage/configuration.md#configuration-with-code) for more details on code configuration API use.

Related errors:

- `An error occurred creating the configuration section handler for AzureSagaPersisterConfig: Could not load file or assembly 'NServiceBus.Azure' or one of its dependencies. The system cannot find the file specified.`
- `An error occurred creating the configuration section handler for AzureSubscriptionStorageConfig: Could not load file or assembly 'NServiceBus.Azure' or one of its dependencies. The system cannot find the file specified.`
- `An error occurred creating the configuration section handler for AzureTimeoutPersisterConfig: Could not load file or assembly 'NServiceBus.Azure' or one of its dependencies. The system cannot find the file specified.`
