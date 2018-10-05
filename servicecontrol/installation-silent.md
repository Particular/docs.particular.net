---
title: Installing ServiceControl Silently
reviewed: 2018-10-05
tags:
- Installation
---

### Silent installation options

Note: This documentation covers silent installation instructions for ServiceControl versions 1.7 and greater.

The command line examples refer to the ServiceControl installation exe as `<install.exe>`.   Replace this with the specific exe name for the version being deployed.  (e.g. Particular.ServiceControl-1.22.0.exe)


The following command line will silently install the ServiceControl Management utility.

```dos
 <install.exe> /quiet
```

Instances of the ServiceControl service can be deleted, added, or upgraded via the ServiceControl Management utility or ServiceControl Management Powershell console .


#### Silently add ServiceControl during installation

The following command will silently install the ServiceControl Management utility and an instance of the ServiceControl service.

```bat
 <install.exe> /quiet /LV* install.log UNATTENDEDFILE=unattendfile.xml
```

For details on how to build the `unattendedfile.xml` file, refer to the ServiceControl Management [PowerShell](installation-powershell.md) documentation. The installed instance will use `localsystem` as the service account. To specify an alternative service account, use the `SERVICEACCOUNT` and `PASSWORD` command line switches.

```dos
<install.exe> /quiet /LV* install.log UNATTENDEDFILE=unattendfile.xml SERVICEACCOUNT=MyServiceAccount PASSWORD=MyPassword
```

NOTE: The settings contained in an unattended installation files are version-specific. The file contents will be validated when used and if a required setting is missing, an error will be logged. To correct this, regenerate the XML file using the `New-ServiceControlUnattendedFile` cmdlet.


#### Silently upgrade ServiceControl during installation

If an existing service matching the name specified in the unattended XML file already exists, the unattended install option is ignored. To update one or more instances of ServiceControl as part of the silent installation, the command line switch `UPGRADEINSTANCES` command line argument can be used.

In this example, ServiceControl Management is silently installed and an upgrade is attempted for all installed instances of the ServiceControl service. Either `*` or `ALL` can be used to specify all instances should be upgraded.

```dos
<install.exe> /quiet /LV* install.log UPGRADEINSTANCES=ALL
```

In this example, the ServiceControl Management utility is silently installed and an upgrade is attempted for just one instance called `TestServiceControl`.

```dos
<install.exe> /quiet /LV* install.log UPGRADEINSTANCES=TestServiceControl
```

To specify multiple instances use a comma-separated list:

```dos
 <install.exe> /quiet /LV* install.log UPGRADEINSTANCES=TestServiceControl,ProdServiceControl
```


#### Add the license file as part of the silent installation

In this example, the ServiceControl Management utility is silently installed and a license file is imported

```dos
<install.exe> /quiet /LV* install.log LICENSEFILE=license.xml
```


#### Specifying ForwardErrorMessages during upgrade

ServiceControl versions 1.11.1 and below automatically forward all messages read from the error queue to a secondary queue known as the _error forwarding_ queue. Starting from version 1.12.0, the MSI command line parameter `FORWARDERRORMESSAGES` was introduced to allow this behavior to be enabled or disabled as part of the upgrade of an instance.

When upgrading instances running on versions 1.11.1 and below, the `FORWARDERRORMESSAGES` parameter is mandatory. Valid options are `TRUE` and `FALSE`.

The error forwarding queue queue exists to allow external tools to receive error messages. If there is no process reading messages from the error forwarding queue, this setting should be `FALSE`.

```dos
<install.exe> /quiet /LV* install.log UPGRADEINSTANCES=ALL FORWARDERRORMESSAGES=FALSE
```


#### Specifying AuditRetentionPeriod and ErrorRetentionPeriod during upgrade

ServiceControl version 1.13 introduced two new mandatory application configuration settings to control the expiry of database content. These settings can be set using two new MSI switches when upgrading. Both of these values should be expressed as `TimeSpan` values

e.g `20.0:0:0` is 20 days

NOTE: If the current configuration already has values for `ServiceControl/AuditRetentionPeriod` or `ServiceControl/ErrorRetentionPeriod`, the command line values will overwrite the configuration


##### AuditRetentionPeriod

If the configuration does not contain the `ServiceControl/AuditRetentionPeriod` or `ServiceControl/HoursToKeepMessagesBeforeExpiring`setting, the value for the audit retention period should be included as a command line value. If the configuration does contain an entry for  `ServiceControl/HoursToKeepMessagesBeforeExpiring` then that value will be migrated to `ServiceControl/AuditRetentionPeriod` and no command line option is required. The valid range for this property is documented in [configuration settings](creating-config-file.md).

```dos
<install.exe> /quiet /LV* install.log UPGRADEINSTANCES=ALL AUDITRETENTION=30.0:0:0
```

NOTE: This value has a large impact on database size. Monitor the size of the ServiceControl database to ensure that this value is suitable.


##### ErrorRetentionPeriod

If the configuration does not contain the `ServiceControl/ErrorRetentionPeriod`, then the command line option is required. The valid range for this property is documented in the [configuration settings](creating-config-file.md).

```dos
<install.exe> /quiet /LV* install.log UPGRADEINSTANCES=ALL ERRORRETENTION=30.0:0:0
```

NOTE: This value has a large impact on database size. Monitor the size of the ServiceControl database to ensure that this value is suitable.


#### Combining command line options

It is valid to combine the `LICENSEFILE`, `UNATTENDEDFILE`,  `SERVICEACCOUNT` and `PASSWORD` options on the same command line. The `SERVICEACCOUNT` and `PASSWORD` options apply only to a new instance, these values are not used on upgrades.

The command line `UPGRADEINSTANCES` can be combined with `FORWARDERRORMESSAGES`, `AUDITRETENTIONPERIOD` and `ERRORRETENTIONPERIOD`.


#### Command line uninstall

The following command can be used to uninstall ServiceControl Management silently:

```dos
wmic product where (name like '%servicecontrol%') call uninstall
```

NOTE: This command will not remove any ServiceControl service instances that are currently deployed.


#### Logging and failures

In each of the samples above, a log file is specified on the command line. The silent install actions will log to the MSI log file specified. For ServiceControl versions 1.6.3 and below, if an installation action failed, the installation was rolled back resulting in failed upgrades acting like a complete uninstall of the product. From version 1.7, a failure to do an unattended install action is logged but the overall installation will not rollback. In this scenario, only the ServiceControl Management utility will have been updated. Instances can subsequently be upgraded through the ServiceControl Management utility.


#### PowerShell

All of the actions that can be carried out as unattended installation action are also available via the [ServiceControl Management PowerShell](installation-powershell.md) module.
