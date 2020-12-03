---
title: Manage ServiceControl instances via PowerShell
reviewed: 2019-07-02
---

NOTE: For general information about using PowerShell with ServiceControl, including troubleshooting and licensing guidance, see [Managing ServiceControl via PowerShell](/servicecontrol/powershell.md).

## ServiceControl instance Cmdlets and Aliases

The following cmdlets and aliases are provided by the ServiceControl Management PowerShell module for the management of ServiceControl instances.

| Alias                  | Cmdlet                                                     |
| ---------------------- | ---------------------------------------------------------- |
| sc-add                 | New-ServiceControlInstance                                 |
| sc-addfromunattendfile | New-ServiceControlInstanceFromUnattendedFile (deprecated)  |
| sc-delete              | Remove-ServiceControlInstance                              |
| sc-instances           | Get-ServiceControlInstances                                |
| sc-makeunattendfile    | New-ServiceControlUnattendedFile                           |
| sc-transportsinfo      | Get-ServiceControlTransportTypes                           |
| sc-upgrade             | Invoke-ServiceControlInstanceUpgrade                       |

The following cmdlets are available in ServiceControl version 4 and above, for the management of ServiceControl Audit instances.

| Alias                  | Cmdlet                                        |
| ---------------------- | --------------------------------------------- |
| audit-add              | New-ServiceControlAuditInstance               |
| audit-delete           | Remove-ServiceControlAuditInstance            |
| audit-instances        | Get-ServiceControlAuditInstances              |
| audit-upgrade          | Invoke-ServiceControlAuditInstanceUpgrade     |

The following cmdlets are available in ServiceControl version 4 and above, for the management of ServiceControl remotes.

| Alias                  | Cmdlet                                        |
| ---------------------- | --------------------------------------------- |
| sc-addremote           | Add-ServiceControlRemote                      |
| sc-deleteremote        | Remove-ServiceControlRemote                   |
| sc-remotes             | Get-ServiceControlRemotes                     |



### Help

All of the cmdlets have local help which can be accessed via the standard PowerShell help command

```ps
Get-Help Get-ServiceControlInstances
```


### Adding an instance

Use the `New-ServiceControlInstance` cmdlet to create a new ServiceControl instance. Version 3 and below contains parameters to configure audit ingestion and retention. Managing an audit queue on version 4 and above requires creating a new ServiceControl Audit instance with `New-ServiceControlAuditInstance`.

There are additional parameters available to set additional configuration options such as forwarding queues, the transport connection string, or host name.

#### Version 3

Create a ServiceControl instance: 

```ps
New-ServiceControlInstance `
-Name Test.ServiceControl `
-InstallPath C:\ServiceControl\Bin `
-DBPath C:\ServiceControl\DB `
-LogPath C:\ServiceControl\Logs `
-Port 33334 `
-DatabaseMaintenancePort 33335 `
-Transport MSMQ `
-ErrorQueue error1 `
-AuditQueue audit1 `
-ForwardAuditMessages:$false `
-AuditRetentionPeriod 01:00:00 `
-ErrorRetentionPeriod 10:00:00:00
```

#### Version 4

Create a ServiceControl instance:

```ps
$serviceControlInstance = New-ServiceControlInstance `
  -Name Test.ServiceControl `
  -InstallPath C:\ServiceControl\Bin `
  -DBPath C:\ServiceControl\DB `
  -LogPath C:\ServiceControl\Logs `
  -Port 33334 `
  -DatabaseMaintenancePort 33335 `
  -Transport MSMQ `
  -ErrorQueue error1 `
  -ErrorRetentionPeriod 10:00:00:00
```

Optionally create a ServiceControl Audit instance to manage an audit queue:

```ps
$auditInstance = New-ServiceControlAuditInstance `
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
  -ServiceControlQueueAddress Test.ServiceControl

Add-ServiceControlRemote `
  -Name $serviceControlInstance.Name `
  -RemoteInstanceAddress $auditInstance.Url
```

NOTE: The ServiceControl Audit instance must be configured with the transport address of a ServiceControl instance.

### Removing an instance

Use the `Remove-ServiceControlInstance` cmdlet to remove a ServiceControl instance. 

```ps
Remove-ServiceControlInstance -Name Test.ServiceControl -RemoveDB -RemoveLogs
```

In version 4 and above, use the `Remove-ServiceControlAuditInstance` cmdlet to remove a ServiceControl Audit instance.

```ps
Remove-ServiceControlRemote `
 -Name Test.ServiceControl `
 -RemoteInstanceAddress http://localhost:44444/api

Remove-AuditInstance `
  -Name Test.ServiceControl.Audit
  -RemoveDB -RemoveLogs
```

NOTE: All connected ServiceControl Audit instances should be removed before removing the main ServiceControl instance. Use the `Get-ServiceControlRemotes` cmdlet to find a list of connected ServiceControl Audit instances for a given ServiceControl instance.


### Upgrading an instance

The cmdlets in this section are used to upgrade the binaries of an existing instance. If the instance is running when the upgrade starts, it will be shut down during the upgrade and restarted once the upgrade is complete.

Before the upgrade begins the configuration file of the existing version is examined to determine if all of the required settings are present. If a configuration setting is missing then the cmdlet will throw an error indicating the required additional parameter for the cmdlet.

Use the `Invoke-ServiceControlInstanceUpgrade` cmdlet to upgrade a ServiceControl instance to the latest binaries.

```ps
Invoke-ServiceControlInstanceUpgrade -Name <Instance To upgrade>
```

Use the following to find a list of all of the ServiceControl instances and their version numbers:

```ps
Get-ServiceControlInstances | Select Name, Version
```

Additional parameters may be required when upgrading an instance to version 4. See the [upgrade guide](/servicecontrol/upgrades/3to4/) for more details.

```ps
Invoke-ServiceControlInstanceUpgrade `
  -Name <Name of main instance> `
  -InstallPath <Path for Audit instance binaries> `
  -DBPath <Path for the Audit instance database> `
  -LogPath <Path for the Audit instance logs> `
  -Port <Port for the Audit instance api> `
  -DatabaseMaintenancePort <Port for the Audit instance embedded database> `
  [-ServiceAccountPassword <password for service account>] `
  [-Force]
```

On version 4 and above, use the `Invoke-AuditInstanceUpgrade` cmdlet to upgrade a ServiceControl audit instance to the latest binaries.

```ps
Invoke-ServiceControlAuditInstanceUpgrade -Name <Instance To upgrade>
```

Use the following command to find a list of all of the ServiceControl Audit instances and their version numbers:

```ps
Get-ServiceControlAuditInstances | Select Name, Version
```
