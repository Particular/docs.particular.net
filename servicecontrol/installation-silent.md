---
title: Installing Silently
reviewed: 2016-10-06
tags:
- ServiceControl
- Installation
---

### Silent Installation

Note: This documentation covers silent installation instructions for ServiceControl Version 2.0 or greater.

The command line examples referred to the ServiceControl installation exe as `<install.exe>`.   Replace this with the specific exe name for the version being deployed.  e.g. `Particular.ServiceControl-2.0.0.exe 


The following command line will silently install the ServiceControl Management Utility and the ServiceControl Management PowerShell Module.

```os
 <install.exe> /quiet
```

#### Logging the output of the Silent Installation

The installation executable uses standard MSI command line switches to specify logging.  The following example shows the command line  switches to turn on verbose logging. For more information on the available command line switches refer to the [Advanced Installer documentation]( http://www.advancedinstaller.com/user-guide/exe-setup-file.html ) 

```os
<install.exe> /quiet /LV* install.log 
```

#### Install the license file as part of the Silent installation

The installation executable can import a Particular License file into the registry as part of the installation process.  

```os
<install.exe> /quiet /LV* install.log LICENSEFILE=license.xml
```

License installation can also be done post installation by using the ServiceControl Management Utility or via the
PowerShell

NOTE: Running the installation updates the ServiceControl Powershell module. When scripting an upgrade do not import the PowerShell module prior to running the installation as it will terminate the script.   

#### Setting up a ServiceControl Control Service

To silently install an instance of the ServiceControl Service use the ServiceControl Management PowerShell after the Installation exe has completed use the `New-ServiceControlInstance` cmdlet.  The cmdlet has a large number of parameters so to make it easier to display the following example uses PowerShell feature called [splatting](https://blogs.technet.microsoft.com/heyscriptingguy/2010/10/18/use-splatting-to-simplify-your-powershell-scripts/)  which allows cmdlet parameters as a hash table

snippet: unattendedInstall

#### Updating a ServiceControl Instance

Installing a newer version of the installation executable updates the ServiceControl Management utility and PowerShell but it does not automatically update any instances of the ServiceControl service.  

The `Invoke-ServiceControlUpgrade` cmdlet is used to script an upgrade of the instance

This cmdlet does the following:

* Updates the binaries to the last version
* Updates the configuration file to include any new settings introduced
* Optionally completes a backup of the RavenDB prior to upgrade (recommended)
* Creates any needed queues or folders that do not currently exist
* Restarts the service if it was already running

##### Updating with default settings

The `Invoke-ServiceControlUpgrade` cmdlet has two parameter sets .  The first set is intended to make upgrading simple. It will add any new required configuration settings to the configuration file using default values. 

snippet: upgradeWithDefaults

##### Updating and specifying settings 

The second parameter set allows the values of the new required settings to be configured.  The sample below shows the upgrade parameters required to update ServiceControl version 1.0 to ServiceControl  version 2.0.  Any parameters provided overwrite existing configuration settings if they exist.   

snippet: upgradeWithSpecificValues

##### Backup the database prior to upgrade

To enable the backup of the embedded RavenDB as part of the upgrade add a `BackupPath` parameter to the options passed to `Invoke-ServiceControlUpgrade` as shown below.  This option is common to both parameters sets.

snippet: backupAndUpgrade 

In order for the backup to succeed the following must be true.  The target directory path passed to the `BackupPath` parameter:

* must be a fully qualified local directory path
* may already exist but must be empty  
* must be writable by the service account user
* can not be a root drive letter only (e.g. `c:`)

To carry out the backup the service will be started in maintenance mode and a RavenDB backup request issued.  Once the backup is completed the ServiceControl upgrade is carried out if the backup was successful.

#### Setting configuration entries not covered in the unattended install or upgrade

Note:  It is possible to suppress the automatic restart of a service after upgrade.  This is done by including the `SuppressRestart` parameter in the arguments passed to `Invoke-ServiceControlUpgrade`.

This example shows how to add configuration setting that are not covered by cmdlet parameters available via `New-ServiceControlInstance` or `Invoke-ServiceControlUpgrade`.  See [Configuration Settings](creating-config-file.md) for a full list of the available settings. 

 
snippet: customSettingsToConfig

Note: Adding incorrect values to the configuration may prevent the service from starting.  If the service fails to start consult the Windows Event log and the ServiceControl log files.