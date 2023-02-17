## Replies

To enable the NServiceBus endpoint to [reply](/nservicebus/messaging/reply-to-a-message.md) back to a native queue, a reply-to address must be provided. To see this in action manually create a queue called `my-native-endpoint` in AWS and use the `CreateHeadersWithReply()` method in the sender to include the [`NServiceBus.ReplyToAddress` header](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-replytoaddress) in the `NServiceBus.AmazonSQS.Headers` message attribute.

snippet: PopulatingNativeReplyToAddress

After running the sample use the AWS management console to view the replies sent back to the native queue.
