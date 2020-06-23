---
title: ServiceControl Forwarding Log Queues
summary: Details of the ServiceControl audit and error configuration and forwarding behavior 
reviewed: 2020-06-23
---

### Audit and error queues

ServiceControl consumes messages from the audit and error queues and stores these messages locally in its own embedded database. These input queues names are specified at install time.

ServiceControl can also forward these messages to two log queues:

 * Error messages are optionally forwarded to the _error_ log queue.
 * Audit messages are optionally forwarded to the _audit_ log queue.

This behavior can be set through ServiceControl Management.

![](managementutil-queueconfig.png 'width=500')


### Error and audit log queues

The log queues retain a copy of the original messages ingested by ServiceControl.
The queues are not directly managed by ServiceControl and are meant as points of external integration.

Note: If external integration is not required, it is strongly recommended to turn forwarding to log queues off. Otherwise, messages will accumulate unprocessed in the forwarding log queue(s) until all available disk space is consumed.
