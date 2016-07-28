---
title: Installing ServiceControl Silently
reviewed: 2016-03-24
tags:
- ServiceControl
- Installation
---


### Silent Installation Options

Note: This documentation covers silent installation instructions for ServiceControl Version 1.7 or greater.


The following command line will silently install the ServiceControl Management utility only.

```dos
Particular.ServiceControl.1.7.0.exe /quiet
```

Instances of the ServiceControl service can be deleted, added or upgraded via the Utility.


#### Silently Add ServiceControl during installation

The following command line will silently install the ServiceControl Management and a ServiceControl instance.

```bat
Particular.ServiceControl.1.7.0.exe /quiet /LV* install.log UNATTENDEDFILE=unattendfile.xml
```

For details on how to make the `unattendedfile.xml` file refer to ServiceControl Management [PowerShell](installation-powershell.md) documentation. The installed instance will use `localsystem` as the service account. To specify an alternative service account use the `SERVICEACCOUNT` and `PASSWORD` command line switches.

```dos
Particular.ServiceControl.1.7.0.exe /quiet /LV* install.log UNATTENDEDFILE=unattendfile.xml SERVICEACCOUNT=MyServiceAccount PASSWORD=MyPassword
```

NOTE: The settings contained in an unattended installation files are version specific. The file contents will be validated when used and if a required setting is missing an error will be logged. To correct this regenerate the XML file using the `New-ServiceControlUnattendedFile` cmdlet.


#### Silently Upgrade ServiceControl during installation

If an existing service matching the name specified in the unattended XML file already exists the unattended install option is ignored. To update one or more instances of ServiceControl as part of the silent installation the command line switch `UPGRADEINSTANCES` command line argument can be used.

In this example the ServiceControl Management Utility is silently installed and attempt to upgrade all the installed instances of the ServiceControl service. Either `*` or `ALL` can be used to specify all instances should be upgraded.

```dos
Particular.ServiceControl.1.7.0.exe /quiet /LV* install.log UPGRADEINSTANCES=ALL
```

In this example the ServiceControl Management Utility is silently installed and attempt to upgrade just one instance called `TestServiceControl`.

```dos
Particular.ServiceControl.1.7.0.exe /quiet /LV* install.log UPGRADEINSTANCES=TestServiceControl
```

To specify multiple instances use a comma separated list:

```dos
Particular.ServiceControl.1.7.0.exe /quiet /LV* install.log UPGRADEINSTANCES=TestServiceControl,ProdServiceControl
```


#### Add the license file as part of the Silent installation

In this example the ServiceControl Management Utility is silently installed and import the license file.

```dos
Particular.ServiceControl.1.7.0.exe /quiet /LV* install.log LICENSEFILE=license.xml
```


#### Specifying ForwardErrorMessages during Upgrade

Version 1.11.1 and below automatically forwarded all messages read from the Error queue to a secondary queue known as the Error Forwarding Queue. From Version 1.12.0 the MSI command line parameter `FORWARDERRORMESSAGES` was introduced to set to allow this behavior to be enabled or disabled as part of the upgrade of an instance.

When upgrading instances running on Version 1.11.1 and below the `FORWARDERRORMESSAGES` parameter is mandatory. Valid options are `TRUE` and `FALSE`.

The Error Forwarding Queue queue exists to allow external tools to receive error messages. If there is no process reading messages from the Error Forwarding Queue this setting should be `FALSE`.

```dos
Particular.ServiceControl.1.11.2.exe /quiet /LV* install.log UPGRADEINSTANCES=ALL FORWARDERRORMESSAGES=FALSE
```


#### Specifying AuditRetentionPeriod and ErrorRetentionPeriod during Upgrade

Version 1.13 introduced two new mandatory application configuration settings to control the expiry of database content. These setting can be set by using two new MSI switches when upgrading. Both of these value should be expressed as `TimeSpan` values

e.g 20.0:0:0 is 20 days

NOTE: If the current configuration already has values for `ServiceControl/AuditRetentionPeriod` or `ServiceControl/ErrorRetentionPeriod` the command line values will  overwrite the configuration


##### AuditRetentionPeriod

If the configuration does not contain the `ServiceControl/AuditRetentionPeriod` or `ServiceControl/HoursToKeepMessagesBeforeExpiring`setting the value for the audit retention period should be included as a command line value. If the configuration does contains an entry for  `ServiceControl/HoursToKeepMessagesBeforeExpiring` then that value will be migrated to `ServiceControl/AuditRetentionPeriod` and no command line option is required. The valid range for this property is documented in [configuration settings](creating-config-file.md).

```dos
Particular.ServiceControl.1.13.exe /quiet /LV* install.log UPGRADEINSTANCES=ALL AUDITRETENTION=30.0:0:0
```

NOTE: This value has a large impact on database size. Monitor the size of the ServiceControl database is recommended to ensure that this value is adequate.


##### ErrorRetentionPeriod

If the configuration does not contain the `ServiceControl/ErrorRetentionPeriod` then the command line option is required. The valid range for this property is documented in [configuration settings](creating-config-file.md).

```dos
Particular.ServiceControl.1.13.exe /quiet /LV* install.log UPGRADEINSTANCES=ALL ERRORRETENTION=30.0:0:0
```

NOTE: This value has a large impact on database size. Monitor the size of the ServiceControl database is recommended to ensure that this value is adequate.


#### Combining command line options

It is valid to combine the `LICENSEFILE`, `UNATTENDEDFILE`,  `SERVICEACCOUNT` and `PASSWORD` options on the same command line. The `SERVICEACCOUNT` and `PASSWORD` only apply to a new instance, these values are not used on upgrades.

The command line `UPGRADEINSTANCES` can be combined with `FORWARDERRORMESSAGES`, `AUDITRETENTIONPERIOD` and `ERRORRETENTIONPERIOD`.


#### Command line Uninstall

The following command can be used to uninstall ServiceControl Management Utility silently:

```dos
wmic product where (name like '%servicecontrol%') call uninstall
```

NOTE: This command will not remove any ServiceControl service instances that are currently deployed.


#### Logging and Failures

In each of the samples above a log file was specified on the command line. The silent install actions will log to the MSI log file specified. For Version 1.6.3 and below if an installation action failed the installation was rolled back, this resulted in failed upgrades acting like a complete uninstall of the product. For Version 1.7 and above a failure to do an unattended install action will be logged but the overall installation will not rollback, in this scenario only the ServiceControl Management Utility will have been updated. Instances can subsequently be upgrade through the ServiceControl Management Utility.


#### PowerShell

All of the actions that can be carried out as unattended installation action are also available via the [ServiceControl Management PowerShell](installation-powershell.md).


#### Setting configuration entries not covered in the unattended file

The unattended file does not cover all the settings available to customize the operation of the ServiceControl service. The following PowerShell script shows a simple way to script the modification of some of the optional configuration settings. The provided script makes use of the ServiceControl Management PowerShell module shipped with version 1.7 to find the configuration file locations.

Prior to using the script, modify the `$customSettings` hash table to set the optional configuration settings desired as key/value pairs. Refer to the [configuration settings](creating-config-file.md) documentation details on how to set those settings.

NOTE: The provided entries in the `$customSettings` hash table are to illustrate how to set the values and are not meant to be a recommendation on the values for these settings.

```ps
#Requires -Version 3
#Requires -RunAsAdministrator

Add-Type -AssemblyName System.Configuration
Import-Module 'C:\Program Files (x86)\Particular Software\ServiceControl Management\ServiceControlMgmt.psd1'

$customSettings = @{
    'ServiceControl/HeartbeatGracePeriod'='00:01:30' 
}

foreach ($sc in Get-ServiceControlInstances)
{
	$exe = Join-Path $sc.InstallPath -ChildPath 'servicecontrol.exe'
	$configManager = [System.Configuration.ConfigurationManager]::OpenExeConfiguration($exe)
	$appSettings = $configManager.AppSettings.Settings
	foreach ($key in $customSettings.Keys)
	{
		$appSettings.Remove($key)
		$appSettings.Add((New-Object System.Configuration.KeyValueConfigurationElement($key, $customSettings[$key])))
	}
	$configManager.Save()
	Restart-Service $sc.Name
}
```
