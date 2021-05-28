---
title: MSMQ Transport Troubleshooting
summary: Resolutions for common problems with the MSMQ transport.
reviewed: 2019-10-18
component: MsmqTransport
isLearningPath: true
redirects:
 - nservicebus/messagequeueexception-insufficient-resources-to-perform-operation
 - nservicebus/msmq/messagequeueexception-insufficient-resources-to-perform-operation
 - nservicebus/msmq/network-load-balancing
 - nservicebus/msmq/troubleshooting
---

This article details common problems encountered with the MSMQ Transport and how to resolve them.


## Worker QMId needs to be unique

Every installation of MSMQ on a Windows machine is represented uniquely by a Queue Manager ID (QMId). The QMId is stored as a key in the registry, `HKEY_LOCAL_MACHINE\Software\Microsoft\MSMQ\Parameters\Machine Cache`. MSMQ uses the QMId to know where it should send acknowledgments and replies for incoming messages.

All the machines must have their own unique QMId. If two or more machines share the same QMId, only one of those machines can successfully send and receive messages with MSMQ. Which machine works changes in a seemingly random fashion.

The primary reason for machines ending up with duplicate QMIds is the cloning of virtual machines from a common Windows image without running the recommended [Sysprep](https://technet.microsoft.com/en-us/library/cc766049.aspx) tool.

If there are two or more machines with the same QMId, reinstall the MSMQ feature to generate a new QMId.

See [this blog post](https://blogs.msdn.microsoft.com/johnbreakwell/2007/02/06/msmq-prefers-to-be-unique/) for more details.


## Messages stuck or not arriving

MSMQ uses store-and-forward to communicate with remote machines. Messages are stored locally, and the MSMQ service repeatedly attempts to deliver them to the destination queue on the remote machine.

Approaches for diagnosing messages stuck in the outgoing queue:

 * Check the **Outgoing Queues** on each server involved while the problem is occurring. Each item represents a connection to a remote server. Items stuck here represent an inability to transfer messages to the remote server. The **State** and **Connection History** columns may point to a connectivity issue between servers.
 * Check the Microsoft support article [MSMQ service might not send or receive messages after a restart](https://support.microsoft.com/en-us/kb/2554746). This details how an error in MSMQ's binding process for IP addresses and ports can cause one server to be unable to validate messages coming from another, causing them to be rejected.
 * If servers are cloned from the same virtual machine image, this causes them to have the same `QMId` in the registry key `HKEY_LOCAL_MACHINE\Software\Microsoft\MSMQ\Parameters\Machine Cache`, which interferes with message delivery. Use the workaround described in [MSMQ prefers to be unique](https://blogs.msdn.microsoft.com/johnbreakwell/2007/02/06/msmq-prefers-to-be-unique/) to reset the `QMId` on an existing machine, but it is preferable to use [Microsoft's Sysprep tool](https://support.microsoft.com/en-us/kb/314828) before capturing the virtual machine image.


## MessageQueueException: Insufficient resources to perform operation

This exception may occur when trying to send messages to a machine that has been offline for a while, or the System is suffering from a larger than expected load spike, or when message queuing quota has exceeded its limit:

```
System.Messaging.MessageQueueException (0x80004005): Insufficient resources to perform operation.
at System.Messaging.MessageQueue.SendInternal(Object obj, MessageQueueTransaction internalTransaction, MessageQueueTransactionType transactionType)
```

The cause of this exception is that the MSMQ has run out of space for holding on to messages. This could be due to messages sent that could not be delivered, or messages received that have not been processed.

Also, check the outgoing queues to see if messages sent to remote servers are received and processed. NServiceBus has dead letter queues enabled by default. Enabled dead letter queues results in messages remaining in the outgoing queue of the sender until they are not only delivered but also processed at the receiver. For more information, read [MSMQ dead-letter queues](dead-letter-queues.md).


### Resolution

 1. Make sure that the hard disk drive has sufficient space.
 1. Purge the transactional dead-letter queue (TDLQ) under System Queues.
  * This queue acts as a recycle bin for other transactional queues. So if other transactional queues have been purged, ensure the TDLQ is purged as well.
  * Within the TDLQ, the Class column shows the reason the message arrived there. Common messages include "The queue was purged" or "The queue was deleted".
 1. If journaling is turned on, purged messages can be found in the journaling queue under System Queues. Ensure that journaling is disabled on each queue level, and only turn it on if needed for debugging purposes.
 1. Increase the MSMQ storage quota ([MSDN article](https://support.microsoft.com/en-us/kb/899612))

WARNING: On production servers, uninstalling MSMQ deletes all queues and messages, which may contain business data. Do not attempt uninstalling MSMQ unless message loss is acceptable.

For more information on this error, see [John Breakwell's article in MSDN](https://blogs.msdn.microsoft.com/johnbreakwell/2006/09/18/insufficient-resources-run-away-run-away/).


## MessageQueueException (0x80004005): Message Queue service is not available.

This exception may occur if the MSMQ service is stopped or crashed.


### Resolution

- Ensure that [Windows Service Restart Recovery is enabled to restart Windows services automatically when they stop or crash](/nservicebus/hosting/windows-service.md#installation-setting-the-restart-recovery-options).
- For every endpoint [configure dependencies on the MSMQ service](/nservicebus/hosting/windows-service.md#installation-specifying-service-dependencies). The endpoints will then be automatically stopped/restarted in case the MSMQ service is restarted. Note the endpoints will be restarted only if the MSMQ service is _restarted_, but not if it's only _stopped_ or only _started_.
- If self-hosting, ensure that the [critical error handling is configured](/nservicebus/hosting/critical-errors.md#custom-handling) correctly and custom callback method has been provided.

## Virtual Private Networks (VPN)

MSMQ cannot dynamically detect network interfaces. If a connection to a VPN is established after the MSMQ service starts, a restart of the MSMQ service is required. Once it starts with the interface, the VPN is free to disconnect/reconnect whenever it wants.

It is recommended to have batch setup scripts that run on server startups to connect the VPN, which then restarts the MSMQ service automatically.

## Network Load Balancing cannot be used

While non-transactional messaging in a Network Load Balancing (NLB) environment is possible, it is much harder to achieve load-balancing in transactional MSMQ. [Microsoft provides a detailed answer](https://support.microsoft.com/en-us/kb/899611).

## Number of messages in Outgoing queues has a high value

Outgoing queues show three values:

- Number of messages
- Unacknowledged (msgs)
- Unprocessed (msgs)

By default *Number of messages* shows the count of messages that have not yet been delivered or processed. It does not indicate the number of messages that still need to be sent. Calculate the number of unsent messages by subtracting *Unprocessed (msgs)* from *Number of messages*.
This means that if an endpoint at the recipient is stopped or slow, the messages remaining to be processed are included in this count.

When MSMQ dead-lettering is disabled *Number of messages* will only indicate the number of messages remaining to be delivered. *Unprocessed (msgs)* will always show the value 0 when dead lettering is disabled.

For more information, see [MSMQ dead-letter queues](dead-letter-queues.md).

## Monitoring MSMQ

The following precautions can be taken to monitor the health of MSMQ in a production environment:

* Monitor all (transactional) dead letter queues.
* Monitor the disk space available to MSMQ.
* Monitor MSMQ performance counters:
  * MSMQ Service / Total bytes in all queues
  * MSMQ Service / Total messages in all queues

Define thresholds for the performance counters. Then create alerts that are triggered when the thresholds are exceeded. The alerts can be integrated with monitoring suites like Solarwinds, New Relic, SCOMM, or similar tools.

To determine useful thresholds, determine what values are typical for the specific environment to act as a baseline. This helps detect and address potential issues early, e.g. before MSMQ reaches its limits with regards to disk and memory usage.

## Virus scanners

Ensure that the relevant MSMQ folders are excluded from virus scanning. Virus scanners can prevent certain file actions from happening.

## Useful links

 - [MSMQ on Windows Server 2008](https://technet.microsoft.com/en-gb/library/cc753070%28WS.10%29.aspx)
 - [List of MSMQ articles](https://blogs.msdn.microsoft.com/johnbreakwell/)
 - [Changing the MSMQ Storage location](https://blogs.msdn.microsoft.com/johnbreakwell/2009/02/09/changing-the-msmq-storage-location/)
 - [Technet content for troubleshooting MSMQ on Windows 2008](https://blogs.msdn.microsoft.com/johnbreakwell/2008/05/07/technet-content-for-troubleshooting-msmq-on-windows-2008-and-vista/)
 - [Publicly available tools for troubleshooting MSMQ problems](https://blogs.msdn.microsoft.com/johnbreakwell/2007/12/13/what-publically-available-tools-are-there-for-troubleshooting-msmq-problems/)
 - [MSMQ service might not send or receive messages after a restart](https://support.microsoft.com/en-us/kb/2554746)
 - [MSMQ Errors and Events](https://technet.microsoft.com/en-us/library/dd337466.aspx)
   - [Message Queueing Resources](https://technet.microsoft.com/en-us/library/dd337480.aspx)
   - [Message Queueing System Resources](https://technet.microsoft.com/en-us/library/dd337537.aspx)
 - [Troubleshooting MSDTC issues with the DTCPing tool](https://blogs.msdn.microsoft.com/distributedservices/2008/11/12/troubleshooting-msdtc-issues-with-the-dtcping-tool/)
