---
title: Scripting MSMQ 
summary: Example code and scripts to facilitate deployment and operational actions against MSMQ.
---

Example code and scripts to facilitate deployment and operational actions against MSMQ.

These examples use the [System.Messaging.dll](https://msdn.microsoft.com/en-us/library/System.Messaging.aspx) and [System.Transactions.dll](https://msdn.microsoft.com/en-us/library/system.transactions.aspx) assemblies.

## Return message to source queue 

### The retry helper methods

The following code shows an example of how to perform the following actions

 * read a message from the error queue.
 * extract the failed queue from the headers.
 * forward that message to the failed queue name so it can be retried.

<!-- import msmq-return-to-source-queue -->

### Using the retry helper methods 

<!-- import msmq-return-to-source-queue-usage -->

## Create queues

There are two use cases for creation of queues

### The create queue helper methods

<!-- import msmq-create-queues -->

### Using the create helper queue methods 

To create all queues for a given endpoint name.

<!-- import msmq-create-queues-endpoint-usage -->

To create shared queues.

<!-- import msmq-create-queues-shared-usage -->

## Delete queues

### The delete helper queue methods

<!-- import msmq-delete-queues -->

### Using the delete queue helper methods

To delete all queues for a given endpoint name.

<!-- import msmq-delete-queues-endpoint-usage -->

To delete shared queues

<!-- import msmq-delete-queues-shared-usage -->

To delete all queues

<!-- import msmq-delete-all-queues -->