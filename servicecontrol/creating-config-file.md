---
title: Configuration Settings
summary: Categorized list of ServiceControl configuration settings.
reviewed: 2017-03-01
---


## Configuration Settings

The configuration of a ServiceControl instance can be adjusted via ServiceControl Management or by directly modifying the `ServiceControl.exe.config` file. The settings listed are applicable to the app settings section of the configuration file unless otherwise specified.


## Host Settings

The following documents should be reviewed prior to modifying configuration settings:

 * [Setting a Custom Hostname](setting-custom-hostname.md) for guidance and details.
 * [Securing ServiceControl](securing-servicecontrol.md) for an overview of the security implications of changing the configuration.


#### ServiceControl/Hostname

The hostname to bind the embedded HTTP server to, modify to bind to a specific hostname, eg. `sc.mydomain.com`.

Type: string

Default: `localhost`

Warning: If the `ServiceControl/Hostname` setting is changed and the `ServiceControl/DbPath` setting is not set then the path of the embedded RavenDB is changed. Refer to [Customize RavenDB Embedded Location](configure-ravendb-location.md).


#### ServiceControl/Port

The port to bind the embedded HTTP server.

Type: int

Default: `33333`.

Warning: If the `ServiceControl/Port` setting is changed and the `ServiceControl/DbPath` setting is not set then the path of the embedded RavenDB is changed. Refer to [Customize RavenDB Embedded Location](configure-ravendb-location.md).


#### ServiceControl/VirtualDirectory

The virtual directory to bind the embedded HTTP server to, modify to bind to a specific virtual directory.

Type: string

Default: `empty`

Note: This setting is provided for backward compatibility and should be considered obsolete.


#### ServiceControl/DbPath

The path where the internal RavenDB is located.

Type: string

Default: `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<Hostname>-<Port>`

The indexes and Esent logs can be stored in a different path from the the RavenDB database data files by using the following [RavenDB configuration app settings](https://ravendb.net/docs/article-page/2.5/csharp/server/administration/configuration):

#### Raven/IndexStoragePath

The path for the indexes on disk.

Type: string

Default: `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<Hostname>-<Port>\indexes`

#### Raven/Esent/LogsPath

The path for the Esent logs on disk.

Type: string

Default: `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<Hostname>-<Port>\Logs`

#### ServiceControl/LogPath

The path for the ServiceControl logs.

Type: string

Default: `%LOCALAPPDATA%\Particular\ServiceControl\logs`

Note: %LOCALAPPDATA% is a user specific environment variable.


#### ServiceControl/LogLevel

Controls the LogLevel of the ServiceControl logs.

Type: string

Default: `Warn`

In Versions 1.9 and above Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

This setting will default to `Warn` if an invalid value is assigned.

In Versions 1.8 and below the log level was `Info` and could not be changed.


#### ServiceControl/RavenDBLogLevel

Controls the LogLevel of the RavenDB logs.
This setting was introduced in Version 1.10. See [Logging](logging.md)

Type: string

Default: `Warn`

Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

This setting will default to `Warn` if an invalid value is assigned.


## Data Retention


#### ServiceControl/ExpirationProcessTimerInSeconds

The number of seconds to wait between checking for expired messages.

Type: int

Default: `600` (10 minutes). The default for Versions below 1.4 is `60` (1 minute), In Version 1.4 and above the default is `600` (10 minutes). Settings the value to `0` will disable the expiration process, this is not recommended and it is only provided for fault finding. Valid Range is `0` through to `10800` (3 Hours).


#### ServiceControl/ExpirationProcessBatchSize

This setting was introduced in Version 1.4. The minimum allowed value for this setting is `10240`, there is no hard coded maximum as this is heavily dependent on system performance.

Type: int

Default: `65512`.


#### ServiceControl/HoursToKeepMessagesBeforeExpiring

This setting is only applicable in Version 1.11.1 and below.

In higher versions this setting can now be set via `ServiceControl/AuditRetentionPeriod`.

The number of hours to keep a message for before it is deleted.

Type: int

Default: `720` (30 days).

In Versions 1.8.2 and below the valid range for this setting was `24` (1 day) through to `1440` (60 days).

In Versions 1.8.3 and above the upper limit has been removed to allow for longer retention. This was done to allow customers with low volumes of messages to retain them longer. Setting this value too high can cause the embedded RavenDB to become large and unresponsive when indexing. See [Capacity and Planning](capacity-and-planning.md).


#### ServiceControl/AuditRetentionPeriod

This setting is only applicable from Versions 1.12 and above.

The period to keep an audit message for before it is deleted.

Type: timespan

Default: There is no default. This setting needs to be specified.

Valid range for this setting is minimum 1 hour and maximum 364 days.


#### ServiceControl/ErrorRetentionPeriod

This setting is only applicable from Version 1.12 and above.

The grace period that faulted messages are kept before they are deleted.

For a message to be considered for deletion, it needs to have a status of either `Archived`, `RetryIssued` or `Resolved`.

Type: timespan

Default: There is no default. This setting needs to be specified.

Valid range for this setting is minimum 10 days and maximum 45 days.


#### ServiceControl/EventRetentionPeriod

This setting is only applicable from Versions 1.25 and above.

The period to keep event logs for before they are deleted.

Type: timespan

Default: `14` (14 days).

Valid range for this setting is minimum 1 hour and maximum 200 days.


## Performance Tuning


#### ServiceControl/MaximumMessageThroughputPerSecond

This setting was introduced in Version 1.5. The setting controls the maximum throughput of messages ServiceControl will handle per second and is necessary to avoid overloading the underlying messages database. An appropriate limit ensures that the database can cope with number of insert operations. Otherwise the query performance would drop significantly and the message expiration process would stop working when under heavy insert load. Make sure to conduct thorough performance tests on the hardware before increasing this value.

Type: int

Default: `350`.


#### ServiceControl/MaxBodySizeToStore

This setting exists in Version 1.6 and above. It allows the upper limit on body size to be configured.

In Version 1.5.x and below ServiceControl only stores bodies of audit messages that are smaller than 100Kb.

Type: int

Default: `102400` (100Kb)


#### ServiceControl/HttpDefaultConnectionLimit

This setting exists in Version 1.6.2 and above. The maximum number of concurrent connections allowed by ServiceControl. When working with transports that operate over HTTP, number of concurrent connections can be increased to meet transport concurrency settings.

Type: string

Default: `100`


## Transport


#### ServiceControl/TransportType

The transport type to run ServiceControl with.

Type: string

Default: `NServiceBus.MsmqTransport, NServiceBus.Core`


#### NServiceBus/Transport

The connection string for the transport. This setting should be placed in `connectionStrings` section of configuration file.

Type: string

#### ServiceBus/AuditQueue

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

Versions 1.28 and below created the queue specified by this setting when the service instance is installed regardless of the value of `ServiceControl/ForwardErrorMessages`.  Versions 1.29 and above only create the queue if `ServiceControl/ForwardErrorMessages` is enabled. 

NOTE: Changing the configuration file directly will not result in the queue being created. Use ServiceControl Management to add or alter the forwarding queue.

#### ServiceBus/AuditLogQueue

The audit queue name to use for forwarding audit messages. This only works if `ServiceControl/ForwardAuditMessages` is true.

Type: string

Default: `<AuditQueue>.log`

Versions 1.28 and below created the queue specified by this setting when the service instance is installed regardless of the value of  `ServiceControl/ForwardAuditMessages`.  Versions 1.29 and above only create the queue if `ServiceControl/ForwardAuditMessages` is enabled.

NOTE: Changing the configuration file directly will not result in the queue being created. Use ServiceControl Management to add or alter the forwarding queue. 

#### ServiceControl/ForwardAuditMessages

Use this setting to configure whether processed audit messages are forwarded to another queue or not. This queue is known as the Audit Forwarding Queue.

Type: bool `true` or `false`

Default: `false`.

In Versions 1.5 and above if this setting is not explicitly set to true or false a warning is shown in the logs at start up.

In Versions 1.12.0 and above there is no default for this setting. This setting needs to be specified.

See [Installation](installation.md) for details on how to set this at install time.


#### ServiceControl/ForwardErrorMessages

This setting is only applicable from Versions 1.12.0 and above.

Use this setting to configure whether processed error messages are forwarded to another queue or not.

Type: bool `true` or `false`

Default: There is no default. This setting needs to be specified.

This entry should be set to `false` if there is no external process reading messages from the `Error Forwarding Queue`.

See [Installation](installation.md) for details on how to set this at install time.


## Plugin Specific


#### ServiceControl/HeartbeatGracePeriod

The period that defines whether an endpoint is considered alive or not since the last received heartbeat.

Type: timespan

Default: `00:00:40` (40 secs)

When configuring heartbeat grace period, make sure it is greater than [heartbeat interval defined by plugin](/monitoring/heartbeats/install-plugin.md).

Note: When monitoring multiple endpoints, ensure that heartbeat grace period is larger than any individual heartbeat interval set by the endpoints.

## Troubleshooting

#### ServiceControl/ExposeRavenDB

ServiceControl stores its data in a RavenDB embedded instance. By default, the RavenDB instance is accessible only by the ServiceControl service. If, during troubleshooting, direct access to the RavenDB instance is required while ServiceControl is running, ServiceControl can be configured to expose the RavenDB studio. 

NOTE: [Maintenance mode](use-ravendb-studio.md) is the recommended way to review documents in the embedded RavenDB instance.

WARNING: The ServiceControl RavenDB embedded instance is used exclusively by ServiceControl and is not intended for external manipulation or modifications.

Type: bool

Default: `false`

After restarting the ServiceControl service, access the RavenDB studio locally at the following endpoint:

```no-highlight
http://localhost:33333/storage
```

NOTE: The ServiceControl embedded RavenDB studio can be accessed from localhost regardless of the hostname customization setting. To allow external access the hostname must be [set to a fully qualified domain name](setting-custom-hostname.md).
