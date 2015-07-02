---
title: 'MessageQueueException: Insufficient resources to perform operation'
summary: Exception due to sending messages to an offline or loaded machine.
tags: []
redirects:
 - nservicebus/messagequeueexception-insufficient-resources-to-perform-operation
---

This exception may occur if you try to send messages to a machine that has been off-line for a while, or the system is suffering from a larger than expected load spike, or when message queuing quota has exceeded its limit:

```
System.Messaging.MessageQueueException (0x80004005): Insufficient resources to perform operation. 
at System.Messaging.MessageQueue.SendInternal(Object obj, MessageQueueTransaction internalTransaction, MessageQueueTransactionType transactionType)
```

The cause of this exception is that the MSMQ has run out of space for holding on to messages. This could be due to messages sent that could not be delivered, or messages received that have not been processed.

## Things to try when running into this problem

1. Purge transactional dead-letter queue (TDLQ) under System Queues. If you purge other transactional queues, make sure to purge TDLQ as well. 
1. If journaling is turned on, purge messages found in journaling queue under System Queues. Ensure that journaling is disabled on each queue level, and only turn it on if needed for debugging purposes.
1. Increase MSMQ storage quota ([MSDN article](https://support.microsoft.com/en-us/kb/899612))

WARNING: On production servers uninstalling MSMQ will delete all queues and messages. Do not attempt MSMQ uninstall unless message loss is acceptable.

You can find more details about this exception in [this blog post](http://blogs.msdn.com/b/johnbreakwell/archive/2006/09/18/insufficient-resources-run-away-run-away.aspx).
