## Payload size padding

Amazon [SQS](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/quotas-messages.html) and [SNS](https://docs.aws.amazon.com/general/latest/gr/sns.html) allows for a maximum message size of 256KiB.

In certain scenarios, third party tool, such as monitoring tools, may add additional information to outgoing messages causing the message size to overflow and messages to be rejected by the infrastructure when sent. The `PayloadPaddingInBytes` can be used to specify an arbitrary number of bytes that will be added to the calculated payload size which is useful to account for any overhead of message attributes determining whether the message payload will be stored in S3 or not.
