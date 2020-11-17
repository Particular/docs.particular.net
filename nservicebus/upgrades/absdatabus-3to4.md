---
title: Azure Blob Storage Databus Upgrade Version 3 to 4
summary: A summary of changes when migrating Azure Blob Storage Data Bus from version 3 to version 4
reviewed: 2020-11-08
component: ABSDataBus
isUpgradeGuide: true
---

## Move to .NET 4.7.2

The minimum .NET Framework version for NServiceBus.DataBus.AzureBlobStorage version 4 is [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472).

**All projects must be updated to .NET Framework 4.7.2 before upgrading to Azure Blob Storage Data Bus version 4.**

It is recommended to update to .NET Framework 4.7.2 and perform a full migration to production **before** updating to version 4. This will isolate any issues that may occur.

For solutions with many projects, the [Target Framework Migrator](https://marketplace.visualstudio.com/items?itemName=PavelSamokha.TargetFrameworkMigrator) Visual Studio extension can reduce the manual effort required in performing an upgrade.

## SDK upgrade

This version leverages improvements in the latest [Azure.Storage.Blobs](https://www.nuget.org/packages/Azure.Storage.Blobs) SDK, moving away from the [WindowsAzure.Storage](https://www.nuget.org/packages/WindowsAzure.Storage/) SDK.

The `.BlockSize()` property has been deprecated due to restrictions of the underlying SDK.

## Registering a BlobServiceClient

The `.AuthenticateWithManagedIdentity()`-method has been deprecated. 
For scenario's in which advanced authentication modes are desirable, a preconfigured `BlobServiceClient` can be configured in the databus in two ways:
- by supplying a preconfigured instance to `.UseBlobContainerClient()`
- by registering a custom provider in the container that implements `IProvideBlobContainerClient` 

## Expired blob cleanup

The built-in cleanup mechanism has been deprecated. See available [clean-up options](/nservicebus/messaging/databus/azure-blob-storage.md?version=absdatabus_4#cleanup-strategies).
