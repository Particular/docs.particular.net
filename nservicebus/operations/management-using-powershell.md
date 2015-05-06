---
title:
summary: 'Install the infrastructure for NServiceBus on servers using PowerShell. '
tags:
- Powershell
- Cmdlets
- installation
redirects:
- nservicebus/managing-nservicebus-using-powershell
---

Particular provides a PowerShell Module to make it easy to setup a computer to run NServiceBus.

# NServiceBus PowerShell 5.0

This is the latest release of the PowerShell module.   

## Prerequisites

Prior to installation ensure that PowerShell V2 or greater is installed. 

Earlier versions of the NServiceBus.PowerShell module required .NET 4. In V5.0 the module is compatible with .NET 2 so there is no longer a requirement to upgrade PowerShell to version V3.0+ or modify the `Powershell.exe.config` to force .NET 4 to be used.

## Installation

The installation file for the module can be found [here](https://github.com/particular/NServicebus.Powershell/releases/latest)

## Usage

After installation the module can be loaded into a PowerShell session by issuing the following command:

	Import-Module NServiceBus.PowerShell

The installation adds the NServiceBus.PowerShell module location to the `PSModulePath` environment variable. If the module isn't available you may need to restart your Windows session for this change to to take effect.

## Help  
  
A list of available cmdlets can be found by issuing the following PowerShell command

	Get-Command -Module NServiceBus.PowerShell

Help for each cmdlet incorporated within the module and can be accessed via the standard
PowerShell Get-Help Command.  For example `Get-Help Set-NServiceBusLocalMachineSettings` returns the following information

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

### RavenDB Cmdlet

NServiceBus now supports multiple versions of the [RavenDB]( http://docs.particular.net/nservicebus/ravendb/version-compatibility) client.  Previous versions of the PowerShell module included cmdlets to install and test  RavenDB V2.0.2375. To avoid confusion we have removed these cmdlets in V5.0:

	Install-NServiceBusRavenDB
	Test-NServiceBusRavenDBInstallation

For RavenDB installation instructions please review [Installing RavenDB](http://docs.particular.net/nservicebus/ravendb/installation) 

### Licensing 
   
Version 5 of the PowerShell module includes a cmdlet for importing a Particular Platform License. This willw ork for NServiceBus versions 4. If you which to to install a license for versions prior to NServiceBus v4.5 then an alternative method should be used. See  [License Management](http://docs.particular.net/nservicebus/licensing/license-management) for

# NServiceBus PowerShell 4.x

## Prerequisites

Prior to installation ensure that PowerShell V3 or greater is installed. 

## Installation

This version is available via [Nuget](https://www.nuget.org/packages/NServiceBus.PowerShell/)

To get this module:

- Create a new class library project
- go to `Package Manager Console`
- Type `Install-Package NServiceBus.PowerShell`

After it is complete you will have the `NServiceBus.Powershell.dll` in your packages folder in your solution directory

## Usage

To use it promptly inside visual studio, you can do run the following from the Package Manager console

    Import-Module .\packages\NServiceBus.PowerShell.<4.x.x your version>\lib\net40\NServiceBus.PowerShell.dll

and then use the cmdlet you want. 

NOTE: there will be less feedback from the script if you run the cmdlet inside Visual Studio. If you get warnings it might be a good idea to run the cmdlet from a real PowerShell Console.

Or you can follow the instructions to load them into PowerShell using the Import-Module cmdlet:

    PS> Import-Module .\NServiceBus.Powershell.dll

Or you can follow the instructions to load them into your [PowerShell profile.](http://www.howtogeek.com/50236/customizing-your-powershell-profile/)

If you use the NuGet package, the commandlets are available automatically in the NuGet console. If you installed NServiceBus using the MSI you can add the import module statement to your

## Help  
  
A list of available cmdlets can be found by issuing the following PowerShell command

	Get-Command -Module NServiceBus.PowerShell

 
# NServiceBus Cmdlets in NServiceBus V3.x

## Prerequisites

Prior to installation ensure that PowerShell V3 or greater is installed.

## Installation 

In NServiceBus V3.x the Powershell module was bundled in the NServiceBus.Core.DLL

## Usage 

Load the cmdlets into PowerShell using the Import-Module cmdlet:

    PM> Import-Module .\NServiceBus.Core.dll

NOTE: The core depends on `NServiceBus.dll` and `log4net.dll` so make sure that both are in the same directory.

If you use the NuGet package, the cmdlets are available automatically in the NuGet console. If you installed NServiceBus using the MSI you can add the import module statement to your [PowerShell profile](http://www.howtogeek.com/50236/customizing-your-powershell-profile/).

## Help  
  
A list of available cmdlets can be found by issuing the following PowerShell command

	Get-Command -Module NServiceBus.PowerShell


# Known Issues with V3 and V4

If you are getting the following error:

```
Import-Module : Could not load file or assembly
'file:///C:\\Program Files (x86)\\Particular Software\\NServiceBus\\v4.0\\NServiceBus\\Binaries\\NServiceBus.PowerShell.dll' or one of its dependencies. This assembly is built by a runtime newer than the currently loaded runtime and cannot be loaded. 
```

Although it is possible to change the existing version of `Powershell.exe.config` to load .NET 4.0 framework, the preferred approach is to install PowerShell 3 than to change the config files. Forcing PowerShell 2 to use .NET 4 can break PS Snapins from other vendors. PowerShell 3 provides a command line option to switch versions of PS and .NET if needed.

  