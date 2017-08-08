---
title: Scripting
summary: Example code and scripts to facilitate deployment and operational actions against the SQS Transport.
component: SQS
reviewed: 2017-06-28
related:
 - nservicebus/operations
---

The following are example code and scripts to facilitate deployment and operations against the SQS Transport.


## Native Send


### The native send helper methods

A send involves the following actions:

 * Create and serialize the payload including headers.
 * Write a message body directly to SQS Transport.


#### In C&#35;

snippet: sqs-nativesend

In this example, the value `MyNamespace.MyMessage` represents the .NET type of the message. See the [headers documentation](/nservicebus/messaging/headers.md) for more information on the `EnclosedMessageTypes` header.


#### In PowerShell

snippet: sqs-powershell-nativesend


### Using the native send helper methods

snippet: sqs-nativesend-usage


## Create queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.


### The create queue helper methods


#### In C&#35;

snippet: sqs-create-queues


#### In PowerShell

snippet: sqs-create-queues-powershell


### Creating queues for an endpoint

To create all queues for a given endpoint name.


#### In C&#35;

snippet: sqs-create-queues-for-endpoint


#### In PowerShell

snippet: sqs-create-queues-for-endpoint-powershell


### Using the create create endpoint queues


#### In C&#35;

snippet: sqs-create-queues-endpoint-usage


#### In PowerShell

snippet: sqs-create-queues-endpoint-usage-powershell


### To create shared queues


#### In C&#35;

snippet: sqs-create-queues-shared-usage


#### In PowerShell

snippet: sqs-create-queues-shared-usage-powershell


## Delete queues


### The delete helper queue methods

snippet: sqs-delete-queues


### To delete all queues for a given endpoint

snippet: sqs-delete-queues-for-endpoint

snippet: sqs-delete-queues-endpoint-usage


### To delete shared queues

snippet: sqs-delete-queues-shared-usage


## Return message to source queue


### The retry helper methods

A retry involves the following actions:

 * Read a message from the error queue.
 * Forward that message to another queue to be retried.

snippet: sqs-return-to-source-queue


### Using the retry helper methods

snippet: sqs-return-to-source-queue-usage
