---
title: Customize RavenDB Embedded Path and Drive
summary: Increase space for monitored data by configuring ServiceControl to save data in a different location other than the default.
tags:
- ServiceControl
- RavenDB Embedded
- Configuration
---
ServiceControl stores its own data in a RavenDB embedded instance. By default it is located here:

`%SystemDrive\ProgramData\Particular\ServiceControl\localhost-33333`

NOTE: `localhost-33333` is the default hostname and port number. Specifying a [custom hostname and port number](setting-custom-hostname) will result in ServiceControl generating a new RavenDB embedded instance in a subdirectory corresponding to the new hostname and port number.

You may want to place the embedded RavenDB database in a different location. For example, if the system drive storage capacity is insufficient.

### Setting a Different Location for RavenDB Embedded Database

To configure ServiceControl to store data in a different location immediately after installation:
```bat
x:\Your_Installed_Path\ServiceControl.exe --restart -d="ServiceControl/DbPath==X:\your\new\path"
```

This will generate a new instance of the RavenDB embedded instance in the specified path. 

<p class="alert alert-success">
<strong>NOTE</strong>
The ServiceControl process must have read/write access to the specified path.
</p>

### Moving an Existing Installation to a Different Location

To move the ServiceControl data directory while preserving already collected data:
```bat
x:\Your_Installed_Path\ServiceControl.exe --stop -d="ServiceControl/DbPath==X:\your\new\path"
x:\Your_Installed_Path\ServiceControl.exe -d="ServiceControl/DbPath==X:\your\new\path"
copy data to new path 
x:\Your_Installed_Path\ServiceControl.exe --start
```
