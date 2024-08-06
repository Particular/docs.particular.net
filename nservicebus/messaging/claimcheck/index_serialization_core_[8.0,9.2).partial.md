## Serialization

To configure the data bus, a serializer must be chosen. The recommended serializer is `SystemJsonDataBusSerializer` which is built into `NServiceBus` and uses the System.Text.Json serializer.

snippet: SpecifyingSerializer

### Additional deserializers

Additional deserializers can be added when configuring the data bus. They are picked up based on the data bus content-type header of the message, and also when the main serializer fails to deserialize a message.

snippet: SpecifyingDeserializer

### Implementing custom serializers

To override the data bus property serializer, create a class that implements `IDataBusSerializer` and add it to the dependency injection container when configuring the data bus. The custom serializer must be available to both the sending and the receiving endpoints.

> [!NOTE]
> Implementations of `IDataBusSerializer` should not close `Stream` instances that NServiceBus provides. NServiceBus manages the lifecycle of these `Stream` instances and may attempt to manipulate them after the custom serializer has been called.

> [!WARNING]
> For security purposes, the type information loaded from the payload should not be used. Instead, use the `Type` property provided to the `Deserialize` method.
