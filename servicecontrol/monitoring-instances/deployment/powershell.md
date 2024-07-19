---
title: Deploying ServiceControl Monitoring instances using PowerShell
reviewed: 2024-06-28
component: ServiceControl
redirects:
 - servicecontrol/monitoring-instances/installation/installation-powershell
---

include: powershell-module-installation

## Monitoring instance Cmdlets and Aliases

The following cmdlets and aliases are provided by the ServiceControl Management PowerShell module for managing Monitoring instances.

| Alias                  | Cmdlet                                        |
| ---------------------- | --------------------------------------------- |
| mon-add                | New-MonitoringInstance                        |
| mon-delete             | Remove-MonitoringInstance                     |
| mon-instances          | Get-MonitoringInstances                       |
| mon-upgrade            | Invoke-MonitoringInstanceUpgrade              |

include: powershell-general-cmdlets-and-aliases

include: powershell-help

### Deploying a Monitoring instance

Use the `New-MonitoringInstance` cmdlet to deploy a new ServiceControl Monitoring instance:

snippet: ps-new-monitoring-instance

include: powershell-new-configuration

### Listing deployed instances

Use the `Get-MonitoringInstances` cmdlet to find a list of all of the ServiceControl Error instances and their version numbers:

snippet: ps-get-monitoring-instances

### Removing an instance

Use the `Remove-MonitoringInstance` cmdlet to remove the instance and delete the logs:

snippet: ps-remove-monitoring-instance

### Upgrading a deployed instance

include: powershell-updatemodule

Once the PowerShell module is updated, use the `Invoke-MonitoringInstanceUpgrade` cmdlet to upgrade the Audit instance to the installed version:

snippet: ps-upgrade-monitoring-instance

To upgrade and instance to the latest version of the binaries run.

```ps
Invoke-MonitoringInstanceUpgrade -Name <Instance To upgrade>
```

include: powershell-instance-upgrade

include: powershell-importlicense

include: powershell-module-troubleshooting
