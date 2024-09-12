### Error queue

By default, when the bridge transfers a message to the ServiceControl error queue it will attempt to translate the [`NServiceBus.ReplyToAddress`](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-replytoaddress) message header.
It can only do this successfully if the endpoint in the ReplyToAddress header is registered with the bridge, therefore all endpoints need to be registered with the bridge otherwise translation of the `NServiceBus.ReplyToAddress` will fail, and the message will be moved to the `bridge.error` queue instead of correctly transferred to the ServiceControl `error` queue.

This behavior can be disabled by configuring the bridge to not translate `NServiceBus.ReplyToAddress` message headers for failed messages.

snippet: do-not-translate-reply-to-address-for-failed-messages
