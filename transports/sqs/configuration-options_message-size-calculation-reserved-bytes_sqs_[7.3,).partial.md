## Reserve bytes when calculating message size

snippet: ReserveBytesInMessageSizeCalculation

Amazon [SQS](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/quotas-messages.html) and [SNS](https://docs.aws.amazon.com/general/latest/gr/sns.html) allows for a maximum message size of 256KiB.

In specific scenarios, third-party tools, such as monitoring tools, may add additional information to outgoing messages, causing the message size to overflow and messages to be rejected by the infrastructure when sent. The `ReserveBytesInMessageSizeCalculation` can specify a number of bytes between 0 and 25 * 1024 that will be added to the calculated payload size. It is helpful to account for any overhead of message attributes added outside the scope of NServiceBus to address the SQS service message size limitation by uploading the message payload to S3.
