---
title: Audit Instance Configuration Settings
summary: Categorized list of ServiceControl Audit instance configuration settings.
component: ServiceControl
reviewed: 2024-06-24
redirects:
 - servicecontrol/audit-instances/creating-config-file
---
The configuration of a ServiceControl Audit instance is controlled by the `ServiceControl.Audit.exe.config` file or by setting environment variables. When a setting configuration exists as both an environment variable and in the application configuration file the environment variable setting takes precedence.

Deployments using the ServiceControl Management utility (SCMU) can use that application to make a subset of configuration settings which are read from and written to the application configuration file.

>[!NOTE]
>Configuration settings in the application configuration file are applicable to the `appSettings` section unless otherwise specified.

## Locating the configuration file using SCMU

![image](https://github.com/Particular/docs.particular.net/assets/88632084/c9b160ba-03a5-4c73-9812-c942af6657da)

## Host settings

The following documents should be reviewed prior to modifying configuration settings:

* [Setting a Custom Hostname](/servicecontrol/setting-custom-hostname.md) for guidance and details.
* [Securing ServiceControl](/servicecontrol/securing-servicecontrol.md) for an overview of the security implications of changing the configuration.

> [!WARNING]
> Changing the host name or port number of an existing ServiceControl Audit instance will break the link from the ServiceControl Error instance. See [Moving a remote instance](/servicecontrol/servicecontrol-instances/remotes.md) for guidelines on changing these settings.

### ServiceControl.Audit/InstanceName

_Added in version 5.5.0_

The name to be used by the audit instance and the name of the input queue.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_INSTANCENAME` |
| **App config key** | `ServiceControl.Audit/InstanceName` |
| **SCMU field** | Instance/Queue Name |

| Type | Default value |
| --- | --- |
| string | `Particular.ServiceControl.Audit` |

> [!NOTE]
> In versions prior to 5.5.0, the `InternalQueueName` setting can be used instead.

### ServiceControl.Audit/HostName

The hostname to bind the embedded HTTP API server to; modify this setting to bind to a specific hostname, eg. `sc.mydomain.com` and make the machine remotely accessible.

This field can also contain a `*` as a wildcard to allow remote connections that use any hostname.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_HOSTNAME` |
| **App config key** | `ServiceControl.Audit/HostName` |
| **SCMU field** | `HOST NAME` |

| Type | Default value |
| --- | --- |
| string | `localhost` |

> [!WARNING]
> If the `ServiceControl.Audit/HostName` setting is changed, and the `ServiceControl.Audit/DbPath` setting is not set, the path of the embedded RavenDB is changed. Refer to [Customize RavenDB Embedded Location](/servicecontrol/configure-ravendb-location.md).

### ServiceControl.Audit/Port

The port to bind the embedded HTTP API server.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_PORT` |
| **App config key** | `ServiceControl.Audit/Port` |
| **SCMU field** | `PORT NUMBER` |

| Type | Default value |
| --- | --- |
| int | `44444` |

> [!WARNING]
> If the `ServiceControl.Audit/Port` setting is changed, and the `ServiceControl.Audit/DbPath` setting is not set, the path of the embedded RavenDB is changed. Refer to [Customize RavenDB Embedded Location](/servicecontrol/configure-ravendb-location.md).

### ServiceControl.Audit/DatabaseMaintenancePort

The port to expose the RavenDB database.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_DATABASEMAINTENANCEPORT` |
| **App config key** | `ServiceControl.Audit/DatabaseMaintenancePort` |
| **SCMU field** | `DATABASE MAINTENANCE PORT NUMBER (1 - 49151)` |

| Type | Default value |
| --- | --- |
| int | `44445` |

> [!NOTE]
> This setting is not relevant when running an audit instance in a container.

### ServiceControl.Audit/VirtualDirectory

The virtual directory to bind the embedded HTTP server to; modify this setting to bind to a specific virtual directory.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_VIRTUALDIRECTORY` |
| **App config key** | `ServiceControl.Audit/VirtualDirectory` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | _None_ |

### ServiceControl.Audit/ShutdownTimeout

_Added in version 6.5.0_

The maximum allowed time for the process to gracefully complete the shutdown after which the process will try to terminate.

> [!NOTE]
> An ungraceful shutdown could result in the next start to require a lengthy database recovery operation.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_SHUTDOWNTIMEOUT` |
| **App config key** | `ServiceControl.Audit/ShutdownTimeout` |
| **SCMU field** | N/A |

| Environment/Installation type            | Type     | Default value |
| Containers | TimeSpan | `00:00:05` (5 seconds) |
| Installation via PowerShell (on Windows) | TimeSpan | `00:02:00` (2 minutes) |
| Installation via ServiceControl Management Utility (SCMU) (on Windows) | TimeSpan | `00:02:00` (2 minutes) |

### ServiceControl.Audit/MaintenanceMode

Run [ServiceControl audit instance in maintenance mode](/servicecontrol/ravendb/accessing-database.md) in order to do database maintenance.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MAINTENANCEMODE` |
| **App config key** | `ServiceControl.Audit/MaintenanceMode` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| boolean | `False` |

## Embedded database

These settings are not valid for ServiceControl instances hosted in a container.

### ServiceControl.Audit/DbPath

The path where the internal RavenDB is located.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_DBPATH` |
| **App config key** | `ServiceControl.Audit/DbPath` |
| **SCMU field** | `DATABASE PATH` |

| Type | Default value |
| --- | --- | --- | --- |
| string | `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB` |

> [!NOTE]
> This setting is not relevant when the audit instance is [deployed using a container](/servicecontrol/audit-instances/deployment/containers.md).

### ServiceControl.Audit/RavenDBLogLevel

Controls the LogLevel of the RavenDB logs.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_RAVENDBLOGLEVEL` |
| **App config key** | `ServiceControl.Audit/RavenDBLogLevel` |
| **SCMU field** | N/A |

#if-version [5,)
| Type | Default value |
| --- | --- |
| string | `Operations` |

Valid settings are: `None`, `Information`, `Operations`.
#end-if
#if-version [,5)
| Type | Default value |
| --- | --- |
| string | `Warn` |

Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.
#end-if

#if-version [,5)
### Raven/IndexStoragePath
> [!NOTE]
> Only supported on the RavenDB 3.5 storage engine. Use [symbolic links (soft links) to map any RavenDB storage subfolder](https://ravendb.net/docs/article-page/5.4/csharp/server/storage/customizing-raven-data-files-locations) to other physical drives.

The path for the indexes on disk.

| Context | Name |
| --- | --- |
| **Environment variable** | `RAVEN_INDEXSTORAGEPATH` |
| **App config key** | `Raven/IndexStoragePath` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB\indexes` |

> [!NOTE]
> This setting is not relevant when running an audit instance in a container.

#end-if

## Logging

### ServiceControl.Audit/LogPath

The path for the ServiceControl logs.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_LOGPATH` |
| **App config key** | `ServiceControl.Audit/LogPath` |
| **SCMU field** | `LOG PATH` |

| Type | Default value |
| --- | --- |
| string | `%LOCALAPPDATA%\Particular\ServiceControl.Audit\logs` |

> [!NOTE]
> %LOCALAPPDATA% is a user-specific path on Windows.
>
> When hosted on containers, logs are sent to **stdout** and this setting is ignored.

### ServiceControl.Audit/LogLevel

Controls the LogLevel of the ServiceControl logs.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_LOGLEVEL` |
| **App config key** | `ServiceControl.Audit/LogLevel` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `Info` |

Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

## Recoverability

### ServiceControl.Audit/TimeToRestartAuditIngestionAfterFailure

Controls the maximum time delay to wait before restarting the audit ingestion pipeline after detecting a connection problem.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_TIMETORESTARTAUDITINGESTIONAFTERFAILURE` |
| **App config key** | `ServiceControl.Audit/TimeToRestartAuditIngestionAfterFailure` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| timespan | 60 seconds |

Valid settings are between 5 seconds and 1 hour.

### ServiceControl.Audit/IngestAuditMessages

Set to `false` to disable ingesting new audit messages. Useful in some upgrade scenarios.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_INGESTAUDITMESSAGES` |
| **App config key** | `ServiceControl.Audit/IngestAuditMessages` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

### ServiceControl/IngestAuditMessages

> [!WARNING]
> This is the same setting as `ServiceControl.Audit/IngestAuditMessages` but kept for backward compatibility

Set to `false` to disable ingesting new audit messages. Useful in some upgrade scenarios.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_INGESTAUDITMESSAGES` |
| **App config key** | `ServiceControl/IngestAuditMessages` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |


## Data retention

### ServiceControl.Audit/ExpirationProcessTimerInSeconds

The number of seconds to wait between checking for expired messages.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_EXPIRATIONPROCESSTIMERINSECONDS` |
| **App config key** | `ServiceControl.Audit/ExpirationProcessTimerInSeconds` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `600` (10 minutes) |

Valid range is `0` to `10800` (3 Hours).

Setting the value to `0` will disable the expiration process. This is not recommended and it is only provided for fault finding.

#if-version [,5)
### ServiceControl.Audit/ExpirationProcessBatchSize

This controls the batch size used when deleting audit messages that have exceeded the audit retention period.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_EXPIRATIONPROCESSBATCHSIZE` |
| **App config key** | `ServiceControl.Audit/ExpirationProcessBatchSize` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `65512` |

The minimum allowed value for this setting is `10240`; there is no hard-coded maximum as this is dependent on system performance.

#end-if
### ServiceControl.Audit/AuditRetentionPeriod

The grace period to keep an audit message before it is deleted.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUDITRETENTIONPERIOD` |
| **App config key** | `ServiceControl.Audit/AuditRetentionPeriod` |
| **SCMU field** | `AUDIT RETENTION PERIOD` |

| Type | Default value |
| --- | --- |
| timespan | None (required) |

Valid range for this setting is from 1 hour to 365 days.

> [!NOTE]
> Starting with version 4.26.0, new audit instances using RavenDB 5 will use the built-in RavenDB expiration process. Changing the audit retention setting will affect only newly ingested messages. Already ingested messages will expire according to the previous retention setting value.

## Performance tuning

### ServiceControl.Audit/MaxBodySizeToStore

This setting specifies the upper limit on body size, in bytes, to be configured.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MAXBODYSIZETOSTORE` |
| **App config key** | `ServiceControl.Audit/MaxBodySizeToStore` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `102400` (100Kb) |

### ServiceControl.Audit/MaximumConcurrencyLevel

The maximum number of messages that can be concurrently pulled from the message transport.

It is important that the maximum concurrency level be incremented only if there are no verified bottlenecks in CPU, RAM, network I/O, storage I/O, and storage index lag. Higher numbers can result in faster audit message ingestion, but also consume more server resources, and can increase costs in the case of cloud transports that have associated per-operation costs. In some cases, the ingestion rate can be too high and the underlying database cannot keep up with indexing the new messages. In this case, consider lowering the maximum concurrency level to a value that still allows a suitable ingestion rate while easing the pressure on the database.

Cloud transports with higher latency can benefit from higher concurrency values, but costs can increase as well. Local transports using fast local SSD drives and low latency do not benefit as much.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MAXIMUMCONCURRENCYLEVEL` |
| **App config key** | `ServiceControl.Audit/MaximumConcurrencyLevel` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `32` in 4.12.0<br/>`10` in earlier versions |

### ServiceControl.Audit/EnableFullTextSearchOnBodies

_Added in 4.17.0_

Use this setting to configure whether the bodies of processed messages should be full-text indexed for searching.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_ENABLEFULLTEXTSEARCHONBODIES` |
| **App config key** | `ServiceControl.Audit/EnableFullTextSearchOnBodies` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

> [!NOTE]
> If the audit instance uses RavenDB 5 persistence (available starting 4.26.0), changing the full-text search setting will cause indexes to be redeployed and rebuilt. Depending on the number of documents stored, this operation might take a long time and search results won't be available until completed.

#if-version [5,)
### ServiceControl.Audit/BulkInsertCommitTimeoutInSeconds

Configures the maximum duration, in seconds, for processing a batch of audited messages.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_BULKINSERTCOMMITTIMEOUTINSECONDS` |
| **App config key** | `ServiceControl.Audit/BulkInsertCommitTimeoutInSeconds` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `60` (1 minute) |

#end-if
## Transport

### ServiceControl.Audit/TransportType

The transport type to run ServiceControl with.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_TRANSPORTTYPE` or `TRANSPORTTYPE` |
| **App config key** | `ServiceControl.Audit/TransportType` |
| **SCMU field** | `TRANSPORT` |

| Type | Default value |
| --- | --- |
| string | `MSMQ` |

Valid values are documented in the [ServiceControl transport configuration documentation](/servicecontrol/transports.md).

### NServiceBus/Transport

The connection string for the transport. This setting must be entered in the `connectionStrings` section of the configuration file when configured using the app config.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_CONNECTIONSTRING` OR `CONNECTIONSTRING` |
| **App config key** | `NServiceBus/Transport` in `connectionStrings` |
| **SCMU field** | `TRANSPORT CONNECTION STRING` |

| Type | Default value |
| --- | --- |
| string | None |

Valid values are documented in the [ServiceControl transport configuration documentation](/servicecontrol/transports.md).

### ServiceBus/AuditQueue

The name of the audit queue to ingest messages from.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICEBUS_AUDITQUEUE` |
| **App config key** | `ServiceBus/AuditQueue` |
| **SCMU field** | `AUDIT QUEUE NAME` |

| Type | Default value |
| --- | --- |
| string | `audit` |

### ServiceControl.Audit/ForwardAuditMessages

Use this setting to configure whether processed audit messages are forwarded to another queue or not. This entry should be set to `false` if there is no external process reading messages from the [`ServiceBus/AuditLogQueue`](#transport-servicebusauditlogqueue)

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_FORWARDAUDITMESSAGES` |
| **App config key** | `ServiceControl.Audit/ForwardAuditMessages` |
| **SCMU field** | `Forward audit messages?` |

| Type | Default value |
| --- | --- |
| bool | `false` (Off) |

### ServiceBus/AuditLogQueue

The audit queue name to use for forwarding audit messages. This setting is ignored unless `ServiceControl.Audit/ForwardAuditMessages` is enabled.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICEBUS_AUDITLOGQUEUE` |
| **App config key** | `ServiceBus/AuditLogQueue` |
| **SCMU field** | `AUDIT FORWARDING QUEUE NAME` |

| Type | Default value |
| --- | --- |
| string | `<AuditQueue>.log` |

> [!NOTE]
> Changing the configuration file or environment value directly will not result in the queue being created. If you are using the ServiceControl Management utility to manage your ServiceControl audit instance changing the value will create the forwarding queue if it has not been created.

### ServiceControl.Audit/ServiceControlQueueAddress

The ServiceControl primary instance queue name to use to send plugin messages (e.g. Heartbeats, Custom Checks, Saga Audit, etc ).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_SERVICECONTROLQUEUEADDRESS` |
| **App config key** | `ServiceControl.Audit/ServiceControlQueueAddress` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `Particular.ServiceControl` |

## Troubleshooting

### ServiceControl.Audit/DataSpaceRemainingThreshold

The percentage threshold for the [Message database storage space](/servicecontrol/servicecontrol-instances/#notifications-health-monitoring-message-database-storage-space) check. If the remaining hard drive space drops below this threshold (as a percentage of the total space on the drive) then the check will fail, alerting the user.

| Type | Default value |
| --- | --- |
| int | `20` |

### ServiceControl.Audit/MinimumStorageLeftRequiredForIngestion

The percentage threshold for the [Critical message database storage space](/servicecontrol/servicecontrol-instances/#notifications-health-monitoring-message-database-storage-space) check. If the remaining hard drive space drops below this threshold (as a percentage of the total space on the drive), then the check will fail, alerting the user. The message ingestion will also be stopped to prevent data loss. Message ingestion will resume once more disk space is made available.

| Type | Default value |
| --- | --- |
| int | `5` |

#if-version [,5)

### Raven/Esent/LogsPath


This setting is applicable only on instances that use the RavenDB 3.5 storage engine.

The path for the Esent logs on disk.

| Type | Default value |
| --- | --- |
| string | `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB\Logs` |

#end-if
