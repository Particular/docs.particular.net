---
title: Import failed audit messages
summary: How to attempt to re-import audit messages that failed to be processed
reviewed: 2018-09-04
redirects:
- servicecontrol/import-failed-audit-messages
---

Audit messages can fail to be imported into ServiceControl database due to one of two reasons:
 * Messages themselves are malformed (e.g. missing headers). This happens e.g. when an outdated version of NServiceBus that contained a bug was used to process the messages.
 * Messages are well-formed but there was an intermittent infrastructure problem (e.g. disk drive) lasting long enough that the processing retries did not resolve the problem

Audit messages that fail import are stored in the ServiceControl database in the `FailedAuditImports` collection. In addition, a log with the failure reason is written for the message in the [`%ServiceControl/LogPath%`](/servicecontrol/creating-config-file.md#host-settings-servicecontrollogpath)`\FailedImports\Audit\%failureid%.txt`. These messages will not be available in ServiceInsight.

When an audit message that has failed import is detected in the ServiceControl database, the **Audit Message Ingestion** custom check is marked as failed to bring the failed audit import(s) to the administrator's attention.

To attempt to reprocess the failed audit import messages ServiceControl needs to be shut down and started from a command line with `--import-failed-audits` option. In this mode ServiceControl will not process any new error or audit messages.

If the import failure audit message is re-processed successfully, the audit message will be now be available in ServiceInsight. 

If the audit message still fails to import it usually means that the message is malformed and ServiceControl won't be able to ingest it. It may be possible to correct the audit message data to allow ServiceControl to import the message. To review the malformed messages start service control in [maintenance mode](/servicecontrol/use-ravendb-studio.md) and inspect the `FailedAuditImports` collection. Review the audit import failure logs to determine why the import continues to fail. If modifying the audit message data can resolve the issue, make the necessary changes to the message document to allow ServiceControl to import the message. Once the data has been modified, the message can be reimported by running ServiceControl from the command line with the `--import-failed-audits` option again.
