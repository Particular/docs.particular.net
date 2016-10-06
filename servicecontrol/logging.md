---
title: Logging
reviewed: 2016-10-06
tags:
- ServiceControl
- Logging
redirects:
- servicecontrol/setting-custom-log-location
---

### Logging Location

The location of the ServiceControl logs are specified at install time or can also be modified later on by launching the ServiceControl Management Utility and editing the configuration settings for the instance.

The default logging location is `%LOCALAPPDATA%\Particular\ServiceControl\logs`.

The `%LOCALAPPDATA%` defines a user-specific location on disk, so the logging location will be different when the service is configured as a user account. So for example


#### For LocalSystem:

```no-highlight
%WINDIR%\System32\config\systemprofile\AppData\Local\Particular\ServiceControl\logs
```

Note: Browsing to this location can be problematic as the default NTFS permissions on the `systemprofile` do not allow access. These permissions may need to be modified to gain access to the logs.


#### For a user account:

```no-highlight
%PROFILEPATH%\AppData\Local\Particular\ServiceControl\logs
```


NOTE: If multiple Service Control instances are configured on the same machine ensure that the log locations for each instance are unique


#### Changing logging location via the ServiceControl Management Utility

To change the location ServiceControl stores its logs:

 * Open the ServiceControl Management Utility
 * Click the Configuration icon for the instance to modify.

![](managementutil-configuration.png)

 * Change the Log Path and click Save

When Save is clicked the the service with be restarted to apply the change.


### Log File Names and Retention


#### Versions 1.9 and below

The current log file is named `logfile.txt`. The log is rolled based on date only. When the log is rolled the old log is named `log.<sequencenumber>.txt`. The sequence number starts at 0. Higher numbers indicate more recent log files.

ServiceControl will retain 14 log files. Older logs are deleted automatically.


#### Versions 1.10 and above

The current ServiceControl log file is named `logfile.<date>.txt`. The current RavenDB embedded log file is named `ravenlog.<date>.txt`. The date is written in the `yyyy-MM-dd` format.

The logs are rolled based on date and size, any log exceeding 30MB will trigger the log to roll. If the log is rolled because of a date change the old log is named `<logname>.<date>.txt` where date is in the format `yyyyMMdd` and log name is either `ravenlog` or `logfile`. If the log is rolled based on size a sequence number is added e.g `<logname>.<date>.<sequence>.txt`. The sequence number starts at 0. Higher numbers indicate more recent log files. ServiceControl will retain 14 log files. Older logs are deleted automatically.

NOTE: The change in log naming will result in logs produced prior to Version 1.10 being ignored by the log cleanup process. These old logs can safely be removed manually.



### Logging Levels

Instances of the ServiceControl service write logging information and failed message import stack traces to the file system. 


#### Versions 1.8.3 and below

The default logging level is `Info`.


#### Versions 1.9 and above

The default logging level is `Warn`, this level is now configurable by adding the following to the `appSettings` section of the  configuration file:

Log Level Options: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

```xml
<add key="ServiceControl/LogLevel" value="Info" />
```


### RavenDB Logging


#### Versions 1.9 and below

RavenDB logging is included in the ServiceControl logs. This logging is hard coded to `ERROR` and above and is not affected by the `ServiceControl/LogLevel` configuration setting.


#### Versions 1.10 and above

ServiceControl stores data in an embedded RavenDB database which generates it's own log messages. In Version 1.10 these log message has been separated out into a separate  log file. This file is co-located with the ServiceControl logs. The default logging level for the RavenDB log is `Warn`. The log level for the RavenDB Logs can be set by adding the following to the `appSettings` section of the configuration file:

Log Level Options: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.

```xml
<add key="ServiceControl/RavenDBLogLevel" value="Info" />
```


### Critical Exception Logging

If ServiceControl experiences a critical exception when running as a Windows Service the exception information will be logged to the Windows EventLog. If ServiceControl is running interactively, the error is shown on the console and not logged. Typically ServiceControl is only run interactively to conduct database maintenance. See [Compacting the ServiceControl RavenDB database](db-compaction.md).