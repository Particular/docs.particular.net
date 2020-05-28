---
title: Managing ServiceControl Audit instances via PowerShell
reviewed: 2019-07-09
---

NOTE: For general information about ServiceControl PowerShell, including troubleshooting and licensing guidance, see [Managing ServiceControl via PowerShell](/servicecontrol/powershell.md).


## ServiceControl Audit instance Cmdlets and Aliases

The following cmdlets and aliases are provided by the ServiceControl Management PowerShell module for managing Audit instances.

| Alias                  | Cmdlet                                        |
| ---------------------- | --------------------------------------------- |
| audit-add              | New-ServiceControlAuditInstance               |
| audit-delete           | Remove-ServiceControlAuditInstance            |
| audit-instances        | Get-ServiceControlAuditInstances              |
| audit-upgrade          | Invoke-ServiceControlAuditInstanceUpgrade     |
| sc-help                | Get-ServiceControlMgmtCommands                |
| sc-transportsinfo      | Get-ServiceControlTransportTypes              |


### Help

All of the cmdlets have local help which can be accessed via the standard PowerShell help command

```ps
Get-Help Get-ServiceControlAuditInstances
```


### Adding an instance

```ps
$primaryInstanceName = "Test.ServiceControl"

$auditInstance New-ServiceControlAuditInstance `
  -Name Test.ServiceControl.Audit `
  -InstallPath C:\ServiceControl.Audit\Bin `
  -DBPath C:\ServiceControl.Audit\DB `
  -LogPath C:\ServiceControl.Audit\Logs `
  -Port 44444 `
  -DatabaseMaintenancePort 44445 `
  -Transport MSMQ `
  -AuditQueue audit1 `
  -AuditRetentionPeriod 10:00:00:00 `
  -ForwardAuditMessages:$false `
  -ServiceControlQueueAddress $primaryInstanceName
```

There are additional parameters available to set configuration options such as hostname and transport connection string.

NOTE: The address of a ServiceControl instance must be provided to send notifications to.

Once a ServiceControl Audit instance is created, it must be added to the primary ServiceControl instance as a remote to be included in results returned to ServiceInsight.

```ps
Add-ServiceControlRemote -Name $primaryInstanceName -RemoteInstanceAddress $auditInstance.Url
```


### Removing an instance

Before removing a ServiceControl Audit instance, it should be removed from the primary ServiceControl instances list of remotes.

```ps
Remove-ServiceControlRemote -Name "Test.ServiceControl" -RemoteInstanceAddress "http://localhost:44444/api"
```

Remove the instance that was created in the Add sample and delete the database and logs:

```ps
Remove-ServiceControlAuditInstance -Name Test.ServiceControl.Audit -RemoveDB -RemoveLogs
```

To List existing instances of the ServiceControl Audit service use `Get-ServiceControlAuditInstances`.


### Upgrading an instance

To upgrade an instance to the latest version of the binaries run.

```ps
Invoke-ServiceControlAuditInstanceUpgrade -Name <Instance To upgrade>
```

Use the following command to find a list of all of the ServiceControl Audit instances and their version numbers:

```ps
Get-ServiceControlAuditInstances | Select Name, Version
```

The upgrade will stop the service if it is running. Additional parameters for `Invoke-ServiceControlAuditInstanceUpgrade` may be required. The configuration file of the existing version is examined prior to determine if all the required settings are present. If a configuration setting is missing  then the cmdlet will throw an error indicating the required additional parameter.