---
title: Install the NServiceBus license
summary: 'Make sure the NServiceBus endpoints use the license: Configuration API, app.config, subfolder in the BIN directory, or registry.'
tags:
- license
redirects:
 - nservicebus/license-management
---

## Overview

There are several ways to make sure that the NServiceBus endpoints pick up and use the license.


## Using the Registry

NOTE: Using the NServiceBus PowerShell cmdlet is the preferred and simplest method of adding the license file.

Using the registry to store the license information is a way that all platform tools can access this information easily. This includes NServiceBus endpoints, ServiceControl, and ServiceInsight. (ServicePulse determines licensing status by querying the ServiceControl API.) Using the registry ensures that all the platform tools can access the license status without requiring additional complexity on every deployment.


### NServiceBus Version 4.5 and above


#### Using the NServiceBus Powershell Cmdlet

The standalone NServiceBus PowerShell Version 5.0 includes a commandlet for importing the Platform License into the `HKEY_LOCAL_MACHINE` registry. See [Managing Using PowerShell](/nservicebus/operations/management-using-powershell.md) for more details and installation instructions for the PowerShell Module.

For 64-bit operating systems the license is written to both the 32-bit and 64-bit registry. The license is stored is `HKEY_LOCAL_MACHINE\Software\ParticularSoftware\License` and `HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\ParticularSoftware`


#### Other Manual Options

These following instructions cover installing the license file without using NServiceBus PowerShell Module. These options give a bit more flexibility as they support storing the the license in `HKEY_CURRENT_USER`. If the licenses is stored in `HKEY_CURRENT_USER` it is only accessible to the current user.


##### Using PowerShell Command Prompt

* Open an administrative PowerShell prompt.
* Change the current working directory to where the license.xml file is.
* Run the following script

```ps
$content = Get-Content license.xml | Out-String
Set-ItemProperty -Path HKLM:\Software\ParticularSoftware -Name License -Force -Value $content
```

If modifying the registry directly using Registry Editor or a PowerShell script to update the license for ServiceControl, it is necessary to restart the ServiceControl service, as it only checks for its license information once at startup.

NOTE: For 64 bit operating systems repeat the process in both the Powershell prompt and the PowerShell(x86) console prompt. This will ensure the license is imported into both the 32 bit and 64 bit registry keys.


##### Using Registry Editor

 * Run `regedit.exe` (usually located in `%windir%`, e.g. `C:\Windows`)
 * Go to `HKEY_LOCAL_MACHINE\Software\ParticularSoftware` or `HKEY_CURRENT_USER\Software\ParticularSoftware`
 * Create a new Multi-String Value (`REG_MULTI_SZ`) named `License`
 * Paste the contents of the license file.

If `HKEY_LOCAL_MACHINE`is the chosen license location and the operating system is 64-bit then repeat the import process for the `HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\ParticularSoftware` key to support 32-bit clients.

It is safe to ignore any warnings regarding empty strings.


### NServiceBus Version 3.3

NServiceBus Version 3.3 supports storing the license in a registry key called `[HKEYCURRENTUSER\Software\NServiceBus\{Major.Minor}\License]`.

To install the license in the registry, use one of these options:

 * The `LicenseInstaller.exe` tool that comes with the NServiceBus install.
 * The [Install-License](/nservicebus/operations/management-using-powershell.md) PowerShell commandlet.
 * If the trial license has expired and running in debug mode, the endpoint shows a dialog that allows installing the license.


### NServiceBus 4.x versions prior to Version 4.5

When installed using the [Install-NServiceBusLicense](/nservicebus/operations/management-using-powershell.md) PowerShell commandlet and the `LicenseInstaller.exe` tool that comes with the NServiceBus install, in NServiceBus Version 4.0, the license file was stored under `HKLM\Software\NServiceBus\{Major.Minor}\License` and in Version 4.3, this location was moved to `HKLM\Software\ParticularSoftware\NServiceBus\License`.

In order to install the license file under HKCU (same location in Version 3.3), use the `-c` option on the `LicenseInstaller.exe`


## Using a sub-directory in the BIN directory

To have NServiceBus automatically pick up the License.xml file, place it in a subfolder named /License under the BIN folder.


## Using the app.config settings

It is possible to specify the license in `app.config`:

-   Use the key `NServiceBus/LicensePath` to specify the path where NServiceBus looks for the license. For example:

```xml
<appSettings>
  <add key="NServiceBus/LicensePath"
       value="C:\NServiceBus\License\License.xml" />
</appSettings>
```
-   Use the key `NServiceBus/License` to transfer the actual HTML-encoded contents of the license. For example:

```xml
<appSettings>
  <add key="NServiceBus/License" value="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;
license id=&quot;1222e1d1-2222-4a46-b1c6-943c442ca710&quot; expiration=&quot;2013-11-30T00:00:00.0000000
&quot; type=&quot;Standard&quot; LicenseType=&quot;Standard&quot; LicenseVersion=&quot;4.0
&quot; MaxMessageThroughputPerSecond=&quot;Max&quot; WorkerThreads=&quot;Max
&quot; AllowedNumberOfWorkerNodes=&quot;Max&quot;&gt;
. . .
&lt;/license&gt;" />
</appSettings>
```


## Using the configuration API

It is possible to specify the license to use in the configuration code:

snippet:License


## Where does NServiceBus look for a license?

This section details where NServiceBus will look for license information, and in what order. For example, an expired license in `HKEY_CURRENT_USER` would overrule a valid license in `HKEY_LOCAL_MACHINE`. Note that these vary somewhat in older versions.

In order to find the license, NServiceBus will examine:

| Version | Location                                                                          | Notes |
|:-------:|-----------------------------------------------------------------------------------|:-----:|
|   4.0+  | License XML defined by `NServiceBus/License` appSetting                           |       |
|   4.0+  | File path configured through `NServiceBus/LicensePath` appSetting                 |       |
|   4.0+  | File located at `{AppDomain.CurrentDomain.BaseDirectory}\NServiceBus\License.xml` |       |
|   3.0+  | File located at `{AppDomain.CurrentDomain.BaseDirectory}\License\License.xml`     |       |
|   4.3+  | `HKEY_CURRENT_USER\Software\ParticularSoftware\NServiceBus\License`               |       |
|   4.3+  | `HKEY_CURRENT_USER\Software\Wow6432Node\ParticularSoftware\NServiceBus\License`   |   1   |
|   4.3+  | `HKEY_LOCAL_MACHINE\Software\ParticularSoftware\NServiceBus\License`              |       |
|   4.3+  | `HKEY_LOCAL_MACHINE\Software\Wow6432Node\ParticularSoftware\NServiceBus\License`  |   1   |
|   4.0+  | `HKEY_CURRENT_USER\Software\NServiceBus\{Version}\License`                        |   2   |
|   4.0+  | `HKEY_CURRENT_USER\Software\Wow6432Node\NServiceBus\{Version}\License`            |  1,2  |
|   4.0+  | `HKEY_LOCAL_MACHINE\Software\NServiceBus\{Version}\License`                       |   2   |
|   4.0+  | `HKEY_LOCAL_MACHINE\Software\Wow6432Node\NServiceBus\{Version}\License`           |  1,2  |

**Notes:**

 1. The `Wow6432Node` registry keys are only accessed if running a 32-bit host on a 64-bit OS.
 1. Storing licenses in the registry by NServiceBus version was abandoned in NServiceBus 4.3. For backwards compatibility, newer versions of NServiceBus will still check this pattern for versions `4.0`, `4.1`, `4.2`, and `4.3`.