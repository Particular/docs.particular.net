---
title: Licensing ServiceControl
summary: Configure, manage and troubleshoot ServiceControl license
reviewed: 2024-06-27
component: ServiceControl
---

In general ServiceControl instances support [all of the license management options that NServiceBus supports](/nservicebus/licensing/#license-management) with the exception of code-first configuration and the Windows Registry.

> [!NOTE]
> When using the [user specific license location](/nservicebus/licensing/#license-management-user-specific-license-location) ensure that the account under which the ServiceControl instance is running has permissions to access the user folder.

In addition, ServiceControl provides additional license management techniques:

## ServiceControl Management utility (SCMU)

ServiceControl has a license user interface which can be accessed in ServiceControl Management.

#if-version [,5)
ServiceControl Management is installed together with ServiceControl and can be found in the Windows Start Menu.
#end-if
#if-version [5,)
If the ServiceControl Management application was not retained when installing instances, a new version can be downloaded to manage license files.
#end-if

The selected license file will be stored into the [machine-wide license file location](/nservicebus/licensing/#license-management-machine-wide-license-location) so it is available to all instances of ServiceControl regardless of the service account used.

![](managementutil-addlicense.png 'width=500')

## ServiceControl PowerShell

To import a license using PowerShell:

 * Ensure [ServiceControl PowerShell Module](https://www.powershellgallery.com/packages/Particular.ServiceControl.Management) is installed.
 * Start PowerShell
 * Execute the following cmdlet with the path to the license file.

snippet: ps-importlicense

## License from file system

It is also possible to load the license from any location in the file system by configuring the `NServiceBus/LicensePath` setting. This allows the license to be loaded from (for example) a central network share instead of the registry.

> [!NOTE]
> The easiest way to find the configuration file is by launching the Service Control Management Utility (SCMU), navigate to the relevant instance and open its deployment paths.

snippet: config-licensepath

> [!NOTE]
> This is the same setting to configure a license path for an NServiceBus 7 or lower endpoint. This license configuration option is [no longer supported in NServiceBus 8](/nservicebus/upgrades/7to8/#change-to-license-file-locations) or later endpoints.

## Troubleshooting

### ServiceControl license was updated, but ServicePulse reports the license has expired

License information is read by ServicePulse from the ServiceControl Error instance via the HTTP API. ServiceControl Error instances read license file during startup which is cached for 8 hours. Therefore, either wait for the cache to expire or restart the ServiceControl Error instance manually to have ServicePulse reflect the new license.
