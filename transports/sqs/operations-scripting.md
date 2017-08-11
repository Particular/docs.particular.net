---
title: Scripting
summary: Example code and scripts to facilitate deployment and operational actions against the SQS Transport.
component: SQS
reviewed: 2017-06-28
related:
 - nservicebus/operations
---

The following are example code and scripts to facilitate deployment and operations against the SQS Transport.

## Requirements

The PowerShell scripts require the PowerShell SDK installed and properly configured. For more information refer to the [PowerShell Getting setup guide](http://docs.aws.amazon.com/powershell/latest/userguide/pstools-getting-set-up.html).

For all operations that create resources in AWS the corresponding rights must be granted. For more information refer to the [IAM policies guide](http://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html).

## QueueNameHelper

#### In C&#35;

snippet: sqs-queue-name-helper

The above `QueueNameHelper` makes sure that queues follow the proper naming guidelines for SQS.

#### In PowerShell

snippet: sqs-powershell-queue-name-helper

## Native Send

### The native send helper methods

A send involves the following actions:

 * Create and serialize the payload including headers.
 * Write a message body directly to SQS Transport.


#### In C&#35;

snippet: sqs-nativesend


#### In PowerShell

snippet: sqs-powershell-nativesend


### Using the native send helper methods

snippet: sqs-nativesend-usage

The headers must contain the message type that is sent as a fully qualified assembly name as well as the message id header. See the [headers documentation](/nservicebus/messaging/headers.md) for more information on the `EnclosedMessageTypes` and `MessageId` headers.

## Native Send Large Messages

### The native send helper methods

A send involves the following actions:

 * Create an unique S3 key containing the `S3Prefix` and the `MessageId`
 * Upload the body of the message to the S3 bucket
 * Create and serialize the message with an empty payload including headers and the `S3BodyKey`.
 * Write a message directly to SQS Transport.


#### In C&#35;

snippet: sqs-nativesend-large


#### In PowerShell

snippet: sqs-powershell-nativesend-large


### Using the native send helper methods

snippet: sqs-nativesend-large-usage

The headers must contain the message type that is sent as a fully qualified assembly name as well as the message id header. See the [headers documentation](/nservicebus/messaging/headers.md) for more information on the `EnclosedMessageTypes` and `MessageId` headers.

The S3 bucket name and the S3 prefix must be provided as defined in the transport configuration of the endpoint that will be receiving the message.

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
