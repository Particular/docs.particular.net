---
title: Upgrade ServiceControl from Version 4 to Version 5
summary: Instructions on how to upgrade ServiceControl from version 4 to 5
reviewed: 2023-11-30
isUpgradeGuide: true
component: ServiceControl
---

Upgrading ServiceControl from version 4 to version 5 is a major upgrade and requires careful planning. Throughout the upgrade process, the instance of ServiceControl will be offline and will not ingest messages

## Breaking changes

* ServiceControl now uses a [new data format](#new-data-format) for data storage which is not compatible with previous versions.
* The Error Instance will no longer process [saga audit data](/nservicebus/sagas/saga-audit.md). If some endpoints are still configured to send saga audit data to the error instance instead of the audit instance, the error instance will attempt to forward the messages to the audit instance and display a [custom check warning](/monitoring/custom-checks/in-servicepulse.md) until the misconfigured endpoints are corrected.
* ServiceControl Management is no longer distributed as an installable package. Starting with version `5.0.0`, ServiceControl Management is now distributed as a portable application that is used to create, update, and delete ServiceControl instances. This allows using different versions of ServiceControl Management side-by-side, without the need to uninstall or reinstall different versions.
* The ServiceControl PowerShell module is no longer installed with ServiceControl. Instead, the PowerShell module can be [installed from the PowerShell Gallery](https://www.powershellgallery.com/packages/Particular.ServiceControl.Management/).
* The [ServiceControl PowerShell module](https://www.powershellgallery.com/packages/Particular.ServiceControl.Management/) requires PowerShell 7.2 or greater to run.
* PowerShell: The `Transport` parameter no longer accepts the DisplayName descriptions but only the Name code. See [PowerShell Transport argument](#powershell-transport-argument)
* ServiceControl instances using **Azure Service Bus - Endpoint-oriented topology (Legacy)** as the message transport cannot be directly upgraded to ServiceControl version 5.
  * See [Migrating Azure Service Bus](#migrating-azure-service-bus)
* `!disable` is no longer supported as an error and/or audit queue names. Instead, dedicated settings i.e. [`ServiceControl/IngestErrorMessages`](/servicecontrol/servicecontrol-instances/configuration.md#transport-servicecontrolingesterrormessages) and [`ServiceControl/IngestAuditMessages`](/servicecontrol/audit-instances/configuration.md#transport-servicecontrolingestauditmessages) should be used to control the message ingestion process. These settings are useful for upgrade scenarios, such as the one that will be described later in this article.
* The setting `IndexStoragePath` is no longer supported.  Use [symbolic links (soft links) to map any storage subfolder](https://ravendb.net/docs/article-page/5.4/csharp/server/storage/customizing-raven-data-files-locations) to other physical drives.
* The [`ServiceControl.Audit/RavenDBLogLevel`](/servicecontrol/audit-instances/configuration.md#host-settings-servicecontrol-auditravendbloglevel) and [`ServiceControl/RavenDBLogLevel`](/servicecontrol/servicecontrol-instances/configuration.md#host-settings-servicecontrolravendbloglevel) settings use new values. The previous values are mapped to new values native to RavenDB5.

## New data format

Version 4.26 of ServiceControl introduced a [new persistence format](/servicecontrol/migrations/new-persistence.md) for audit instances. Version 5 of ServiceControl uses the new persistence format for _all_ instance types.

As a result, not all ServiceControl instances can be automatically upgraded from Version 4 to Version 5, including the data. An automatic upgrade process is available for:

* Primary instances **but the process does not include data migration** i.e. all the data stored are deleted in the process. [The manual migration process](#upgrading-to-version-5) describes how to migrate the data.
* Audit instances that use `RavenDB 5` storage engine (instances created with version 4.26 or later).
* All Monitoring instances.

## PowerShell Transport argument

The value passed for the `-Transport` argument has changed in ServiceControl version 5. This value must now be a code which is shorter in length and does not have spaces.

To find the information for the transports supported by any particular release of the PowerShell plugin, use the cmdlet `Get-ServiceControlTransportTypes`.

## Planning

### Time for upgrade

This upgrade does not contain any data migrations, the size of the database does not have any impact on the time to perform the upgrade.

### Editing older instances

ServiceControl Management Utility version 5 cannot be used to edit ServiceControl instances until they have been upgraded to version 4. These instances can still be started, stopped, put into maintenance mode, and removed using ServiceControl Management.

ServiceControl Management version 4.33.0 can be used to continue managing older instances. Version 4.33.0, which is still installed, can be used side-by-side with ServiceControl 5.

### Disk space requirements

ServiceControl instances that use RavenDB 5 must use a completely different database than the older RavenDB 3.5 counterparts:

* Adding a new Audit instance will have a new database, which will likely grow in size to the same or slightly larger than the previous Audit instance, given similar message volumes and retention settings.
* Upgrading an Error instance will set aside the old database by renaming the directory. When following the upgrade procedure above, it should be safe to delete the old database after all failed messages are dealt with, but the database directory is renamed instead as an extra precaution.

To create a cautious estimate, total the size of any existing RavenDB 3.5 databases and assume that 20% more space than that will be required during the migration process.

The old audit instance database can be removed after the retention period has lapsed, and the old error instance database can be removed once confident of a successful upgrade, meaning ultimately the remaining databases will be roughly the same size or slightly larger than their previous RavenDB 3.5 counterparts.

## Upgrading to Version 5

Follow this procedure to upgrade all necessary ServiceControl 4 instances to version 5.

> [!NOTE]
> This procedure should first be run in a test environment.

### Getting ready

These steps should be followed whether updating using ServiceControl Management or via PowerShell:

1. Upgrade all ServiceControl instances to 4.33.0 or later. *This is required to support the upgrade path that keeps all failed messages safe.*
2. To preserve audit data, install a new Audit instance that uses RavenDB 5 persistence as described in [Replacing Audit instances](/servicecontrol/migrations/replacing-audit-instances/), if this has not already been done.
3. In ServicePulse, clean up all [failed messages](/servicepulse/intro-failed-messages.md). It's acceptable if a few failed messages still come in, but ideally, all failed messages should either be retried or archived.
4. Disable error message ingestion:
   * Stop the ServiceControl instance.
   * Open the `ServiceControl.exe.config` file.
   * Add an `appSetting` value: `<add key="ServiceControl/IngestErrorMessages" value="false" />`
   * Restart the ServiceControl instance. ServiceControl will be able to manage failed messages that have come in before the ingestion was disabled, but will not ingest any new error messages.
5. In ServicePulse, retry or archive any failed messages that have arrived during the upgrade process.
   * If a retried message fails again, it will go to the error queue, but the instance will not ingest it.
   * Once the failed message list is "clean" there will be no data of any value left in the database, making it safe to upgrade.


> [!NOTE]
> If data migration is not required for existing error messages, for example on developer workstations, the [forced upgrade procedure](#force-upgrading) can be used on instances with version 4.26.0 or later. This is the minimum version that can be upgraded to ServiceControl 5.

### Upgrading using ServiceControl Management

Follow this procedure to upgrade using the ServiceControl Management Utility:

1. Ensure the [getting ready](#upgrading-to-version-5-getting-ready) steps above have been completed.
2. Using ServiceControl Management version 5, perform a forced upgrade on the Error instance:
    * Click the wrench <kbd> :wrench: </kbd> icon to access to the Error instance's Advanced Options screen.
    * Under **Force Upgrade to Version 5**, click the **Upgrade Instance** button and follow the prompts.
    * _**Note:** This is a destructive operation. A database backup is made but will require application re-installation of the instance to recover._
3. Re-enable error message ingestion by removing the `IngestErrorMessages` setting from the `ServiceControl.exe.config` file.
4. Restart the primary instance for the configuration change to take effect.
5. Upgrade any Audit instances that do not use RavenDB 3.5 persistence to ServiceControl 5.
6. Upgrade any Monitoring instances to ServiceControl 5.
7. Remove the old database for the Error instance:
    * In ServiceControl Management, click the **Browse…** button under **DB Path**.
    * In Windows Explorer, move up one directory level.
    * The old database will be located in this directory with a suffix. For example, if the database directory name was `DB`, the previous database directory will be named `DB_UpgradeBackup`. This database directory can be deleted to save disk space once confident that the upgrade process has been a success.

### Upgrading with PowerShell

After completing the , follow this procedure to upgrade using PowerShell 7.2 or higher.

1. Install the **Particular.ServiceControl.Management** module, replacing `5.0.0` with a newer 5.x version if available:
    ```ps1
    Install-Module -Name Particular.ServiceControl.Management -RequiredVersion 5.0.0

    Import-Module Particular.ServiceControl.Management -RequiredVersion 5.0.0
    ```
2. Ensure the [getting ready](#upgrading-to-version-5-getting-ready) steps above have been completed. The following query can be used to list all instances and their current versions:
    ```ps1
    Get-ServiceControlInstances | Select Name, Version
    Get-ServiceControlAuditInstances | Select Name, Version
    Get-MonitoringInstances | Select Name, Version
    ```
3. Perform a **Forced upgrade** for the version 4 ServiceControl instance. _**Note:** This is a destructive operation. A database backup is made but will require application re-installation of the instance to recover._
    ```ps1
    # List existing error/primary instances
    Get-ServiceControlInstances | Select Name, Version
    # For each instance
    Invoke-ServiceControlInstanceUpgrade -Name <InstanceName> -Force
    ```
4. Re-enable error message ingestion by removing the `IngestErrorMessages` setting from the `ServiceControl.exe.config` file.
5. Restart the primary instance for the configuration change to take effect.
6. Upgrade any Audit instances that do not use RavenDB 3.5 persistence to ServiceControl 5.
    ```ps1
    # List existing audit instances:
    Get-ServiceControlAuditInstances | ? PersistencePackageName -eq RavenDB | Select Name, PersistencePackageName
    # For each instance
    Invoke-ServiceControlAuditInstanceUpgrade -Name <InstanceName>
    ```
7.  Upgrade any Monitoring instances to ServiceControl 5.
    ```ps1
    # Upgrade monitoring instances
    Get-MonitoringInstances | Select Name, Version
    # For each instance
    Invoke-MonitoringInstanceUpgrade <InstanceName>
    ```
8.  Remove the old database for the Error instance:
    * Find the current database path for the Error instance:
      ```ps1
      Get-ServiceControlAuditInstances | Select Name, DBPath
      ```
    * The old database will have the same path as the curent `DBPath` but with a suffix `_UpgradeBackup`. For example, if the `DBPath` was `D:\ServiceControl\DB`, the previous database directory will be located at `D:\ServiceControl\DB_UpgradeBackup`. This database directory can be deleted to save disk space once confident that the upgrade process has been a success.

## Force upgrading

In ServiceControl 5, it is also possible to perform a forced upgrade on instances that still uses RavenDB 3.5 persistence, which will discard all the data in the current database and start with a fresh RavenDB 5 database. This is sometimes preferable on non-production and developer systems where the audit data has little value, or in situations where the [audit retention period](/servicecontrol/audit-instances/configuration.md#data-retention-servicecontrol-auditauditretentionperiod) is low and a decision is made that the value of the temporally-limited audit data is not worth the complexity of following the [Replacing Audit instances](/servicecontrol/migrations/replacing-audit-instances/) procedure.

Force upgrading instance requires upgrading version 4 instances to 4.26.0 or later.

### Using ServiceControl Management

In these cases, the error or audit instance can be force-upgraded in ServiceControl Management:

1. Click the wrench icon <kbd> :wrench: </kbd> to access the **Advanced Options** screen for the instance.
2. Under **Force Upgrade to Version 5**, click the **Upgrade Instance** button and follow the prompts.

### Using Powershell

To force upgrade an Audit instance in PowerShell:

```ps1
# List existing audit instances NOT using RavenDB 5 storage engine:
Get-ServiceControlAuditInstances | ? PersistencePackageName -ne RavenDB | Select Name, PersistencePackageName
# For each instance
Invoke-ServiceControlAuditInstanceUpgrade -Name <InstanceName> -Force
```

To force upgrade an Error instance in Powershell

```ps1
# List existing audit instances NOT using RavenDB 5 storage engine:
Get-ServiceControlInstances | ? PersistencePackageName -ne RavenDB | Select Name, PersistencePackageName
# For each instance
Invoke-ServiceControlInstanceUpgrade -Name <InstanceName> -Force
```


## Support for version 4

Version 4 is supported for one year after version 5 is released as defined by the [ServiceControl support policy](/servicecontrol/upgrades/support-policy.md). The ServiceControl support end-date is available at [ServiceControl supported versions](/servicecontrol/upgrades/supported-versions.md).


## Migrating Azure Service Bus

ServiceControl instances using **Azure Service Bus – Endpoint-oriented topology (Legacy)** cannot be directly upgraded to version 5.

To upgrade, follow these steps:

### 1. Upgrade the Transport

> [!NOTE]
> This step is only required if there are subscribers for [ServiceControl integration events](/servicecontrol/contracts.md).

Migrate those subscribers to the supported **Azure Service Bus (Forwarding Topology)** transport using the following guide:

- [Azure Service Bus Transport (Legacy) Upgrade Version 9 to 9.1](/transports/upgrades/asb-9to9.1.md)

### 2. Reconfigure ServiceControl 4.x

After completing the transport upgrade:

1. Upgrade **ServiceControl** to **v4.33.5** (Do NOT use v5.x or newer. Validate the version of the ServiceControl Management Utility that you have open).
2. Note the **instance name** and **database folder path**.
3. Remove the instance via 🔧 (Advanced options), but **leave “Remove DB subdirectory and data” unchecked** in the Remove instance dialog.
4. Re-create a **new v4.33.5 instance**:
    - Reuse the **same instance name**.
    - Point it to the **same database path**.
    - Select **Azure Service Bus (Forwarding Topology)** as the transport.

### 3. Upgrade to ServiceControl v5

Once the instance is reconfigured and running with the supported transport, you can safely upgrade to **ServiceControl v5**.
With the instance now using the supported transport, you can [upgrade your error instance to **ServiceControl v5**](#upgrading-to-version-5)
