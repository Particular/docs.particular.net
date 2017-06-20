---
title: ServiceControl Forwarding Queues
summary: Details the ServiceControl Audit and Error forwarding behavior and configuration
reviewed: 2016-11-09
---

### Audit and Error queues

ServiceControl consumes messages from the audit and error queues and stores these messages locally in its own embedded database. These input queues names are specified at install time.

ServiceControl can also forward these messages to two forwarding queues.

 * Error messages are optionally forwarded to the Error forwarding queue.
 * Audit messages are optionally forwarded to the Audit forwarding queue.

This behavior can be set through ServiceControl Management.

![](managementutil-queueconfig.png 'width=500')

NOTE: ServiceControl Monitoring instances also contain a configuration option for an error queue. ServiceControl Monitoring instances do not read from their error queue. This error queue is used for messages that the ServiceControl Monitoring instance cannot process. The ServiceControl Monitoring error queue can be the same queue that the standard ServiceControl instance consumes.  

### Error and Audit Forwarding Queues

The forwarding queues retain a copy of the original messages ingested by ServiceControl.
The queues are not directly managed by ServiceControl and are meant as points of external integration.

Note: If external integration is not required, it is highly recommend to turn forwarding queues off. Otherwise, messages will accumulate unprocessed in the forwarding queue until eventually all available disk space is consumed.
