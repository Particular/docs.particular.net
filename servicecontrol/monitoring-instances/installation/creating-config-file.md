---
title: Monitoring Instance Configuration Settings
summary: Categorized ServiceControl Monitoring instance configuration settings list.
reviewed: 2024-05-22
---

## Configuration Settings

The Monitoring instance configuration can be adjusted via ServiceControl Management or by directly modifying the `ServiceControl.Monitoring.exe.config` file. Unless otherwise specified, the settings listed apply to the app settings section of the configuration file.

## Host Settings

Prior to modifying these configuration settings review [Setting a Custom Hostname](configure-the-uri.md):

### Monitoring/EndpointName

The endpoint name to be used by the monitoring instance and the name of the monitoring queue.

Type: string

Default: `Particular.Monitoring`

> [!WARNING]
> After changing this setting it's necessary to run the monitoring instance setup procedure by executing `ServiceControl.Monitoring.exe -s` in the command prompt. This ensures that all necessary queues are created and properly configured.

### Monitoring/HttpHostname

The hostname to bind the embedded HTTP server to, modify to bind to a specific hostname, eg. `monitoring.mydomain.com`.

Type: string

Default: `localhost`

> [!NOTE]
> This setting must have a value for the Monitoring instance API to be available from remote machines.

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

The connection string for the transport. This setting should be placed in the `connectionStrings` section of the configuration file.

Type: string

### Monitoring/ErrorQueue

The error queue name.

Type: string

Default: `error`

### Monitoring/MaximumConcurrencyLevel

The maximum concurrency that will be used for ingesting metric messages.

Type: int

Default: `32`

### Monitoring/EndpointUptimeGracePeriod

The time after which the endpoint is considered stale if it stops sending messages.

Type: timespan

Default: 40 seconds

## Usage Reporting

### Monitoring/ServiceControlThroughputDataQueue

The queue on which throughput data is received by ServiceControl error instance. This setting must match the equivalent [`LicensingComponent/ServiceControlThroughputDataQueue`](/servicecontrol/creating-config-file.md#usage-reporting-licensingcomponentservicecontrolthroughputdataqueue) setting on the ServiceControl error instance.

In most instances these settings do not need to be modified.

If running multiple setups of the Platform Tools (i.e. multiple versions of ServiceControl Error and monitoring instances) then modify these settings so that the queue on each Monitoring instance is matched to the queue of its error instance.

If using [MSMQ transport](/transports/msmq) and the monitoring instance is installed on a different machine to the ServiceControl error instance, then only the monitoring instance setting needs to be modified to include the machine name of the error instance in the queue address.

Version: 5.4+

Type: string

Default: `ServiceControl.ThroughputData`
