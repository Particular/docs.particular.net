## Serialization

To configure DataBus a choice of serializer needs to be made. The recommended serializer that's builtin is `SystemJsonDataBusSerializer`.

TODO: Update the databus snippet for v8 and link

WARN: `BinaryFormatterDataBusSerializer` [is not supported in .NET 5](https://docs.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/5.0/binaryformatter-serialization-obsolete) and later. This serializer will be removed in future versions of NServiceBus. For projects that target .NET 5, `SystemJsonDataBusSerializer` is recommended. Alternatively, use a custom serializer as described below.

### Using a custom serializer

To override the data bus property serializer, create a class that implements `IDataBusSerializer` and add pass it when configuring the DataBus. The custom serializer needs to be available by both sending and the receiving endpoint.

NOTE: Implementations of `IDataBusSerializer` should not close `Stream` instances that NServiceBus provides. NServiceBus manages the lifecycle of these `Stream` instances and may attempt to manipulate them after the custom serializer has been called.

WARNING: For security purposes, the type information loaded from the payload shoud not be used. Instead the `Type` property provided to the `Deserialize` method should be used. 

