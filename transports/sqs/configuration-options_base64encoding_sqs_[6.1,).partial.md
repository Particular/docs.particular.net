## DoNotBase64EncodeOutgoingMessages

**Optional**

**Default**: `false`

By default the transport base64 encodes outgoing messages to ensure compatibility with endpoints running version 6.0 of the transport or below.

**Example**: To disable base64 encoding of outgoing messages:

snippet: DoNotBase64EncodeOutgoingMessages

WARN: This setting should only be enabled if all endpoints are running version 6.1 or above of the transport.