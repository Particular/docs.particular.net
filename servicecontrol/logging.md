---
title: ServiceControl Logging
summary: Where and what ServiceControl logs and how to change the location
tags:
- ServiceControl
- Logging
redirects:
- servicecontrol/setting-custom-log-location
---

### Logging

Instances of the ServiceControl service write logging information and failed message import stack traces to the file system.  

Before v1.9 the default logging level was `Info`, however this level can be quite verbose, so from v1.9 the default logging level is now `Warn`, this level is also configurable by adding by adding the following to the `appSettings` section of the  configuration file:

```xml
<!-- Log Level Options: Trace, Debug, Info, Warn, Error, Fatal, Off -->
<add key="ServiceControl/LogLevel" value="Info" /> 
```

From Version 1.10 the RavenDB logging has been separated out into it's own log files.  These logs are located in the same directory as ServiceControl logs.  The default logging level for the RavenDB log is `Warn`.
The log level for the RavenDB Logs can be set by adding the following to the `appSettings` section of the configuration file:


```xml
<!-- Log Level Options: Trace, Debug, Info, Warn, Error, Fatal, Off -->
<add key="ServiceControl/RavenDBLogLevel" value="Info" /> 
```

### Rolling Logs

Before v1.10 The service control logs rolled based on date.  

From v1.10 the logs roll based on date or if the log exceeds 30MB.  This applies to both the ServiceControl logs and the RavenDB logs.   

### Log File Names


Before v1.10 the current log file is named `logfile.txt`.  Rolled logfiles are named `log.<sequencenumber>.txt`  
The sequence number starts at 0.  Lower numbers indicate more recent logfiles.


From v1.10 the current log files are named `logfile.<data>.txt` and  `Ravenlog.<date>.txt`.  
If the log rolled based on size the the rolled log will be contain a sequence number after the date.


### Log Retention

ServiceControl will retain 14 old logs files.

NOTE: The change in log naming in V1.10 will result in logs produced prior to 1.10 being ignored by the log cleanup process.  These old logs can safely be removed manually.  

### Critical Exception Logging

If ServiceControl experiences a critical exception when running as a Windows Service the exception information will be logged to the Windows EventLog rather than the log file.

### Logging Location

The location of the ServiceControl logs are controlled via the `ServiceControl/LogPath` configuration setting. Refer to [Customizing ServiceControl configuration](creating-config-file.md)) for more details.

If the ServiceControl configuration file does not this setting the the default logging location is used.
The default logging location is `%LOCALAPPDATA%\Particular\ServiceControl\logs`.

The `%LOCALAPPDATA%` defines a user-specific location on disk, so the logging location will be different when the service is configured as a user account. So for example

 * For LocalSystem it will evaluate to `%WINDIR%\System32\config\systemprofile\AppData\Local\Particular\ServiceControl\logs`
 * For a user account it will be `%PROFILEPATH%\AppData\Local\Particular\ServiceControl\logs`

Note: Browsing to  `%WINDIR%\System32\config\systemprofile\AppData\Local\Particular\ServiceControl\logs` can be problematic
as the default NTFS permissions on the systemprofile do not allow access. These permissions may need to be modified to gain access to the logs.


NOTE: If multiple Service Control instances are configured on the same machine ensure that the log locations for each instance are unique


#### Changing logging location via the ServiceControl Management Utility

To change the location ServiceControl stores its logs:

 * Open the ServiceControl Management Utility
 * Click the Configuration icon  for the instance you wish to modify.

![](managementutil-configuration.png)

 * Change the Log Path and click Save

When Save is clicked the the service with be restarted to apply the change.


#### Changing logging location by editing the configuration file

To change the location where ServiceControl stores its log:

 * Stop the ServiceControl service.
 * Locate/Create the ServiceControl configuration file (see [Customizing ServiceControl configuration](creating-config-file.md)).
 * Edit the configuration file, adding the following setting:

```xml
<add key="ServiceControl/LogPath" value="x:\new\log\location" />
```
 * Start the ServiceControl service.

NOTE: Ensure the account ServiceControl, is running under, has write and modify permissions to that directory.