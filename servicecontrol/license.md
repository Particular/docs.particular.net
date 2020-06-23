---
title: Licensing ServiceControl
reviewed: 2020-06-23
---

There are a few options to add a license to ServiceControl.

include: registry-licensing

## ServiceControl Management tool

ServiceControl has a license user interface which can be accessed in ServiceControl Management. ServiceControl Management is installed together with ServiceControl and can be found in the Windows Start Menu.

The designated license file will be imported into the file system at `%ProgramData%\ParticularSoftware\license.xml` so it is available to all instances of ServiceControl regardless of the service account used.

A copy of the imported license will be added to the `HKEY_LOCAL_MACHINE` registry hive for backward compatibility with older instances of ServiceControl.

![](managementutil-addlicense.png 'width=500')


## ServiceControl PowerShell

To import a license using PowerShell:

 * Start PowerShell from the start menu, ensure [ServiceControl PowerShell Module](/servicecontrol/powershell.md) is loaded
 * Execute the following cmdlet with the path to the license file.

```ps
Import-ServiceControlLicense <LicenseFile>
```

## License from file system

It is also possible to load the license from the file system but this must be done manually. This allows the license to be loaded from (for example) a central network share instead of the registry.

Add the `NServiceBus/LicensePath` application setting to the `ServiceControl.exe.config` configuration file. 

Note: The easiest way to find the configuration file is by launching the Service Control Management Utility (SCMU), navigate to the relavant instance and open its deployment paths.

```xml
<add key="NServiceBus/LicensePath" value="d:\NServiceBus\License\License.xml" />
```

Note: [This is the same setting to configure a license path for an NServiceBus 5 endpoint](/nservicebus/licensing/?version=core_5#license-management-using-app-config-appsettings).


## Using other platform tools

See the [ServiceInsight licensing page](/serviceinsight/license.md) or the [instructions for licensing manually using the registry](/nservicebus/licensing/?version=core_6#license-management-using-the-registry) for more information.

## Instance licensing

In Versions 1.17 and below, a license can be applied to an individual instance rather than using a license installed in the registry. To do this, copy the `license.xml` file to a `license` directory under the installation path of the instance.

NOTE: Instance Licensing is deprecated in Version 1.18 and above. Use ServiceControl Management or the ServiceControl PowerShell module to install the license file to the registry.

## Troubleshooting

### ServiceControl license was updated, but ServicePulse reports the license has expired

License information is read at service startup and cached for 8 hours. Therefore, either wait for the cache to expire or restart the ServiceControl instance manually to have ServicePulse reflect the new license.
