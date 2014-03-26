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

You may want to place the embedded RavenDB database in a different location; for example, if the system drive storage capacity is insufficient.

### Setting a Different Location for RavenDB Embedded Database

To configure ServiceControl to store data in a different location immediately after installation:

 * Stop the ServiceControl service.
 * Locate the ServiceControl.dll.config configuration file in the ServiceControl installation folder.
 * Edit the configuration file to add a new setting:

```xml
<add key="ServiceControl/DbPath" value="X:\your\new\path" />
```

 * Start the ServiceControl service.

### Moving an Existing Installation to a Different Location

To move the ServiceControl data directory while preserving already collected data, do the procedure described above with an additional step: before starting the ServiceControl service, move the existing data directory to the new location.
