---
title: Deploying ServiceControl Error instances using PowerShell
summary: A guide to setting up and deploying ServiceControl Error instances using PowerShell. Prerequisites, installation and deployment
reviewed: 2024-06-28
component: ServiceControl
redirects:
- servicecontrol/powershell
---

include: powershell-module-installation

## Error instance Cmdlets and Aliases

The following cmdlets and aliases are provided by the ServiceControl Management PowerShell module for the management of ServiceControl Error instances.

| Alias                  | Cmdlet                                                     |
| ---------------------- | ---------------------------------------------------------- |
| sc-add                 | New-ServiceControlInstance                                 |
| sc-delete              | Remove-ServiceControlInstance                              |
| sc-instances           | Get-ServiceControlInstances                                |
| sc-upgrade             | Invoke-ServiceControlInstanceUpgrade                       |

include: powershell-general-cmdlets-and-aliases

include: powershell-help

### Deploying an Error instance

Use the `New-ServiceControlInstance` cmdlet to deploy a new ServiceControl Error instance:

snippet: ps-new-error-instance

include: powershell-new-configuration

### Listing deployed instances

Use the `Get-ServiceControlInstances` cmdlet to find a list of all of the ServiceControl Error instances and their version numbers:

snippet: ps-get-error-instances

### Removing an instance

Use the `Remove-ServiceControlInstance` cmdlet to remove the instance and delete the database and logs:

snippet: ps-remove-error-instance

### Upgrading a deployed instance

include: powershell-updatemodule

Once the PowerShell module is updated, use the `Invoke-ServiceControlInstanceUpgrade` cmdlet to upgrade the Audit instance to the installed version:

snippet: ps-upgrade-error-instance

include: powershell-instance-upgrade

include: powershell-importlicense

include: powershell-module-troubleshooting
