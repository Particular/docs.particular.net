---
title: Replacing an Audit instance using PowerShell
summary: Instructions on how to replace a ServiceControl Audit instance with zero downtime
reviewed: 2024-07-10
component: ServiceControl
related:
  - servicecontrol/migrations/replacing-audit-instances/scmu
  - servicecontrol/migrations/replacing-audit-instances/containers
  - servicecontrol/migrations/replacing-error-instances
---

This article describes how to use PowerShell to replace an Audit instance with zero downtime. For an overview of the process and details for other deployment scenarios, see [Replacing an Audit Instance](/servicecontrol/migrations/replacing-audit-instances/).

## Add a new audit instance

First, a new audit instance must be created. If it is on the same machine, different ports must be specified.

```ps1
New-ServiceControlAuditInstance `
  -Name Particular.ServiceControl.NewAudit `
  -InstallPath C:\ServiceControl.NewAudit\Bin `
  -DBPath C:\ServiceControl.NewAudit\DB `
  -LogPath C:\ServiceControl.NewAudit\Logs `
  -Port 44446 `
  -DatabaseMaintenancePort 44447 `
  -Transport MSMQ `
  -AuditQueue audit `
  -AuditRetentionPeriod 10:00:00:00 `
  -ForwardAuditMessages:$false `
  -ServiceControlQueueAddress "Particular.ServiceControl"
```

## Add the instance to RemoteInstances

Then, to add the new instance to the Error instance's collection of remotes, execute the following on the ServiceControl Error instance machine:

```ps1
Add-ServiceControlRemote `
  -Name "Particular.ServiceControl" `
  -RemoteInstanceAddress "http://localhost:44446/api"
```

## Disable audit queue ingestion on the old instance

Configure the old audit instance so that it will no longer ingest new messages from the audit queue, making the instance effectively read-only:

1. Locate the `ServiceControl.Audit.exe.config` file.
2. In the `appSettings` section, add a setting key for `ServiceControl/IngestAuditMessages` with a value of `false`.
3. Restart the Audit instance for the changes to take effect.

> [!NOTE]
> For versions 4.32.0 of ServiceControl and older use `!disable` as the [`AuditQueue`](/servicecontrol/audit-instances/configuration.md#transport-servicebusauditqueue) name to disable the audit message ingestion.

## Decommission the old audit instance

When the audit retention period has expired and there are no remaining processed messages in the database, you can decommission the old audit instance.

On the ServiceControl Error instance machine, remove the Audit instance URL from the collection of remote instances:

```ps1
Remove-ServiceControlRemote `
  -Name "Particular.ServiceControl" `
  -RemoteInstanceAddress "http://localhost:44444/api"
```

Then, on the ServiceControl Audit instance machine, remove the Audit instance, including the database and logs.

```ps1
Remove-ServiceControlAuditInstance `
  -Name "Particular.ServiceControl.OriginalAudit" `
  -RemoveDB `
  -RemoveLogs
```