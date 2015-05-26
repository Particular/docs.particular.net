---
title: Scripting RabbitMQ 
summary: Example code and scripts to facilitate deployment and operational actions against RabbitMQ.
---

Example code and scripts to facilitate deployment and operational actions against RabbitMQ.

NOTE: Using statements omitted from below code for simplicity

These samples use the [RabbitMQ.Client](http://www.nuget.org/packages/RabbitMQ.Client/) nuget. 

Since RabbitMQ.Client is not [CLS Compliant](https://msdn.microsoft.com/en-us/library/system.clscompliantattribute.aspx) it is not possible to run this code within PowerShell.

## Return message to source queue 

### The retry helper methods

The following code shows an example of how to perform the following actions

 * read a message from the error queue.
 * extract the failed queue from the headers.
 * forward that message to the failed queue name so it can be retried.

<!-- import rabbit-return-to-source-queue -->

### Using the retry helper methods 

<!-- import rabbit-return-to-source-queue-usage -->

## Create queues

There are two use cases for creation of queues

### The create queue helper methods

<!-- import rabbit-create-queues -->

### Using the create hqueue elper methods 

To create all queues for a given endpoint name.

<!-- import rabbit-create-queues-endpoint-usage -->

To create shared queues.

<!-- import rabbit-create-queues-shared-usage -->

## Delete queues

### The delete helper queue methods

<!-- import rabbit-delete-queues -->

### Using the delete queue helper methods

To delete all queues for a given endpoint name.

<!-- import rabbit-delete-queues-endpoint-usage -->

To delete shared queues

<!-- import rabbit-delete-queues-shared-usage -->