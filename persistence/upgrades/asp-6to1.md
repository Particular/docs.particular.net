---
title: Upgrade from NServiceBus Azure Version 6
summary: Instructions on how to migrate from NServiceBus.Azure Storage Persistence version 6 to NServiceBus.Persistence.AzureStorage version 1.
reviewed: 2020-08-31
component: ASP
related:
 - persistence/azure-table
 - persistence/upgrades/asp-saga-deduplication
redirects:
 - nservicebus/upgrades/asp-6to1
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

WARNING: Upgrades from NServiceBus.Azure versions 6.2.3 and below will need to apply the [saga de-duplication patch](/persistence/upgrades/asp-saga-deduplication.md) followed by the [saga index patch](/persistence/upgrades/asp-saga-pruning.md) before completing the remainder of these upgrade steps.


## Changing NuGet packages and namespaces

One of the most visible changes for this persister was moving it from the NServiceBus.Azure package to the NServiceBus.Persistence.AzureStorage package. This change in packaging has reset the version number to version 1.

Upgrading to the new package requires removing the [NServiceBus.Azure NuGet package](https://www.nuget.org/packages/NServiceBus.Azure/) from the project and adding the [NServiceBus.Persistence.AzureStorage NuGet package](https://www.nuget.org/packages/NServiceBus.Persistence.AzureStorage/).

Once the packages have been changed, any use of the `NServiceBus.Azure` namespace must be replaced with `NServiceBus.Persistence.AzureStorage`.


## New configuration API

In versions 6 and below, the Azure Storage Persistence was configured using XML configuration sections called `AzureSubscriptionStorageConfig`, `AzureSagaPersisterConfig`, and `AzureTimeoutPersisterConfig`. These XML configuration sections have been deprecated in favor of a more granular, code-based configuration API.

For example, the following XML:

```
<configSections>
  <section name="AzureSubscriptionStorageConfig"
           type="NServiceBus.Config.AzureSubscriptionStorageConfig, NServiceBus.Azure" />
  <section name="AzureSagaPersisterConfig"
           type="NServiceBus.Config.AzureSagaPersisterConfig, NServiceBus.Azure" />
  <section name="AzureTimeoutPersisterConfig"
           type="NServiceBus.Config.AzureTimeoutPersisterConfig, NServiceBus.Azure" />
</configSections>
<AzureSagaPersisterConfig ConnectionString="UseDevelopmentStorage=true" />
<AzureTimeoutPersisterConfig ConnectionString="UseDevelopmentStorage=true" />
<AzureSubscriptionStorageConfig ConnectionString="UseDevelopmentStorage=true" />
```

changes to this code configuration:

```
var persistence = endpointConfiguration.UsePersistence<AzureStoragePersistence>();
persistence.ConnectionString("connectionString");
```

The new configuration APIs are accessible through extension methods on the `UsePersistence<AzureStoragePersistence, StorageType.Sagas>()`, `UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>()`, and `UsePersistence<AzureStoragePersistence, StorageType.Timeouts>()` extension points in the endpoint configuration. See [Azure Storage Persistence Code Configuration](/persistence/azure-table/configuration.md) for more details on code configuration API use.

Related errors:

- `An error occurred creating the configuration section handler for AzureSagaPersisterConfig: Could not load file or assembly 'NServiceBus.Azure' or one of its dependencies. The system cannot find the file specified.`
- `An error occurred creating the configuration section handler for AzureSubscriptionStorageConfig: Could not load file or assembly 'NServiceBus.Azure' or one of its dependencies. The system cannot find the file specified.`
- `An error occurred creating the configuration section handler for AzureTimeoutPersisterConfig: Could not load file or assembly 'NServiceBus.Azure' or one of its dependencies. The system cannot find the file specified.`
