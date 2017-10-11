---
title: Scripting
summary: Example code and scripts to facilitate deployment and operational actions against MSMQ.
reviewed: 2016-08-31
component: MsmqTransport
redirects:
 - nservicebus/msmq/operations-scripting
related:
 - nservicebus/operations
---

Example code and scripts to facilitate deployment and operational actions against MSMQ.

These examples use the [System.Messaging.dll](https://msdn.microsoft.com/en-us/library/System.Messaging.aspx) and [System.Transactions.dll](https://msdn.microsoft.com/en-us/library/system.transactions.aspx) assemblies.


## Native Send


### The native send helper methods

A send involves the following actions:

 * Create and serialize headers.
 * Write a message body directly to MSMQ.


#### In C&#35;

snippet: msmq-nativesend


#### In PowerShell

snippet: msmq-nativesend-powershell


### Using the native send helper methods


#### In C&#35;

snippet: msmq-nativesend-usage


#### In PowerShell

snippet: msmq-nativesend-powershell-usage


## Return message to source queue


### The retry helper methods

A retry involves the following actions:

 * Read a message from the error queue.
 * Extract the failed queue from the headers.
 * Forward that message to the failed queue name so it can be retried.


#### In C&#35;

snippet: msmq-return-to-source-queue


#### In PowerShell

snippet: msmq-return-to-source-queue-powershell


### Using the retry helper methods

snippet: msmq-return-to-source-queue-usage


## Create queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.

partial: extra-queues


See also: [Queue Permissions](/transports/msmq/#permissions)


### The create queue helper methods


#### In C&#35;

snippet: msmq-create-queues


#### In PowerShell

snippet: msmq-create-queues-powershell


### Creating queues for an endpoint

To create all queues for a given endpoint name.


#### In C&#35;

snippet: msmq-create-queues-for-endpoint


#### In PowerShell

snippet: msmq-create-queues-for-endpoint-powershell


### Using the create endpoint queues


#### In C&#35;

snippet: msmq-create-queues-endpoint-usage


#### In PowerShell

snippet: msmq-create-queues-endpoint-usage-powershell


### To create shared queues


#### In C&#35;

snippet: msmq-create-queues-shared-usage


#### In PowerShell

snippet: msmq-create-queues-shared-usage-powershell


## Delete queues


### The delete helper queue methods


#### In C&#35;

snippet: msmq-delete-queues


#### In PowerShell

snippet: msmq-delete-queues-powershell


### To delete all queues for a given endpoint

snippet: msmq-delete-queues-for-endpoint

snippet: msmq-delete-queues-endpoint-usage


### To delete shared queues

snippet: msmq-delete-queues-shared-usage
