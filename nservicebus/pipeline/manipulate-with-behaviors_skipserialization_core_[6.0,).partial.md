
## Skip Serialization

When writing extensions to the pipeline it may be necessary to either take control of the serialization or to skip it entirely. One example usage of this is the [Callbacks](/nservicebus/messaging/callbacks.md). Callbacks skips serialization for integers and enums and instead embeds them in the message headers.

To skip serialization implement a behavior that targets `IOutgoingLogicalMessageContext`. For example, the following behavior skips serialization if a send on an integer is requested. It instead places that in the header.

snippet: SkipSerialization

On the receiving side this header can then be extracted from the headers and decisions on the incoming message processing pipeline can be made based on it.

