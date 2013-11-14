<!--
title: "MessageQueueException: Insufficient resources to perform operation"
tags: ""
summary: "<p>This exception may occur if you try to send messages to a machine that has been offline for a while, or the system is suffering from a larger than expected load spike:</p>
"
-->

This exception may occur if you try to send messages to a machine that has been offline for a while, or the system is suffering from a larger than expected load spike:


System.Messaging.MessageQueueException (0x80004005): Insufficient resources to perform operation. at System.Messaging.MessageQueue.SendInternal(Object obj, MessageQueueTransaction internalTransaction, MessageQueueTransactionType transactionType)


The cause of this exception is that the MSMQ has run out of space for holding on to messages. This could be due to messages sent that could not be delivered, or messages received that have not been processed.

You can find more details about this exception in [this blog post](http://blogs.msdn.com/b/johnbreakwell/archive/2006/09/18/insufficient-resources-run-away-run-away.aspx).

