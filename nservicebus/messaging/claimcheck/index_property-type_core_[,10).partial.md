## Specifying `DataBus` properties

There are two ways to specify the message properties to be sent using the `DataBus`:

1. Using the `DataBusProperty<T>` type
1. Using message conventions

> [!NOTE]
> `DataBus` properties must be top-level properties on a message class.
>
> Apart from `byte[]`, any data type is supported as long as it is serializable. For example, `string`, custom classes, or other serializable types can be used.

### Using `DataBusProperty<T>`

Set the type of the property to be sent over as `DataBusProperty<byte[]>`, or any other serializable type:

snippet: MessageWithLargePayload

### Using message conventions

NServiceBus also supports defining `DataBus` properties by convention. This allows data properties to be sent without using `DataBusProperty<T>`, thus removing the need for a dependency on NServiceBus from the message types.

In the configuration of the endpoint include:

snippet: DefineMessageWithLargePayloadUsingConvention

Set the type of the property as `byte[]`, or any other serializable type:

snippet: MessageWithLargePayloadUsingConvention
