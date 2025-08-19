---
title: ServiceControl Forwarding Log Queues
summary: Details of the ServiceControl audit and error configuration and forwarding behavior
reviewed: 2025-07-25
---

## Audit and error queues

ServiceControl Error and Audit instances consume messages from endpoint-defined `audit` and `error` queues. These are specified during instance setup and ingested through the instance's configured input queues, such as `Particular.ServiceControl` and `Particular.ServiceControl.Audit`. Once received, messages are persisted to ServiceControl's embedded RavenDB database.

Optionally, ServiceControl can forward these messages to external log queues:

* Error messages are optionally forwarded to the _error_ log queue. Default: `error.log`.
* Audit messages are optionally forwarded to the _audit_ log queue. Default: `audit.log`.

This forwarding behavior is controlled via ServiceControl Management and can be enabled or disabled as needed.

![](managementutil-queueconfig.png 'width=500')

## Processing failures are not forwarded immediately

When forwarding is enabled, ServiceControl does not forward [failed imports](/servicecontrol/import-failed-messages.md) to log queues immediately. It first attempts to ingest the messages from the error or audit queues to persist them in its embedded RavenDB database. Only after the message is successfully stored does ServiceControl forward a copy of the messages to the configured log queues (error.log and/or audit.log). If the message ingestion fails (e.g. due to message corruption, transport issues, invalid headers etc.) then the message is not forwarded; instead it is stored internally in the database under the `FailedAuditImports` and `FailedErrorImports` collections. It may also be routed to the ServiceControl instance's [internal error queue](/servicecontrol/queues.md#error-instance-error-queue).

If immediate forwarding is required, regardless of whether ServiceControl can ingest the message or not, the solution is to invert the processing order. The endpoints are configured to send failed messages to a `process_errors` queue. A custom processor will read from this `process_errors` queue and can then forward the messages to the `error` queue that ServiceControl will process.

The same can be done for audit messages.

Forwarding by ServiceControl(default):

   "error" -> ServiceControl -> "error.log" -> Custom Processor

Forwarding by Custom Processor(inverted model):

   "error" -> Custom Processor -> "error.log" -> ServiceControl

This inverted model gives external processors pre-ingestion control over message behavior, including filtering, tagging, and early forwarding. It can be similarly applied to audit messages.

## Error and audit log queues

The log queues(error.log, audit.log) retain a copy of the original messages ingested by ServiceControl.
The queues are not directly managed by ServiceControl and are meant as points of external integration.

> [!NOTE]
> If external integration is not required, it is strongly recommended to turn forwarding to log queues off. Otherwise, messages will accumulate unprocessed in the forwarding log queue(s) until storage resource (message count limits, messages size limit, available disk space) are exhausted. When this happens new messages cannot be added.
