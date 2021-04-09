---
title: Configuration Settings
summary: Categorized list of ServiceControl configuration settings.
reviewed: 2019-12-20
---


The configuration of a ServiceControl instance can be adjusted via the ServiceControl Management utility or by directly modifying the `ServiceControl.exe.config` file. The settings listed are applicable to the `appSettings` section of the configuration file unless otherwise specified.


## Host settings

The following documents should be reviewed prior to modifying configuration settings:

 * [Setting a Custom Hostname](setting-custom-hostname.md) for guidance and details.
 * [Securing ServiceControl](securing-servicecontrol.md) for an overview of the security implications of changing the configuration.


#### ServiceControl/HostName

The hostname to bind the embedded HTTP server to; modify this setting to bind to a specific hostname, e.g. `sc.mydomain.com`.

Type: string

Default: `localhost`

Warning: If the `ServiceControl/HostName` setting is changed, and the `ServiceControl/DbPath` setting is not set, the path of the embedded RavenDB is changed. Refer to [Customize RavenDB Embedded Location](configure-ravendb-location.md).


#### ServiceControl/Port

The port to bind the embedded HTTP server.

Type: int

Default: `33333`.

Warning: If the `ServiceControl/Port` setting is changed, and the `ServiceControl/DbPath` setting is not set, the path of the embedded RavenDB is changed. Refer to [Customize RavenDB Embedded Location](configure-ravendb-location.md).


#### ServiceControl/DatabaseMaintenancePort

The port to bind the RavenDB when in maintenance mode or [RavenDB is exposed](creating-config-file.md#troubleshooting-servicecontrolexposeravendb). This setting is only applicable from version 2 and above.

Type: int

Default: `33334`.


#### ServiceControl/VirtualDirectory

The virtual directory to bind the embedded HTTP server to; modify this setting to bind to a specific virtual directory.

Type: string

Default: `empty`

Note: This setting is provided for backward compatibility and should be considered obsolete.


#### ServiceControl/DbPath

The path where the internal RavenDB is located.

Type: string

Default: `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB`

The indexes and Esent logs can be stored in a different path from the the RavenDB database data files by using the following [RavenDB configuration app settings](https://ravendb.net/docs/article-page/2.5/csharp/server/administration/configuration):

#### Raven/IndexStoragePath

The path for the indexes on disk.

Type: string

Default: `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB\indexes`

#### Raven/Esent/LogsPath

The path for the Esent logs on disk.

Type: string

Default: `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB\Logs`

#### ServiceControl/LogPath

The path for the ServiceControl logs.

Type: string

Default: `%LOCALAPPDATA%\Particular\ServiceControl\logs`

Note: %LOCALAPPDATA% is a user-specific environment variable.


#### ServiceControl/LogLevel

Controls the LogLevel of the ServiceControl logs.

Type: string

Default: `Info`

In ServiceControl version 1.9 and above, valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

This setting will default to `Info` if an invalid value is assigned.

In version 1.8 and below, the log level is `Info` and can not be changed.


#### ServiceControl/RavenDBLogLevel

Controls the LogLevel of the RavenDB logs.
This setting was introduced in ServiceControl version 1.10. See [Logging](logging.md)

Type: string

Default: `Warn`

Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

This setting will default to `Warn` if an invalid value is assigned.


#### ServiceControl/TimeToRestartErrorIngestionAfterFailure

Controls the maximum time delay to wait before restarting the error ingestion pipeline after detecting a connection problem.
This setting was introduced in ServiceControl version 4.4.1.

Type: timespan

Default: 60 seconds

Valid settings are between 5 seconds and 1 hour.


## Data retention


#### ServiceControl/ExpirationProcessTimerInSeconds

The number of seconds to wait between checking for expired messages.

Type: int

Default: `600` (10 minutes). The default for ServiceControl version 1.3 and below is `60` (1 minute), Starting in version 1.4, the default is `600` (10 minutes). Setting the value to `0` will disable the expiration process. This is not recommended and it is only provided for fault finding. Valid range is `0` to `10800` (3 Hours).


#### ServiceControl/ExpirationProcessBatchSize

This setting was introduced in version 1.4. The minimum allowed value for this setting is `10240`; there is no hard-coded maximum as this is dependent on system performance.

Type: int

Default: `65512`.


#### ServiceControl/HoursToKeepMessagesBeforeExpiring

This setting is only applicable in version 1.11.1 and below.

In higher versions, this setting can be set via `ServiceControl/AuditRetentionPeriod`.

The number of hours to keep a message before it is deleted.

Type: int

Default: `720` (30 days).

In ServiceControl versions, 1.8.2 and below, the valid range for this setting is `24` (1 day) to `1440` (60 days).

Starting in versions 1.8.3, the upper limit has been removed to allow for longer retention. This was done to allow scenarios with low volumes of messages to retain them longer. Setting this value too high can cause the embedded RavenDB to become large and unresponsive when indexing. See [Capacity and Planning](capacity-and-planning.md).


#### ServiceControl/AuditRetentionPeriod

This setting is only applicable, starting from versions 1.12.

This setting is deprecated in version 4.0.0. See [ServiceControl Audit configuration](/servicecontrol/audit-instances/creating-config-file.md).

The period to keep an audit message before it is deleted.

Type: timespan

Default: There is no default; this setting is required.

Valid range for this setting is from 1 hour to 364 days.


#### ServiceControl/ErrorRetentionPeriod

This setting is only applicable, starting from version 1.12.

The grace period that faulted messages are kept before they are deleted.

For a message to be considered for deletion, it needs to have a status of either `Archived`, `RetryIssued`, or `Resolved`.

Type: timespan

Default: There is no default; this setting is required.

Valid range for this setting is between 10 days and 45 days.


#### ServiceControl/EventRetentionPeriod

This setting is only applicable, starting from version 1.25.

The period to keep event logs before they are deleted.

Type: timespan

Default: `14` (14 days).

Valid range for this setting is from 1 hour to 200 days.

## Performance tuning

#### ServiceControl/MaximumConcurrencyLevel

This setting controls how many messages can be processed concurrently (in parallel) by ServiceControl. The default value is 10.

In some cases, the ingestion rate can be too high and the underlying database cannot keep up with indexing the new messages. In this case, consider lowering the maximum concurrency level to a value that still allows a suitable ingestion rate while easing the pressure on the database.

Warning: The maximum concurrency level should be incremented only if there are no verified bottlenecks in CPU, RAM, network I/O, storage I/O, and storage index lag.

#### ServiceControl/MaximumMessageThroughputPerSecond

NOTE: This setting was removed in version 2.0.

The setting controls the maximum throughput of messages ServiceControl will handle per second and is necessary to avoid overloading the underlying messages database. An appropriate limit ensures that the database can cope with the anticipated number of insert operations. Otherwise, the query performance would drop significantly, and the message expiration process would stop working when under heavy insert load. Make sure to conduct thorough performance tests on the hardware before increasing this value.

Type: int

Default: `350`.


#### ServiceControl/MaxBodySizeToStore

This setting was introduced in version 1.6. It allows the upper limit on body size to be configured.

In version 1.5.* and below, ServiceControl stores only the bodies of audit messages that are smaller than 100Kb.

NOTE: This setting is not available in versions 4.4 and higher. It is still supported in Audit instances via [ServiceControl.Audit/MaxBodySizeToStore](/servicecontrol/audit-instances/creating-config-file.md#performance-tuning-servicecontrol-auditmaxbodysizetostore) setting

Type: int

Default: `102400` (100Kb)


#### ServiceControl/HttpDefaultConnectionLimit

This setting was introduced in version 1.6.2. The maximum number of concurrent connections allowed by ServiceControl. When working with transports that operate over HTTP, the number of concurrent connections can be increased to meet transport concurrency settings.

Type: string

Default: `100`


## Transport

#### ServiceControl/TransportType

The transport type to run ServiceControl with.

Type: string

Default: `ServiceControl.Transports.Msmq.MsmqTransportCustomization, ServiceControl.Transports.Msmq`

The assembly containing the transport type needs to be present in the ServiceControl directory for ServiceControl being able to instantiate the transport type.

#### NServiceBus/Transport

The connection string for the transport. This setting should be placed in the `connectionStrings` section of the configuration file.

Type: string

#### ServiceBus/AuditQueue

This setting is only applicable in versions 3.8.2 and below. See [ServiceControl Audit configuration](/servicecontrol/audit-instances/creating-config-file.md).

The audit queue name.

Type: string

Default: `audit`


#### ServiceBus/ErrorQueue

The error queue name.

Type: string

Default: `error`


#### ServiceBus/ErrorLogQueue

The error queue name to use for forwarding error messages.

Type: string

Default: `<ErrorQueue>.log`

Starting in version 1.29, ServiceControl creates the queue specified by this setting only if `ServiceControl/ForwardErrorMessages` is enabled. In previous versions, the queue specified by this setting is created when the service instance is installed regardless of the value of `ServiceControl/ForwardErrorMessages`.

NOTE: Changing the configuration file directly will not result in the queue being created. Use ServiceControl Management to add or alter the forwarding queue.

#### ServiceBus/AuditLogQueue

This setting is only applicable in versions 3.8.2 and below. See [ServiceControl Audit configuration](/servicecontrol/audit-instances/creating-config-file.md).

The audit queue name to use for forwarding audit messages. This works only if `ServiceControl/ForwardAuditMessages` is true.

Type: string

Default: `<AuditQueue>.log`

Starting in version 1.29, ServiceControl creates the queue specified by this setting only if `ServiceControl/ForwardAuditMessages` is enabled. In previous versions, the queue specified by this setting is created when the service instance is installed regardless of the value of `ServiceControl/ForwardAuditMessages`.


NOTE: Changing the configuration file directly will not result in the queue being created. Use ServiceControl Management to add or alter the forwarding queue. 

#### ServiceControl/ForwardAuditMessages

This setting is only applicable in versions 3.8.2 and below. See [ServiceControl Audit configuration](/servicecontrol/audit-instances/creating-config-file.md).

Use this setting to configure whether processed audit messages are forwarded to another queue or not. This queue is known as the Audit Forwarding Queue.

Type: bool `true` or `false`

Default: `false`.

In version 1.5 and above, if this setting is not explicitly set to true or false, a warning is shown in the logs at startup.

In version 1.12.0 and above, there is no default for this setting. This setting needs to be specified.

See [Installation](installation.md) for details on how to set this at install time.


#### ServiceControl/ForwardErrorMessages

This setting is only applicable from version 1.12.0 and above.

Use this setting to configure whether processed error messages are forwarded to another queue or not.

Type: bool `true` or `false`

Default: There is no default. This setting needs to be specified.

This entry should be set to `false` if there is no external process reading messages from the `Error Forwarding Queue`.

See [Installation](installation.md) for details on how to set this at install time.


## Plugin-specific


#### ServiceControl/HeartbeatGracePeriod

The period that defines whether an endpoint is considered alive or not since the last received heartbeat.

Type: timespan

Default: `00:00:40` (40 secs)

When configuring the heartbeat grace period, make sure it is greater than the [heartbeat interval defined by the plugin](/monitoring/heartbeats/install-plugin.md).

Note: When monitoring multiple endpoints, ensure that the heartbeat grace period is larger than any individual heartbeat interval set by the endpoints.

## Troubleshooting

#### ServiceControl/ExposeRavenDB

ServiceControl stores its data in a RavenDB embedded instance. By default, the RavenDB instance is accessible only by the ServiceControl service. If during troubleshooting, direct access to the RavenDB instance is required while ServiceControl is running, ServiceControl can be configured to expose the RavenDB studio. 

NOTE: [Maintenance mode](maintenance-mode.md) is the recommended way to review documents in the embedded RavenDB instance.

WARNING: The ServiceControl RavenDB embedded instance is used exclusively by ServiceControl and is not intended for external manipulation or modifications.

Type: bool

Default: `false`

After restarting the ServiceControl service, access the RavenDB studio locally at the following endpoint:

```no-highlight
http://localhost:{selected RavenDB port}/studio/index.html#databases/documents?&database=%3Csystem%3E
```

NOTE: The ServiceControl embedded RavenDB studio can be accessed from localhost regardless of the hostname customization setting. To allow external access, the hostname must be [set to a fully qualified domain name](setting-custom-hostname.md).


#### ServiceControl/DataSpaceRemainingThreshold

This setting was introduced in version 3.8. The percentage threshold for the [Message database storage space](/servicecontrol/servicecontrol-instances/#self-monitoring-via-custom-checks-message-database-storage-space) check. If the remaining hard drive space drops below this threshold (as a percentage of the total space on the drive), then the check will fail, alerting the user.

Type: int

Default: 20
