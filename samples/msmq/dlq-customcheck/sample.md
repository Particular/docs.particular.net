---
title: Basic MSMQ Dead Letter Queue Monitoring With Custom Checks
reviewed: 2020-06-15
component: MsmqTransport
related:
- transports/msmq
- transports/msmq/dead-letter-queues
- monitoring/custom-checks
---


When using [MSMQ](https://msdn.microsoft.com/en-us/library/ms706032.aspx) (as with any queuing technology), the delivery of a message may fail. This can occur for a number of reasons, such as including network failure, a deleted queue, a full queue, authentication failure, or a failure to deliver on time. In these cases, the message will be moved to the [MSMQ Dead Letter Queue (DLQ)](https://msdn.microsoft.com/en-us/library/ms706227.aspx).

Since these messages can contain valuable business data, it is important to monitor the dead letter queue. Also, if the dead letter queue fills up, it can cause MSMQ to run out of resources and the system as a whole to fail.

This sample shows how to use a `Custom Check` to monitor the dead letter queue. The custom check runs once every minute. If there are no messages in the dead letter queue then the custom check reports success. Otherwise, the custom check reports failure and includes the number of messages in the DLQ.

This is what the CustomCheck will look like in ServicePulse.

![CustomCheck reported in ServicePulse](screenshot.png)


## Prerequisites

 * Ensure that MSMQ has been installed.
 * Ensure that ServiceControl and ServicePulse have been installed.


## Code walk-through

Configure the custom checks plugin with the location of the ServiceControl input queue.

snippet: configure-custom-checks


### Check dead letter queue length

INFO: Performance counter categories, names, and instance names might be localized. Consider using the language-specific names on a localized version of Windows.

snippet: check-dead-letter-queue
