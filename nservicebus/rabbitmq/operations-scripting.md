---
title: 'RabbitMQ Transport: Scripting'
summary: Example code and scripts to facilitate deployment and operational actions against RabbitMQ.
reviewed: 2016-03-11
---

Example code and scripts to facilitate deployment and operational actions against RabbitMQ.

NOTE: Using statements omitted from below code for simplicity

These samples use the [RabbitMQ.Client](https://www.nuget.org/packages/RabbitMQ.Client/) NuGet.

Since RabbitMQ.Client is not [CLS Compliant](https://msdn.microsoft.com/en-us/library/system.clscompliantattribute.aspx) it is not possible to run this code within PowerShell.


## Native Send


### The native send helper methods

A send involves the following actions:

 * Create and serialize headers.
 * Write a message body directly to RabbitMQ.


#### In C&#35;

snippet:rabbit-nativesend


### Using the native send helper methods

snippet:rabbit-nativesend-usage

In this example, the value `MyNamespace.MyMessage` represents the .NET type of the message. See the [headers documentation](/nservicebus/messaging/headers.md) for more information on the `EnclosedMessageTypes` header.


## Return message to source queue


### The retry helper methods

The following code shows an example of how to perform the following actions:

 * Read a message from the error queue.
 * Extract the failed queue from the headers.
 * Forward that message to the failed queue name so it can be retried.

snippet:rabbit-return-to-source-queue


### Using the retry helper methods

snippet:rabbit-return-to-source-queue-usage


## Create queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.


### The create queue helper methods

snippet:rabbit-create-queues


### Using the create queue helper methods

To create all queues for a given endpoint name.

snippet:rabbit-create-queues-endpoint-usage

To create shared queues.

snippet:rabbit-create-queues-shared-usage


## Create HA policy

To configure HA policy, refer to [RabbitMQ official documentation](https://www.rabbitmq.com/ha.html)


## Delete queues


### The delete helper queue methods

snippet:rabbit-delete-queues


### Using the delete queue helper methods

To delete all queues for a given endpoint name:

snippet:rabbit-delete-queues-endpoint-usage

To delete shared queues:

snippet:rabbit-delete-queues-shared-usage