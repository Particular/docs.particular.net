
## Skip serialization

When writing extensions to the pipeline it may be necessary to either take control of the serialization or to skip it entirely. One example of this is with the [callbacks feature](/nservicebus/messaging/callbacks.md). Callbacks skip serialization for integers and enums and instead embed them in the message headers.

To skip serialization, implement a behavior that targets `IOutgoingLogicalMessageContext`. For example, the following behavior skips serialization if the message is an integer, placing it in a header instead.

snippet: SkipSerialization

On the receiving side, this header can then be extracted and decisions on the incoming message processing pipeline can be made based on it.
