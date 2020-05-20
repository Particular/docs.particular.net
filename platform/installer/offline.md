---
title: Installing the Particular Platform Without the Platform Installer
summary: How to install the Particular Platform with no internet connection
reviewed: 2020-05-20
---

The [Particular Platform Installer](/platform/installer) handles installing prerequisites for NServiceBus and the Particular Platform products. This guide details how to install to achieve the same results on a computer that does not have internet connectivity.


## .NET prerequisite version

The Particular Platform products require [.NET 4.6.2](https://dotnet.microsoft.com/download/dotnet-framework/thank-you/net462-offline-installer). Before proceeding, ensure that this version of .NET is available on the system. More detailed information about .NET can be found on the [Wikipedia .NET Overview](https://en.wikipedia.org/wiki/.NET_Framework_version_history#Overview).


##  Platform Installer components

The Platform Installer installs 4 component parts:

 * NServiceBus Prerequisites
 * ServiceControl
 * ServicePulse
 * ServiceInsight

Each of these will be detailed below:


### NServiceBus Prerequisites

The NServiceBus Prerequisites option in the Platform Installer configures the system to run NServiceBus endpoints using Microsoft Message Queuing (MSMQ) by doing the following:

 * Adds, configures, and starts the MSMQ service.
 * Configures and starts the Microsoft Distributed Coordinator Service.

These actions are available via the [NServiceBus PowerShell module](https://github.com/Particular/NServiceBus.PowerShell/releases/latest). This module is available as a standalone installation. Once the module is installed, open a PowerShell prompt as an administrator and issue the following commands:

```ps
Import-Module NServiceBus.PowerShell
Install-NServiceBusDTC
Install-NServiceBusMSMQ
```


### ServiceControl

The installation executable for ServiceControl can be downloaded directly from the [ServiceControl releases page](https://github.com/Particular/ServiceControl/releases/latest).


### ServiceInsight

The installation executable for ServiceInsight can be downloaded directly from the [ServiceInsight releases page](https://github.com/Particular/ServiceInsight/releases/latest).


### ServicePulse

The installation executable for ServicePulse can be downloaded directly from the [ServicePulse releases page](https://github.com/Particular/ServicePulse/releases/latest).
