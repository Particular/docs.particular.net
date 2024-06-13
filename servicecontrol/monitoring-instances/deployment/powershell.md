---
title: Deploying ServiceControl Monitoring instances using PowerShell
reviewed: 2024-06-11
component: ServiceControl
redirects:
 - servicecontrol/monitoring-instances/installation/installation-powershell
---

include: installation-powershell-module

## Monitoring instance Cmdlets and Aliases

The following cmdlets and aliases are provided by the ServiceControl Management PowerShell module for managing Monitoring instances.

| Alias                  | Cmdlet                                        |
| ---------------------- | --------------------------------------------- |
| mon-add                | New-MonitoringInstance                        |
| mon-delete             | Remove-MonitoringInstance                     |
| mon-instances          | Get-MonitoringInstances                       |
| mon-upgrade            | Invoke-MonitoringInstanceUpgrade              |
| sc-help                | Get-ServiceControlMgmtCommands                |
| sc-transportsinfo      | Get-ServiceControlTransportTypes              |

### Help

All of the cmdlets have local help which can be accessed via the standard PowerShell help command

```ps
Get-Help Get-MonitoringInstances
```

### Adding an instance

```ps
New-MonitoringInstance -Name Test.Monitoring -InstallPath C:\ServiceControlMonitor\Bin -LogPath C:\ServiceMonitor\Logs -Port 33335 -Transport MSMQ
```

Additional parameters are available to set configuration options such as hostname, transport connection string, and error queue.


### Removing an instance

Remove the instance that was created in the Add sample and delete the logs:

```ps
Remove-MonitoringInstance -Name Test.Monitoring -RemoveLogs
```

To List existing instances of the ServiceControl Monitoring service use `Get-MonitoringInstances`.

### Upgrading an instance

The following command will list the ServiceControl Monitoring instances currently installed and their version number.

```ps
Get-MonitoringInstances | Select Name, Version
```

To upgrade and instance to the latest version of the binaries run.

```ps
Invoke-MonitoringInstanceUpgrade -Name <Instance To upgrade>
```

The upgrade will stop the service if it is running. Additional parameters for `Invoke-MonitoringInstanceUpgrade` may be required. The configuration file of the existing version is examined before determining if all required settings are present. If a configuration setting is missing the cmdlet will throw an error indicating the required additional parameter.

include: troubleshooting-powershell-module
