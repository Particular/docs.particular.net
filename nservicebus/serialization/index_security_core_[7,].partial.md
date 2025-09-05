### Dynamic type loading

Incoming messages might refer to a message type that has not yet been loaded by the endpoint. In these cases, the endpoint will automatically try to load the specified message type at runtime. This behavior can be disabled:

snippet: disable-dynamic-type-loading

> [!NOTE]
> When disabling dynamic type loading, all expected message types must be detected at endpoint startup time via [assembly scanning](/nservicebus/hosting/assembly-scanning.md).

### Message type inference

When an incoming message does not provide message type information via the `NServiceBus.EnclosedMessageTypes` header, the serializer can attempt to determine the message type based on the message's content (e.g., using Json.NET's `TypeNameHandling` setting). The exact capabilities and behavior depends heavily on the specific serializer being used but might introduce unintended security vulnerabilities. The endpoint can be configured to fail message processing immediately when the `NServiceBus.EnclosedMessageTypes` header does not contain a valid message type without passing the message content to the serializer:

snippet: disable-message-type-inference
