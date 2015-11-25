---
title: Scripting SQLServer Transport 
summary: Example code and scripts to facilitate deployment and operational actions against the SQLServer Transport.
---

The followings are example codes and scripts to facilitate deployment and operations against the SQLServer Transport.


## Native Send  


### The native send helper methods

The following code shows an example of how to perform the following actions

 * create and serialize headers.
 * write a message body directly to SQL Server Transport.


#### In C&#35;

snippet:sqlserver-nativesend


#### In Powershell;

snippet:sqlserver-powershell-nativesend


### Using the native send helper methods

snippet:sqlserver-nativesend-usage


## Create queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.


### The create queue helper methods

snippet:sqlserver-create-queues


### Using the create helper queue methods 

To create all queues for a given endpoint name.

snippet:sqlserver-create-queues-endpoint-usage

To create shared queues.

snippet:sqlserver-create-queues-shared-usage


## Delete queues


### The delete helper queue methods

snippet:sqlserver-delete-queues


### Using the delete queue helper methods

To delete all queues for a given endpoint name.

snippet:sqlserver-delete-queues-endpoint-usage

To delete shared queues

snippet:sqlserver-delete-queues-shared-usage


## Return message to source queue


### The retry helper methods

The following code shows an example of how to perform the following actions

 * read a message from the error queue table.
 * forward that message to another queue table to be retried.

NOTE: Since the connection information for the endpoint that failed is not contained in the error queue table that information is explicitly passed in.

snippet:sqlserver-return-to-source-queue


### Using the retry helper methods

snippet:sqlserver-return-to-source-queue-usage
