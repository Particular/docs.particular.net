---
title: 'MSMQ Transport: Scripting'
summary: Example code and scripts to facilitate deployment and operational actions against MSMQ.
reviewed: 2016-08-31
component: Core
---

Example code and scripts to facilitate deployment and operational actions against MSMQ.

These examples use the [System.Messaging.dll](https://msdn.microsoft.com/en-us/library/System.Messaging.aspx) and [System.Transactions.dll](https://msdn.microsoft.com/en-us/library/system.transactions.aspx) assemblies.


## Native Send


### The native send helper methods

A send involves the following actions:

 * Create and serialize headers.
 * Write a message body directly to MSMQ.


#### In C&#35;

snippet:msmq-nativesend


#### In PowerShell

snippet:msmq-nativesend-powershell


### Using the native send helper methods


#### In C&#35;

snippet:msmq-nativesend-usage


#### In PowerShell

snippet:msmq-nativesend-powershell-usage


## Return message to source queue


### The retry helper methods

A retry involves the following actions:

 * Read a message from the error queue.
 * Extract the failed queue from the headers.
 * Forward that message to the failed queue name so it can be retried.


#### In C&#35;

snippet:msmq-return-to-source-queue


#### In PowerShell

snippet:msmq-return-to-source-queue-powershell


### Using the retry helper methods

snippet:msmq-return-to-source-queue-usage


## Create queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.


### Default permissions

| Group | Permissions |
|---|---|
| Owning account | Write, Receive, Peek |
| Administrators | Full |
| Anonymous | Write  |
| Everyone | Write |

To retrieve the group names the [WellKnownSidType](https://msdn.microsoft.com/en-us/library/system.security.principal.wellknownsidtype.aspx) enumeration is used.

MSMQ permissions are defined in the [MessageQueueAccessRights](https://msdn.microsoft.com/en-us/library/system.messaging.messagequeueaccessrights.aspx) enumeration.

NOTE: Write access is granted to both `Everyone` and `Anonymous`. The reason for this is so that a given endpoint can receive messages from other endpoints running under different accounts. To further lock down MSMQ write permissions remove `Everyone` and `Anonymous` and instead grant specific access to a know subset of account.


### The create queue helper methods


#### In C&#35;

snippet:msmq-create-queues


#### In PowerShell

snippet:msmq-create-queues-powershell


### Using the create helper queue methods

To create all queues for a given endpoint name.

snippet:msmq-create-queues-endpoint-usage

To create shared queues.

snippet:msmq-create-queues-shared-usage


## Delete queues


### The delete helper queue methods


#### In C&#35;

snippet:msmq-delete-queues


#### In PowerShell

snippet:msmq-delete-queues-powershell


### Using the delete queue helper methods

To delete all queues for a given endpoint name.

snippet:msmq-delete-queues-endpoint-usage

To delete shared queues

snippet:msmq-delete-queues-shared-usage