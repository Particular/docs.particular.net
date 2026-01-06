---
title: ServiceControl Troubleshooting
summary: Troubleshooting ServiceControl installation and common issues
component: ServiceControl
reviewed: 2025-06-11
---

> [!NOTE]
> Many issues can be resolved by upgrading to the [latest version of ServiceControl](https://particular.net/downloads) and by ensuring the host meets the minimum [general hardware considerations for ServiceControl](/servicecontrol/servicecontrol-instances/hardware.md#general-recommendations).

## Check configuration

Review the instance configuration for each instance:

* Windows services store configuration in an `*.exe.config` file next to the executable, which can be found in ServiceControl Management by clicking on the Installation Path and opening the configuration file from Windows Explorer.
* Container instances load all configuration from environment variables.

## Service stops unexpectedly

ServiceControl instances are configured to stop when essentialy dependencies (such as the message transport) are unavailable for too long, so that administrators are notified of the problem.

ServiceControl Windows Services are configured for automatic restart via Windows Service recoverability policy. The services are restarted after 1 minute of the unplanned shutdown.

## Service fails to start

There are various reasons that can cause the ServiceControl Windows Service fail to start. Details can be found in the [ServiceControl logs](logging.md).

In Windows applications, critical exception thrown at service start up will be reported via an error message in the `Application` Windows Event Log.

## The port is already in use (Windows)

When adding a ServiceControl instance the configured port number is checked to ensure it is available. This is not infallible though as another application or service that uses the same port may not be running at the time the service is added.

In the event that the service fails to start to check if the configured port (typically port 33333) is available. To do this open up an elevated command prompt and issue the following command:

```shell
netstat -a -b
```
or install and use the provided [ServiceControl Management PowerShell module](https://www.powershellgallery.com/packages/Particular.ServiceControl.Management) cmdlet to check a specific port:

snippet: ps-testport

## Missing queue

The service expects to be able to connect to the error, audit and forwarding queues specified in the configuration. If the configuration has been manually changed ensure the specified queues exist.

## Cannot connect to the queues

Some transports have access controls built into them. Ensure the instance has sufficient rights to access the queues.

## Service won't start after changing service accounts (Windows)

 1. The service account has access read rights to the directory the service is installed
 1. The service account has access read/write rights to the database and logs directories specified in the configuration.
 1. The service account has the logon as a service privilege.
 1. Confirm that the service account has sufficient writes to manage the configured queues. See [Configuring a Non-Privileged Service Account](configure-non-privileged-service-account.md) for a breakdown of the queues to check.

## Service fails to start: EsentInstanceUnavailableException

_ServiceControl 4.x and below only._

If ServiceControl fails to start and the logs contain a `Microsoft.Isam.Esent.Interop.EsentInstanceUnavailableException` ensure that ServiceControl [database directory](configure-ravendb-location.md), sub-directory and files, is excluded from any anti-virus and anti-malware real-time and scheduled scan.

## Service fails to start: EsentDatabaseDirtyShutdownException

_ServiceControl 4.x and below only._

If ServiceControl fails to start and the logs contain a `Microsoft.Isam.Esent.Interop.EsentDatabaseDirtyShutdownException` run Esent Recovery against the ServiceControl database followed by an Esent Repair.

 1. Open an elevated command prompt and navigate to the ServiceControl [database directory](configure-ravendb-location.md) (the default is `%PROGRAMDATA%\Particular\ServiceControl\Particular.ServiceControl\DB`)
 1. Run `esentutl /r RVN /l "logs"` to run Recovery (bringing all databases to a clean-shutdown state) and wait for it to finish
 1. Run `esentutl /p Data` to run Repair (Repairs a corrupted or damaged database) and wait for it to finish
 1. Restart ServiceControl

## Service fails to start: SqlException certificate chain not trusted

If ServiceControl fails to start and the logs contain the following exception, then ServiceControl is not able to connect to the SQL Server instance.

```
System.Data.SqlClient.SqlException
  HResult=0x80131904
  Message=A connection was successfully established with the server, but then an error occurred during the login process. (provider: SSL Provider, error: 0 - The certificate chain was issued by an authority that is not trusted.)
  Source=.Net SqlClient Data Provider
```

When encryption is enabled, SQL Server uses a certificate to encrypt communication between itself and ServiceControl. Version 4 of the `Microsoft.Data.SqlClient` package includes a [breaking change](https://github.com/dotnet/SqlClient/pull/1210) to set `Encrypt=true` by default (the previous default was `false`) which causes this exception.

To fix this error, [update the SQL Server installation with a valid certificate and update the ServiceControl machine to trust this certificate](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/enable-encrypted-connections-to-the-database-engine) or add `Encrypt=False` to the connection string if encryption is truly not necessary.

## Unable to connect to ServiceControl from ServicePulse

Attempt to connect to the ServiceControl instance's URL using a web browser. A valid response with JSON data should be returned. If not, verify that network configuration and/or firewall settings do not block access to the ServiceControl port specified in the URL.

> [!NOTE]
> Before changing firewall setting to expose ServiceControl read [Securing ServiceControl](securing-servicecontrol.md).

## Resolve messages that cannot be retried

If certain messages are not scheduled for retry and the logs show the following message then the database could be in an inconsistent state:

```txt
2020-10-16 13:31:58.9863|190|Info|ServiceControl.Recoverability.RetryProcessor|Retry batch RetryBatches/1c33af76-8177-494d-ae9a-af060cefae02 cancelled as all matching unresolved messages are already marked for retry as part of another batch.
2020-10-16 13:31:59.2826|173|Info|ServiceControl.Recoverability.InMemoryRetry|Retry operation bf05499a-9261-41ec-9b49-da40e22a6f20 completed. 1 messages skipped, 0 forwarded. Total 1.
```

The internal *FailedMessageRetries* collection must be purged in order to restore retries for such messages.

1. Upgrade to the [latest ServiceControl version](https://particular.net/downloads)
1. Ensure that currently there are no retry operations active
1. [Access the ServiceControl database](/servicecontrol/ravendb/accessing-database.md)
1. Select the "FailedMessageRetries" collection in the left tree
2. Delete all documents in the collection

## SmartScreen blocks the installer (Windows)

The installer is [code signed](https://en.wikipedia.org/wiki/Code_signing), but [SmartScreen](https://en.wikipedia.org/wiki/Microsoft_SmartScreen) (called Windows SmartScreen, Windows Defender SmartScreen and SmartScreen Filter in different places) may classify the code signing certificate as "untrusted" and block the installer from running until permission is granted by the user.

Although the installer is code signed correctly with a certificate owned by "NServiceBus Ltd", SmartScreen will block it from running until Microsoft has built enough "trust" in the certificate. One of the main inputs to building that trust is when users grant permission to run the installer. To grant permission to run the installer, click "Run Anyway". This will no longer be required when Microsoft decides to trust the certificate.

When building ServiceControl, all build artifacts are virus scanned to ensure no viruses or malware are shipped with the installer packages.

## Stale indexes

Each ServiceControl instance stores its data in a RavenDB database. Indexes are used by RavenDB as way to query the documents. This indexing is done asynchronously in the background and is triggered whenever data is added or changed. The benefit of asynchronous indexing is that it allows the server to respond quickly even when large amounts of data has changed and avoids costly table scan operations.

A downside of this is that indexes are not updated immediately and can become stale. A healthy system has indexes updated in milliseconds, while a system under load can take up to several seconds to update the indexes. When indexes get very stale, the process of updating them can last for a long duration and can affect data presented and started tasks.

Systems are affected by severe index lag when the following custom check message is presented:

> At least one index significantly stale. Please run maintenance mode if this custom check persists to ensure index(es) can recover. See log file in `{LogPath}` for more details.

A warning message is seen in the logs when the Indexing lag exceeds the default threshold value of 10,000

```txt
2023-07-03 09:41:08.4189|6|Warn|ServiceControl.CheckRavenDBIndexLag|Index [ExpiryKnownEndpointsIndex] IndexingLag 22,242 is above warning threshold (10,000). Launch in maintenance mode to let indexes catch up.
```

This can be resolved by temporarily stopping message ingestion to let the indexes catch up:

* For Windows instances, launch the ServiceControl instance in [maintenance mode](/servicecontrol/ravendb/accessing-database.md#windows-deployment-maintenance-mode), which runs the database but does not ingest new messages.
* For container instances, stop the ServiceControl container temporarily, but keep the connected database container running.

While message ingestion is disabled, the database engine still runs and messages will continue to queue. This ensures that any tasks related to index rebuilding or index scanning can run without interruption. This is useful to resolve situations where the storage isn't fast enough to do both message ingestion and index operations, such as when an unexpected spike in message processing occurred.

Consider upgrading the storage if these errors persists.

Contact [Particular support](https://particular.net/support) for assistance.

## Index errors

Index issues are usually automatically corrected at start-up time but sometimes index issues require manual intervention. When unrecoverable index issues occur the following custom check message is visible:

> Detected RavenDB index errors, please start maintenance mode and resolve the following issues:

Sometimes [indexes get corrupted](#corrupted-indexes). Resolve these errors by [accessing the ServiceControl database](/servicecontrol/ravendb/accessing-database.md) and inspecting the errors directly.

Contact [Particular support](https://particular.net/support) for assistance.

## Corrupted indexes

Sometimes the following error may be observed:

```txt
Raven.Abstractions.Exceptions.IndexDisabledException: The index has been disabled due to errors
```

or

```txt
2021-03-23 09:27:50.0593|14|Warn|Raven.Database.DocumentDatabase|Could not create index batch
System.InvalidOperationException: Cannot modify indexes while indexing is in progress (already waited full minute). Try again later
```

This risk of these error occurring is mitigated by:

- [Excluding the database storage folder from virus scanning](servicecontrol-in-practice.md#anti-virus-checks)
- [Ensuring enough storage space is available](capacity-and-planning.md#storage-size)
- [Setting up server monitoring and proactively monitoring free storage space](servicecontrol-in-practice.md#server-monitoring)

#### Reset selected indexes

To resolve these errors, the affected indexes must be rebuilt:

- [Access the ServiceControl database](/servicecontrol/ravendb/accessing-database.md)
- In RavenDB Management Studio, navigate to the Indexes view
- [Reset the relevant index(es)](https://ravendb.net/docs/article-page/5.4/csharp/indexes/index-administration)


## High CPU utilization

> [!WARNING]
> Avoid forcibly terminating the ServiceControl process (e.g. through Task Manager) as this can cause index corruption and can trigger the index rebuilding process. In turn, this causes long and excessive resource utilization for large databases.

If a ServiceControl instance shows high CPU utilization, it's usually due to:

- Unclean process termination forcing ServiceControl internal database to validate indexes consistency at the next startup.
- Rebuilding of indexes which requires all database records to be read and indexed.
- Large messages ingestion due to a massive backlog of messages in the queue.

Usually this fixes itself over time as long as the process is not terminated. Again, do not forcibly terminate the process. Always use Windows Services, PowerShell, or the ServiceControl Management Utility to stop and start the service.

> [!NOTE]
> It is recommended to host ServiceControl instances on isolated (virtual) machines with dedicated (non-shared) resources not to affect any other process when ServiceControl requires a lot of system resources.

Resolution:

- Ensure all the latest performance enhancements are available by having the latest version of ServiceControl installed. The most recent version is available at <https://particular.net/downloads>
- Ensure storage disks are **at least** capable of 7,500 IOPS as stated in the [hardware considerations for ServiceControl](/servicecontrol/servicecontrol-instances/hardware.md). If the system continuously produces messages or generates many or substantial messages (several kilobytes or larger), ServiceControl requires even faster disks than specified by the **minimum** requirements.
- Ensure no custom checks shown in ServicePulse indicate index issues. The log file could indicate the type of index issues (See [stale indexes](#stale-indexes), [index errors](#index-errors), and [corrupted indexes](#corrupted-indexes))
- Consider disabling message bodies and headers *Full-Text search* as this causes most resource utilization for CPU and disk IO. This can be disabled in the latest version of ServiceControl by configuring each ServiceControl instance: open configuration (gear icon), scroll down to Advanced Configuration and set "Full-Text Search On Message Bodies" to Off, finally select Save, and then restart the instance.

> [!WARNING]
> Disabling *Full-Text Search* causes text search to be unavailable in ServicePulse.

## Saga audit data retention custom check failure

Users who have migrated from earlier versions of ServiceControl may have historical saga audit records still in the database. This custom check will fail if there is no audit retention period set on the ServiceControl Error instance when saga audit data exists. To resolve this issue a retention period should be configured by adding:

  ```xml
  <add key="ServiceControl/AuditRetentionPeriod" value="DD:HH:MM" />
  ```

For example, a 20-day retention period would be set as follows:

  ```xml
  <add key="ServiceControl/AuditRetentionPeriod" value="20:00:00" />
  ```

## Logs contain EsentOutOfLongValueIDsException

If ServiceControl logs contain a `Microsoft.Isam.Esent.Interop.EsentOutOfLongValueIDsException: Long-value ID counter has reached maximum value. (perform offline defrag to reclaim free/unused LongValueIDs)` error similar to the following snippet, its [database must be compacted](db-compaction.md).

```txt
2022-03-25 18:46:50.6564|287|Warn|ServiceControl.Audit.Auditing.AuditIngestionComponent|OnCriticalError. 'Failed to execute recoverability policy for message with native ID: `4f6d43c9-5a78-4232-8daa-6065201edeac`'
Raven.Abstractions.Connection.ErrorResponseException: Url: "/bulk_docs"
Microsoft.Isam.Esent.Interop.EsentOutOfLongValueIDsException: Long-value ID counter has reached maximum value. (perform offline defrag to reclaim free/unused LongValueIDs)
```

## API is slow

If the ServiceControl API is slow, the cause is usually one of the following:

### Incorrect remotes configuration

An invalid value for the [`ServiceControl/RemoteInstances` configuration setting](/servicecontrol/servicecontrol-instances/remotes.md#configuration) can result in connectivity issues. Review this setting and check if the entry contains the correct host names.

This usually happens when the host name was changed. Changing the host name will not automatically update the `ServiceControl/RemoteInstances` setting.

1. Launch ServiceControl Management Utility (SCMU)
1. Scroll to the ServiceControl instance
1. Select the installation path Browse button
1. Open the file `ServiceControl.exe.config` in an editor with administrative privileges
1. Review the `ServiceControl/RemoteInstances` setting for invalid host names or obsolete instances

### Unreachable audit instance

Review if the listed hostname in the `ServiceControl/RemoteInstances` setting is reachable from the machine that hosts the ServiceControl instance. Common methods are:

- Pinging the host
- Opening the API url (`http://localhost:33333/api/`) in the browser with the same hostname as specified in the config file

### Stopped audit instance

Review if all listed audit instances are running.

## No destination specified for message

A missing `ServiceControlQueueAddress` in the configuration file for audit instances will cause the following error to be seen in the audit logs:

```txt
2023-06-20 13:38:45.4426|7|Warn|ServiceControl.Audit.Auditing.AuditPersister|Processing of message '7437b01e-32c1-4a29-9aed-f0c86ac64ebe' failed.
System.Exception: No destination specified for message: ServiceControl.Contracts.EndpointControl.RegisterNewEndpoint
   at NServiceBus.UnicastSendRouter.RouteUsingTable(IOutgoingSendContext context) in /_/src/NServiceBus.Core/Routing/UnicastSendRouter.cs:line 108
```

This error is caused if there is no setting for `ServiceControl.Audit/ServiceControlQueueAddress`. Check the audit instance config file in the audit installation path and add in the missing key value.

 ```xml
<add key="ServiceControl.Audit/ServiceControlQueueAddress" value="[QUEUENAMEGOESHERE]"/>
```

## File/Folder used by another process

Sometimes the [RavenDB logs](/servicecontrol/logging.md#ravendb-logging) could have the following errors:

```txt
System.IO.IOException: Error during flush for FailedMessageFacetsIndex ---> System.IO.IOException: The process cannot access the file 'D:\ServiceControl-Share\Particular.Servicecontrol\DB\Indexes\10\_49l.cfs' because it is being used by another process.
```

 or

```txt
 2023-06-01 16:58:02.9925|8|Error|Raven.Database.DocumentDatabase|Could not initialize transactional storage, not creating database
System.InvalidOperationException: Could not write to location: C:\ProgramData\Particular\ServiceControl\Particular.servicecontrol.audit\DB\
Make sure there is  read/write permissions for this path.
Microsoft.Isam.Esent.Interop.EsentFileAccessDeniedException: Cannot access file, the file is locked or in use
   at Microsoft.Isam.Esent.Interop.Api.JetInit(JET_INSTANCE& instance)
```

Such errors indicate that the file or folder is being used by another process. A symptom of this could also be high CPU usage or errors when ServiceControl is starting.

Ensure that the ServiceControl and ServiceControl Audit database directories, sub-directories, and files are [excluded from any anti-virus](servicecontrol-in-practice.md#anti-virus-checks) and anti-malware real-time and scheduled scans. If there are no virus scans on the file or folder, then use [Process Explorer](https://learn.microsoft.com/en-us/sysinternals/downloads/process-explorer) or a similar tool to determine what other application may be opening or loading the file or folder.

## Unable to connect to ServiceControl

By default, ServiceControl uses "localhost" as the hostname (Eg: http://localhost:33333/api/) to connect as anyone who can access the ServiceControl URL has complete access to the message data stored within ServiceControl. Users who try to connect to ServiceControl either using the machine name or the IP address can get the following error in the browser if the hostname is not configured correctly.
Eg: Connect to http://my-machine:33333/api/

```txt
Bad Request - Invalid Hostname
HTTP Error 400. The request hostname is invalid.
```

The hostname can be configured via the [config file 'ServiceControl/HostName' ](/servicecontrol/servicecontrol-instances/configuration.md#host-settings-servicecontrolhostname) or the [SCMU hostname](/servicecontrol/setting-custom-hostname.md) setting.

## VoronUnrecoverableErrorException

This indicates a severe issue with the RavenDB database. To recover the database:

1. Go to the [RavenDB download page](https://ravendb.net/download)
2. Click the `Tools` button to download the `Voron.recover.exe` tool
3. Once downloaded, extract the archive
4. Open an admin console (elevated to have write permissions on protected paths)
5. Navigate to the tool folder
6. Run
    ```
    .\Voron.Recovery.exe recover "[DBFOLDERPATH]\Databases\audit" "C:\DBRecoverFolder"
    ```

The `[DBFOLDERPATH]` can be found by opening SCMU and clicking on the DB Path link for the audit instance.

Folder `C:\DBRecoverFolder` will now contain RavenDB recovery files. Rename the original database folder to act as a backup, then start the ServiceControl audit instance again. Once started, a new and empty database will be created. To [import the RavenDB recovery files into the new database](https://ravendb.net/docs/article-page/5.4/csharp/studio/database/tasks/import-data/import-data-file):

1. Open a browser
2. Navigate to: http://localhost:44445
3. Click on the **"Databases"** link in the bottom left of the screen
4. Click on the database named **"audit"**
5. Click on the **"Tasks"** icon near the top left of the screen
6. Click on **"Import data"**
7. Click **"browse"**
8. Navigate to the RavenDB recovery files
9. Select all of the recovery files to import them

Contact [Particular support](https://particular.net/support) for assistance.

## Low on storage space

ServiceControl will halt ingestion when low on storage. [Capacity planning](/servicecontrol/capacity-and-planning.md) must be done initially and reviewed at regular intervals to prevent this.

To mitigate growth or not having enough storage:

1. Mount a new disk that is larger, stop the ServiceControl instance, [move the database](/servicecontrol/configure-ravendb-location.md) to the new disk, adjust the drive letter or update the location in the ServiceControl configuration, and re-start instance

2. Enlarge the storage partition if the environment supports it:

   - Expand the partition if the drive has enough storage
   - Mount a new disk and join these with the existing disk ([JBOD](https://en.wikipedia.org/wiki/Non-RAID_drive_architectures#JBOD))

3. Lower retention and, optionally, compact database:

   - [ServiceControl - Error instance setting `ServiceControl/ErrorRetentionPeriod`](/servicecontrol/servicecontrol-instances/configuration.md#data-retention-servicecontrolerrorretentionperiod)
   - [ServiceControl - Error instance setting `ServiceControl/EventRetentionPeriod`](/servicecontrol/servicecontrol-instances/configuration.md#data-retention-servicecontroleventretentionperiod)
   - [ServiceControl - Audit instance setting `ServiceControl.Audit/AuditRetentionPeriod`](/servicecontrol/audit-instances/configuration.md#data-retention-servicecontrol-auditauditretentionperiod)
   - [ServiceControl - How to compact database](/servicecontrol/db-compaction.md)
   - [ServiceControl - How to purge expired data](/servicecontrol/how-purge-expired-data.md)

4. Disable auditing on endpoints that don't require it:

   - [NServiceBus - Message auditing](/nservicebus/operations/auditing.md#configuring-auditing)

5. Use an audit filter to filter the messages to be sent to the audit queue:

   - [NServiceBus.AuditFilter](https://github.com/NServiceBusExtensions/NServiceBus.AuditFilter)

6. Setup multiple audit instances with different retention periods if retention requirements can vary between endpoints:

   - [ServiceControl remote instances  Sharding audit messages with split audit queues](/servicecontrol/servicecontrol-instances/remotes.md#overview-sharding-audit-messages-with-split-audit-queues)

7. Scale out audit storage over multiple disks and/or machines:

   - [ServiceControl remote instances  Sharding audit messages with split audit queues](/servicecontrol/servicecontrol-instances/remotes.md#overview-sharding-audit-messages-with-split-audit-queues)

## Audit instances: Corrupted indexes or corrupted database after a service shutdown

When the following conditions are met:

- ServiceControl Audit instances are installed on Windows as a service
- The audit database size is above 100Gb
- There is a constant load on the database due to:
  - Continuously ingesting messages from the audit queue
  - Deletion of expired audit messages
- The database full-text indexes use the Corax indexing engine

There is a higher probability that the database engine cannot shut down gracefully because flushing index and journal data during a shutdown can take a considerably long time. That is due to regular operations, continuous ingestion of messages, and tombstone cleaning created by deleting expired messages.

> [!NOTE]
> Although no data will be lost, an ungraceful shutdown will delay a restart. The database engine will be required to run a lengthy recovery operation, resulting in a lot of storage I/O.

To mitigate this situation, migrating full-text search indexes from Corax to the Lucene indexing engine can solve the issue.

It might be sufficient to migrate to Lucene the `MessagesViewIndex` (even though full-text search is enabled), which has the highest load.

1. Migrate full-text indexes from the Corax to the Lucene index engine
2. Lock the index to ensure the index will not be recreated using Corax at restart


### Migrate from the Corax to the Lucene

To migrate indexes from the Corax to the Lucene indexing engine, perform the following steps:

1. Start the ServiceControl Audit instance in [maintenance mode](/servicecontrol/ravendb/accessing-database.md#windows-deployment-maintenance-mode)
2. Access the RavenDB studio
3. Edit the `MessagesViewIndex` index that needs to be changed
4. Select the index **Configuration** tab (tabs row after the first section)
5. Change the indexing engine from Corax or Corax (inherited) to Lucene
6. Click save (upper left)

At this point, there will be two indexes, the original Corax based one and the new Lucene based index. The RavenDB studio will offer the option to swap them. The swap operation will:

- Make the Lucene index the default
- Delete the Corax index

> [!NOTE]
> Indexes can be swapped immediately if storage space is an issue but search operations will return stale results until the index has been fully rebuild

After the swap operation, the new Lucene-based index must be rebuilt. Depending on the index size, the operation might take a long time.

### Lock the index

When ServiceControl is restarted, the Corax-based index may get recreated. To prevent the ServiceControl instance from recreating the index, the index can be locked.

To lock an index, from the RavenDB studio, while ServiceControl is still in maintenance mode, look for the index that was set to use Lucene and click the `🔓 Unlocked` button. Change the setting to `🔒 Locked` ([Locked Ignore](https://ravendb.net/docs/article-page/7.0/csharp/client-api/operations/maintenance/indexes/set-index-lock#lock-modes)). The RavenDB studio will notify the operation completion with the message: _Lock mode was set to: Locked (ignore)_.

## RavenDB dirty memory

_Available in version 6.5_

Each ServiceControl instance stores its data in a RavenDB database. RavenDB immediately writes data to the journal files and synchronizes writes to the data files in the background. The amount of data that needs to be flushed to disk is called "dirty memory."

Continuous dirty memory increase indicates too much pressure on the ServiceControl instance database. When that happens, the following custom check message is presented:

> There is a high level of RavenDB dirty memory ({dirtyMemoryKb}kb). See `https://docs.particular.net/servicecontrol/troubleshooting#ravendb-dirty-memory` for guidance on how to mitigate the issue.

> [!NOTE]
> The same message is logged with a warning severity in the ServiceControl instance logs.

Dirty memory issues can be mitigated using one or more of the following strategies:

- Consider adding faster storage to reduce I/O impact and allow the RavenDB instance to flush dirty memory faster
- Reduce the instance max concurrency level by reducing the `MaximumConcurrencyLevel` setting ([error instance documentation](servicecontrol-instances/configuration.md#performance-tuning-servicecontrolmaximumconcurrencylevel), [audit instance documentation](audit-instances/configuration.md#performance-tuning-servicecontrol-auditmaximumconcurrencylevel))
- If the issue affects an audit instance, consider [scaling it out using a sharding or a competing consumer approach](servicecontrol-instances/remotes.md).
