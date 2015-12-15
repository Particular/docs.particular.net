---
title: Management using PowerShell
summary: 'Install the infrastructure for NServiceBus on servers using PowerShell. '
tags:
- PowerShell
- Cmdlets
- installation
redirects:
- nservicebus/managing-nservicebus-using-powershell
---

Particular provides a PowerShell module to make it easy to setup a computer to run NServiceBus.

The PowerShell module provides cmdlets to assist with:

- Installing Microsoft Message Queuing Service (MSMQ)
- Configuring Microsoft Distributed Transaction Coordinator (MSDTC)
- Installing performance counters for NServiceBus 
- Setting the addresses of the default Error and Audit queues for use by deployed Endpoints 
- Importing a Particular Platform license into the Registry
- Removing a worker from a [Distributor](/nservicebus/scalability-and-ha/distributor/)


## Prerequisites

Prior to installation ensure that PowerShell Version 2 or greater is installed. 

From Version 5.0 the module is compatible with .NET 2 so there is no longer a requirement to upgrade PowerShell to Version 3.0+ or modify the `Powershell.exe.config` to force .NET 4 to be used.


## Installation

The installation file for the module can be **[downloaded here](https://github.com/particular/NServicebus.Powershell/releases/latest)**.


## Usage

After installation the module can be loaded into a PowerShell session by issuing the following command:

	Import-Module NServiceBus.PowerShell

The installation adds the NServiceBus.PowerShell module location to the `PSModulePath` environment variable. If the module isn't available you may need to restart your Windows session for this change to to take effect.

As most of the cmdlets require elevated privileges the module should be used in a PowerShell session that has been `Run As Administrator`. 

## Help  
  
A list of available cmdlet can be found by issuing the following PowerShell command

	Get-Command -Module NServiceBus.PowerShell

Help for each cmdlet incorporated within the module and can be accessed via the standard [PowerShell Get-Help Command](https://technet.microsoft.com/en-us/library/ee176848.aspx). For example `Get-Help Set-NServiceBusLocalMachineSettings` returns the following information

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


## Upgrade information


### RavenDB cmdlets

NServiceBus now supports multiple versions of the [RavenDB](/nservicebus/ravendb/version-compatibility.md) client. Previous versions of the PowerShell module included cmdlets to install and test RavenDB V2.0.2375. To avoid confusion we have removed these cmdlets in Version 5.0:

	Install-NServiceBusRavenDB
	Test-NServiceBusRavenDBInstallation

For RavenDB installation instructions please review [Installing RavenDB](/nservicebus/ravendb/installation.md) 


### Licensing 
   
Version 5 of the PowerShell module includes a commandlet for importing a Particular Platform License. This will work for NServiceBus versions 4. If you which to to install a license for versions prior to NServiceBus Version 4.5 then an alternative method should be used. See [License Management](/nservicebus/licensing/license-management.md) for


## Older versions of NServiceBus PowerShell

The previous version of NServiceBus PowerShell have been deprecated.