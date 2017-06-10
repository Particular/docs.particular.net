---
title: Monitoring Configuration Settings
summary: Categorized list of ServiceMonitor configuration settings.
reviewed: 2017-06-09
---


## Configuration Settings

The configuration of a ServiceControl Monitor instance can be adjusted via ServiceControl Management or by directly modifying the `ServiceControl.Monitoring.exe.config` file. The settings listed are applicable to the app settings section of the configuration file unless otherwise specified.


## Host Settings

The following documents should be reviewed prior to modifying configuration settings:

 * [Setting a Custom Hostname](setting-custom-hostname.md) for guidance and details.
 * [Securing ServiceControl](securing-servicecontrol.md) for an overview of the security implications of changing the configuration.


#### Monitoring/HttpHostname

The hostname to bind the embedded HTTP server to, modify to bind to a specific hostname, eg. `sc.mydomain.com`.

Type: string

Default: `localhost`

#### Monitoring/HttpPort

The port to bind the embedded HTTP server.

Type: int

Default: `33333`.

#### Monitoring/LogPath

The path for the ServiceControl Monitor logs.

Type: string

Default: `%LOCALAPPDATA%\Particular\Particular.ServiceControl.Monitoring\logs`

Note: %LOCALAPPDATA% is a user specific environment variable.


#### Monitoring/LogLevel

Controls the LogLevel of the ServiceControl Monitor logs.

Type: string

Default: `Warn`

Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

This setting will default to `Warn` if an invalid value is assigned.


## Transport


#### Monitoring/TransportType

The transport type to run ServiceControl Monitor with.

Type: string

Default: `NServiceBus.MsmqTransport, NServiceBus.Core`


#### NServiceBus/Transport

The connection string for the transport. This setting should be placed in `connectionStrings` section of configuration file.

Type: string


#### Monitoring/ErrorQueue

The error queue name.

Type: string

Default: `error`
