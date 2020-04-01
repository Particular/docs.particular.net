---
title: Re-processing messages that failed to be imported
summary: How to attempt to re-process messages that failed to be imported
reviewed: 2020-03-31
redirects:
- servicecontrol/import-failed-audit-messages
- servicecontrol/import-failed-audits
- servicecontrol/import-failed-audit
---

Messages can fail to be imported into ServiceControl database due to one of two reasons:
 * Messages themselves are malformed (e.g., missing headers). This happens e.g., when an outdated version of NServiceBus that contained a bug was used to process the messages.
  * Messages are well-formed, but there was an intermittent database problem lasting long enough that the built-in retries did not resolve the problem.
  * [Forwarding](/servicecontrol/errorlog-auditlog-behavior.md) is enabled, and the destination queue does not exist, or ServiceControl cannot send messages to it.

Messages that fail to be imported are stored in the ServiceControl database in the `FailedAuditImports` and `FailedErrorImports` collections. In addition, a log with the failure reason is written for the message in the [`%ServiceControl/LogPath%`](/servicecontrol/creating-config-file.md#host-settings-servicecontrollogpath)`\FailedImports\{Audit|Error}\%failureid%.txt`. These messages will not be visible in ServiceInsight.

 When a message that has failed the import is detected in the ServiceControl database, the [**Message Ingestion** custom check](/servicecontrol/servicecontrol-instances/#self-monitoring-via-custom-checks-failed-imports) is marked as failed to bring the failed import(s) to the administrator's attention.

To reimport the failed messages, ServiceControl needs to be shut down and started from a command line with `--import-failed-errors` option. To import failed audit messages ServiceControl Audit needs to be shut down and started from a command line with `--import-failed-audits`. While in this mode, ServiceControl or ServiceControl Audit will not be processing its input queues. Once the imported failure message is re-processed successfully, the message will be available in ServiceInsight. 

If the message still fails to import it usually means that the message is malformed, and ServiceControl won't be able to ingest it. It may be possible to correct the message data manually to allow ServiceControl to import the message. To review the malformed messages, start ServiceControl in [maintenance mode](/servicecontrol/maintenance-mode.md) and inspect the `FailedAuditImports` or `FailedErrorImports` collection. Review the import failure logs to determine why the import continues to fail. If modifying the audit message, data can resolve the issue, make the necessary changes to the message document to allow ServiceControl to import the message. 

Once the data has been modified, the message can be reimported by running ServiceControl from the command line with the `--import-failed-errors` or ServiceControl Audit with the `--import-failed-audits` option again.
