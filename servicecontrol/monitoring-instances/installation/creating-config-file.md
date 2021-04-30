---
title: Monitoring Instance Configuration Settings
summary: Categorized list of ServiceControl Monitoring instance configuration settings.
reviewed: 2021-04-30
---


## Configuration Settings

The configuration of a Monitoring instance can be adjusted via ServiceControl Management or by directly modifying the `ServiceControl.Monitoring.exe.config` file. The settings listed are applicable to the app settings section of the configuration file unless otherwise specified.


## Host Settings

Prior to modifying these configuration settings review [Setting a Custom Hostname](configure-the-uri.md):

### Monitoring/EndpointName
The endpoint name to be used by the monitoring instance and the name of the monitoring queue.

Type: string

Default: `Particular.Monitoring`

Warning: After changing this setting it's necessary to run the monitoring instance setup procedure by executing `ServiceControl.Monitoring.exe -s` in the command prompt. This ensures that all necessary queues are created and properly configured.

### Monitoring/HttpHostname

The hostname to bind the embedded HTTP server to, modify to bind to a specific hostname, eg. `monitoring.mydomain.com`.

Type: string

Default: `localhost`

NOTE: This setting must have a value in order for the Monitoring instance API to be available from remote machines.


### Monitoring/HttpPort

The port to bind the embedded HTTP server.

Type: int

Default: `33633`


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

### NServiceBus/Transport

The connection string for the transport. This setting should be placed in `connectionStrings` section of configuration file.

Type: string


### Monitoring/ErrorQueue

The error queue name.

Type: string

Default: `error`

### Monitoring/MaximumConcurrencyLevel

The maximum concurrency that will be used for ingesting metric messages. 

Type: int

Default: `32`
