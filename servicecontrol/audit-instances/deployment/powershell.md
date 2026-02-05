---
title: Deploying ServiceControl Audit instances using PowerShell
reviewed: 2024-08-01
component: ServiceControl
redirects:
 - servicecontrol/audit-instances/installation-powershell
---
include: powershell-module-installation

## Audit instance Cmdlets and Aliases

The following cmdlets and aliases are provided by the ServiceControl Management PowerShell module for managing Audit instances.

| Alias                  | Cmdlet                                        |
| ---------------------- | --------------------------------------------- |
| audit-add              | New-ServiceControlAuditInstance               |
| audit-delete           | Remove-ServiceControlAuditInstance            |
| audit-instances        | Get-ServiceControlAuditInstances              |
| audit-upgrade          | Invoke-ServiceControlAuditInstanceUpgrade     |

include: powershell-remote-cmdlets-and-aliases

include: powershell-general-cmdlets-and-aliases

include: powershell-help

### Deploying an Audit instance

Using PowerShell, deploy the [ServiceControl Error instance](/servicecontrol/servicecontrol-instances/deployment/powershell.md) first, then deploy the ServiceControl Audit instance using the `New-ServiceControlAuditInstance` cmdlet:

snippet: ps-new-audit-instance

include: powershell-new-configuration

> [!NOTE]
> The address of a ServiceControl Error instance must be provided to send notifications to.

Once a ServiceControl Audit instance is created, it must be added to the ServiceControl Error instance as a [remote](/servicecontrol/servicecontrol-instances/remotes.md) to be included in results returned to [ServicePulse](/servicepulse/). Use the `Add-ServiceControlRemote` cmdlet to add a remote to the Error instance:

snippet: ps-add-audit-remote

### Listing deployed instances

Use the `Get-ServiceControlAuditInstances` cmdlet to find a list of all of the ServiceControl Audit instances and their version numbers:

snippet: ps-get-audit-instances

### Removing an instance

Before removing a ServiceControl Audit instance, it should be removed from the ServiceControl Error instance's registered [remotes](/servicecontrol/servicecontrol-instances/remotes.md) using the `Remove-ServiceControlRemote` cmdlet:

snippet: ps-remove-audit-remote

Use the `Remove-ServiceControlAuditInstance` cmdlet to remove the instance and delete the database and logs:

snippet: ps-remove-audit-instance

### Upgrading a instance

include: powershell-updatemodule

Once the PowerShell module is updated, use the `Invoke-ServiceControlAuditInstanceUpgrade` cmdlet to upgrade the Audit instance to the installed version:

snippet: ps-upgrade-audit-instance

include: powershell-instance-upgrade

include: powershell-importlicense

include: powershell-module-troubleshooting
