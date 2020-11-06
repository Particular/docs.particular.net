---
title: Azure Blob Storage Databus Upgrade Version 3 to 4
reviewed: 2020-10-29
component: ABSDataBus
isUpgradeGuide: true
---

### SDK upgrade

This version leverages improvements in the latest [Azure.Storage.Blobs](https://www.nuget.org/packages/Azure.Storage.Blobs) SDK, moving away from the [WindowsAzure.Storage](https://www.nuget.org/packages/WindowsAzure.Storage/) SDK.

The `.BlockSize()` property has been deprecated due to restrictions of the underlying SDK.

### Improved retrieval

The memory foot print for retrieving databus properties from the blob storage has been reduced which leads to increased throughput when using databus properties.

### Registering a BlobServiceClient

The `.AuthenticateWithManagedIdentity()`-method has been deprecated. 
For scenario's in which advanced authentication modes are desirable, a preconfigured `BlobServiceClient` can be configured in the databus in two ways:
- by supplying a preconfigured instance to `.UseBlobContainerClient()`
- by registering a custom provider in the container that implements `IProvideBlobContainerClient` 

### Expired blob cleanup

The built-in cleanup mechanism has been deprecated in favour of the following options:
- Using an Azure Durable Function
- Using the Blob Lifecycle Management policy
