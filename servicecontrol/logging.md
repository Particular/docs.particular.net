---
title: ServiceControl Logging 
summary: Where and what ServiceControl logs and how to change the location
tags:
- ServiceControl
- Logging
redirects:
- servicecontrol/setting-custom-log-location
---

## What is written

ServiceControl writes diagnostic information and failed message imports to the file system.

### Location 

The default location it `%LOCALAPPDATA%\Particular\ServiceControl\logs`. The `%LOCALAPPDATA%` defines a user-specific location on disk, so the logging location will be different when the service is configured as a user account. So for example

 * For LocalSystem it will evaluate to `%WINDIR%\System32\config\systemprofile\AppData\Local\Particular\ServiceControl\logs`
 * For a user account it will be `%PROFILEPATH%\AppData\Local\Particular\ServiceControl\logs`

### Custom logging location
     
To change the location on disk where ServiceControl stores its log information:

 * Stop the ServiceControl service.
 * Locate/Create the ServiceControl configuration file (see [Customizing ServiceControl configuration](creating-config-file.md)).
 * Edit the configuration file, adding the following setting:

```xml
<add key="ServiceControl/LogPath" value="x:\new\log\location" />
```

 * Start the ServiceControl service.

NOTE: Ensure the account ServiceControl, is running under, has write and modify permissions to that directory.
