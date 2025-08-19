---
title: Replacing an Error instance using ServiceControl Management
summary: Instructions on how to replace a ServiceControl Error instance with zero downtime
reviewed: 2024-07-10
component: ServiceControl
related:
  - servicecontrol/migrations/replacing-error-instances/powershell
  - servicecontrol/migrations/replacing-error-instances/containers
  - servicecontrol/migrations/replacing-audit-instances
---

This article describes the details of how to use the ServiceControl Management Utility to replace an Error instance with zero downtime.

> [!NOTE]
> This does not include the complete process, only the steps specific to ServiceControl Management.
>
> For an overview of the process and details for other deployment scenarios, see [Replacing an Error Instance](/servicecontrol/migrations/replacing-error-instances/).

## Disable error message ingestion

Configure the Error instance so that it will no longer ingest new messages from the error queue:

1. Open ServiceControl Management.
2. For the Error instance, click the **Installation Path > Browse** button to open the installation folder in Windows Explorer.
3. Edit the `ServiceControl.exe.config` file.
4. In the `appSettings` section, add a setting key for [`ServiceControl/IngestErrorMessages`](/servicecontrol/servicecontrol-instances/configuration.md#recoverability-servicecontrolingesterrormessages) with a value of `false`.
5. In ServiceControl Management, stop and restart the Error instance for the changes to take effect.

## Replace the Error instance

In some cases, an instance where the database cannot be easily upgraded can be [**force upgraded**](#replace-the-error-instance-force-upgrading-the-error-instance), which involves moving the existing database aside and creating a brand new database in its place. The old database is not lost, but using it again would require reconfiguring the instance to look at the old database instead of the new one. A force-upgraded instance will retain the same API URL of the original.

The alternative is to [create a brand new instance](#replace-the-error-instance-create-a-new-error-instance), which can be done to make it easier to roll back to the original, or must be done if the specific upgrade scenario doesn't support force upgrading.

### Force upgrading the Error instance

1. Click the wrench <kbd> :wrench: </kbd> icon to access to the Error instance's Advanced Options screen.
2. If a **Force Upgrade** banner does not appear, stop and use the [Create a new Error instance](#replace-the-error-instance-create-a-new-error-instance) procedure instead.
3. Under **Force Upgrade**, click the **Upgrade Instance** button and follow the prompts.
 * _**Note:** This is a destructive operation. A database backup is made but will require application re-installation of the instance to recover._
4. Once the instance is running, re-enable error message ingestion by removing the `ServiceControl/IngestErrorMessages` setting from the `ServiceControl.exe.config` file, and restart the Error instance for the configuration change to take effect.

Once condfident of a successful upgrade, the old database can be removed:

1. In ServiceControl Management, click the Browse… button under DB Path.
2. In Windows Explorer, move up one directory level.
3. The old database will be located in this directory with a suffix. For example, if the database directory name was DB, the previous database directory will be named DB_UpgradeBackup. This database directory can be deleted to save disk space once confident that the upgrade process has been a success.

### Create a new Error instance

If the Error instance cannot be force-upgraded, instead create a new Error instance.

1. Open ServiceControl Management.
2. Click **New**, then **Add ServiceControl and Audit Instances**.
3. Uncheck the **ServiceControl.Audit** checkbox so that only a ServiceControl (Error) instance will be installed.
4. Configure the new Error instance as desired, or to match the previous instance, except that new ports must be selected if deploying on the same machine.
5. Click the **Add** button to create and start the new instance.

Now that the instance is created, the configuration must be modified so that the Error instance can communicate with the same Audit instances as the previous Error instance.

1. On the newly installed Error instance, click the **Installation Path > Browse** button.
2. Edit the **ServiceControl.exe.config** file.
3. Locate the old Error instance's configuration file in the same way.
4. Edit the value of the new Error instance's `ServiceControl/RemoteInstances` setting (adding the key if not present) with the same value as the old Error instance.
5. Save the file.
6. In ServiceControl Management, stop and restart the Error instance for the changes to take effect.

Now, the old and new Error instance's are both available, but the old Error instance is not ingesting messages.

When confident of a successful upgrade, the old Error instance can be removed by clicking the wrench <kbd> :wrench: </kbd> icon, then **Remove**, and following the prompts.