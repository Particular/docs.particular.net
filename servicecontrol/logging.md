---
title: Logging
summary: Understand about ServiceControl logs; change log location and customize logging 
reviewed: 2026-07-17
redirects:
- servicecontrol/setting-custom-log-location
---

## Logging Location

The location of the ServiceControl logs are specified at install time or can also be modified later on by launching ServiceControl Management and editing the configuration settings for the instance.

The default logging location for instances created with ServiceControl Management is `%ProgramData%\Particular\ServiceControl\<Instance Name>\logs`.

### Changing logging location via ServiceControl Management

To change the location ServiceControl stores logs:

 * Open ServiceControl Management
 * Click the Configuration icon for the instance to modify.

![ServiceControl Management utility configuration screen](managementutil-configuration.png)

 * Change the Log Path and click Save

When Save is clicked the service will be restarted to apply the change.

## Windows Event Log

All ServiceControl instances will log to the Windows Event Log as well as the ServiceControl log. The default level is `INFO` but this setting can be changed by [specifying an explicit log level](#logging-levels).

## Monitoring

It is recommended to actively monitor the ServiceControl `logfile.txt` log file for any log entries with log level `ERROR` or `FATAL`.

## Customize logging

By default, ServiceControl logs to the event log and filesystem. The active ServiceControl log file is named `logfile.txt`, while the RavenDB log file names have the format `yyyy-MM-dd-HH-mm.###.log`. ServiceControl uses [NLog](https://nlog-project.org/) for logging and the configuration can be overridden by supplying a custom `nlog.config` configuration file in the ServiceControl, ServiceControl.Audit, and ServiceControl.Monitoring application folders. A variety of [NLog logging targets](https://nlog-project.org/config/?tab=targets) can be used to log to almost any destination.

> [!NOTE]
> Any logging related settings (i.e. `ServiceControl/LogLevel`, `ServiceControl/LogPath`, `ServiceControl/RavenDBLogLevel`) are ignored when overriding the NLog configuration.

Example:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <time type="FastUTC" />
  <targets async="true">
    <target name="fileErrorsTarget" xsi:type="File" fileName="exceptions.log" keepFileOpen="true" concurrentWrites="true" layout="${longdate:universalTime=true}|${level:uppercase=true}|${threadid}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}" />
    <target name="fileTraceTarget" xsi:type="File" fileName="trace.log" keepFileOpen="true" concurrentWrites="true" layout="${longdate:universalTime=true}|${level:uppercase=true}|${threadid}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}" />
    <target name="consoleTarget" xsi:type="ColoredConsole" layout="${longdate}|${level:uppercase=true}|${threadid}|${logger}|${message}" />
    <target name="debugTarget" xsi:type="OutputDebugString" layout="${level:uppercase=true}|${threadid}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"/>
  </targets>
  <rules>
    <logger name="Raven.*" maxLevel="Info" final="true" />  <!-- BlackHole for Raven non-critical log levels -->
    <logger name="*" minlevel="Info" writeTo="consoleTarget,fileTraceTarget" />
    <logger name="*" minlevel="Trace" writeTo="debugTarget" />
    <logger name="*" minlevel="Error" writeTo="fileErrorsTarget" />
  </rules>
</nlog>
```

## Log File Names and Retention

The current ServiceControl log file is named `logfile.txt`

The logs are rolled based on date and size, any log exceeding 30MB will trigger the log to roll. If the log is rolled, the old log is named `logfile.<date>.##.txt` where date is in the format `yyyy-MM-dd`. The `##` represents a 2 digit sequence number that starts at `00`. Higher numbers indicate more recent log files. ServiceControl will retain 14 log files. Older logs are deleted automatically.

## Logging Levels

Instances of the ServiceControl service write logging information and failed message import stack traces to the Windows Event log and the file system.

To configure logging for ServiceControl Audit and Monitoring instances, refer to the [ServiceControl Audit configuration settings](/servicecontrol/audit-instances/configuration.md#logging-servicecontrol-auditloglevel) or [ServiceControl Monitoring configuration settings](/servicecontrol/monitoring-instances/configuration.md#logging-monitoringloglevel) documentation pages.

The default logging level is `Warn`, this level is now configurable by adding the following to the `appSettings` section of the  configuration file:

Log Level Options: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

```xml
<add key="ServiceControl/LogLevel" value="Info" />
```

## Authorization audit trail

When [role-based access control](/servicecontrol/security/configuration/authorization.md) is enabled (ServiceControl 6.18.0 and later), every authorization decision and message action is written to a dedicated audit trail, separate from the operational log, as [Elastic Common Schema (ECS)](https://www.elastic.co/docs/reference/ecs) JSON. See [the authorization documentation](/servicecontrol/security/configuration/authorization.md#authorization-audit-log) for the format and destinations, and the [audit-log-over-OTLP sample](/samples/servicecontrol/audit-log-otlp/) for an end-to-end pipeline into a SIEM.

The audit trail uses two logger categories so they can be handled independently:

| Category | Contents |
|----------|----------|
| `ServiceControl.Audit` | Authorization decisions (allow/deny) and operation-level message actions (e.g. "retry group X, 42 messages"). |
| `ServiceControl.Audit.Messages` | One entry per individual message affected by a bulk action. High volume on large retry/archive operations. |

### Filtering the per-message audit stream

The `ServiceControl.Audit.Messages` category emits one entry per message, so a bulk retry or archive of thousands of messages produces thousands of entries. Because ServiceControl routes these categories through [`Microsoft.Extensions.Logging`](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging), the per-message stream can be filtered independently using the standard [log-level configuration](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging#configure-logging), without affecting the operation-level trail on `ServiceControl.Audit`.

Set the minimum level for the `ServiceControl.Audit.Messages` category to `Warning` to keep only failed per-message actions (successful ones are logged at `Information`), or to `None` to drop the per-message stream entirely. Either configuration source the host reads works:

Environment variable (recommended for container deployments):

```
Logging__LogLevel__ServiceControl.Audit.Messages=Warning
```

Or an `appsettings.json` file in the instance folder:

```json
{
  "Logging": {
    "LogLevel": {
      "ServiceControl.Audit.Messages": "Warning"
    }
  }
}
```

> [!NOTE]
> This filters the category before it reaches any destination, so it applies to the `audit.json` file, the console, and the OTLP export alike. The operation-level `ServiceControl.Audit` trail — which still records that the bulk action occurred, who performed it, and the affected count — is unaffected. The same mechanism can raise or lower the level of any audit category; for example, setting `ServiceControl.Audit` to `Warning` keeps only denied decisions and failed actions.

### Exporting the audit trail over OTLP

ServiceControl can export its logs — including the audit trail — over OTLP (OpenTelemetry Protocol) in addition to, or instead of, the file/console output. Set the `LoggingProviders` setting to include `Otlp` (for example `NLog,Otlp`) and point the standard `OTEL_EXPORTER_OTLP_ENDPOINT` environment variable at a collector. Each ECS audit document is exported as the OTLP log record body.

When forwarding to Elasticsearch through the [OpenTelemetry Collector](https://opentelemetry.io/docs/collector/)'s `elasticsearch` exporter, select the exporter's `bodymap` mapping mode so the ECS document is indexed exactly as ServiceControl emitted it. In collector-contrib 0.156.0 and later the exporter's `mapping::mode` configuration option is deprecated and ignored; set the mode with the `elastic.mapping.mode` scope attribute (or the `X-Elastic-Mapping-Mode` client-metadata key) instead. The [audit-log-over-OTLP sample](/samples/servicecontrol/audit-log-otlp/) shows the full collector configuration.

## RavenDB Logging

ServiceControl stores data in an embedded RavenDB database which generates its own log messages into a different log file. This file is co-located with the ServiceControl logs. The current RavenDB embedded log file is named `<date>-<time>.<sequence>.txt`. The date is written in the `yyyy-MM-dd` format and the time is in 24 hour format `HH:mm`. The sequence number is 3 digits long and starts at `000`.

The default logging level for the RavenDB logs is `Warn`. The log level for the RavenDB logs can be set by adding the following to the `appSettings` section of the configuration file:

Log Level Options: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

```xml
<add key="ServiceControl/RavenDBLogLevel" value="Info" />
```

## Critical Exception Logging

If ServiceControl experiences a critical exception when running as a Windows Service, the exception information will be logged to the Windows EventLog. If ServiceControl is running interactively, the error is shown on the console and not logged. Typically ServiceControl is only run interactively to conduct database maintenance. See [Compacting the ServiceControl RavenDB database](/servicecontrol/db-compaction.md).
