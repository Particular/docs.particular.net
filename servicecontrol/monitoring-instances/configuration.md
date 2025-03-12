---
title: Monitoring Instance Configuration Settings
summary: Categorized list of ServiceControl Monitoring instance configuration settings.
component: ServiceControl
reviewed: 2024-06-24
redirects:
 - servicecontrol/monitoring-instances/installation/creating-config-file
---
The configuration of a ServiceControl Monitoring instance is controlled by the `ServiceControl.Monitoring.exe.config` file or by setting environment variables. When a setting configuration exists as both an environment variables and in the application configuration file, the environment variable setting takes precedence.

Deployments using the ServiceControl Management utility (SCMU) can use that application to make a subset of configuration settings which are read from and written to the application configuration file.

>[!NOTE]
>Configuration settings in the application configuration file are applicable to the `appSettings` section unless otherwise specified.

## Locating the configuration file using SCMU

![image](https://github.com/Particular/docs.particular.net/assets/88632084/c9b160ba-03a5-4c73-9812-c942af6657da)

## Host Settings

Prior to modifying these configuration settings review [Setting a Custom Hostname](configure-the-uri.md):

### Monitoring/InstanceName

_Added in version 5.5.0_

The name to be used by the monitoring instance and the name of the monitoring queue.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_INSTANCENAME` |
| **App config key** | `Monitoring/InstanceName` |
| **SCMU field** | Instance/Queue Name |

| Type | Default value |
| --- | --- |
| string | `Particular.Monitoring` |

> [!WARNING]
> After changing this setting it's necessary to run the monitoring instance setup procedure by executing `ServiceControl.Monitoring.exe -s` in the command prompt. This ensures that all necessary queues are created and properly configured.

### Monitoring/HttpHostname

The hostname to bind the embedded HTTP server to, modify to bind to a specific hostname, eg. `monitoring.mydomain.com`.

_Not applicable to container deployments. Containers bind to any hostname._

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_HTTPHOSTNAME` |
| **App config key** | `Monitoring/HttpHostname` |
| **SCMU field** | Host Name |

| Type | Default value |
| --- | --- |
| string | `localhost` |

> [!NOTE]
> This setting must have a value for the Monitoring instance API to be available from remote machines.

### Monitoring/HttpPort

The port to bind the embedded HTTP server.

_Not applicable to container deployments. Containers always expose port `33633`._

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_HTTPPORT` |
| **App config key** | `Monitoring/HttpPort` |
| **SCMU field** | Port Number |

| Type | Default value |
| --- | --- |
| int | `33633` |

### Monitoring/ShutdownTimeout

_Added in version 6.5.0_

The maximum allowed time for the process to complete the shutdown.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_SHUTDOWNTIMEOUT` |
| **App config key** | `Monitoring/ShutdownTimeout` |
| **SCMU field** | N/A |

| Environment/Installation type            | Type     | Default value |
| Containers | TimeSpan | `00:00:05` (5 seconds) |
| Installation via PowerShell (on Windows) | TimeSpan | `00:02:00` (2 minutes) |
| Installation via ServiceControl Management Utility (SCMU) (on Windows) | TimeSpan | `00:02:00` (2 minutes) |

## Logging

### Monitoring/LogPath

The path for the Monitoring instance logs.

_Not applicable to container deployments. Containers always log to stdout._

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_LOGPATH` |
| **App config key** | `Monitoring/LogPath` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | The folder that contains the Monitoring instance executable. |

### Monitoring/LogLevel

Controls the LogLevel of the Monitoring instance logs.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_LOGLEVEL` |
| **App config key** | `Monitoring/LogLevel` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `Warn` |

Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

This setting will default to `Warn` if an invalid value is assigned.

## Transport

### Monitoring/TransportType

The transport type to run ServiceControl Monitor with.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_TRANSPORTTYPE` |
| **App config key** | `Monitoring/TransportType` |
| **SCMU field** | Transport |

| Type | Default value |
| --- | --- |
| string | None |

Valid values are documented in the [ServiceControl transport configuration documentation](/servicecontrol/transports.md).

### NServiceBus/Transport

The connection string for the transport.

| Context | Name |
| --- | --- |
| **Environment variable** | `NSERVICEBUS_TRANSPORT` |
| **App config key** | `NServiceBus/Transport` in `connectionStrings` |
| **SCMU field** | Connection String |

| Type | Default value |
| --- | --- |
| string | None |

Valid values are documented in the [ServiceControl transport configuration documentation](/servicecontrol/transports.md).

### Monitoring/ErrorQueue

The error queue name.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_ERRORQUEUE` |
| **App config key** | `Monitoring/ErrorQueue` |
| **SCMU field** | Error Queue Name |

| Type | Default value |
| --- | --- |
| string | `error` |

### Monitoring/MaximumConcurrencyLevel

The maximum concurrency that will be used for ingesting metric messages.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_MAXIMUMCONCURRENCYLEVEL` |
| **App config key** | `Monitoring/MaximumConcurrencyLevel` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `32` |

### Monitoring/EndpointUptimeGracePeriod

The time after which the endpoint is considered stale if it stops sending messages.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_ENDPOINTUPTIMEGRACEPERIOD` |
| **App config key** | `Monitoring/EndpointUptimeGracePeriod` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| TimeSpan | `00:00:40` (40 seconds) |

## Usage Reporting

### Monitoring/ServiceControlThroughputDataQueue

_Added in version 5.4.0_

The queue on which throughput data is received by ServiceControl error instance. This setting must match the equivalent [`LicensingComponent/ServiceControlThroughputDataQueue`](/servicecontrol/servicecontrol-instances/configuration.md#usage-reporting-when-using-servicecontrol-licensingcomponentservicecontrolthroughputdataqueue) setting on the ServiceControl error instance.

In most instances these settings do not need to be modified.

If running multiple setups of the Platform Tools (i.e. multiple versions of ServiceControl Error and monitoring instances) then modify these settings so that the queue on each Monitoring instance is matched to the queue of its error instance.

If using [MSMQ transport](/transports/msmq) and the monitoring instance is installed on a different machine to the ServiceControl error instance, then only the monitoring instance setting needs to be modified to include the machine name of the error instance in the queue address.

If using [PostgreSQL transport](/transports/postgresql/), and a schema other than `public` is required, then the schema name needs to be included in the `Monitoring/ServiceControlThroughputDataQueue` setting

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_SERVICECONTROLTHROUGHPUTDATAQUEUE` |
| **App config key** | `Monitoring/ServiceControlThroughputDataQueue` |

| Type | Default value |
| --- | --- |
| string | `ServiceControl.ThroughputData` |
