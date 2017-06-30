---
title: MSMQ Transport Troubleshooting
summary: Resolutions for common problems with the MSMQ transport.
reviewed: 2017-06-30
component: MsmqTransport
tags:
- Transport
- MSMQ
redirects:
 - nservicebus/messagequeueexception-insufficient-resources-to-perform-operation
 - nservicebus/msmq/messagequeueexception-insufficient-resources-to-perform-operation
 - nservicebus/msmq/network-load-balancing
 - nservicebus/msmq/troubleshooting
---

This article details common problems encountered with the MSMQ Transport and how to resolve them.


## Messages stuck or not arriving

MSMQ uses store-and-forward to communicate with remote machines. Messages are stored locally, and then the MSMQ service repeatedly attempts to deliver them to the destination queue on the remote machine.

Approaches for diagnosing messages stuck in the outgoing queue.

 * Check the **Outgoing Queues** on each server involved, while the problem is occurring. Each item represents a connection to a remote server. Items stuck here represent an inability to transfer messages to the remote server. The **State** and **Connection History** columns may point to a connectivity issue between servers.
 * Check the Microsoft support article [MSMQ service might not send or receive messages after a restart](https://support.microsoft.com/en-us/kb/2554746). This details how an error in how MSMQ binds to IP addresses and ports can cause one server to be unable to validate messages coming from another, causing them to be rejected.
 * If servers are cloned from the same virtual machine image, this causes them to have the same `QMId` in the registry key `HKEY_LOCAL_MACHINE\Software\Microsoft\MSMQ\Parameters\Machine Cache`, which interferes with message delivery. Use the workaround described in [MSMQ prefers to be unique](https://blogs.msdn.microsoft.com/johnbreakwell/2007/02/06/msmq-prefers-to-be-unique/) to reset the `QMId` on an existing machine, but it is preferable to use [Microsoft's Sysprep tool](https://support.microsoft.com/en-us/kb/314828) before capturing the virtual machine image.


### Note on MSMQ Distributor

To scale out MSMQ processing, a Distributor node accepts messages in one queue and then distributes it to eligible workers as they come available. This is accomplished by having each worker send a ReadyMessage to the distributor's *control queue* when it is ready for more work, and then the distributor forwards a message to that worker.

The problems outlined above are the leading cause of distributor issues, due to worker's ReadyMessages getting stuck in the workers' outgoing queues unable to reach the distributor or messages stuck in the distributor's outgoing queue unable to reach the workers.


## MessageQueueException: Insufficient resources to perform operation

This exception may occur if trying to send messages to a machine that has been offline for a while, or the system is suffering from a larger than expected load spike, or when message queuing quota has exceeded its limit:

```
System.Messaging.MessageQueueException (0x80004005): Insufficient resources to perform operation.
at System.Messaging.MessageQueue.SendInternal(Object obj, MessageQueueTransaction internalTransaction, MessageQueueTransactionType transactionType)
```

The cause of this exception is that the MSMQ has run out of space for holding on to messages. This could be due to messages sent that could not be delivered, or messages received that have not been processed.


### Resolution

 1. Make sure that the hard disk drive has sufficient space.
 1. Purge the transactional dead-letter queue (TDLQ) under System Queues.
  * This queue acts as a recycle bin for other transactional queues, so if other transactional queues have been purged, ensure the TDLQ is purged as well.
  * Within the TDLQ, the Class column shows the reason the message arrived there. Common messages include "The queue was purged" or "The queue was deleted".
 1. If journaling is turned on, purged messages can be found in the journaling queue under System Queues. Ensure that journaling is disabled on each queue level, and only turn it on if needed for debugging purposes.
 1. Increase the MSMQ storage quota ([MSDN article](https://support.microsoft.com/en-us/kb/899612))

WARNING: On production, servers uninstalling MSMQ deletes all queues and messages, which may contain business data. Do not attempt uninstalling MSMQ unless message loss is acceptable.

For more information on this error, see [John Breakwell's article in MSDN](https://blogs.msdn.microsoft.com/johnbreakwell/2006/09/18/insufficient-resources-run-away-run-away/).


## Virtual Private Networks (VPN)

MSMQ is cannot to dynamically detect network interfaces. If a connection to a VPN is established after the MSMQ service starts, a restart of the MSMQ service is required. Once it starts with the interface, the VPN is free to disconnect/reconnect whenever it wants.

It is recommended to have batch setup scripts that run on server startups to connect the VPN, which then restarts the MSMQ service automatically.


## Network Load Balancing cannot be used

While non-transactional messaging in a Network Load Balancing (NLB) environment is possible, it is much harder to achieve load-balancing in transactional MSMQ. [Microsoft provides a detailed answer](https://support.microsoft.com/en-us/kb/899611).


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
