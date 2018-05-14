---
title: Upgrade AmazonSQS Transport Version 1 to 3
summary: Instructions on how to upgrade AmazonSQS Transport Version 1 to 3.
component: SQS
isUpgradeGuide: true
reviewed: 2018-05-14
upgradeGuideCoreVersions:
 - 6
---


## Destination Queue creation

Previous versions of the transport automatically created destination queues on send if not available. The automatic creation of destination queues has been removed. Setting up a topology with queues is an operations concern and should happen during the [installation phase](/nservicebus/operations/installers.md) of the endpoint or via scripting when provisioning the environment.


## Permissions

A new set of permissions is required to run SQS transport. The following permissions must be granted to run SQS transport.


### [SQS permissions](http://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-api-permissions-reference.html)

 * CreateQueue
 * DeleteMessage
 * DeleteMessageBatch
 * GetQueueUrl
 * ReceiveMessage
 * SendMessage
 * SendMessageBatch
 * SetQueueAttributes
 * ChangeMessageVisibility
 * ChangeMessageVisibilityBatch
 * PurgeQueue


### [S3 permissions](http://docs.aws.amazon.com/AmazonS3/latest/dev/using-with-s3-actions.html)

When the transport is used in combination with large messages on S3 the following permissions are required

 * CreateBucket
 * DeleteObject
 * GetObject
 * PutObject
 * PutLifecycleConfiguration
 * GetLifecycleConfiguration
 * ListAllMyBuckets

## Wire Compatibility with 1.x endpoints

Versions 2 and 3 of the transport break wire compatibility with version 1 endpoints. The `TimeToBeReceived` and `ReplyToAddress` properties are no longer present in the message envelope, but instead are available in the message headers. Starting with version 3.3.0 of the transport a setting has been introduced to enable wire compatibility with 1.x endpoints when needed, at the expense of larger message size. To do so use the `EnableV1CompatibilityMode` setting:

snippet: V1BackwardsCompatibility