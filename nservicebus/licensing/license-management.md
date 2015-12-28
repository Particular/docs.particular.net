---
title: How to install your NServiceBus license file
summary: 'Make sure your NServiceBus endpoints use your license: Configuration API, app.config, subfolder in your BIN directory, or registry.'
tags:
- license
redirects:
 - nservicebus/license-management
---

## Overview

There are several ways to make sure that your NServiceBus endpoints pick up and use your license.


## Using the Registry

Using the registry to store your license information is a way that all platform tools can access this information easily. This includes NServiceBus endpoints, ServiceControl, and ServiceInsight. (ServicePulse determines licensing status by querying the ServiceControl API.) Using the registry ensures that all the platform tools can access the license status without requiring additional complexity on every deployment.


### NServiceBus Version 4.5 and above

The standalone NServiceBus PowerShell Version 5.0 includes a commandlet for importing the Platform License into the `HKEY_LOCAL_MACHINE` registry. See [Managing Using PowerShell](/nservicebus/operations/management-using-powershell.md) for more details and installation instructions for the PowerShell Module.

For 64-bit operating systems the license is written to both the 32-bit and 64-bit registry. The license is stored is `HKEY_LOCAL_MACHINE\Software\ParticularSoftware\License`.


#### Advanced Registry Options

These following instructions cover installing the license file without using NServiceBus PowerShell Module. These options give a bit more flexibility as they allow you to store the the license in `HKEY_CURRENT_USER` if you wish. If the licenses is stored in `HKEY_CURRENT_USER` it is only accessible to the current user.


##### Using Registry Editor

- Run `regedit.exe` (usually located in `%windir%`, e.g. `C:\Windows`)
- Go to `HKEY_LOCAL_MACHINE\Software\ParticularSoftware` or `HKEY_CURRENT_USER\Software\ParticularSoftware`
- Create a new Multi-String Value (`REG_MULTI_SZ`) named `License`
- Paste the contents of the license file you received from Particular Software.

(You can safely ignore any warnings regarding empty strings.)


##### Using PowerShell Script

* Open an administrative PowerShell prompt.
* Change the current working directory to where your license.xml file is.
* Run the following script

```
$content = Get-Content license.xml | Out-String
Set-ItemProperty -Path HKLM:\Software\ParticularSoftware -Name License -Force -Value $content
```

If modifying the registry directly using Registry Editor or a PowerShell script to update the license for ServiceControl, you will need to restart the ServiceControl service, as it only checks for its license information once at startup.

NOTE: On a 64 bit operating system this script should not be run through the PowerShell(x86) console prompt, doing so will result in the license being imported into the 32 registry key. Please use a 64bit PowerShell session.

NOTE: As of Version 4.5, both the `LicenseInstaller.exe` tool and the `install-NServiceBusLicense` PowerShell commandlet has been deprecated.


### NServiceBus Version 3.3

NServiceBus Version 3.3 supports storing the license in a registry key called `[HKEYCURRENTUSER\Software\NServiceBus\{Major.Minor}\License]`.

To install the license in the registry, use one of these options:

 * The `LicenseInstaller.exe` tool that comes with the NServiceBus install.
 * The [Install-License](/nservicebus/operations/management-using-powershell.md) PowerShell commandlet.
 * If your trial license has expired and you are running in debug mode, the endpoint shows you a dialog that enables you to install the license.


### NServiceBus 4.x versions prior to Version 4.5

When installed using the [Install-NServiceBusLicense](/nservicebus/operations/management-using-powershell.md) PowerShell commandlet and the `LicenseInstaller.exe` tool that comes with the NServiceBus install, in NServiceBus Version 4.0, the license file was stored under `HKLM\Software\NServiceBus\{Major.Minor}\License` and in Version 4.3, this location was moved to `HKLM\Software\ParticularSoftware\NServiceBus\License`.

In order to install the license file under HKCU (same location in Version 3.3), please use the `-c` option on the `LicenseInstaller.exe`


## Using a sub-directory in your BIN directory

To have NServiceBus automatically pick up your License.xml file, place it in a subfolder named /License under your BIN folder.


## Using the app.config settings

As a developer you can specify the license in `app.config`:

-   Use the key `NServiceBus/LicensePath` to specify the path where NServiceBus looks for your license. For example:

```XML
<appSettings>
  <add key="NServiceBus/LicensePath" value="C:\NServiceBus\License\License.xml" />
</appSettings>
```
-   Use the key `NServiceBus/License` to transfer the actual HTML-encoded contents of your license. For example:

```XML
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

As a developer you can specify the license to use in your configuration code:

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
 2. Storing licenses in the registry by NServiceBus version was abandoned in NServiceBus 4.3. For backwards compatibility, newer versions of NServiceBus will still check this pattern for versions `4.0`, `4.1`, `4.2`, and `4.3`.