---
title: Managing NServiceBus Using PowerShell
summary: 'Install the infrastructure for NServiceBus on servers using PowerShell commandlets. '
originalUrl: http://www.particular.net/articles/managing-nservicebus-using-powershell
tags:
- Powershell
- Commandlets
- installation
---

NServiceBus provides a set of PowerShell commandlets to make it easy to manage and run the software. The initial focus is to provide support when preparing machines for use with NServiceBus. Additional features are in the pipeline.

**NOTE** : Before installing NServiceBus, ensure PowerShell 2.0+ is present on the target machine.

Installing the NServiceBus commandlets in V4.0
----------------------------------------------

These cmdlets are built into the NServiceBus.Powershell.dll.

You can use the SDK installer (it will install everything you need to run your development environment).

You can use Nuget:

Create a new class library project, go to `Package Manager Console`

Type `Install-Package NServiceBus.PowerShell`

After it is complete you will have the `NServiceBus.Powershell.dll` in your packages folder in you solution directory

to use it promptly inside visual studio, you can do

    PM> Import-Module .\packages\NServiceBus.PowerShell.<4.x.x your version>\lib\net40\NServiceBus.PowerShell.dll

and then use the cmdlet you want.

Or you can follow the instructions to load them into PowerShell using the Import-Module cmdlet:

    PS> Import-Module .\NServiceBus.Powershell.dll

Or you can follow the instructions to load them into your [PowerShell profile.](http://www.howtogeek.com/50236/customizing-your-powershell-profile/)

If you use the NuGet package, the commandlets are available automatically in the NuGet console. If you installed NServiceBus using the MSI you can add the import module statement to your

For a detailed description of all our commandlets, use the get-help command:

    PM> get-help about_NServiceBus

NServiceBus PowerShell cmdlets have been renamed so that they do not clash with existing version 3.0 cmdlets:


### Install-NServiceBusMSMQ

Installs MSMQ on the machine.

### Test-NServiceBusMSMQInstallation

Validates if MSMQ is correctly installed on the machine.

### Install-NServiceBusDTC

Installs DTC on the machine.

### Test-NServiceBusDTCInstallation

Validates if DTC is installed and running on the machine.

### Install-NServiceBusRavenDB

Installs RavenDB on the machine.

### Test-NServiceBusRavenDBInstallation

Ensures RavenDB is installed on the machine. Port 8080 is used to check to see if RavenDB is already installed on the machine. If RavenDB is installed on a different port, the test might display false.

### Install-NServiceBusPerformanceCounters

Installs NServiceBus performance counters on the machine.

### Test-NServiceBusPerformanceCountersInstallation

Validates that NServiceBus performance counters are correctly installed on the machine.

### Install-NServiceBusLicense

Installs a NServiceBus license file in HKLM. All endpoints can use this machine wide setting without having to specify the license file either in the bin folder or in the app.config file.

### Set-NServiceBusLocalMachineSettings

Allows specifying the default error and audit queues. Sets up the error and audit queue in the registry in HKLM as a machine wide setting. Each endpoint on the machine no longer need to specify these settings in the app.config file

### Get-NServiceBusLocalMachineSettings

Shows the default error and audit queues.

### Get-NServiceBusVersion

Displays the NServiceBus installed version.

###  Get-NServiceBusMSMQMessage</span>

Displays all messages in a queue.

<span style="font-weight: 600;">NOTE:</span> NServiceBus.Host no longer supports /installinfrastructure. Use PowerShell cmdlets instead.


Installing the NServiceBus commandlets in V3.0
----------------------------------------------

Load the cmdlets into PowerShell using the Import-Module cmdlet:

    PM> Import-Module .\NServiceBus.Core.dll

**NOTE**: The core depends on `NServiceBus.dll` and `log4net.dll` so make sure that both are in the same directory.

If you use the NuGet package, the commandlets are available automatically in the NuGet console. If you installed NServiceBus using the MSI you can add the import module statement to your [PowerShell profile.](http://www.howtogeek.com/50236/customizing-your-powershell-profile/)

For a detailed description of all our commandlets, use the get-help command:

    PM> get-help about_NServiceBus

Infrastructure commandlets
--------------------------

Following are the available infrastructure cmdlets:

### Install-Dtc

Sets up the MSDTC for use with NServiceBus. Use `-WhatIf` to verify current status without doing any modifications.

### Install-Msmq

Sets up MSMQ for use with NServiceBus. Use `-WhatIf` to verify current status without doing any modifications. If a reinstall of MSMQ is needed, rerun the command with the -Force switch.

### Install-RavenDB

Sets up RavenDB for use with NServiceBus. Use `-WhatIf` to verify current status without doing any modifications.

### Install-PerformanceCounters

Installs the NServiceBus performance counters. Use `-WhatIf` to verify current status without doing any modifications.

### Get-NServiceBusVersion

Gets the current version of NServiceBus (Semver).

### Install-License

Installs a NServiceBus license file into the registry.

<a id="fixit">But it doesn't work</a>
-------------------------------------

If you are getting the following error:

```
Import-Module : Could not load file or assembly
'file:///C:\\Program Files (x86)\\Particular Software\\NServiceBus\\v4.0\\NServiceBus\\Binaries\\NServiceBus.PowerShell.dll' or one of its dependencies. This assembly is built by a runtime newer than the currently loaded runtime and cannot be loaded. 
```

Although it is possible to change the existing version of `Powershell.exe.config` to load .NET 4.0 framework, the preferred approach is to install PowerShell 3 than to change the config files. Forcing PowerShell 2 to use .NET 4 can break PS Snapins from other vendors. PowerShell 3 provides a command line option to switch versions of PS and .NET if needed.

