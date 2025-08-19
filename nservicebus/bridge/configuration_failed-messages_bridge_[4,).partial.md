### Error queue

By default, when the bridge transfers a message to the ServiceControl error queue it will attempt to translate the [`NServiceBus.ReplyToAddress`](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-replytoaddress) message header.
It can only do this successfully if the address in the `ReplyToAddress` header maps to some endpoint registered with the bridge. Therefore all endpoints in the system need to be registered with the bridge. Otherwise, the translation of the `ReplyToAddress` header might fail for some messages, which in turn will be moved to [the bridge error queue](/nservicebus/bridge/configuration.md#recoverability-error-queue).

The translation of the `ReplyToAddress` header value for failed messages can be disabled via a dedicated API setting.

snippet: do-not-translate-reply-to-address-for-failed-messages

> [!NOTE]
> Not translating the `NServiceBus.ReplyToAddress` message header for failed messages can cause bridge shovel errors, especially when bridging the SqlServer and MSMQ transports.

