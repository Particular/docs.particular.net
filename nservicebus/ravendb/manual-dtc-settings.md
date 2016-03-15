---
title: Setting RavenDB DTC settings manually
summary: 'Guidance on how to change the RavenDB ResourceManagerID and TransactionRecoveryStorage'
tags: [RavenDB, Persistence]
redirects:
 - nservicebus/ravendb/how-to-change-resourcemanagerid
 - nservicebus/ravendb/resourcemanagerid
---

WARNING: As of NServiceBus.RavenDB 3.1.0 the RavenDB ResourceManagerId is automatically set to an appropriate value, unique between all endpoints on an individual server, based on a hash of the endpoint's local address and endpoint version. It should not be necessary to manually set this value. All customers are encouraged to upgrade to at least NServiceBus.RavenDB 3.1.0 or higher. This article remains for legacy purposes only.

## Definitions

Support for the Distributed Transaction Coordinator (DTC) in RavenDB is dependent upon the **ResourceManagerId** and **TransactionRecoveryStorage** settings.

### ResourceManagerId

The ResourceMangaerId is an Guid that uniquely identifies a transactional resource on a machine, and must be both unique on that machine, and stable (deterministic) between restarts of the process utilizing the DTC. If more than one RavenDB document store attempts to use the same ResourceManagerId, it can result in the following error during a commit operation:

> "A resource manager with the same identifier is already registered with the specified transaction coordinator"

### TransactionRecoveryStorage

To guard against the loss of a committed transaction, RavenDB requires a storage location to persist data in the event of a process crash immediately following a transaction commit.

RavenDB provides the following methods for persisting transaction recovery storage information:

* `VolatileTransactionRecoveryStorage` persists the information in memory, and since the information will be lost during a process restart, is not a safe storage method.
* `IsolatedStorageTransactionRecoveryStorage` persists the information in [Isolated Storage](https://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.aspx). This was the default for NServiceBus.RavenDB 3.0.x and below, but it was found that this method becomes unstable under high-contention scenarios, and is now considered unsafe for production scenarios.
* `LocalDirectoryTransactionRecoveryStorage` stores transaction recovery information to a local path on disk, and is the only safe option for production scenarios, but must be configured with a directory that is writeable by the application.

## When to manually configure DTC settings

Determining whether to manually configure the DTC settings for RavenDB depends upon the version of NServiceBus.RavenDB in use.

### NServiceBus.RavenDB 3.1.1 and higher

In these versions, NServiceBus will automatically configure the DTC settings, utilizing the configured transaction recovery storage path, which is a required setting.

The DTC settings must be configured manually if the `DocumentStore` instance must be initialized by the host application before NServiceBus is started. This is because RavenDB will execute transaction recovery when the `DocumentStore` is initialized, in which case NServiceBus will not have the opportunity to configure the settings properly.

In all other cases, the RavenDB DTC settings do not need to be manually configured for these versions.

If NServiceBus detects that the `DocumentStore` instance has already been initialized, it will check to make sure the settings are safe and throw an exception if they are not.

### NServiceBus.RavenDB 3.0.x and lower

In these versions, isolated storage was the default transaction recovery storage implementation. Because isolated storage has been found to be unstable for high-contention scenarios, it is advisable to provide a custom DocumentStore and configure the DTC settings in all cases.

## DTC Setting Configuration

In order to configure the DTC settings manually, construct a `DocumentStore` instance and configure the `ResourceManagerId` and `TransactionRecoveryStorage` properties as shown in the following code:

snippet:RavenDBManualDtcSettingExample

It is important that the `ResourceManagerId` is constant across process restarts, and uniquely identifies the process running on the machine. (Do not use `Guid.NewGuid()`.) Otherwise the transaction recovery process will fail when the process restarts.
