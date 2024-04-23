### Message type detection

include: native-integration-messagetypedetection-intro

During message processing, the SQS transport inspects the native message attributes for an attribute with the name `NServiceBus.AmazonSQS.Headers`. This attribute's value should be a serialized `Dictionary<string, string>`. If the attribute is present, the message is treated as a native message, and the body is deserialized into the target type [inferred from the payload](/nservicebus/messaging/message-type-detection.md).

> [!NOTE]
> To represent an empty `NServiceBus.AmazonSQS.Headers` message attribute, the value should be `"{}"`

The native message body is loaded from the configured [S3 bucket](/transports/sqs/configuration-options.md#offload-large-messages-to-s3) when the `NServiceBus.AmazonSQS.Headers` attribute contains an entry with the key `S3BodyKey` and the value representing an S3 object key, including the [necessary prefix](/transports/sqs/configuration-options.md#offload-large-messages-to-s3-key-prefix) as configured by the receiving endpoint.

Whenever the native message needs to be copied for [moving messages to the error queue](/nservicebus/recoverability), [auditing](/nservicebus/operations/auditing.md) or [delayed retries](/nservicebus/recoverability/configure-delayed-retries.md) purposes, the native message is converted into the transport's internal structure. The collection from `NServiceBus.AmazonSQS.Headers` is moved from the native message attribute into the [headers collection](/nservicebus/messaging/headers.md). All other available message attributes from the original native message are copied over into the newly formed native message.
