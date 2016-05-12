---
title: Azure Storage Persistence
summary: Using Azure Storage as persistence
reviewed: 2016-05-12
tags:
- Azure
- Cloud
- Persistence
- Performance
- Hosting
redirects:
 - nservicebus/using-azure-storage-persistence-in-nservicebus
 - nservicebus/azure/azure-storage-persistence
---

Certain features of NServiceBus require persistence to permanently store data. Among them are subscription storage, sagas, and timeouts. Various storage options are available including Azure Storage Services.


## How to enable persistence with Azure Storage Services

First add a reference to the assembly that contains the Azure storage persisters. When working with NServiceBus Version 5 or lower this is done by adding a NuGet package reference to `NServiceBus.Azure`. For NServiceBus Version 6 and above the NuGet package reference will be to `NServiceBus.Persistence.AzureStorage`.

If self hosting, the persistence technology should be configured using the configuration API.

snippet:PersistanceWithAzure

