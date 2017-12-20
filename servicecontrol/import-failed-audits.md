---
title: Import failed audit messages
summary: How to attempt to re-import audit messages that failed to be processed
reviewed: 2017-12-20
---

Audit messages can fail to be imported into ServiceControl database due to one of two reasons:
 * Messages themselves are malformed (e.g. missing headers). This happens e.g. when an outdated version of NServiceBus that contained a bug was used to process the messages.
 * Messages are well-formed but there was an intermittent infrastructure problem (e.g. disk drive) lasting long enough that the processing retries did not resolve the problem

When that happens the periodic **Audit Message Ingestion** custom check is marked as failed to bring the administrator's attention.

To resolve the problem ServiceControl needs to be shut down and started from a command line with `--import-failed-audits` option. In this mode ServiceControl will not process the incoming failed or audit messages but will instead attempt to re-process all the audit messages that previously failed.

If a message is re-processed successfully, the failed audit document is removed. If the message fails processing it usually means that the message is malformed and ServiceControl won't be able to ingest it. Such a message won't be available via ServiceInsight.

To review the malformed messages please start the service control in the [maintenance mode](/servicecontrol/use-ravendb-studio.md) and inspect the `FailedAuditImports` collection.