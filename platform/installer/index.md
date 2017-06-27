---
title: Platform Installer
summary: Guidance on how to use the Platform Installer and its underlying components
reviewed: 2016-10-26
---

## Overview

The Platform Installer is recommended for use on development machines only.

This is primarily because:

 * The Platform Installer requires Internet access which may not be available in a production environment.
 * The Platform Installer `setup.exe` will fail on Windows servers where [IE Enhanced Security Configuration](https://support.microsoft.com/en-au/kb/815141) is enabled.

[Download the Platform Installer](https://particular.net/start-platform-download).

For testing and production environments it is recommended to:

 * Use the [NServiceBus PowerShell Module](/nservicebus/operations/management-using-powershell.md) to install any required prerequisites.
 * Download and run the individual installers from [downloads](https://particular.net/downloads) rather than installing via the Platform Installer.


## What to expect

The Platform Installer is a [Microsoft Click-Once](https://msdn.microsoft.com/en-us/library/t71a733d.aspx) application, which means it has a built in self-updating mechanism. Click-Once applications are sometimes blocked by corporate firewalls or software restriction policies. If the Platform Installer fails, review the [Offline Install](offline.md) page for installation instructions.


### Dependencies

The Click-Once `setup.exe` will install [.NET 4.5.2](https://www.microsoft.com/en-us/download/details.aspx?id=42643) if required and will then bootstrap the Platform Installer Application.


### License Acceptance

Before proceeding with product selection, the Platform Installer will prompt to accept the NServiceBus License Agreement.


### Proxy Credentials

The Platform Installer requires Internet access to download individual packages. If non-Windows integrated proxy authentication is required, then a credentials dialog will be shown.

![](save-credentials.png)

This dialog offers to save credentials for future use. If the Save Credentials option is chosen, the credentials will be encrypted and stored in the registry at `HKEY_CURRENT_USER\Software\Particular\PlatformInstaller\Credentials` for use in subsequent launches of the Platform Installer.


## Select items to install

The Platform Installer will prompt for which items to install. Individual components can be selected  for installation or upgrade. If the latest version of a product is installed, no checkbox will be displayed for that item as there is no installation or upgrade action required. Similarly if the Platform Installer cannot communicate with the version information feed, it will also disable product selection.

![](select-items.png)


#### Configure Microsoft Message Queuing

This installation runs the appropriate [Deployment Image Servicing and Management (DISM.exe)](https://technet.microsoft.com/en-au/library/hh825236.aspx) command line to install the required Windows Features for [Message Queuing (MSMQ)](https://msdn.microsoft.com/en-us/library/ms711472.aspx). The installation will also check to ensure that any unsupported MSMQ Windows Features are not installed. This installation is only required if MSMQ is going to be used as the message transport. The other supported message transports are detailed in the [Transports](/transports/) documentation.


#### Configure MSDTC for NServiceBus

This installation configures [Microsoft Distributed Transaction Coordinator (DTC)](https://msdn.microsoft.com/en-us/library/ms684146.aspx) for usage by NServiceBus. The configuration sets the following registry values in `HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSDTC\Security`:

 * Enable Network DTC Access. `NetworkDtcAccess` is set to `1`.
 * Allow Outbound transactions:
  * `NetworkDtcAccessOutbound` is set to `1`
  * `NetworkDtcAccessTransactions` is set to `1`
  * `XaTransactions` is set to `1`

 This install is optional.


#### ServiceInsight

Installs the ServiceInsight Package. This MSI can be downloaded directly from [ServiceInsight Releases](https://github.com/Particular/ServiceInsight/releases/latest).


#### ServicePulse

Installs the ServicePulse Package. This MSI can be downloaded directly from [ServicePulse Releases](https://github.com/Particular/ServicePulse/releases/latest).


#### ServiceControl

Installs the ServiceControl Package. This MSI can be downloaded directly from [ServiceControl Releases](https://github.com/Particular/ServiceControl/releases/latest).


## Troubleshooting


### Downloads

The Platform Installer caches the downloaded MSI files in `%temp%\Particular\PlatformInstaller`. These files are downloaded directly from GitHub. Some corporate firewalls prevent the downloading of executable files via content filters or by white/black listing specific web sites. If the Platform Installer cannot download the individual applications, consult with the network administration staff.


### Logs

The Platform Installer logs activity in `%appdata%\PlatformInstaller`. The current log file will be named according to the current date. For example a log created on the 25 January 2016 would result in the filename `log-20160125.txt`.


### MSI Logs

The command line options used for the MSI installations ensure that a detailed log file is produced for each installation. These files are co-located with the Platform Installer logs in `%appdata%\PlatformInstaller`.

An installation or upgrade of a product will overwrite any existing MSI log for that product.

MSI installers provide detailed error information via error codes. [MSI error messages](https://msdn.microsoft.com/en-us/library/aa376931.aspx) can assist in fault finding installation issues.


### Click-Once

As mentioned above, in some circumstances Click-Once can be problematic. The following links provide some useful tips on troubleshooting issues with Click-Once.

* [Click-Once Deployment](https://msdn.microsoft.com/en-us/library/t71a733d.aspx)
* [Troubleshooting Click-Once Deployments](https://msdn.microsoft.com/en-us/library/fb94w1t5.aspx)


### Click-Once and Enhanced Security on Windows Server 2012 R2

Error shown:

```
An error occurred trying to download
'https://s3.amazonaws.com/particular.downloads/PlatformInstaller/PlatformInstaller.application'.

See the setup log file located at
'C:\Users\ADMINI~1\AppData\Local\Temp\VSD9C86.tmp\install.log' for more information.
```

Log file content:

```
URLDownloadToCacheFile failed with HRESULT '-2146697208'
Error: An error occurred trying to download
'https://s3.amazonaws.com/particular.downloads/PlatformInstaller/PlatformInstaller.application'.
```

Resolve this by (temporarily) disabling IE Enhanced Security.
