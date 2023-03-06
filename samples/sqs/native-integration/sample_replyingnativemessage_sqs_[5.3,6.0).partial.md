## Replies

To enable the NServiceBus endpoint to [reply](/nservicebus/messaging/reply-to-a-message.md) back to a native queue, a reply-to address must be provided. To see this in action manually create a queue called `my-native-endpoint` in AWS and uncomment the line in the sender that adds the `ReplyToAddress` message attribute. The receiver uses a behavior to transfer the reply-to address in the native attribute to the [`NServiceBus.ReplyToAddress` header](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-replytoaddress) that NServiceBus uses to route replies:

snippet: PopulatingNativeReplyToAddress

After running the sample use the AWS management console to view the replies sent back to the native queue.
