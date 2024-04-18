## Do not wrap message payload in a transport envelope

**Optional**

**Default**: `false`

By default the transport wraps outgoing messages in an envelope that contains also the message headers. The payload itself is encoded with Base64. This is done to ensure compatibility with endpoints running version 6.0 of the transport or below.

**Example**: To disable message wrapping and Base64-encoding of outgoing messages:

snippet: DoNotWrapOutgoingMessages

> [!WARNING]
> This setting should only be enabled if all endpoints are running a version of the transport that contains this setting. ServiceControl should be on version 4.29.3 or above.

### Message Attributes

When the `DoNotWrapOutgoingMessages` setting is enabled, all NServiceBus headers are stored in the `NServiceBus.AmazonSQS.Headers` message attribute.
If the message is being sent to a non-NServiceBus endpoint, the consumer can use message attributes to handle a message in a particular way without having to process the message body first.

> [!NOTE]
> When sending messages from a non-NServiceBus endpoint to an NServiceBus endpoint, use UTF8 encoding and add the [`NServiceBus.AmazonSQS.Headers` message attribute](/transports/sqs/native-integration.md#message-type-detection) to ensure compatibility.
