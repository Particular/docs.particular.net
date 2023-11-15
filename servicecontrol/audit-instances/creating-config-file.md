---
title: Configuration Settings
summary: Categorized list of ServiceControl Audit configuration settings.
reviewed: 2022-10-27
---


The configuration of a ServiceControl Audit instance can be adjusted via the ServiceControl Management utility or by directly modifying the `ServiceControl.Audit.exe.config` file. The settings listed are applicable to the `appSettings` section of the configuration file unless otherwise specified.

![image](https://github.com/Particular/docs.particular.net/assets/88632084/c9b160ba-03a5-4c73-9812-c942af6657da)



## Host settings

Anyone who can access the ServiceControl Audit instance URL has complete access to the audit data stored by the ServiceControl Audit instance. This is why the default is to only respond to `localhost`. Consider carefully the implications of exposing a ServiceControl Audit instance via a custom or wildcard URI.

WARN: Changing the host name or port number of an existing ServiceControl Audit instance will break the link from the ServiceControl Error instance. See [Moving a remote instance](/servicecontrol/servicecontrol-instances/remotes.md) for guidelines on changing these settings.

### ServiceControl.Audit/HostName

The hostname to bind the embedded HTTP server to; modify this setting to bind to a specific hostname, eg. `sc.mydomain.com`.

Type: string

Default: `localhost`

### ServiceControl.Audit/Port

The port to bind the embedded HTTP server.

Type: int

Default: `44444`

### ServiceControl.Audit/DatabaseMaintenancePort

The port to bind the RavenDB when in [maintenance mode](/servicecontrol/audit-instances/maintenance-mode.md).

Type: int

### ServiceControl.Audit/DbPath

The path where the internal RavenDB is located.

Type: string

### Raven/IndexStoragePath

INFO: Only supported on RavenDB 3.5 storage engine (prior version 5). Use [symbolic links (soft links) to map any RavenDB storage subfolder](https://ravendb.net/docs/article-page/5.4/csharp/server/storage/customizing-raven-data-files-locations) to other physical drives.

The path for the indexes on disk.

Type: string

Default: `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB\indexes`

#### ServiceControl.Audit/LogPath

The path for the ServiceControl logs.

Type: string

Default: `%LOCALAPPDATA%\Particular\ServiceControl.Audit\logs`

Note: %LOCALAPPDATA% is a user-specific environment variable.

### ServiceControl.Audit/LogLevel

Controls the LogLevel of the ServiceControl logs.

Type: string

Default: `Warn`

Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

This setting will default to `Warn` if an invalid value is assigned.

### ServiceControl.Audit/RavenDBLogLevel

Controls the LogLevel of the RavenDB logs.

Type: string

Default: `Warn`

Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

This setting will default to `Warn` if an invalid value is assigned.

### ServiceControl.Audit/TimeToRestartAuditIngestionAfterFailure

Controls the maximum time delay to wait before restarting the audit ingestion pipeline after detecting a connection problem.
This setting was introduced in ServiceControl version 4.7.0.

Type: timespan

Default: 60 seconds

Valid settings are between 5 seconds and 1 hour.

### ServiceControl.Audit/InternalQueueName

Controls the name of the internal queue that ServiceControl uses for internal control messages. This can be used when the internal queue name does not match the Windows Service Name.

This setting was introduced in ServiceControl version 4.27.0.

Type: string

Default: The Service Name

## Data retention

### ServiceControl.Audit/ExpirationProcessTimerInSeconds

The number of seconds to wait between checking for expired messages.

Type: int

Default: `600` (10 minutes). The default for ServiceControl version 1.3 and below is `60` (1 minute), Starting in version 1.4, the default is `600` (10 minutes). Setting the value to `0` will disable the expiration process. This is not recommended and it is only provided for fault finding. Valid range is `0` to `10800` (3 Hours).

### ServiceControl.Audit/ExpirationProcessBatchSize

The minimum allowed value for this setting is `10240`; there is no hard-coded maximum as this is dependent on system performance.

Type: int

Default: `65512`.

### ServiceControl.Audit/AuditRetentionPeriod

The period to keep an audit message before it is deleted.

Type: timespan

Valid range for this setting is from 1 hour to 365 days.

NOTE: Starting with version 4.26.0, new audit instances using RavenDB 5 will use the built-in RavenDB expiration process. Changing the audit retention setting will affect only newly ingested messages. Already ingested messages will expire according to the previous retention setting value.

## Performance tuning

### ServiceControl.Audit/MaxBodySizeToStore

This setting specifies the upper limit on body size to be configured.

Type: int

Default: `102400` (100Kb)

### ServiceControl.Audit/HttpDefaultConnectionLimit

The maximum number of concurrent connections allowed by ServiceControl. When working with transports that operate over HTTP, the number of concurrent connections can be increased to meet transport concurrency settings.

Type: string

Default: `100`

### ServiceControl.Audit/MaximumConcurrencyLevel

The maximum number of messages that can be concurrently pulled from the message transport.

It is important that the maximum concurrency level be incremented only if there are no verified bottlenecks in CPU, RAM, network I/O, storage I/O, and storage index lag. Higher numbers can result in faster audit message ingestion, but also consume more server resources, and can increase costs in the case of cloud transports that have associated per-operation costs. In some cases, the ingestion rate can be too high and the underlying database cannot keep up with indexing the new messages. In this case, consider lowering the maximum concurrency level to a value that still allows a suitable ingestion rate while easing the pressure on the database.

ServiceControl version 4.12.0 introduced batch ingestion, which allows for multiple audit messsages (up to the maximum concurrency level) to be persisted to the database in a batch. With this change, the default was changed from `10` to `32`. Cloud transports with higher latency can benefit from higher concurrency values, but costs can increase as well. Local transports using fast local SSD drives and low latency do not benefit as much.

Type: int

Default:

* In ServiceControl version 4.12 and above: `32`
* In ServiceControl version 4.11 and below: `10`

#### ServiceControl.Audit/EnableFullTextSearchOnBodies

This setting is only applicable starting from version 4.17.0.

Use this setting to configure whether the bodies of processed messages should be full-text indexed for searching.

Type: bool `true` or `false`

Default: `true`.

NOTE: If the audit instance uses RavenDB 5 persistence (available starting 4.26.0), changing the full-text search setting will cause indexes to be redeployed and rebuilt. Depending on the number of documents stored, this operation might take a long time and search results won't be available until completed.

## Transport

### ServiceControl.Audit/TransportType

The transport type to run ServiceControl with.

Type: string

Default: `ServiceControl.Transports.Msmq.MsmqTransportCustomization, ServiceControl.Transports.Msmq`

The assembly containing the transport type needs to be present in the ServiceControl directory for ServiceControl being able to instantiate the transport type.

### NServiceBus/Transport

The connection string for the transport. This setting should be placed in the `connectionStrings` section of the configuration file.

Type: string

### ServiceBus/AuditQueue

The audit queue name.

Type: string

Default: `audit`

### ServiceBus/AuditLogQueue

The audit queue name to use for forwarding audit messages. This works only if `ServiceControl.Audit/ForwardAuditMessages` is true.

Type: string

Default: `<AuditQueue>.log`

NOTE: Changing the configuration file directly will not result in the queue being created. Use ServiceControl Management to add or alter the forwarding queue.

### ServiceControl/IngestAuditMessages

Set to `false` to disable ingesting new audit messages. Useful in some upgrade scenarios. _Available in version 5.0.0 and above._

Type: bool `true` or `false`

Default: `true`

### ServiceControl.Audit/ForwardAuditMessages

Use this setting to configure whether processed audit messages are forwarded to another queue or not. This queue is known as the Audit Forwarding Queue.

Type: bool `true` or `false`

Default: `false`.

### ServiceControl.Audit/ServiceControlQueueAddress

The ServiceControl queue name to use for plugin messages (e.g. Heartbeats, Custom Checks, Saga Audit, etc ).

Type: string

## Troubleshooting

ServiceControl Audit stores its data in a RavenDB embedded instance. If direct access to the RavenDB instance is required for troubleshooting while ServiceControl Audit is running, use the following instructions.

NOTE: [Maintenance mode](/servicecontrol/audit-instances/maintenance-mode.md) is the recommended way to review documents in the embedded RavenDB instance.

WARNING: The ServiceControl RavenDB embedded instance is used exclusively by ServiceControl Audit and is not intended for external manipulation or modifications.

### RavenDB 5

For instances running version 4.26 and above, which are configured to use [the new RavenDB 5 persistence](/servicecontrol/upgrades/new-persistence.md) browse to:

```no-highlight
http://localhost:{configured ServiceControl instance maintenance port}
```

to access the internal database via [the RavenDB studio interface](https://ravendb.net/docs/article-page/5.4/csharp/studio/overview).

### RavenDB 3.5

For instances running version 4.25 and below or using the old RavenDB 3.5 persistence the ServiceControl Audit instance can be configured to expose the RavenDB studio.

#### ServiceControl.Audit/ExposeRavenDB

Type: bool

Default: `false`

After restarting the ServiceControl Audit service, access the RavenDB studio locally at the following endpoint:

```no-highlight
http://localhost:{configured ServiceControl instance maintenance port}/studio/index.html#databases/documents?&database=%3Csystem%3E
```

NOTE: The ServiceControl Audit embedded RavenDB studio can be accessed from localhost regardless of the hostname customization setting. To allow external access, the hostname must be [set to a fully qualified domain name](/servicecontrol/setting-custom-hostname.md).

#### ServiceControl.Audit/DataSpaceRemainingThreshold

The percentage threshold for the [Message database storage space](/servicecontrol/servicecontrol-instances/#self-monitoring-via-custom-checks-message-database-storage-space) check. If the remaining hard drive space drops below this threshold (as a percentage of the total space on the drive) then the check will fail, alerting the user.

Type: int

Default: 20

#### ServiceControl.Audit/MinimumStorageLeftRequiredForIngestion

The percentage threshold for the [Critical message database storage space](/servicecontrol/servicecontrol-instances/#self-monitoring-via-custom-checks-critical-message-database-storage-space) check. If the remaining hard drive space drops below this threshold (as a percentage of the total space on the drive), then the check will fail, alerting the user. The message ingestion will also be stopped to prevent data loss. Message ingestion will resume once more disk space is made available.

Type: int

Default: 5

#### Raven/Esent/LogsPath


This setting is applicable only on instances that use the RavenDB 3.5 storage engine.

The path for the Esent logs on disk.

Type: string

Default: `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB\Logs`
