---
title: Azure Blob Storage Databus Upgrade Version 3 to 4
reviewed: 2020-11-08
component: ABSDataBus
isUpgradeGuide: true
---

### SDK upgrade

This version leverages improvements in the latest [Azure.Storage.Blobs](https://www.nuget.org/packages/Azure.Storage.Blobs) SDK, moving away from the [WindowsAzure.Storage](https://www.nuget.org/packages/WindowsAzure.Storage/) SDK.

The `.BlockSize()` property has been deprecated due to restrictions of the underlying SDK.

### Registering a BlobServiceClient

The `.AuthenticateWithManagedIdentity()`-method has been deprecated. 
For scenario's in which advanced authentication modes are desirable, a preconfigured `BlobServiceClient` can be configured in the databus in two ways:
- by supplying a preconfigured instance to `.UseBlobContainerClient()`
- by registering a custom provider in the container that implements `IProvideBlobContainerClient` 

### Expired blob cleanup

The built-in cleanup mechanism has been deprecated. See available [clean-up options](/nservicebus/messaging/databus/azure-blob-storage.md?version=absdatabus_4#cleanup-strategies).