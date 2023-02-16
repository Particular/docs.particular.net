## DoNotWrapOutgoingMessages

**Optional**

**Default**: `false`

By default the transport wraps and base64 encodes outgoing messages to ensure compatibility with endpoints running version 6.0 of the transport or below.

**Example**: To disable message wrapping and base64 encoding of outgoing messages:

snippet: DoNotWrapOutgoingMessages

WARN: This setting should only be enabled if all endpoints are running version 6.1 or above of the transport.

NOTE: When sending messages from a non NServiceBus endpoint to a NServiceServiceBus endpoint, use UTF8 encoding to ensure compatibility.

### Message Attributes

When the `DoNotWrapOutgoingMessages` setting is enabled, all NServiceBus headers will be stored in the `NServiceBus.AmazonSQS.Headers` message attribute.
If the message is being sent to a non NServiceBus endpoint, the consumer can use message attributes to handle a message in a particular way without having to process the message body first.
