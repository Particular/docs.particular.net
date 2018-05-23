## Destination queue creation

Previous versions of the transport automatically created destination queues on send if they weren't available. The automatic creation of destination queues has been removed. Setting up a topology with queues is an operations concern and should happen during the [installation phase](/nservicebus/operations/installers.md) of the endpoint or via scripting when provisioning the environment.


## Permissions

A new set of permissions is required to run SQS transport. The following permissions must be granted to run the SQS transport.


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