---
title: Customize RavenDB Embedded Location
summary: Increase space for monitored data by configuring ServiceControl to save data in a different location other than the default.
tags:
- ServiceControl
- RavenDB Embedded
---

ServiceControl stores its own data in a RavenDB embedded instance. By default it is located here:

`%SystemDrive%\ProgramData\Particular\ServiceControl\localhost-33333`

NOTE: The default hostname and port number `localhost-33333`. Specifying a [custom hostname and port number](setting-custom-hostname.md) will result in ServiceControl generating a new RavenDB embedded instance in a subdirectory corresponding to the new hostname and port number.

You may want to place the embedded RavenDB database in a different location. For example, if the system drive storage capacity is insufficient.

### Setting a Different Location for RavenDB Embedded Database

To configure ServiceControl to store data in a different location immediately after installation:

 * Stop the ServiceControl service.
 * Locate/Create the ServiceControl configuration file (see [Customizing ServiceControl configuration](creating-config-file.md))
 * Edit the configuration file to add a new setting:

```xml
<add key="ServiceControl/DbPath" value="X:\your\new\path" />
```

 * Start the ServiceControl service.
 
This will generate a new instance of the RavenDB embedded instance in the specified path. 

NOTE: The ServiceControl process must have read/write access to the specified path.
 

### Moving an Existing Installation to a Different Location

To move the ServiceControl data directory while preserving already collected data, perform the procedure described above with an additional step: before starting the ServiceControl service, move the existing data directory to the new location.