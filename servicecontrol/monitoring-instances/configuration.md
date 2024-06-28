---
title: Monitoring Instance Configuration Settings
summary: Categorized list of ServiceControl Monitoring instance configuration settings.
component: ServiceControl
reviewed: 2024-06-24
redirects:
 - servicecontrol/monitoring-instances/installation/creating-config-file
---
The configuration of a ServiceControl Monitoring instance is controlled by the `ServiceControl.Monitoring.exe.config` file or by setting environment variables. When a setting configuration exists as both an environment variables and in the application configuration file, the environment variable setting takes precidence.

Deployments using the ServiceControl Management utility (SCMU) can use that application to make a subset of configuration settings which are read from and written to the application configuration file.

>[!NOTE]
>Configuration settings in the application configuration file are applicable to the `appSettings` section unless otherwise specified.

## Locating the configuration file using SCMU

![image](https://github.com/Particular/docs.particular.net/assets/88632084/c9b160ba-03a5-4c73-9812-c942af6657da)

## Host Settings

Prior to modifying these configuration settings review [Setting a Custom Hostname](configure-the-uri.md):

### Monitoring/EndpointName

The endpoint name to be used by the monitoring instance and the name of the monitoring queue.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_ENDPOINTNAME` |
| **App config key** | `Monitoring/EndpointName` |
| **SCMU field** | `Instance Name` |

<!-- //TODO: Confirm the field name -->

| Type | Default value |
| --- | --- |
| string | `Particular.Monitoring` |

> [!WARNING]
> After changing this setting it's necessary to run the monitoring instance setup procedure by executing `ServiceControl.Monitoring.exe -s` in the command prompt. This ensures that all necessary queues are created and properly configured. <!-- //TODO: Perhaps just link to the setup procedure so we can handle all 3 deployment types. -->

### Monitoring/HttpHostname

The hostname to bind the embedded HTTP server to, modify to bind to a specific hostname, eg. `monitoring.mydomain.com`.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_HTTPHOSTNAME` |
| **App config key** | `Monitoring/HttpHostname` |
| **SCMU field** | `Host Name` |

<!-- //TODO: Confirm field name -->

| Type | Default value |
| --- | --- |
| string | `localhost` |

> [!NOTE]
> This setting must have a value for the Monitoring instance API to be available from remote machines.

### Monitoring/HttpPort

The port to bind the embedded HTTP server.

| Context | Name |
| --- | --- |
| **Environment variable** | `MONITORING_HTTPPORT` |
| **App config key** | `Monitoring/HttpPort` |
| **SCMU field** | `Port Number` |

<!-- //TODO: Confirm field name -->

| Type | Default value |
| --- | --- |
| int | `33633` |

## Logging

### Monitoring/LogPath

The path for the Monitoring instance logs.

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
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `MSMQ` |

Valid values are documented in the [ServiceControl transport configuration documentation](/servicecontrol/transports.md).

### NServiceBus/Transport

The connection string for the transport.

| Context | Name |
| --- | --- |
| **Environment variable** | `NSERVICEBUS_TRANSPORT` |
| **App config key** | `NServiceBus/Transport` in `connectionStrings` |
| **SCMU field** | `Connection String` |

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
| **SCMU field** | N/A |

<!-- //TODO: Is it, or can you set it in SCMU? -->

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

<!-- //TODO: Is it? Can you set this in SCMU? -->

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

<!-- //TODO: Is it? Can you set this in SCMU? -->

| Type | Default value |
| --- | --- |
| timespan | 40 seconds |
