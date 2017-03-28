{{WARNING: Using `IMessageSession` or `IEndpointInstance` to send messages inside a handler instead of the provided `IHandlerMessageContext` should be avoided.

Some of the dangers when using an `IMessageSession` or `IEndpointInstance` inside a message handler to send or publish messages are:

 * Those messages will not participate in the same transaction as that of the message handler. This could result in messages being dispatched or events published even if the message handler resulted in an exception and the operation was rolled back.
 * Those messages will not be part of the [batching operation](/nservicebus/messaging/batched-dispatch.md).
 * Those messages will not contain any important message header information that is available via the `IHandlerMessageContext` interface parameter, e.g., CorrelationId.
}}