### Error queue

By default, when the bridge transfers a message to the ServiceControl error queue it will not attempt to translate the [`NServiceBus.ReplyToAddress`](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-replytoaddress) message header.
This means that endpoints only need to be registered with the bridge if they are directly involved with messages transferred between transports.
The drawback is that if an endpoint is moved to a different transport, then any failed messages from that endpoint that perform a `ReplyTo` operation as part of the handler logic, cannot be retried since the value in the `NServiceBus.ReplyToAddress` header is unreachable.

This behavior can be changed by configuring the bridge to translate the `NServiceBus.ReplyToAddress` message header for failed messages.

snippet: translate-reply-to-address-for-failed-messages

> [!WARNING]
> Since any endpoint can generate a failed message, enabling translation of the `NServiceBus.ReplyToAddress` header requires that all endpoints are registered with the bridge.
> Otherwise failed messages from unregistered endpoints will be moved to the `bridge.error` queue instead of correctly transferred to the ServiceControl `error` queue.

> [!NOTE]
> Translating the `NServiceBus.ReplyToAddress` message header for failed messages will become the default behavior in the next major version of the bridge. 