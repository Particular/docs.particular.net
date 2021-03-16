---
title: Logging
reviewed: 2020-06-26
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

## Monitoring

It is recommended to actively monitor the ServiceControl `logfile.${shortdate}.txt` log file for any log entries with log level `ERROR` or `FATAL`.

## Customize logging

By default, ServiceControl logs to the filesystem and generates files named `logfile.${shortdate}.txt` and `ravenlog.${shortdate}.txt`. ServiceControl uses [NLog](https://nlog-project.org/) for logging and the configuration can be overridden by supplying a custom `nlog.config` configuration file in the ServiceControl, ServiceControl.Audit, and ServiceControl.Monitoring application folders. A variety of [NLog logging targets](https://nlog-project.org/config/?tab=targets) can be used to log to almost any possible destination.

NOTE: Any logging related settings (i.e. `ServiceControl/LogLevel`, `ServiceControl/LogPath`, `ServiceControl/RavenDBLogLevel`) are ignored when overriding the NLog configuration.

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

### Versions 1.9 and below

The current log file is named `logfile.txt`. The log is rolled based on date only. When the log is rolled the old log is named `log.<sequencenumber>.txt`. The sequence number starts at 0. Higher numbers indicate more recent log files.

ServiceControl will retain 14 log files. Older logs are deleted automatically.

### Versions 1.10 and above

The current ServiceControl log file is named `logfile.<date>.txt`. The current RavenDB embedded log file is named `ravenlog.<date>.txt`. The date is written in the `yyyy-MM-dd` format.

The logs are rolled based on date and size, any log exceeding 30MB will trigger the log to roll. If the log is rolled because of a date change the old log is named `<logname>.<date>.txt` where date is in the format `yyyyMMdd` and log name is either `ravenlog` or `logfile`. If the log is rolled based on size a sequence number is added e.g `<logname>.<date>.<sequence>.txt`. The sequence number starts at 0. Higher numbers indicate more recent log files. ServiceControl will retain 14 log files. Older logs are deleted automatically.

NOTE: The change in log naming will result in logs produced prior to Version 1.10 being ignored by the log cleanup process. These old logs can safely be removed manually.

## Logging Levels

Instances of the ServiceControl service write logging information and failed message import stack traces to the file system. 

To configure logging for ServiceControl Audit and Monitoring instances, refer to the [ServiceControl Audit configuration settings](/servicecontrol/audit-instances/creating-config-file.md#host-settings-servicecontrol-auditloglevel) or [ServiceControl Monitoring configuration settings](/servicecontrol/monitoring-instances/installation/creating-config-file.md#logging-monitoringloglevel) documentation pages.

### Versions 1.8.3 and below

The default logging level is `Info`.

### Versions 1.9 and above

The default logging level is `Warn`, this level is now configurable by adding the following to the `appSettings` section of the  configuration file:

Log Level Options: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

```xml
<add key="ServiceControl/LogLevel" value="Info" />
```

## RavenDB Logging

### Versions 1.9 and below

RavenDB logging is included in the ServiceControl logs. This logging is hard-coded to `ERROR` and above and is not affected by the `ServiceControl/LogLevel` configuration setting.

### Versions 1.10 and above

ServiceControl stores data in an embedded RavenDB database which generates its own log messages. In Version 1.10 these log messages have been separated out into a different log file. This file is co-located with the ServiceControl logs. The default logging level for the RavenDB logs is `Warn`. The log level for the RavenDB logs can be set by adding the following to the `appSettings` section of the configuration file:

Log Level Options: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

```xml
<add key="ServiceControl/RavenDBLogLevel" value="Info" />
```

## Critical Exception Logging

If ServiceControl experiences a critical exception when running as a Windows Service, the exception information will be logged to the Windows EventLog. If ServiceControl is running interactively, the error is shown on the console and not logged. Typically ServiceControl is only run interactively to conduct database maintenance. See [Compacting the ServiceControl RavenDB database](db-compaction.md).
