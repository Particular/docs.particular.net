---
title: Deploying ServiceControl Error instances using PowerShell
summary: A guide to setting up and deploying ServiceControl Error instances using PowerShell. Prerequisites, installation and deployment
reviewed: 2026-06-01
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

Use the `New-ServiceControlInstance` cmdlet to deploy a new ServiceControl Error instance. The storage parameters depend on the storage type, and each storage type has its own example below.

include: powershell-new-configuration

include: powershell-skip-queue-creation

#### RavenDB storage

RavenDB storage requires `-DBPath` and `-DatabaseMaintenancePort`. From ServiceControl 7, `-StorageType RavenDB` is also required. Earlier versions do not have the `-StorageType` parameter, so leave it out.

snippet: ps-new-error-instance-ravendb

#### SQL Server or PostgreSQL storage

_Added in version 7.0.0_

Set `-StorageType` to `SQLServer` or `PostgreSQL` and pass `-DatabaseConnectionString`. See [SQL Server storage](/servicecontrol/storage/sqlserver/) or [PostgreSQL storage](/servicecontrol/storage/postgresql/) to prepare the database. The cmdlet does not accept `-DBPath`, `-DatabaseMaintenancePort`, or `-EnableFullTextSearchOnBodies` with these storage types.

The following example stores message bodies on the file system, in the directory given by `-MessageBodyPath`.

snippet: ps-new-error-instance-sqlserver

#### Message body storage

include: powershell-message-body-storage

### Listing deployed instances

Use the `Get-ServiceControlInstances` cmdlet to find a list of all of the ServiceControl Error instances and their version numbers:

snippet: ps-get-error-instances

### Removing an instance

Use the `Remove-ServiceControlInstance` cmdlet to remove the instance. `-RemoveLogs` also deletes the logs directory. `-RemoveDB` also deletes the data that is stored on the machine, which depends on the storage type:

include: instance-data-removal

snippet: ps-remove-error-instance

### Upgrading a deployed instance

include: powershell-updatemodule

Once the PowerShell module is updated, use the `Invoke-ServiceControlInstanceUpgrade` cmdlet to upgrade the Audit instance to the installed version:

snippet: ps-upgrade-error-instance

include: powershell-instance-upgrade

include: powershell-importlicense

include: powershell-module-troubleshooting
