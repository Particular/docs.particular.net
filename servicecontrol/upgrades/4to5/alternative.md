---
title: Alternative ServiceControl v4 upgrade option
reviewed: 2025-05-21
summary: Advanced alternative ServiceControl v4 upgrade option for environments that cannot retry all messages immediately
---

The [standard guide to upgrade ServiceControl from 4 to 5](/servicecontrol/upgrades/4to5/) assumes that all failed messages can be retried without side effects. In cases where this cannot be confirmed, the approach below can be used to upgrade ServiceControl v4.

> [!NOTE]  
> This setup results in **two** separate error instances.  
> ServicePulse *cannot* show data from both simultaneously.  
> [ServicePulse can be reconfigured to connect to another instance](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui).

## Monitor Instances

Monitor instances are stateless and can be safely reinstalled elsewhere:

- Remove the old instance from (virtual) machine 1.  
- Install the same monitor instance name on (virtual) machine 2.  

No special migration steps required.

## Audit Instances

Audit v4 instances do **not** need to be migrated. They can remain active until the retention period expires.

To stop ingesting audit messages:

1. Stop and **disable** the old audit instance in *Windows Services*.
2. Add the following setting to `servicecontrol.audit.exe.config`:
   ```xml
   <add key="ServiceBus/AuditQueue" value="!disable" />
   ```
3. Start and **enable** the instance again.
4. Confirm it's no longer ingesting by checking that the audit queue is building up.

## Error Instances

The error instance needs to be prevented from being ingested, renamed, and replaced.

Steps:

1. [Stop and disable the v4 error instance](#error-instances-stop-and-disable-v4-error-instance)  
2. [Prevent error queue ingestion](#error-instances-configure-to-stop-ingestion-of-error-queue)  
3. [Change the instance’s queue name](#error-instances-change-servicecontrol-instance-queue)  
4. [Run setup](#error-instances-run-setup)  
5. [Enable and start the v4 error instance](#error-instances-enable-and-start-v4-error-instance)  
6. [Verify the instance](#error-instances-verify-instance-is-running-without-issues)  
7. [Add a new error instance](#error-instances-add-new-error-instance)  

### Stop and Disable v4 Error Instance

1. Open *Windows Services*.
2. Locate the error instance, right-click → **Properties**.
3. Set `Startup type` to `Disabled` → Apply.
4. Click **Stop**.

### Configure to Stop Ingestion of Error Queue

1. Locate the instance folder (via ServiceControl Management Utility → *Installation location* → 📁 **Browse...**)
2. Open `servicecontrol.exe.config` in an elevated text editor
3. Add:
   ```xml
   <add key="ServiceControl/IngestErrorMessages" value="False" />
   ```
4. Save the file

### Change ServiceControl Instance Queue

> [!NOTE]  
> The swapping of the instance names is only needed if **heartbeats** are used to avoid requiring to update the configuration of all endpoints.

1. Open **regedit** and navigate to:  
   ```
   HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\
   ```
2. Find the key representing the ServiceControl service
3. Export it as a backup
4. Locate `ImagePath`, note the `--serviceName` argument
5. Modify the service name to a unique value, e.g.:
   ```
   --serviceName=particular.servicecontrol_v4
   ```

### Run Setup

1. Open an elevated command prompt
2. Navigate to the install folder, e.g.:
   ```
   C:\Program Files (x86)\Particular Software\Particular.ServiceControl
   ```
3. Run:
   ```
   servicecontrol.exe -s --serviceName=particular.servicecontrol_v4
   ```

### Enable and Start v4 Error Instance

1. Open *Windows Services*
2. Locate the error instance, right-click → **Properties**
3. Set `Startup type` to `Automatic` → Apply
4. Click **Start**

### Verify Instance is Running Without Issues

1. Open the log folder (via ServiceControl Management Utility → *Log Path* → 📁 **Browse...**)
2. Open the latest `logfile.{YYYY-MM-DD}`
3. Scroll to the end and ensure no errors are present

### Add New Error Instance

> [!NOTE]  
> Use the **original** service name (before the rename) for this new instance.

1. Add a new error instance as usual (on the same machine or a new one)
2. Configure it as needed to take over ingestion
