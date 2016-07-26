---
title: Platform Installer
summary: Guidance on how to use the Platform Installer and its underlying components
reviewed: 2016-03-16
tags:
- Platform
- Installation
---

## Overview

The Platform Installer (PI) is recommended for use on development machines only.

This is primarily because:

 * The PI requires Internet access which may not be available in a production environment.
 * The PI `setup.exe` will fail on Windows servers were `IE Enhanced Security Configuration` is enabled.

[Download the PI](http://particular.net/start-platform-download).

For testing and production environments it is recommended to:

 * Use the [NServiceBus PowerShell Module](/nservicebus/operations/management-using-powershell.md) to install any required prerequisites.
 * Download and run the individual installers from [downloads](http://particular.net/downloads) rather than installing via the PI.


## What to expect

The PI is a [Microsoft Click-Once](https://msdn.microsoft.com/en-us/library/t71a733d.aspx) application which means it has a built in self-updating mechanism. Click once applications are sometimes blocked by corporate firewalls or software restriction policies. If the PI fails review the [Offline Install](offline.md) page for installation instructions.


### Dependencies

The Click-Once `setup.exe` will install [.Net 4.5.2](https://www.microsoft.com/en-us/download/details.aspx?id=42643) if required and with then bootstrap the PI Application.


### License Acceptance

Before proceeding with product selection the PI will prompt to accept the NServiceBus License Agreement.


### Proxy Credentials

The PI requires Internet access to download individual packages. If non-windows integrated proxy authentication is required then a credentials dialog will be show.

![](save-credentials.png)

This dialog offers to save credentials for future use. If the Save Credentials option is chosen the credentials will be encrypted and stored in the registry at `HKEY_CURRENT_USER\Software\Particular\PlatformInstaller\Credentials` for use in subsequent launches of the PI.


## Select items to install

The PI will prompt for which items to install. Individual components can be selected how for installation or upgrade. If the latest version of a product is installed that item no checkbox will be displayed as there is no installation or upgrade action required. Similarly if the PI cannot communicate with the version information feed it will also disable product selection.

![](select-items.png)


#### NServiceBus Performance Counters

This installation adds the performance counters category "NServiceBus" with the following counters:

 * `Critical Time` - The age of the oldest message in the queue.
 * `SLA violation countdown` - The number of seconds until the SLA for this endpoint is breached.
 * `# of msgs successfully processed / sec` - The current number of messages processed successfully by the transport per second.
 * `# of msgs pulled from the input queue /sec` - The current number of messages pulled from the input queue by the transport per second.
 * `# of msgs failures / sec` - The current number of failed processed messages by the transport per second.

This installation is optional.


#### Configure Microsoft Message Queuing

This installation runs the appropriate [Deployment Image Servicing and Management (DISM.exe)](https://technet.microsoft.com/en-au/library/hh825236.aspx) command line to install the required Windows Features for [Message Queuing (MSMQ)](https://msdn.microsoft.com/en-us/library/ms711472.aspx). The installation will also check to ensure that any unsupported MSMQ Windows Features are not installed. This installation is only required if MSMQ is going to be used as the message transport. The other supported message transports are detailed in the [Transports](/nservicebus/transports/) documentation.


#### Configure MSDTC for NServiceBus

This installation configures [Microsoft Distributed Transaction Coordinator (DTC)](https://msdn.microsoft.com/en-us/library/ms684146.aspx) for usage by NServiceBus. The configuration sets the following registry values in `HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSDTC\Security`:

 * Enable Network DTC Access. `NetworkDtcAccess` is set to `1`.
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

The PI caches the downloaded MSI files in `%temp%\Particular\PlatformInstaller`. These files are download directly from GitHub. Some corporate firewalls prevent the downloading of executable files via content filters or by white/black listing specific web sites. If the PI cannot download the individual applications consult with the network administration staff.


### Logs

The PI logs activity in `%appdata%\PlatformInstaller`. The current log file will be named according to the current date. For example a log created on the 25 January 2016 would result in the filename `log-20160125.txt`.


### MSI Logs

The command line options used for the MSI installations ensure that a detailed log file is produced for each installation. These files are co-located with the PI logs in `%appdata%\PlatformInstaller`.

An installation or upgrade of a product will overwrite any existing MSI log for that product.

MSI provide detailed error information via error codes. [MSI error messages](https://msdn.microsoft.com/en-us/library/aa376931.aspx) can assist in fault finding installation issues.


### Click-Once

As mentioned above, in some circumstances Click-Once can be problematic. The following links provide some useful tips on troubleshooting issues with Click-Once.

* [Click-Once Deployment](https://msdn.microsoft.com/en-us/library/t71a733d.aspx)
* [Troubleshooting Click-Once Deployments](https://msdn.microsoft.com/en-us/library/fb94w1t5.aspx)


### Click-once and Enhanced Security on Windows Server 2012 R2

Error shown:

```no-highlight
An error occurred trying to download 'https://s3.amazonaws.com/particular.downloads/PlatformInstaller/PlatformInstaller.application'.

See the setup log file located at 'C:\Users\ADMINI~1\AppData\Local\Temp\VSD9C86.tmp\install.log' for more information.
```

Log file content:

```no-highlight
URLDownloadToCacheFile failed with HRESULT '-2146697208'
Error: An error occurred trying to download 'https://s3.amazonaws.com/particular.downloads/PlatformInstaller/PlatformInstaller.application'.
```

Resolve this by (temporarily) disabling IE Enhanced Security.
