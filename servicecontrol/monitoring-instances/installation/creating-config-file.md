---
title: Monitoring Instance Configuration Settings
summary: Categorized list of ServiceControl Monitoring instance configuration settings.
reviewed: 2017-07-26
---


## Configuration Settings

The configuration of a Monitoring instance can be adjusted via ServiceControl Management or by directly modifying the `ServiceControl.Monitoring.exe.config` file. The settings listed are applicable to the app settings section of the configuration file unless otherwise specified.


## Host Settings

Please review [Setting a Custom Hostname](setting-custom-hostname.md) prior to modifying configuration settings:


### Monitoring/HttpHostname

The hostname to bind the embedded HTTP server to, modify to bind to a specific hostname, eg. `monitoring.mydomain.com`.

Type: string

Default: `localhost`

NOTE: This setting must have a value in order for the Monitoring instance API to be available from remote machines. 

### Monitoring/HttpPort

The port to bind the embedded HTTP server.

Type: int

Default: `33633`.

## Logging

### Monitoring/LogPath

The path for the Monitoring instance logs.

Type: string

Default: The folder that contains the Monitoring instance executable.


### Monitoring/LogLevel

Controls the LogLevel of the Monitoring instance logs.

Type: string

Default: `Warn`

Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

This setting will default to `Warn` if an invalid value is assigned.


## Transport


### Monitoring/TransportType

The transport type to run ServiceControl Monitor with.

Type: string

Default: `NServiceBus.MsmqTransport, NServiceBus.Core`


### NServiceBus/Transport

The connection string for the transport. This setting should be placed in `connectionStrings` section of configuration file.

Type: string


### Monitoring/ErrorQueue

The error queue name.

Type: string

Default: `error`