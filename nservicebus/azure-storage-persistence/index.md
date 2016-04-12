---
title: Azure Storage Persistence
summary: Using Azure Storage as persistence
reviewed: 2016-04-11
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

First add a reference to the assembly that contains the Azure storage persisters. The recommended way of doing this is by adding a NuGet package reference to the `NServiceBus.Azure` package.

If self hosting, the persistence technology could be configured using the configuration API and the extension method found in the `NServiceBus.Azure` assembly.

snippet:PersistanceWithAzure

