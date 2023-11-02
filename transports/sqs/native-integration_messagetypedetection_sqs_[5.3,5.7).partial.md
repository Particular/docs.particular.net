
### Message type detection

include: native-integration-messagetypedetection-intro

During message processing, the SQS transport inspects the native message attributes for an attribute with the name `MessageTypeFullName` and a value representing a full type name (e.g. `Sales.OrderAccepted`). If the attribute is present, the message is treated as a native message, and the body is deserialized into the target type represented by `MessageTypeFullName`.

The native message body is loaded from the configured [S3 bucket](/transports/sqs/configuration-options.md#offload-large-messages-to-s3) when the message attribute contains an attribute with the key `S3BodyKey` and the value representing an S3 object key, including the [necessary prefix](/transports/sqs/configuration-options.md#offload-large-messages-to-s3-key-prefix).

Whenever the native message needs to be copied for [moving messages to the error queue](/nservicebus/recoverability), [auditing](/nservicebus/operations/auditing.md) or [delayed retries](/nservicebus/recoverability/configure-delayed-retries.md) purposes, the native message is converted into the transport's internal structure. The `MessageTypeFullName` and `S3BodyKey` headers are moved from the native message attributes into the [headers collection](/nservicebus/messaging/headers.md). All other available message attributes from the original native message are copied over into the newly formed native message.
