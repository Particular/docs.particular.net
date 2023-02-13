## V1 Compatibility Mode

**Optional**

**Default**: Disabled

This option enables serialization of the `TimeToBeReceived` and `ReplyToAddress` message headers in the message envelope for compatibility with endpoints using version 1 of the transport.

NOTE: This feature is available in version 3.3.0 to 6.0.*, however it has been marked as obsolete in version 6.1.0 with a warning, and will be treated as an error from version 7.0.0.

**Example**: To enable version 1 compatibility, specify:

snippet: V1BackwardsCompatibility
