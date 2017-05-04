---
title: Installing Platform Components without Platform Installer
reviewed: 2016-10-26
---

The [Platform Installer](/platform/installer) handles installing prerequisites for NServiceBus and the Platform products. This guide details how to install to achieve the same results as the Platform Installer on a computer that does not have the required Internet connectivity.


## .NET Prerequisite Version

The Particular platform products require [.NET 4.5.2](https://www.microsoft.com/en-us/download/details.aspx?id=42643). Before proceeding ensure that this version of .NET is available on the system. More detailed information about .NET can be found on the [Wikipedia .NET Overview](https://en.wikipedia.org/wiki/.NET_Framework_version_history#Overview).


##  Platform Installer Components

The Platform Installer installs 4 component parts:

 * NServiceBus Prerequisites
 * ServiceControl
 * ServicePulse
 * ServiceInsight

Each of these will be detailed below:


### NServiceBus Prerequisites

The NServiceBus Prerequisites option in the Platform Install configures the system to run NServiceBus endpoints using Microsoft Message Queuing (MSMQ) by doing the following:

 * Adds, configures and starts the MSMQ service.
 * Configures and starts the Microsoft Distributed Coordinator Service.

These actions are available via [NServiceBus PowerShell Module](https://github.com/Particular/NServiceBus.PowerShell/releases/latest). This module is available as standalone installation. Once the module is installed, open a PowerShell prompt as a Administrator and issue the following commands:

```ps
Import-Module NServiceBus.PowerShell
Install-NServiceBusDTC
Install-NServiceBusMSMQ
```


### ServiceControl

This MSI can be downloaded directly from [ServiceControl Releases](https://github.com/Particular/ServiceControl/releases/latest).


### ServiceInsight

This MSI can be downloaded directly from [ServiceInsight Releases](https://github.com/Particular/ServiceInsight/releases/latest).


### ServicePulse

This MSI can be downloaded directly from [ServicePulse Releases](https://github.com/Particular/ServicePulse/releases/latest).