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

<!-- import sqlserver-nativesend -->


### Using the native send helper methods

<!-- import sqlserver-nativesend-usage -->


## Create queues

Queue creation can be done for a specific endpoint or queues shared between multiple endpoints.


### The create queue helper methods

<!-- import sqlserver-create-queues -->


### Using the create helper queue methods 

To create all queues for a given endpoint name.

<!-- import sqlserver-create-queues-endpoint-usage -->

To create shared queues.

<!-- import sqlserver-create-queues-shared-usage -->


## Delete queues


### The delete helper queue methods

<!-- import sqlserver-delete-queues -->


### Using the delete queue helper methods

To delete all queues for a given endpoint name.

<!-- import sqlserver-delete-queues-endpoint-usage -->

To delete shared queues

<!-- import sqlserver-delete-queues-shared-usage -->