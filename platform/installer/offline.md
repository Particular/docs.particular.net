---
title: Installing the Particular Platform
summary: How to install the Particular Platform
reviewed: 2022-10-28
---

## .NET and redistributable packages prerequisite version

The Particular Service Platform products require the following to be installed on a x64 windows operating system:

- [.NET 4.7.2, or later](https://dotnet.microsoft.com/download/dotnet-framework/thank-you/net472-offline-installer).
- [.NET Runtime 6.0.10, or later](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).
- [ASP.NET Core Runtime 6.0.10, or later](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).
- [VC++ 2015 Redistributable](https://www.microsoft.com/en-us/download/details.aspx?id=53840).

Before proceeding, ensure that these versions are available on the system.

## MSMQ prerequisites

The following steps are required when running NServiceBus endpoints using Microsoft Message Queuing (MSMQ):

* Add, configure, and start the MSMQ service.
* Configure and start the Microsoft Distributed Coordinator Service.

These actions can be performed using the [NServiceBus PowerShell module](https://github.com/Particular/NServiceBus.PowerShell/releases/latest) which is available as a standalone installation. Once the module is installed, open a PowerShell prompt as an administrator and issue the following commands:

```ps
Import-Module NServiceBus.PowerShell
Install-NServiceBusDTC
Install-NServiceBusMSMQ
```

## ServiceControl

The installation executable for ServiceControl can be downloaded directly from the [ServiceControl releases page](https://github.com/Particular/ServiceControl/releases/latest).

## ServiceInsight

The installation executable for ServiceInsight can be downloaded directly from the [ServiceInsight releases page](https://github.com/Particular/ServiceInsight/releases/latest).

## ServicePulse

The installation executable for ServicePulse can be downloaded directly from the [ServicePulse releases page](https://github.com/Particular/ServicePulse/releases/latest).
