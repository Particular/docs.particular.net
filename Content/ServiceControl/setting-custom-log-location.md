---
title: Configuring the Log Location
summary: How to configure ServiceControl to store logs in a different location
tags:
- ServiceControl
- Log
- Configuration
---
When ServiceControl runs as LocalSystem, the default log file location is `%WINDIR%\System32\config\systemprofile\AppData\Local\Particular\ServiceControl\logs `.
You can change the location on disk where ServiceControl stores its log information, as follows:

 * Stop the ServiceControl service.
 * Locate/Create the ServiceControl configuration file (see [Customizing ServiceControl configuration](creating-config-file.md)).
 * Edit the configuration file, adding the following setting:

```xml
<add key="ServiceControl/LogPath" value="x:\new\log\location" />
```

 * Start the ServiceControl service.
