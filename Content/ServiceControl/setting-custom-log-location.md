---
title: Configuring ServiceControl Log Location
summary: How to configure ServiceControl to store logs in a different location
tags:
- ServiceControl
- Log
- Configuration
---
You can change the location on disk where ServiceControl stores its log information, as follows:

 * Stop the ServiceControl service.
 * Locate/Create the `ServiceControl.exe.config` configuration file located in the ServiceControl installation folder.
 * Edit the configuration file, adding the following setting:

```xml
<add key="ServiceControl/LogPath" value="x:\new\log\location" />
```

 * Start the ServiceControl service.
