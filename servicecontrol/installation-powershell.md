---
title: Manage ServiceControl instances via PowerShell
reviewed: 2017-07-26
tags:
 - Installation
 - PowerShell
---

NOTE: For general information about ServiceControl Powershell, including troubleshooting and licensing guidance, see [Managing ServiceControl via PowerShell](/servicecontrol/powershell.md).

## ServiceControl instance Cmdlets and Aliases

The following cmdlets and aliases are provided by the ServiceControl Management PowerShell module for the management of ServiceControl instances.

| Alias                  | Cmdlet                                        |
| ---------------------- | --------------------------------------------- |
| sc-add                 | New-ServiceControlInstance                    |
| sc-addfromunattendfile | New-ServiceControlInstanceFromUnattendedFile  |
| sc-delete              | Remove-ServiceControlInstance                 |
| sc-instances           | Get-ServiceControlInstances                   |
| sc-makeunattendfile    | New-ServiceControlUnattendedFile              |
| sc-transportsinfo      | Get-ServiceControlTransportTypes              |
| sc-upgrade             | Invoke-ServiceControlInstanceUpgrade          |



### Help

All of the cmdlets have local help which can be accessed via the standard PowerShell help command

```ps
Get-Help Get-ServiceControlInstances
```


### Adding an instance

```ps
New-ServiceControlInstance -Name Test.ServiceControl -InstallPath C:\ServiceControl\Bin -DBPath C:\ServiceControl\DB -LogPath C:\ServiceControl\Logs -Port 33334 -DatabaseMaintenancePort 33335 -Transport MSMQ -ErrorQueue error1 -AuditQueue audit1 -ForwardAuditMessages:$false -AuditRetentionPeriod 01:00:00 -ErrorRetentionPeriod 10:00:00:00
```

There are additional parameters available to set additional configuration options such as forwarding queues, the transport connection string or hostname.


### Removing an instance

The following commands show how to remove a ServiceControl instance(s). To List existing instances of the ServiceControl service use `Get-ServiceControlInstances`.

Remove the instance that was created in the Add sample and delete the database and logs:

```ps
Remove-ServiceControlInstance -Name Test.ServiceControl -RemoveDB -RemoveLogs
```

Remove all ServiceControl instance created in the Add sample and delete the database and logs for each one:

```ps
Get-ServiceControlInstances | Remove-ServiceControlInstance -RemoveDB -RemoveLogs
```

There are additional parameters available to set additional configuration options such as forwarding queues, the transport connection string or host name.


### Upgrading an instance

The following command will list the ServiceControl instances current installed and their version number.

```ps
Get-ServiceControlInstances | Select Name, Version
```

To upgrade and instance to the latest version of the binaries run.

```ps
Invoke-ServiceControlInstanceUpgrade -Name <Instance To upgrade>
```

The upgrade will stop the service if it is running. Additional parameters for `Invoke-ServiceControlInstanceUpgrade` may be required. The configuration file of the existing version is examined prior to determine if all the required settings are present. If a configuration setting is missing then the cmdlet will throw an error indicating the required additional parameter.
