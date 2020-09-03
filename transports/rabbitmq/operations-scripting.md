---
title: RabbitMQ Transport Scripting
summary: Example code and scripts to facilitate deployment and operational actions against RabbitMQ.
reviewed: 2020-09-03
component: Rabbit
related:
 - nservicebus/operations
redirects:
- nservicebus/rabbitmq/operations-scripting
---

Example code and scripts to facilitate deployment and operational actions against RabbitMQ.

These samples use the [RabbitMQ.Client NuGet Package](https://www.nuget.org/packages/RabbitMQ.Client/).

Since the RabbitMQ.Client is not [CLS Compliant](https://msdn.microsoft.com/en-us/library/system.clscompliantattribute.aspx) it is not possible to run this code within PowerShell.


## Native Send


### The native send helper methods

A send involves the following actions:

 * Create and serialize headers.
 * Write a message body directly to RabbitMQ.


#### In C&#35;

snippet: rabbit-nativesend


### Using the native send helper methods

snippet: rabbit-nativesend-usage

In this example, the value `MyNamespace.MyMessage` represents the .NET type of the message. See the [headers documentation](/nservicebus/messaging/headers.md) for more information on the `EnclosedMessageTypes` header.


## Return message to source queue


### The retry helper methods

This code shows an example of how to perform the following actions:

 * Read a message from the error queue.
 * Extract the failed queue from the headers.
 * Forward that message to the failed queue name so it can be retried.

snippet: rabbit-return-to-source-queue


### Using the retry helper methods

snippet: rabbit-return-to-source-queue-usage


## Create queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.


### The create queue helper methods

snippet: rabbit-create-queues


### Creating queues for an endpoint

To create all queues for a given endpoint name.

snippet: rabbit-create-queues-for-endpoint


### Using the create endpoint queues

snippet: rabbit-create-queues-endpoint-usage


### To create shared queues

snippet: rabbit-create-queues-shared-usage


## Create HA policy

To configure HA policy, refer to the [RabbitMQ HA documentation](https://www.rabbitmq.com/ha.html).


## Delete queues


### The delete helper queue methods

snippet: rabbit-delete-queues


### To delete all queues for a given endpoint

snippet: rabbit-delete-queues-for-endpoint

snippet: rabbit-delete-queues-endpoint-usage


### To delete shared queues

snippet: rabbit-delete-queues-shared-usage
