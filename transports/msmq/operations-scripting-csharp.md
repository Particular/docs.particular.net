---
title: Scripting using C#
summary: Example code and scripts to facilitate deployment and operational actions against MSMQ.
reviewed: 2016-08-31
component: MsmqTransport
redirects:
 - nservicebus/msmq/operations-scripting
related:
 - nservicebus/operations
---

These examples use the [System.Messaging.dll](https://msdn.microsoft.com/en-us/library/System.Messaging.aspx) and [System.Transactions.dll](https://msdn.microsoft.com/en-us/library/system.transactions.aspx) assemblies.


## Native Send

include: operations-scripting-send


### The native send helper methods

snippet: msmq-nativesend


### Usage

snippet: msmq-nativesend-usage


## Return message to source queue


### The retry helper methods

include: operations-scripting-retry


snippet: msmq-return-to-source-queue


### Usage

snippet: msmq-return-to-source-queue-usage


## Create queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.

partial: extra-queues


See also: [Queue Permissions](/transports/msmq/#permissions)


### The create queue helper methods


snippet: msmq-create-queues


### Creating queues for an endpoint

To create all queues for a given endpoint name.

snippet: msmq-create-queues-for-endpoint


### Usage

snippet: msmq-create-queues-endpoint-usage


### To create shared queues


snippet: msmq-create-queues-shared-usage


## Delete queues


### The delete helper queue methods

snippet: msmq-delete-queues


### To delete all queues for a given endpoint

snippet: msmq-delete-queues-for-endpoint

snippet: msmq-delete-queues-endpoint-usage


### To delete shared queues

snippet: msmq-delete-queues-shared-usage
