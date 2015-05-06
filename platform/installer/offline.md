---
title: Installing The Platform Components Manually without Platform Installer
summary: 'Guidance on how to install the platform components'
tags: [Platform, Installation, Offline]
---

The [Platform Installer](/platform/installer) handles installing pre-requisites for NServiceBus and the Platform products.  This guide details how to install to achieve the same results as the Platform Installer on a computer that does not have the required Internet connectivity.


## .Net Prerequisite Version

The Particualr platform products require [.Net 4.5](http://www.microsoft.com/en-au/download/details.aspx?id=40779). Before proceeding please ensure that this version of .Net is available on the system you are setting up.  

Note: Windows 8.x and Windows 2012 or greater already ship with this version so no action is required on these products,


##  Platform Installer Components

The Platform Installer installs 6 component parts:

- NServiceBus Prerequisities
- ServiceControl
- ServicePulse
- ServiceInsight
- ServiceMatrix for Visual Studio 2013 
- ServiceMatrix for Visual Studio 2012 

Each of these will be detailed below:

### NServiceBus Prerequistes

The NServiceBus Prerequisites option in the Platform Install configures the system to run NServiceBus endpoints using Microsoft Message Queuing(MSMQ) by doing the following:

- Adds, configures and starts the MSMQ service.
- Configures and starts the Microsoft Distributed Co-ordinator Service
- Adds NServiceBus Performance Counters

These actions are available via [NServicebus PowerShell Module](https://github.com/Particular/NServiceBus.PowerShell/releases/latest).  This module is available as standalone installation.  Once the module is installed, open a PowerShell prompt as a Administrator and issue the following commands:

```bat
Import-Module NServiceBus.PowerShell
Install-NServiceBusDTC
Install-NServiceBusMSMQ
Install-NServiceBusPerformanceCounters
```

### ServiceControl 

This MSI can be downloaded directly from here: [ServiceControl Releases](https://github.com/Particular/ServiceControl/releases/latest).

### ServiceInsight

This MSI can be downloaded directly from here: [ServiceInsight Releases](https://github.com/Particular/ServiceInsight/releases/latest).

### ServicePulse

This MSI can be downloaded directly from here: [ServicePulse Releases](https://github.com/Particular/ServicePulse/releases/latest).

### ServiceMatrix 

This VSIX files for ServiceMatrix can be downloaded directly from here: [ServiceMatrix Releases](https://github.com/Particular/ServiceMatrix/releases/latest).  

The file name for the Visual Studio 2013 version is `Particular.ServiceMatrix.12.0.vsix`.
The file name for the Visual Studio 2012 version is `Particular.ServiceMatrix.11.0.vsix`.

To install ServiceMatrix the VSIXInstaller.exe is required which is shipped with commercial versions of Visual Studio ( i.e Visual Studio Professional or better).  The Express editions of Visual Studio are not supported.

The VSIX file can either be double-clicked on to install it or run from the command line.  The following example shows the installation using Visual Studio 2013.

```bat
"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDEVsixinstaller.exe" Particular.ServiceMatrix.12.0.vsix  
```

Note: ServiceMatrix requires an Internet connection to download packages from the NuGet public feed. So whilst ServiceMatrix can be installed offline without a NuGet feed it will not be able to create and manage projects properly.

