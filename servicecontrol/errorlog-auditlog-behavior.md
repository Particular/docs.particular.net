---
title: ServiceControl Forwarding Log Queues
summary: Details of the ServiceControl audit and error configuration and forwarding behavior 
reviewed: 2020-06-23
---

## Audit and error queues

ServiceControl consumes messages from the audit and error queues and stores these messages locally in its own embedded database. These input queues names are specified at install time.

ServiceControl can also forward these messages to two log queues:

 * Error messages are optionally forwarded to the _error_ log queue. Default: `error.log`.
 * Audit messages are optionally forwarded to the _audit_ log queue. Default: `audit.log`.

This behavior can be set through ServiceControl Management.

![](managementutil-queueconfig.png 'width=500')

## Processing failures are not forwarded immediately

Failed imports are not forwarded to the error log queue immediately. These will be stored in ServiceControl as Failed Imports or in the error queue of the ServiceControl instance type. Messages are forwarded by ServiceControl only after it is successfully stored in its datebase.

If messages must be forwarded immediately, even if ServiceControl cannot process the messages, the solution is to invert the processing order. Failed messages should be sent to a `process_errors` queue. A custom process will read from the `process_errors` queue, then forward the messages to the `error` queue that ServiceControl will process.

The same can be done for audit messages.

Forwarding by ServiceControl:

   "error" -> ServiceControl -> "error.log" -> Custom Processor

Forwarding by Custom Processor:

   "error" -> Custom Processor -> "error.log" -> ServiceControl

This will prioritize the custom processor over ServiceControl for audit and error processing


## Error and audit log queues

The log queues retain a copy of the original messages ingested by ServiceControl.
The queues are not directly managed by ServiceControl and are meant as points of external integration.

Note: If external integration is not required, it is strongly recommended to turn forwarding to log queues off. Otherwise, messages will accumulate unprocessed in the forwarding log queue(s) until storage resource (message count limits, messages size limit, available disk space) are exhausted. When this happens new messages cannot be added.
