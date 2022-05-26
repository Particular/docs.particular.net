## Serialization

To configure DataBus a choice of serializer needs to be made. The recommended serializer is `SystemJsonDataBusSerializer` that's built into the `NServiceBus` and uses the System.Text.Json serializer.

snippet: SpecifyingSerializer

### Additional deserializers

Additional deserializers can also be added when configuring DataBus. They are picked up based on the databus content-type header of the message, and also when the main serializer fails to deserialize a message.

snippet: SpecifyingDeserializer

### Implementing custom serializers

To override the data bus property serializer, create a class that implements `IDataBusSerializer` and add pass it when configuring the DataBus. The custom serializer needs to be available by both sending and the receiving endpoint.

NOTE: Implementations of `IDataBusSerializer` should not close `Stream` instances that NServiceBus provides. NServiceBus manages the lifecycle of these `Stream` instances and may attempt to manipulate them after the custom serializer has been called.

WARNING: For security purposes, the type information loaded from the payload shoud not be used. Instead the `Type` property provided to the `Deserialize` method should be used.