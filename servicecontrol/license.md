---
title: Licensing ServiceControl
tags:
 - servicecontrol
 - license
 - platform
---


## Licensing ServiceControl

The following options outline how to add a license to ServiceControl.

### ServiceControl Management Utility

The ServiceControl Management utility allow has a simple Add License user interface  option which will import the designated license file into the registry. The license file is added to the `HKEY_LOCAL_MACHINE` registry hive so it available to be all instances of ServiceControl regardless of the service account used.
  

![](managementutil-addlicense.png)


### ServiceControl PowerShell

To import a license using PowerShell:

* Start the ServiceControl PowerShell Management Console from the start menu 
* Execute the following cmdlet with the path to the license file.

```bat
Import-ServiceControlLicense <LicenseFile>
```

### Deprecated Licensing method
It is also possible to apply a license to an individual instance rather than globally. This can be done by by creating a license folder under the installation path of an instance and copying the `license.xml` to that directory.
Adding a license this way is not supported via the ServiceControl Management Utility or the PowerShell module.


### Troubleshooting


#### ServiceControl license was updated but ServicePulse reports the license has expired

ServiceControl reads license information at service start up and caches it.  Once a new license is applied the ServiceControl instance must be restarted to pick up the license change, in the meantime the license status shown in ServicePulse is based on the cached state.

