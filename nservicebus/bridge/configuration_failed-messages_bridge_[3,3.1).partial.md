### Error queue

By default, when the bridge transfers a message to the ServiceControl error queue it will not attempt to translate the [`NServiceBus.ReplyToAddress`](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-replytoaddress) message header.
This means that endpoints only need to be registered with the bridge if they are directly involved with messages transferred between transports.
The drawback is that if an endpoint is moved to a different transport, then any failed messages from that endpoint that perform a `ReplyTo` operation as part of the handler logic, cannot be retried since the value in the `NServiceBus.ReplyToAddress` header is unreachable.
