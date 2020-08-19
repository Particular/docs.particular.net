## Message header correlation

Sagas can be correlated to messages using a message header instead of a message property.

snippet: saga-find-by-message-header

NOTE: An exception is thrown if an incoming message does not contain a header with the configured key, or the value cannot be converted into the saga correlation property type.