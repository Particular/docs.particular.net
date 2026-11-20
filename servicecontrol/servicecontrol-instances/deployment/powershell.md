---
title: Deploying ServiceControl Error instances using PowerShell
summary: A guide to setting up and deploying ServiceControl Error instances using PowerShell. Prerequisites, installation and deployment
reviewed: 2026-10-07
component: ServiceControl
redirects:
- servicecontrol/powershell
---

include: powershell-module-installation

## Error instance Cmdlets and Aliases

The following cmdlets and aliases are provided by the ServiceControl Management PowerShell module for managing ServiceControl Error instances.

| Alias                  | Cmdlet                                                     |
| ---------------------- | ---------------------------------------------------------- |
| sc-add                 | New-ServiceControlInstance                                 |
| sc-delete              | Remove-ServiceControlInstance                              |
| sc-instances           | Get-ServiceControlInstances                                |
| sc-maintenance-start   | Start-ServiceControlMaintenanceMode                        |
| sc-maintenance-stop    | Stop-ServiceControlMaintenanceMode                         |
| sc-upgrade             | Invoke-ServiceControlInstanceUpgrade                       |

include: powershell-general-cmdlets-and-aliases

include: powershell-help

### Deploying an Error instance

Use the `New-ServiceControlInstance` cmdlet to deploy a new ServiceControl Error instance:

snippet: ps-new-error-instance

include: powershell-new-configuration

include: powershell-skip-queue-creation

### Listing deployed instances

Use the `Get-ServiceControlInstances` cmdlet to find a list of all of the ServiceControl Error instances and their version numbers:

snippet: ps-get-error-instances

### Removing an instance

Use the `Remove-ServiceControlInstance` cmdlet to remove the instance and delete the database and logs:

snippet: ps-remove-error-instance

### Maintenance mode

Use the `Start-ServiceControlMaintenanceMode` cmdlet to stop ingestion while keeping the database available. The cmdlet stops the Windows service, enables [maintenance mode](/servicecontrol/storage/ravendb/accessing-database.md#windows-deployment-maintenance-mode) in the configuration file, and starts the service again:

snippet: ps-start-error-maintenance-mode

Use the `Stop-ServiceControlMaintenanceMode` cmdlet to stop the service and clear maintenance mode. The service stays stopped until it is started normally, for example with `Start-Service`. Maintenance mode stays enabled until this cmdlet runs:

snippet: ps-stop-error-maintenance-mode

### Upgrading a deployed instance

include: powershell-updatemodule

Once the PowerShell module is updated, use the `Invoke-ServiceControlInstanceUpgrade` cmdlet to upgrade the Audit instance to the installed version:

snippet: ps-upgrade-error-instance

include: powershell-instance-upgrade

include: powershell-importlicense

include: powershell-module-troubleshooting
