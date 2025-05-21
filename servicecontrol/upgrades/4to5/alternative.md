---
title: Alternative ServiceControl v4 to newer migration
reviewed: 2025-05-21
summary: Advanced alternative ServiceControl v4 migration for environment that cannot retry all messages immediately
---

The approach available at </servicecontrol/upgrades/4to5/> assumes that all failed message can be retried without side-effects. Not all users are able to analyze all failures but do want to upgrade a supported version.

This is an alternative migration which will ensure the existing old instance to no longer ingest error messages.

> [!NOTE]
> This results in **2** separate error instances. ServicePulse *cannot* show data for both simultaniously. [ServicePulse can be configured to connect to a different instance](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui)

## Monitor instances

Monitor instance are stateless and can be upgraded without any issues. It is OK to remove the instance from (virtual) machine 1 and install with the same name on (virtual) machine 2.


## Audit instances

V4 instance do not need to be migrated and can remain active until the retention period has passed. After that the database no longer contains any messages and can be removed.

However, the instance needs to be configured to longer ingest messages from the audit queue.

Steps:

- Stop and DISABLE old audit instances in Windows Service Manager
- Add the following setting to `servicecontrol.audit.exe.config`:
  - ```xml
    <add key="ServiceControl.Audit/IngestAuditMessages" value="False" />
	```
- Enable and Start the old audit instance in Windows Service Manager
- Validate if the instance is no longer ingesting messages from the audit queue by observing the number of messages building up in the audit queue.


## Error instance

- Stop and disable v4 error instance
- Configure to stop ingestion of error queue
- Change ServiceControl instance queue
- Run setup
- Enable and start v4 error instance
- Verify instance is running without issues
- Add new error instance


### Stop and disable v4 error instance

Steps:

- Open Windows Services console
- Navigate error instance service, right click and open properties
- Select `Startup type` is `Disabled` and select `Apply`
- Select `Stop`


### Configure to stop ingestion of error queue

- Open the installation location of the error instance
  - For example, via the ServiceControl Management Utility, locate the error instance, the `Installation location` label and select `📁 Browse...`
- Open the file `servicecontrol.exe.config` in an elevated editor
- Add the following setting to `servicecontrol.exe.config`:
  - ```xml
    <add key="ServiceControl/IngestErrorMessages" value="True" />
    ```
- Save the file


### Change ServiceControl instance queue

Change the queue name used as existing endpoints are sending heartbeats to this.

> [!NOTE]
> If you are not using heartbeats this step it not required

- Adjust primary instance service name:
  - Open Regedit and navigate to `HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\`
  - Locate the "map" that represents the service
  - Right click the node and select `EXPORT` to make a backup
  - Locate the sub item `ImagePath`
  - Notice its value for `--serviceName`
  - **Write down the *current* service name**
  - Modify its value and change the service name value to a unique value like `--serviceName=particular.servicecontrol_v4`

### Run setup

Re-run setup so the queues for this instance are created:

- Locate to the installation folder via an elevated command prompt (i.e. `C:\Program Files (x86)\Particular Software\Particular.ServiceControl`)
- Run `servicecontrol.exe -s --serviceName=particular.servicecontrol_v4`
  - Notice the `-s` that will run setup


### Enable and start v4 error instance

- Open Windows Services console
- Navigate error instance service, right click and open properties
- Select `Startup type` is `Automatic` and select `Apply`
- Select `Start`


### Verify instance is running without issues

- Navigate to the log path
  - For example, via ServiceControl Management utititly, locate to the error instance, locate its `Log Path` label, click `📁 Browse...`
- Open the most recent `logfile.{YYYY-MM-DD}` file
- Go to the end
- Observe if the instance is running correctly and no errors are shown


### Add new error instance

> [!NOTE
> It is essential that the previous service name is used

Steps:

- Add a new error instance on this machine, on another VM, as normally.
