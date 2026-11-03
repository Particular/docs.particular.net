---
title: Deploying ServiceControl Audit instances using PowerShell
reviewed: 2026-10-07
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

The following example uses RavenDB storage, which requires `-DBPath` and `-DatabaseMaintenancePort`. From ServiceControl 7, `-StorageType` is also required; leave it out for earlier versions.

snippet: ps-new-audit-instance-ravendb

From ServiceControl 7, instances can use [SQL Server](/servicecontrol/storage/sqlserver/) or [PostgreSQL](/servicecontrol/storage/postgresql/) storage instead. Set `-StorageType` to `SQLServer` or `PostgreSQL` and pass `-DatabaseConnectionString` instead of `-DBPath` and `-DatabaseMaintenancePort`, which apply only to RavenDB. The instance stores message bodies on the file system, in `%ProgramData%\Particular\ServiceControl\<instance name>\MessageBodies`. To use a different folder, Azure Blob Storage, or Amazon S3, change the [message body settings](/servicecontrol/audit-instances/configuration.md#storage) in the instance's configuration file. See [external message body storage](/servicecontrol/storage/external-body/). The following example uses the same database as the Error instance; the Audit instance's tables go in the `sc_audit` schema by default.

snippet: ps-new-audit-instance-sqlserver

include: powershell-new-configuration

include: powershell-skip-queue-creation

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

### Upgrading an instance

include: powershell-updatemodule

Once the PowerShell module is updated, use the `Invoke-ServiceControlAuditInstanceUpgrade` cmdlet to upgrade the Audit instance to the installed version:

snippet: ps-upgrade-audit-instance

include: powershell-instance-upgrade

include: powershell-importlicense

include: powershell-module-troubleshooting
