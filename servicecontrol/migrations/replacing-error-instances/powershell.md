---
title: Replacing an Error instance using PowerShell
summary: Instructions on how to replace a ServiceControl Error instance with zero downtime
reviewed: 2024-07-10
component: ServiceControl
related:
  - servicecontrol/migrations/replacing-error-instances/scmu
  - servicecontrol/migrations/replacing-error-instances/containers
  - servicecontrol/migrations/replacing-audit-instances
---

This article describes how to use PowerShell to replace an Error instance with zero downtime.

> [!NOTE]
> This does not include the complete process, only the steps specific to PowerShell.
>
> For an overview of the process and details for other deployment scenarios, see [Replacing an Error Instance](/servicecontrol/migrations/replacing-error-instances/).

## Disable error message ingestion

Configure the old Error instance so that it will no longer ingest new messages from the error queue:

1. Locate the `ServiceControl.exe.config` file.
2. In the `appSettings` section, add a setting key for `ServiceControl/IngestErrorMessages` with a value of `false`.
3. Restart the Error instance for the changes to take effect.

## Replace the Error instance

In some cases, an instance where the database cannot be easily upgraded can be [**force upgraded**](#replace-the-error-instance-force-upgrading-the-error-instance), which involves moving the existing database aside and creating a brand new database in its place. The old database is not lost, but using it again would require reconfiguring the instance to look at the old database instead of the new one. A force-upgraded instance will retain the same API URL of the original.

The alternative is to [create a brand new instance](#replace-the-error-instance-create-a-new-error-instance), which can be done to make it easier to roll back to the original, or must be done if the specific upgrade scenario doesn't support force upgrading.

### Force upgrading the Error instance

Perform a **Forced upgrade** for the old Error instance. 
> [!WARNING]
> This is a destructive operation. A database backup is made but will require application re-installation of the instance to recover.

```ps1
# List existing error/primary instances
Get-ServiceControlInstances | Select Name, Version
# For each instance
Invoke-ServiceControlInstanceUpgrade -Name <InstanceName> -Force
```

Then, re-enable error message ingestion by removing the `ServiceControl/IngestErrorMessages` setting from the `ServiceControl.exe.config` file.

Once condfident that the upgrade process has been a success, the old database path (in a directory named `DB_UpgradeBackup` can be deleted.

### Create a new Error instance

If the Error instance cannot be force-upgraded, instead create a new Error instance.

```ps1
$serviceControlInstance = New-ServiceControlInstance `
  -Name Particular.ServiceControl.NewError `
  -InstallPath C:\ServiceControl.NewError\Bin `
  -DBPath C:\ServiceControl.NewError\DB`
  -LogPath C:\ServiceControl.NewError\Logs `
  -Port 33343 `
  -DatabaseMaintenancePort 33344 `
  -Transport MSMQ `
  -ErrorQueue error `
  -ErrorRetentionPeriod 10:00:00:00
```

Now that the instance is created, the configuration must be modified so that the Error instance can communicate with the same Audit instances as the previous Error instance. In this example, the Audit instance at `http://localhost:4444` is added.

```ps1
# For each Audit instance URL used by the existing Error instance
Add-ServiceControlRemote `
  -Name $serviceControlInstance.Name `
  -RemoteInstanceAddress "http://localhost:44444"
```

Now, the old and new Error instance's are both available, but the old Error instance is not ingesting messages.

When confident of a successful upgrade, the old Error instance can be removed:

```ps1
Remove-ServiceControlInstance `
  -Name <OLD_INSTANCE_NAME> `
  -RemoveDB `
  -RemoveLogs
```
