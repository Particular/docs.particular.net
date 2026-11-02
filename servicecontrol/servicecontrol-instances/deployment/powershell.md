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
| sc-upgrade             | Invoke-ServiceControlInstanceUpgrade                       |

include: powershell-general-cmdlets-and-aliases

include: powershell-help

### Deploying an Error instance

Use the `New-ServiceControlInstance` cmdlet to deploy a new ServiceControl Error instance:

The following example uses RavenDB storage, which requires `-DBPath` and `-DatabaseMaintenancePort`. From ServiceControl 7, `-StorageType` is also required; leave it out for earlier versions.

snippet: ps-new-error-instance-ravendb

From ServiceControl 7, instances can use [SQL Server](/servicecontrol/storage/sqlserver/) or [PostgreSQL](/servicecontrol/storage/postgresql/) storage instead. Set `-StorageType` to `SQLServer` or `PostgreSQL` and pass `-DatabaseConnectionString` instead of `-DBPath` and `-DatabaseMaintenancePort`, which apply only to RavenDB. Choose where large message bodies are stored with `-MessageBodyStorageType`: `FileSystem` (the default, with `-MessageBodyPath`), `AzureBlob` (with the `-AzureBlob*` parameters), or `S3` (with the `-S3*` parameters). See [external message body storage](/servicecontrol/storage/external-body/) for what each setting does.

snippet: ps-new-error-instance-sqlserver

include: powershell-new-configuration

include: powershell-skip-queue-creation

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
