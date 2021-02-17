---
title: SQS Transport Scripting
summary: Example code and scripts to facilitate deployment and operational actions against the SQS Transport.
component: SQS
reviewed: 2021-02-17
related:
 - nservicebus/operations
---

The following are example code and scripts to facilitate deployment and operations against the SQS Transport.

## Requirements

The PowerShell scripts require the PowerShell SDK installed and properly configured. For more information refer to the [PowerShell Getting setup guide](https://docs.aws.amazon.com/powershell/latest/userguide/pstools-getting-set-up.html).

For all operations that create resources in AWS the corresponding rights must be granted. For more information refer to the [IAM policies guide](https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html).

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

partial: create

partial: delete

## Return message to source queue

### The retry helper methods

A retry involves the following actions:

 * Read a message from the error queue.
 * Forward that message to another queue to be retried.

snippet: sqs-return-to-source-queue

WARNING: This example code will receive other messages from the error queue while it finds the desired message. All messages received by this code will be marked as invisible until the [visibility timeout](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-visibility-timeout.html) expires.

### Using the retry helper methods

snippet: sqs-return-to-source-queue-usage
