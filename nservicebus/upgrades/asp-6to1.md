---
title: Azure Storage Persistence Upgrade Version 6 to 1
summary: Instructions on how to upgrade NServiceBus.Azure Storage Persistence Version 6 to NServiceBus.Persistence.AzureStorage Version 1.
reviewed: 2016-05-12
tags:
 - upgrade
 - migration
related:
- nservicebus/azure-storage-persistence
- nservicebus/upgrades/asp-saga-deduplication
---

DANGER: Upgrades from NServiceBus.Azure v6.2.3 or lower will need to apply the [saga de-duplication patch](/nservicebus/upgrades/asp-saga-deduplication.md) before completing the remainder of these upgrade steps.

## Changing Nuget Packages and namespaces
Upgrading requires removing the NServiceBus.Azure Nuget package from the project and adding the NServiceBus.Persistence.AzureStorage package.

Once the packages have been changed, any use of the `NServiceBus.Azure` namespace needs to be replaced with `NServiceBus.Persistence.AzureStorage`

## New Configuration API
In Versions 6 and below the Azure Storage Persistence was configured using XML configuration sections called `AzureSubscriptionStorageConfig`, `AzureSagaPersisterConfig`, and `AzureTimeoutPersisterConfig`. These XML configuration sections have been obsoleted in favor of a more granular, code based configuration API.

For example, the following Xml:

snippet:AzurePersistenceFromAppConfig

changes to this code configuration:

snippet:AzurePersistenceSubscriptionsAllConnectionsCustomization

The new configuration APIs are accessible through extension methods on the `UsePersistence<AzureStoragePersistence, StorageType.Sagas>()`, `UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>()`, and `UsePersistence<AzureStoragePersistence, StorageType.Timeouts>()` extension points in the endpoint configuration. See [Azure Storage Persistence Code Configuration](/nservicebus/azure-storage-persistence/configuration.md#configuration-with-code) for more details on code configuration API use.