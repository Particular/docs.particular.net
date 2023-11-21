---
title: Manage ServiceControl instances via PowerShell
reviewed: 2021-08-23
component: ServiceControl
---

NOTE: For general information about using PowerShell with ServiceControl, including troubleshooting and licensing guidance, see [Managing ServiceControl via PowerShell](/servicecontrol/powershell.md).

## ServiceControl instance Cmdlets and Aliases

The following cmdlets and aliases are provided by the ServiceControl Management PowerShell module for the management of ServiceControl instances.

partial: cmdlets-and-aliases

### Help

All of the cmdlets have local help which can be accessed via the standard PowerShell help command

snippet: ps-get-help

### Adding an instance

Use the `New-ServiceControlInstance` cmdlet to create a new ServiceControl instance. Version 3 and below contains parameters to configure audit ingestion and retention. Managing an audit queue on version 4 and above requires creating a new ServiceControl Audit instance with `New-ServiceControlAuditInstance`.

There are additional parameters available to set additional configuration options such as forwarding queues, the transport connection string, or host name.

Create a ServiceControl instance:

snippet: new-servicecontrol-instance

Create a ServiceControl Audit instance to manage an audit queue:

snippet: new-audit-instance

NOTE: The ServiceControl Audit instance must be configured with the transport address of a ServiceControl instance.

### Removing an instance

Use the `Remove-ServiceControlInstance` cmdlet to remove a ServiceControl instance.

snippet: remove-servicecontrol-instance

Use the `Remove-ServiceControlAuditInstance` cmdlet to remove a ServiceControl Audit instance.

snippet: remove-audit-instance

NOTE: All connected ServiceControl Audit instances should be removed before removing the ServiceControl Error instance. Use the `Get-ServiceControlRemotes` cmdlet to find a list of connected ServiceControl Audit instances for a given ServiceControl instance.

### Upgrading an instance

The cmdlets in this section are used to upgrade the binaries of an existing instance. If the instance is running when the upgrade starts, it will be shut down during the upgrade and restarted once the upgrade is complete.

WARN: The `sc-upgrade` and `audit-upgrade` commands do not download the latest updates. The required packages must be downloaded to a local folder before running the upgrade command. The update files are typically zip files that must be placed in the ServiceControl Management installation location.

Before the upgrade begins the configuration file of the existing version is examined to determine if all of the required settings are present. If a configuration setting is missing then the cmdlet will throw an error indicating the required additional parameter for the cmdlet.

Use the `Invoke-ServiceControlInstanceUpgrade` cmdlet to upgrade a ServiceControl instance to the latest binaries.

snippet: upgrade-servicecontrol-instance

Use the following to find a list of all of the ServiceControl instances and their version numbers:

snippet: get-instances

Additional parameters may be required when upgrading instances. See the [upgrade guide](/servicecontrol/upgrades/) for the specific version for more details.

Use the `Invoke-AuditInstanceUpgrade` cmdlet to upgrade a ServiceControl audit instance to the latest binaries.

snippet: upgrade-audit-instance

Use the following command to find a list of all of the ServiceControl Audit instances and their version numbers:

snippet: get-audit-instances

### Licensing

Copies the license file to the correct location on the file system (`%PROGRAMDATA%/ParticularSoftware/license.xml`) so it is available to all instances of ServiceControl installed on the machine.

snippet: ps-importlicense
