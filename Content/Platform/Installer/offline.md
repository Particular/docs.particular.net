---
title: Installing The Platform Components Manually - Offline
summary: 'Guidance on how to install the platform components offline'
tags: [Platform, Installation, Offline]
---

The [Platform Installer](/platform/installer) handles installing pre-requisites for NServiceBus and the Platform products, such as [ServiceInsight](/ServiceInsight), [ServiceMatrix](/ServiceMatrix), [ServicePulse](/ServicePulse) and [ServiceControl](/ServicePulse).

The Platform Installer depends on [Chocolatey](https://chocolatey.org) & Chocolatey packages, and those packages contain the URLs for the installation binaries. For this reason the Platform Installer requires an internet connection and requires access to the Chocolatey repository.

## Platform Offline Installation

It is possible to perform an offline installation of the Platform.

### Pre-requisites

Setting up NServiceBus pre-requisites offline involves the following steps

#### MSMQ 

Download and execute the following PowerShell script:

* https://github.com/Particular/Packages.Msmq/blob/master/src/tools/setup.ps1

#### MSDTC

Download the following files:

* https://github.com/Particular/Packages.DTC/blob/master/src/tools/setup.ps1
* https://github.com/Particular/Packages.DTC/blob/master/src/tools/RegHelper.cs

Execute the Setup.ps1 PowerShell script.   

#### Performance Counters

Download and execute the following PowerShell script:

* https://github.com/Particular/Packages.PerfCounters/blob/master/src/tools/setup.ps1

### Platform Tools

Platform tools can be manually installed using their own standalone installer, available at the Particular web site [download page](http://www.particular.net/downloads).

#### ServiceMatrix

ServiceMatrix requires to download packages from the NuGet public feed to function properly, currently ServiceMatrix can be installed offline using the stand alone installer but requires an internet connection, and access to NuGet public feed, to create and manage projects.

#### Custome Installation Notes
To run the installer in quiet mode form commandline you can pass in the `/quiet` option 

For Example: `Particular.ServiceControl-1.3.0.exe /quiet`

To set the target directory you can use the property setter `APPDIR`

For example: `Particular.ServiceControl-1.3.0.exe /quiet APPDIR=“C:\MyTagretDirectory”`


