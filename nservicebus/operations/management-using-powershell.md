---
title: Management using PowerShell
summary: Install the infrastructure for NServiceBus on servers using PowerShell.
reviewed: 2017-01-06
related:
 - nservicebus/operations
tags:
- PowerShell
- installation
redirects:
- nservicebus/managing-nservicebus-using-powershell
---

A PowerShell module that sets up a computer to run NServiceBus.

The PowerShell module provides cmdlets to assist with:

 * Installing Microsoft Message Queuing Service (MSMQ)
 * Configuring Microsoft Distributed Transaction Coordinator (MSDTC)
 * Installing performance counters for NServiceBus _(obsolete)_
 * Setting the addresses of the default Error and Audit queues for use by deployed Endpoints
 * Importing a Particular Platform license into the Registry
 * Removing a worker from a [Distributor](/transports/msmq/distributor/)


## Prerequisites

Prior to installation ensure that PowerShell Version 2 or greater is installed.

From Version 5.0 the module is compatible with .NET 2 so there is no longer a requirement to upgrade PowerShell to Version 3.0+ or modify the `PowerShell.exe.config` to force .NET 4 to be used.


## Installation

The installation file for the module can be **[downloaded here](https://github.com/particular/NServiceBus.PowerShell/releases/latest)**.


## Usage

After installation the module can be loaded into a PowerShell session by issuing the following command:

```ps
Import-Module NServiceBus.PowerShell
```

The installation adds the NServiceBus.PowerShell module location to the `PSModulePath` environment variable. If the module isn't available restarting the Windows session may be required for this change to take effect.

As most of the cmdlets require elevated privileges the module should be used in a PowerShell session that is launched with `Run As Administrator`.


## Help

A list of available cmdlets can be found by issuing the following PowerShell command

```ps
Get-Command -Module NServiceBus.PowerShell
```

Help for each cmdlet is incorporated within the module. Help can be accessed via the standard [PowerShell Get-Help Command](https://technet.microsoft.com/en-us/library/ee176848.aspx). For example `Get-Help Set-NServiceBusLocalMachineSettings` returns the following information

```
NAME
    Set-NServiceBusLocalMachineSettings

SYNOPSIS
    Sets the default Error and Audit queues.

SYNTAX
    Set-NServiceBusLocalMachineSettings [-ErrorQueue <String>] [-AuditQueue <String>] [<CommonParameters>]

DESCRIPTION
    Sets the registry settings for the default audit and error queues.
    These settings can be found in the registry under "HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\ServiceBus".
    On 64 bit operating system the settings are applied to both the 32-bit and 64-bit registry.
```


## Upgrade information


### RavenDB cmdlets

Multiple versions of the [RavenDB](/persistence/ravendb/version-compatibility.md) client are supported. Previous versions of the PowerShell module included cmdlets to install and test RavenDB V2.0.2375. To avoid confusion these cmdlets were removed in Version 5.0:

```
Install-NServiceBusRavenDB
Test-NServiceBusRavenDBInstallation
```

For RavenDB installation instructions review [Installing RavenDB](/persistence/ravendb/installation.md)


### Licensing

Version 5 of the PowerShell module includes a cmdlet for importing a Particular Platform License file. This will also work for NServiceBus version 4.5 and newer. To install an license file for versions prior to 4.5 then an alternative method must be used. See [License](/nservicebus/licensing/).


## Older versions of NServiceBus PowerShell

The previous version of NServiceBus PowerShell have been deprecated.
