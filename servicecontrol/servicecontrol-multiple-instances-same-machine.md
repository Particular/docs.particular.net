---
title: Installing multiple instances of ServiceControl on the same machine 
summary: How to configure ServiceControl to be installed more than once on a single machine
tags:
- ServiceControl
- Configuration
---

## Install

It is possible to manually install ServiceControl multiple times on the same machine following these steps:

* Copy the ServiceControl installation folder to a different location, such as `C:\Program Files (x86)\Particular Software\ServiceControl-instance-2`

Note: The default ServiceControl installation folder is located under `%programfiles(x86)%\Particular Software`

* Ensure that a configuration file exists in the new location: by default ServiceControl does not require a configuration file unless you need to [customize its configuration](creating-config-file).
     * Configure [host name and port](setting-custom-hostname) according to your environment;
     * Configure [RavenDB location](configure-ravendb-location); 

* Using an elevated command prompt run, from the new instance location, the following command:

```
ServiceControl.exe -install -serviceName="custom-service-name" -displayName="Particular ServiceControl #2" -description="your description, if any, here"```

* Start the newly installed instance: `net start "custom-service-name"`## UninstallTo manually uninstall a previously configured ServiceControl service run, from an elevated command prompt, the following commands:

* `net stop "custom-service-name"` to stop the instance that needs to be removed;
* `ServiceControl.exe -uninstall -serviceName="custom-service-name"` to uninstall the Windows Service;

ServiceControl directories can be now safely removed from the system. 
