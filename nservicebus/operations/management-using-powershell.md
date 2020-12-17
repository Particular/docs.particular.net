---
title: Management using PowerShell
summary: Install the infrastructure for NServiceBus on servers using PowerShell.
reviewed: 2020-08-03
isLearningPath: true
related:
 - nservicebus/operations
redirects:
- nservicebus/managing-nservicebus-using-powershell
---

A PowerShell module that sets up a computer to run NServiceBus.

The PowerShell module provides cmdlets to assist with:

 * Installing Microsoft Message Queuing Service (MSMQ)
 * Configuring Microsoft Distributed Transaction Coordinator (MSDTC)
 * Setting the addresses of the default Error and Audit queues for use by deployed Endpoints
 * Importing a Particular Platform license into the Registry
 * Removing a worker from a [Distributor](/transports/msmq/distributor/)


## Prerequisites

Prior to installation ensure that PowerShell 2 or higher is installed. NServiceBus PowerShell modules are compatible with PowerShell 5. Versions of PowerShell later than 5 (including PowerShell Core) are not supported and might not work as expected.

NOTE: In order to run PowerShell cmdlets, the PowerShell execution policy needs to be set to `Unrestricted` or a bypass neds to be granted to the module file. Refer to PowerShell documentation on how to change the execution policy.

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

Help for each cmdlet is incorporated within the module. Help can be accessed via the standard [PowerShell Get-Help Command](https://technet.microsoft.com/en-us/library/ee176848.aspx), e.g. `Get-Help Set-NServiceBusLocalMachineSettings`.
