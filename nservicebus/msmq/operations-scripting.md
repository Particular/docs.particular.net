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

**In C&#35;**

<!-- import msmq-return-to-source-queue -->

**In Powershell**

<!-- import msmq-return-to-source-queue-powershell -->


### Using the retry helper methods

<!-- import msmq-return-to-source-queue-usage -->


## Create queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.


### The create queue helper methods

**In C&#35;**

<!-- import msmq-create-queues -->

**In Powershell**

<!-- import msmq-create-queues-powershell -->


### Using the create helper queue methods 

To create all queues for a given endpoint name.

<!-- import msmq-create-queues-endpoint-usage -->

To create shared queues.

<!-- import msmq-create-queues-shared-usage -->


## Delete queues


### The delete helper queue methods

**In C&#35;**

<!-- import msmq-delete-queues -->

**In Powershell**

<!-- msmq-delete-queues-powershell -->


### Using the delete queue helper methods

To delete all queues for a given endpoint name.

<!-- import msmq-delete-queues-endpoint-usage -->

To delete shared queues

<!-- import msmq-delete-queues-shared-usage -->