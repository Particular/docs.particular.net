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

Instances of the ServiceControl service write diagnostic information and failed message imports to the file system.


### Location

The location of the ServiceControl logs are controlled via the `ServiceControl/LogPath` configuration setting. Refer to [Customizing ServiceControl configuration](creating-config-file.md)) for more details.

If the ServiceControl configuration file does not this setting the the default logging location is used.
The default logging location is `%LOCALAPPDATA%\Particular\ServiceControl\logs`.

The `%LOCALAPPDATA%` defines a user-specific location on disk, so the logging location will be different when the service is configured as a user account. So for example

 * For LocalSystem it will evaluate to `%WINDIR%\System32\config\systemprofile\AppData\Local\Particular\ServiceControl\logs`
 * For a user account it will be `%PROFILEPATH%\AppData\Local\Particular\ServiceControl\logs`

Note: Browsing to  `%WINDIR%\System32\config\systemprofile\AppData\Local\Particular\ServiceControl\logs` can be problematic
as the default NTFS permissions on the systemprofile do not allow access.  These permissions may need to be modified to gain access to the logs.


NOTE: If multiple Service Control instances are configured on the same machine ensure that the log locations for each instance are unique

### Custom logging location

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


