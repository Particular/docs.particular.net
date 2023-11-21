---
title: Upgrade ServiceControl from Version 4 to Version 5
summary: Instructions on how to upgrade ServiceControl from version 4 to 5
reviewed: 2023-10-12
isUpgradeGuide: true
---

## Overview

Upgrading ServiceControl from version 4 to version 5 is a major upgrade and requires careful planning. Throughout the upgrade process, the instance of ServiceControl will be offline and will not ingest messages.

## Breaking changes

* ServiceControl now uses a [new data format](#new-data-format) for data storage which is not compatible with previous versions.
* The Error Instance will no longer process [saga audit data](/nservicebus/sagas/saga-audit.md). If some endpoints are still configured to send saga audit data to the error instance instead of the audit instance, the error instance will attempt to forward the messages to the audit instance and display a [custom check warning](/monitoring/custom-checks/in-servicepulse.md) until the misconfigured endpoints are corrected.
* ServiceControl Management is no longer distributed as an installable package. Starting with version `5.0.0`, ServiceControl Management is shipped as a self-contained executable. This allows using different versions of ServiceControl Management side-by-side, without the need to reinstall a version before using it.
* The [ServiceControl PowerShell module](/servicecontrol/powershell.md) is no longer installed with ServiceControl.  Instead, the PowerShell module can be [installed from the PowerShell Gallery](/servicecontrol/powershell.md#installing-and-using-the-powershell-module).
* The [ServiceControl PowerShell module](/servicecontrol/powershell.md) requires PowerShell 7.2 or greater to run.
* `!disable` is no longer supported as an error and/or audit queue names. Instead, dedicated settings i.e. [`ServiceControl\IngestErrorMessages`](/servicecontrol/creating-config-file.md#transport-servicecontrolingesterrormessages) and [`ServiceControl\IngestAuditMessages`](/servicecontrol/audit-instances/creating-config-file.md#transport-servicecontrolingestauditmessages) should be used to control the message ingestion process. These settings are useful for upgrade scenarios, such as the one that will be described later in this article.

## New data format

Version 4.26 of ServiceControl introduced a [new persistence format](../new-persistence.md) for audit instances. Version 5 of ServiceControl uses the new persistence format for _all_ instance types.

As a result, not all ServiceControl instances can be automatically upgraded from Version 4 to Version 5, including the data. An automatic upgrade process is available for:

* Primary instances **but the process does not include data migration** i.e. all the data stored are deleted in the process. [The manual migration process](#new-data-format) describes how to migrate the data.
* Audit instances that use `RavenDB 5` storage engine (instances created with version 4.26 or later).
* All Monitoring instances.

## Upgrading to Version 5

Follow this procedure to upgrade all necessary ServiceControl instances to version 5:

1. Upgrade all ServiceControl instances to 4.33.0 or later. This is required to support the upgrade path that keeps all failed messages safe.
2. To preserve audit data, install a new Audit instance that uses RavenDB 5 persistence as described in [zero-downtime upgrades](../zero-downtime.md), if this has not already been done.
3. In ServicePulse, clean up all [failed messages](/servicepulse/intro-failed-messages.md). It's acceptable if a few failed messages still come in, but ideally, all failed messages should either be retried or archived.
4. Disable error message ingestion:
   * Stop the ServiceControl instance.
   * Open the `ServiceControl.exe.config` file.
   * Add an `appSetting` value: `<add key="ServiceControl/IngestErrorMessages" value="false" />`
   * Restart the ServiceControl instance. ServiceControl will be able to manage failed messages that have come in before the ingestion was disabled, but will not ingest any new error messages.
5. In ServicePulse, retry or archive any failed messages that have arrived during the upgrade process.
   * If a retried message fails again, it will go to the error queue, but the instance will not ingest it.
   * Once the failed message list is "clean" there will be no data of any value left in the database, making it safe to upgrade.
6. Using ServiceControl Management version 5, navigate to the [instance advanced options view](/servicecontrol/maintenance-mode.md)  and run a **Forced upgrade** for the version 4 ServiceControl instance.
7. Re-enable error message ingestion by removing the `IngestErrorMessages` setting from the `ServiceControl.exe.config` file.
8. Start the primary instance.
9. Upgrade any Audit instances that do not use RavenDB 3.5 persistence to ServiceControl 5.
10. Upgrade any Monitoring instances to ServiceControl 5.
11. Remove the old database for the Error instance:
    * In ServiceControl Management, click the **Browse…** button under **DB Path**.
    * In Windows Explorer, move up one directory level.
    * The old database will be located in this directory with a suffix. For example, if the database directory name was `DB`, the previous database directory will be named `DB_UpgradeBackup`. This database directory can be deleted to save disk space once confident that the upgrade process has been a success.

// Not sure what to do with this list, and it's somewhat duplicative of the bullet list under "new data format"

* Note that not all instances will be directly upgradeable:
  * The primary/error instance cannot be upgraded and must be replaced with a new instance as described below.
  * Any audit instances that use **RavenDB 5** for persistence can be upgraded to Version 5.
  * Any audit instances that use **RavenDB 3.5** for persistence cannot be upgraded, but can continue to serve queries until the stored data reaches its expiration according to [audit retention period settings](/servicecontrol/audit-instances/creating-config-file.md#data-retention-servicecontrol-auditauditretentionperiod) after which the instance can be removed.


## Support for version 4

Version 4 is supported for one year after version 5 is released as defined by the [ServiceControl support policy](/servicecontrol/upgrades/support-policy.md). The ServiceControl support end-date is available at [ServiceControl supported versions](/servicecontrol/upgrades/supported-versions.md).

## Planning

### Time for upgrade

This upgrade does not contain any data migrations, the size of the database does not have any impact on the time to perform the upgrade.

### Editing older instances

ServiceControl Management Utility version 5 cannot be used to edit ServiceControl instances until they have been upgraded to version 4. These instances can still be started, stopped, put into maintenance mode, and removed using ServiceControl Managament.

ServiceControl version 4.33.0 can be used to continue managing older instances. Version 4.33.0, which is still installed, can be used side-by-side with ServiceControl 5.

### Disk space requirements

ServiceControl instances that use RavenDB 5 must use a completely different database than the older RavenDB 3.5 counterparts:

* Adding a new Audit instance will have a new database, which will likely grow in size to the same or slightly larger than the previous Audit instance, given similar message volumes and retention settings.
* Upgrading an Error instance will set aside the old database by renaming the directory. When following the upgrade procedure above, it should be safe to delete the old database after all failed messages are dealt with, but the database directory is renamed instead as an extra precaution.

To create a cautious estimate, total the size of any existing RavenDB 3.5 databases and assume that 20% more space than that will be required during the migration process.

The old audit instance database can be removed after the retention period has lapsed, and the old error instance database can be removed once confident of a successful upgrade, meaning ultimately the remaining databases will be rougly the same size or slightly larger than their previous RavenDB 3.5 counterparts.

### Upgrading with PowerShell

// TODO: Didn't look at or try Powershell stuff below

Use the `Invoke-ServiceControlInstanceUpgrade` PowerShell cmdlet to  upgrade an existing ServiceControl instance to version 4.

```ps
Invoke-ServiceControlInstanceUpgrade -Name <Instance to Upgrade>
```

Example:
```ps
Invoke-ServiceControlInstanceUpgrade -Name Particular.ServiceControl
```

If the ServiceControl instance being upgraded manages an audit queue, then additional parameters must be specified for the creation of a new ServiceControl Audit instance.

WARN: The settings specified must not be for the current instance, but for the audit instance that will be created as part of this upgrade. Specifying settings that match the current instance will result in a failed upgrade.

```ps
Invoke-ServiceControlInstanceUpgrade `
  -Name <Name of Error instance> `
  -InstallPath <Path for Audit instance binaries> `
  -DBPath <Path for the Audit instance database> `
  -LogPath <Path for the Audit instance logs> `
  -Port <Port for the Audit instance api> `
  -DatabaseMaintenancePort <Port for the Audit instance embedded database> `
  [-ServiceAccountPassword <password for service account>] `
  [-Force]
```

Example:
```ps
Invoke-ServiceControlInstanceUpgrade `
  -Name Particular.ServiceControl `
  -InstallPath "C:\Program Files (x86)\Particular Software\Particular.Servicecontrol.Audit" `
  -DBPath "P:\Particular.Servicecontrol.Audit\DB" `
  -LogPath "P:\Particular.Servicecontrol.Audit\Logs" `
  -Port 44444 `
  -DatabaseMaintenancePort 44445
```

The following information is copied from the existing ServiceControl instance:

- Audit queue
- Audit log queue
- Forward audit messages
- Audit retention period
- Transport
- Connection string
- Host name
- Service account

NOTE: If this instance uses a domain account, the  account password must be supplied.

The name of the new audit instance will be derived from the name of the original instance.


## Primary instances migration procedure

WARNING: It is recommended to perform the following procedure on a test environment first and to perform most steps via Powershell

The following procedure will migrate the primary instance to minimize the impact on:

- existing endpoints which might be sending heartbeat and custom check messages to the servicecontrol queue.
- existing ServicePulse / ServiceInsight instance

The configurations for these do not need to be adjusted.

The following steps need to be performed

1. Cleanup error messages in ServicePulse
  - Retry/Archive failed messages
2. Disable error queue ingestion in ServiceControl Management Utility (SCMU)
3. Retry all remaining messages on the primary instance in ServicePulse
4. Wait until the retry group(s) completes
5. Stop the primary instance Windows service
  - via SCMU, Powershell, or Windows Service Control Manager
6. Move the instance
  - Unregister the windows service
  - Rename the installation folder
  - Rename the database folder
7. Create a new instance that uses the previous name
  - Any failed messages that were retried in step 3 but still fail will now reappear in ServicePulse

### Re-add remote audit instances

The previous instance likely had one or more remote audit instances registered. These can be readded via the [ServiceControl Powershell module](/servicecontrol/installation-powershell.md), specifically the `Add-ServiceControlRemote` command.

### Archive obsolete primary instance

In step 3 the previous primary instance was moved. Consider creating a backup of the installation and database folders and schedule when these folder can be deleted to free disk space.


## Audit instances migration procedure for RavenDB 3.5 instances

Perform the [zero downtime upgrade](/servicecontrol/upgrades/zero-downtime.md)

## Audit instances migration procedure for RavenDB 5 instances

Upgrade the instance via SCMU or Powershell
