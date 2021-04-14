---
title: Configuring RavenDB DTC
summary: Guidance on how to change the RavenDB ResourceManagerID and TransactionRecoveryStorage.
versions: '[,5)'
component: Raven
related:
 - nservicebus/operations
redirects:
 - nservicebus/ravendb/how-to-change-resourcemanagerid
 - nservicebus/ravendb/resourcemanagerid
 - nservicebus/ravendb/manual-dtc-settings
reviewed: 2019-06-10
---

include: dtc-warning

include: cluster-configuration-warning

DANGER: Since distributed transactions are not supported for RavenDB persistence, this page remains for archival purposes only.

In order to provide reliable support for distributed transactions in RavenDB, a custom DocumentStore must be provided and configured to uniquely identify the endpoint to the Distributed Transaction Coordinator (DTC) and provide a storage location for uncommitted transaction recovery.


## Definitions

Support for the Distributed Transaction Coordinator (DTC) in RavenDB is dependent upon the **ResourceManagerId** and **TransactionRecoveryStorage** settings.

NOTE: When using the [Outbox feature](/nservicebus/outbox/) instead of distributed transactions, these settings are not required and should not be used. See [Outbox with RavenDB persistence](/persistence/ravendb/outbox.md) for more information.


### ResourceManagerId

The ResourceManagerId is a Guid that uniquely identifies a transactional resource on a machine, and must be both unique system wide, and stable (deterministic) between restarts of the process utilizing the DTC. If more than one RavenDB document store attempts to use the same ResourceManagerId, it can result in the following error during a commit operation:

> "A resource manager with the same identifier is already registered with the specified transaction coordinator"

NOTE: It is possible to [configure the ResourceManagerId from a RavenDB connection string](https://ravendb.net/docs/search/3.0/csharp?searchTerm=connection-string), however this is not recommended as this method does not allow for the configuration of a suitable TransactionRecoveryStorage.


### TransactionRecoveryStorage

To guard against the loss of a committed transaction, RavenDB requires a storage location to persist data in the event of a process crash immediately following a transaction commit.

RavenDB provides [transaction recovery storage options](https://ravendb.net/docs/search/3.0/csharp?searchTerm=TransactionRecoveryStorage) for volatile (in-memory) storage, IsolatedStorage, and local directory storage. `LocalDirectoryTransactionRecoveryStorage` is recommended as the only stable and reliable option.


## Configuring safe settings

In order to configure safe settings for production use of RavenDB, construct a `DocumentStore` instance and configure the `ResourceManagerId` and `TransactionRecoveryStorage` properties as shown in the following code:

snippet: RavenDBManualDtcSettingExample

In order to provide transaction safety, the following must be observed:

 * `documentStore.ResourceManagerId` must be constant across process restarts, and uniquely identify the process running on the machine. **Do not use `Guid.NewGuid()`. Otherwise, the transaction recovery process will fail when the process restarts.**
 * `documentStore.TransactionRecoveryStorage` must be set to an instance of `LocalDirectoryTransactionRecoveryStorage`, configured to a directory that is constant across process restarts, and writable by the process.


## Configuration by convention

It can be cumbersome to manage these settings for multiple endpoints, so it is preferable to create a convention that will calculate a unique ResourceManagerId, and then use this value to create a storage location for TransactionRecoveryStorage as well.

snippet: RavenDBDtcSettingsByConvention

INFO: It is important to dispose the RavenDB `DocumentStore` when the endpoint is shut down. Depending on the way the endpoint is hosted there are multiple options. If the endpoint is self-hosted, the `DocumentStore` can be manually disposed before the hosting process is shutdown. When using the [NServiceBus Host](/nservicebus/hosting/nservicebus-host/), an implementation of [`IWantToRunWhenEndpointStartsAndStops`](/nservicebus/hosting/nservicebus-host/#when-endpoint-instance-starts-and-stops) can be provided to hook into the shutdown process. One last option is to use a [feature startup task](/nservicebus/pipeline/features.md#feature-startup-tasks).

It is important to keep a few things in mind when determining a convention.

The string provided to DeterministicGuidBuilder will define the ResourceManagerId, and thus the identity of the endpoint. This string value must then be unique within the scope of the server. The EndpointName or LocalAddress provide attractive options as they are normally unique per server.

An exception is side-by-side deployment, where an old version and new version of the same endpoint run concurrently, processing messages from the same queue, in order to verify the new version and enable rollback to the previous version. In this case using EndpointName or LocalAddress would result in duplicate ResourceManagerId values on the same server, which would lead to DTC exceptions. In this case, each release of the endpoint must be versioned (for example, with the [AssemblyFileVersion attribute](https://msdn.microsoft.com/en-us/library/system.reflection.assemblyfileversionattribute.aspx)), and the endpoint's version must be included in the string provided to DeterministicGuidBuilder.

The exact convention used must be appropriately defined to match the deployment strategy in use for the endpoint. If a new endpoint version is deployed by taking the old one offline, replacing the binaries, and then starting it back up, fixed values should be used at all times so that the new version can recover transactions for the old version. If endpoint versions are run side-by-side, then independent values should be used for each version, and old versions should be safely decommissioned when they are shut down.


## Safely decommissioning endpoints

If an endpoint terminates unexpectedly for any reason, data can be left behind in the transaction recovery storage which represents transactions not yet committed to the RavenDB database, but which may be recovered when the endpoint restarts.

In order to avoid losing data, it is important to ensure that endpoints that are decommissioned are taken offline gracefully, i.e. stopped at the request of the Service Control Manager, and not terminated from the Task Manager. Then, the transaction recovery storage directory should be inspected to ensure it is empty. If any files remain, the endpoint should be started again briefly so that RavenDB can perform a recovery.