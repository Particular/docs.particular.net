---
title: Installing The Platform Components Manually without Platform Installer
summary: 'Guidance on how to install the platform components'
tags: [Platform, Installation, Offline]
---

The [Platform Installer](/platform/installer) handles installing prerequisites for NServiceBus and the Platform products. This guide details how to install to achieve the same results as the Platform Installer on a computer that does not have the required Internet connectivity.


## .Net Prerequisite Version

The Particular platform products require [.Net 4.5](https://www.microsoft.com/en-au/download/details.aspx?id=30653). Before proceeding please ensure that this version of .Net is available on the system you are setting up. More detailed information about .NET can be found on the [Wikipedia .NET Overview](https://en.wikipedia.org/wiki/.NET_Framework_version_history#Overview). 


##  Platform Installer Components

The Platform Installer installs 4 component parts:

- NServiceBus Prerequisites
- ServiceControl
- ServicePulse
- ServiceInsight

Each of these will be detailed below:


### NServiceBus Prerequisites

The NServiceBus Prerequisites option in the Platform Install configures the system to run NServiceBus endpoints using Microsoft Message Queuing(MSMQ) by doing the following:

 * Adds, configures and starts the MSMQ service.
 * Configures and starts the Microsoft Distributed Coordinator Service.
 * Adds NServiceBus Performance Counters.

These actions are available via [NServiceBus PowerShell Module](https://github.com/Particular/NServiceBus.PowerShell/releases/latest). This module is available as standalone installation. Once the module is installed, open a PowerShell prompt as a Administrator and issue the following commands:

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

ServiceMatrix is no longer part of the platform installer. Read this article on [how to install ServiceMatrix](/servicematrix/installing-servicematrix-2.0.md).