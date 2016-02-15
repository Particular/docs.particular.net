---
title: Configuration Settings
summary: Categorized list of ServiceControl configuration settings
tags:
- ServiceControl
---

## Configuration Settings

The configuration of a ServiceControl instance can be adjusted via the ServiceControl Management utility or by directly modifying the ServiceControl.exe.config file. The settings listed are applicable to the app settings section of the configuration file unless otherwise specified.

## Host Settings  

The following documents should be reviewed prior to modifying configuration settings:

- [Setting a Custom Hostname](setting-custom-hostname.md) for guidance and details.
- [Securing ServiceControl](securing-servicecontrol.md) for an overview of the security implications of changing the configuration.

#### ServiceControl/Hostname

The hostname to bind the embedded http server to, modify if you want to bind to a specific hostname, eg. sc.mydomain.com.

Type: string

Default: `localhost`

Warning: If the `ServiceControl/Hostname` setting is changed and the `ServiceControl/DbPath` setting is not set then the path of the embedded RavenDB is changed. Refer to [Customize RavenDB Embedded Location](configure-ravendb-location.md) 

#### ServiceControl/Port

The port to bind the embedded http server.

Type: int

Default: `33333`.

Warning: If the `ServiceControl/Port` setting is changed and the `ServiceControl/DbPath` setting is not set then the path of the embedded RavenDB is changed. Refer to [Customize RavenDB Embedded Location](configure-ravendb-location.md) 

#### ServiceControl/VirtualDirectory

The virtual directory to bind the embedded http server to, modify if you want to bind to a specific virtual directory.

Type: string

Default: `empty`

Note: This setting is provided for backward compatibility and should be considered obsolete.


#### ServiceControl/DbPath

The path where the internal RavenDB is located.

Type: string

Default: `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<Hostname>-<Port>`


#### ServiceControl/LogPath

The path for the ServiceControl logs.

Type: string

Default: `%LOCALAPPDATA%\Particular\ServiceControl\logs`

Note: %LOCALAPPDATA% is a user specific environment variable. 


#### ServiceControl/LogLevel

Controls the LogLevel of the ServiceControl logs.

Type: string

Default: Warn

This setting was introduced in version 1.9.  Valid settings are: `Debug`, `Info`, `Warn` and `Error`.   
This setting will default to `Warn` if an invalid value is assigned.  
Prior to 1.9 the log level was `Info` and could not be changed

## Data Retention

#### ServiceControl/ExpirationProcessTimerInSeconds

The number of seconds to wait between checking for expired messages. 

Type: int

Default: `600` (10 minutes). The default prior to version 1.4 was `60` (1 minute), the new default is `600` (10 minutes). Settings the value to `0` will disable the expiration process, this is not recommended and it is only provided for fault finding. Valid Range is `0` through to `10800` (3 Hours)


#### ServiceControl/ExpirationProcessBatchSize

This setting was introduced in version 1.4. This minimum allowed value for this settings is `10240`, there is no hard coded maximum as this is heavily dependent on system performance. 

Type: int

Default: `65512`.


#### ServiceControl/HoursToKeepMessagesBeforeExpiring

The number of hours to keep a message for before it is deleted,

Type: int

Default: `720` (30 days). 

Prior to 1.8.3 the valid range for this setting was `24` (1 day) through to `1440` (60 days)
From 1.8.3 the upper limit has been removed to allow for longer retention.  This was done to allow customers with low volumes of messages to retain them longer.  Setting this value too high can cause the embeddeded RavenDB to become large and unresponsive when indexing.  See [Capacity and Planning](capacity-and-planning.md)

## Performance Tuning

#### ServiceControl/MaximumMessageThroughputPerSecond

This setting was introduced in version 1.5. The setting controls the maximum throughput of messages ServiceControl will handle per second and is necessary to avoid overloading the underlying messages database. An appropriate limit ensures that the database can cope with number of insert operations. Otherwise the query performance would drop significantly and the message expiration process would stop working when under heavy insert load. Make sure to conduct thorough performance tests on your hardware before increasing this value. 

Type: int

Default: `350`.


#### ServiceControl/MaxBodySizeToStore

Up until version 1.6 ServiceControl only stores bodies of audit messages that are smaller than 100Kb.
Version 1.6 introduced this setting which allows the upper limit on body size to be configured. 

Type: int

Default: `102400` (100Kb)


#### ServiceControl/HttpDefaultConnectionLimit

This setting for version 1.6.2 and up. The maximum number of concurrent connections allowed by ServiceControl. When working with transports that operate over HTTP, number of concurrent connections can be increased to meet transport concurrency settings.

Type: string

Default: `100`


## Transport


#### ServiceControl/TransportType

The transport type to run ServiceControl with.

Type: string

Default: `NServiceBus.MsmqTransport, NServiceBus.Core`


#### NServiceBus/Transport

Type: string

The connection string for the transport. This setting should be placed in `connectionStrings` section of configuration file.


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


#### ServiceBus/AuditLogQueue

The audit queue name to use for forwarding audit messages. This only works if `ServiceControl/ForwardAuditMessages` is true.

Type: string

Default: `<AuditQueue>.log`


#### ServiceControl/ForwardAuditMessages

Use this setting to configure whether processed audit messages are forwarded to another queue or not.

Type: bool `true` or `false`

Default: `false`. From v1.5 if this setting is not explicitly set to true or false a warning is shown in the logs at start up.
See [Installation](installation.md) for details on how to set this at install time.


## Plugin Specific


#### ServiceControl/HeartbeatGracePeriod

The period that defines whether an endpoint is considered alive or not since the last received heartbeat.

Type: timespan

Default: `00:00:40` (40 secs)

When configuring heartbeat grace period, make sure it is greater than heartbeat interval defined by plugin or overwritten by [`heartbeat/interval`](/servicecontrol/plugins/heartbeat.md#configuration-heartbeat-interval) setting.

Note: When monitoring multiple endpoints, ensure that heartbeat grace period is larger than any individual heartbeat interval set by the endpoints.
