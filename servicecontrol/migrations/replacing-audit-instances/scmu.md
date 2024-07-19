---
title: Replacing an Audit instance using ServiceControl Management
summary: Instructions on how to replace a ServiceControl Audit instance with zero downtime
reviewed: 2024-07-10
component: ServiceControl
related:
  - servicecontrol/migrations/replacing-audit-instances/powershell
  - servicecontrol/migrations/replacing-audit-instances/containers
  - servicecontrol/migrations/replacing-error-instances
---

This article describes how to use the ServiceControl Management Utility to replace an Audit instance with zero downtime. For an overview of the process and details for other deployment scenarios, see [Replacing an Audit Instance](/servicecontrol/migrations/replacing-audit-instances/).

## Add a new audit instance

First, a new audit instance must be created. If it is on the same machine, different ports must be specified. Deploying it on a separate machine is preferable, as the databases of each instance will not compete for the same resources.

1. Open ServiceControl Management.
2. Click **New**, then **Add ServiceControl and Audit Instances**.
3. Uncheck the **ServiceControl** checkbox so that only an Audit instance will be installed.
4. Configure the new Audit instance as desired, or to match the previous instance, except that new ports must be selected if deploying on the same machine.
5. Click the **Add** button to create and start the new instance.

## Add the instance to RemoteInstances

Then, the new Audit instance must be added to the Error instance's collection of remotes. This cannot be done in ServiceControl Management and must be done by editing the configuration file:

1. Open ServiceControl Management.
2. For the Error instance, click the **Installation Path > Browse** button to open the installation folder in Windows Explorer.
3. Edit the **ServiceControl.exe.config** file.
4. Edit the value of the `ServiceControl/RemoteInstances` setting:
    * The value is XML-encoded JSON containing an array of values, each having an `api_uri` value.
    * All JSON double-quotes (`"`) must be represented as `&quot;`.
    * Add the API URL of the new Audit instance to the value.
    * Example with two `localhost` URIs on ports `44444` and `44446`:
      ```xml
      [{&quot;api_uri&quot;:&quot;http://localhost:44444/api/&quot;},{&quot;api_uri&quot;:&quot;http://localhost:44446/api/&quot;}]
5. Save the file.
6. In ServiceControl Management, stop and restart the Error instance for the changes to take effect.

## Disable audit queue ingestion on the old instance

Configure the old audit instance so that it will no longer ingest new messages from the audit queue, making the instance effectively read-only:

1. Open ServiceControl Management.
2. For the old Audit instance, click the **Installation Path > Browse** button to open the installation folder in Windows Explorer.
3. Edit the `ServiceControl.Audit.exe.config` file.
4. In the `appSettings` section, add a setting key for `ServiceControl/IngestAuditMessages` with a value of `false`.
5. In ServiceControl Management, stop and restart the Audit instance for the changes to take effect.

> [!NOTE]
> For versions 4.32.0 of ServiceControl and older use `!disable` as the [`AuditQueue`](/servicecontrol/audit-instances/configuration.md#transport-servicebusauditqueue) name to disable the audit message ingestion.

## Decommission the old audit instance

When the audit retention period has expired and there are no remaining processed messages in the database, you can decommission the old audit instance.

First, use the same instructions above to edit the Error instance's configuration file, but this time removing the old Audit instance URL from the `ServiceControl/RemoteInstances` setting.

Lastly, using ServiceControl Monitoring, stop and remove the old Audit instance.