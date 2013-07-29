---
layout:
title: "Managing NServiceBus Using PowerShell"
tags: 
origin: http://www.particular.net/Articles/managing-nservicebus-using-powershell
---
NServiceBus provides a set of PowerShell commandlets to make it easy to manage and run the softwre. The initial focus is to provide support when preparing machines for use with NServiceBus. Additional features are in the pipeline.

 **NOTE**: Before installing NServiceBus, ensure PowerShell 2.0+ is present on the target machine.

Installing the NServiceBus commandlets in V3.0
----------------------------------------------

These cmdlets are built into the NServiceBus.Core.dll, so [download NServiceBus](http://particular.net/downloads) and load them into PowerShell using the Import-Module cmdlet:


    PM> Import-Module .\NServiceBus.Core.dll


**NOTE**: The core depends on NServiceBus.dll and log4net.dll so make sure that both are in the same directory. If you use the NuGet package, the commandlets are available automatically in the NuGet console. If you installed NServiceBus using the MSI you can add the import module statement to your [PowerShell profile.](http://www.howtogeek.com/50236/customizing-your-powershell-profile/)

For a detailed description of all our commandlets, use the get-help command:


    PM> get-help about_NServiceBus


Infrastructure commandlets
--------------------------

Following are the available infrastructure cmdlets:

### Install-Dtc

Sets up the MSDTC for use with NServiceBus. Use -WhatIf to verify current status without doing any modifications.

### Install-Msmq

Sets up MSMQ for use with NServiceBus. Use -WhatIf to verify current status without doing any modifications. If a reinstall of MSMQ is needed, rerun the command with the -Force switch.

### Install-RavenDB

Sets up RavenDB for use with NServiceBus. Use -WhatIf to verify current status without doing any modifications.

### Install-PerformanceCounters

Installs the NServiceBus performance counters. Use -WhatIf to verify current status without doing any modifications.

### Get-NServiceBusVersion

Gets the current version of NServiceBus (Semver).

### Install-License

Installs a NServiceBus license file into the registry.

Updates to PowerShell commandlets in V4.0
-----------------------------------------

NServiceBus PowerShell commandlets have moved to NServiceBus.PowerShell.dll. To import the DLL, run:


    PM> Import-Module .\NServiceBus.PowerShell.dll

NServiceBus PowerShell cmdlets have been renamed so that they do not clash with existing cmdlets:

<span style="font-weight: 600;">Install-NServiceBusLicense</span>

Installs a NServiceBus license file.

**Get-NServiceBusMSMQMessage**

Displays all messages in a queue.

**Get-NServiceBusVersion**

Displays the NServiceBus installed version.

**Install-NServiceBusDTC**

Installs DTC on the machine.

**Install-NServiceBusRavenDB**

Installs RavenDB on the machine.

**Install-NServiceBusPerformanceCounters**

Installs NServiceBus performance counters on the machine.

**Install-NServiceBusMSMQ**

Installs MSMQ on the machine.

**Test-NServiceBusDTCInstallation**

Validates if DTC is installed and running on the machine.

**Test-NServiceBusRavenDBInstallation**

Ensures RavenDB is on the machine.

**Test-NServiceBusPerformanceCountersInstallation**

Validates that NServiceBus performance counters are correctly installed on the machine.

**Test-NServiceBusMSMQInstallation**

Validates MSMQ is correctly installed on the machine.

**Add-NServiceBusMessageForwardingInCaseOfFaultConfig**

Adds the required configuration section to the config file.

**Get-NServiceBusLocalMachineSettings**

Shows the default error and audit queues.

**Set-NServiceBusLocalMachineSettings**

Allows specifying the default error and audit queues.

 **NOTE:** NServiceBus.Host no longer supports /installinfrastructure. Use PowerShell cmdlets instead.




