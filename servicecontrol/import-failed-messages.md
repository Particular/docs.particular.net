---
title: Re-processing messages that failed to be imported
summary: How to attempt to re-process messages that failed to be imported
reviewed: 2024-12-02
redirects:
- servicecontrol/import-failed-audit-messages
- servicecontrol/import-failed-audits
- servicecontrol/import-failed-audit
---

Messages can fail to be imported into the ServiceControl database for the following reasons:
 * Messages are malformed (e.g. missing headers)
   * This can happen, for example, when an outdated version of NServiceBus that contained a bug was used to process the messages.
 * Messages are well-formed, but an intermittent database problem lasts long enough that the built-in retries did not resolve the problem.
 * [Forwarding](/servicecontrol/errorlog-auditlog-behavior.md) is enabled, and the destination queue does not exist, or ServiceControl cannot send messages to it. This could happen when the message or size limit has been reached or storage resources are exhausted.

> [!NOTE]
> Messages with corrupt (i.e. unreadable, not deserializable) header data will not be processed and will move to ServiceControl's 'error' queue.

Messages that fail to be imported are stored in the ServiceControl database in the `FailedAuditImports` and `FailedErrorImports` collections.

In addition, a log with the failure reason is written for the message in the `%ServiceControl/LogPath%` ([error instances](/servicecontrol/servicecontrol-instances/configuration.md#logging-servicecontrollogpath)/[audit instances](/servicecontrol/audit-instances/configuration.md#logging-servicecontrol-auditlogpath)) `\FailedImports\{Audit|Error}\%failureid%.txt`. These messages will not be visible in ServicePulse.

## Failed message custom check

When a failed import is detected in the ServiceControl database, the [**Message Ingestion** custom check](/servicecontrol/servicecontrol-instances/) is marked as failed to bring the failed import(s) to the administrator's attention.

## How to reimport

To reimport the failed messages, the instance must be shut down and started from a command line using one of the following commands:

While in import mode, ServiceControl or ServiceControl Audit will not process its input queues. Once the message is re-processed successfully, it is available in ServicePulse.

The custom check will no longer be displayed if all failed imports have been successfully reimported.

> [!NOTE]
> Older, unsupported versions of ServiceControl (prior to 5.5.0) require a `--serviceName` command line option. The value to use for `--serviceName` is the instance name. It is available in the Windows Service information and ServiceControl Management Utility. 

### ServiceControl deployed using a container

1. Stop the container for the instance. Note the options and tag used.
2. Run the container image as a short-term foreground process (`--rm`) using the following command line:
```bash
docker run --rm {OPTIONS} particular/servicecontrol:{TAG} --import-failed-errors
```
3. Start the container for the instance.

### ServiceControl deployed using PowerShell or the ServiceControl Management Utility

1. First stop the instance using the ServiceControl Management Utility or by stopping the Windows service directly.
2. Run the following command line:
```cmd
ServiceControl.exe --import-failed-errors
```
3. After the import has completed, start the instance using the ServiceControl Management Utility or by starting the Windows service directly.

### ServiceControl.Audit deployed using a container

1. Stop the container for the instance. Note the options and tag used.
2. Run the container image as a short-term foreground process (`--rm`) using the following command line:
```bash
docker run --rm {OPTIONS} particular/servicecontrol-audit:{TAG} --import-failed-audits
```
3. Start the container for the instance.

### ServiceControl.Audit deployed using PowerShell or the ServiceControl Management Utility

1. First stop the instance using the ServiceControl Management Utility or by stopping the Windows service directly.
2. Run the following command line:
```cmd
ServiceControl.Audit.exe --import-failed-audits
```
3. After the import has completed, start the instance using the ServiceControl Management Utility or by starting the Windows service directly.

## Modify message data

If the message still fails to import, it usually means that the message is malformed, and ServiceControl won't be able to ingest it. It may be possible to correct the message data manually to allow ServiceControl to import the message. To review the malformed messages, [access the ServiceControl database](/servicecontrol/ravendb/accessing-database.md) and inspect the `FailedAuditImports` or `FailedErrorImports` collection. Review the import failure logs to determine why the import continues to fail. If modifying the audit message data can resolve the issue, make the necessary changes to the message document to allow ServiceControl to import the message.

Once the data has been modified, the message can be [reimported again](#how-to-reimport).
