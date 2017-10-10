---
title: Upgrade AmazonSQS Transport Version 2 to 3
summary: Instructions on how to upgrade AmazonSQS Transport Version 2 to 3.
component: SQS
isUpgradeGuide: true
reviewed: 2017-10-05
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


## MaxTTLDays renamed MaxTimeToLive

The `MaxTTLDays` method has been renamed to `MaxTimeToLive` and now takes a [TimeSpan](https://msdn.microsoft.com/en-us/library/system.timespan.aspx)

snippet: 3to4_MaxTTL