## Specifying property types

There are two ways to specify the message properties to be sent using the Claim Check feature:

1. Using the `ClaimCheckProperty<T>` type
1. Using message conventions

> [!NOTE]
> Claim Check properties must be top-level properties on a message class.
>
> Apart from `byte[]`, any data type is supported as long as it is serializable. For example, `string`, custom classes, or other serializable types can be used.

### Using `ClaimCheckProperty<T>`

Set the type of the property to be sent over as `ClaimCheckProperty<byte[]>`, or any other serializable type:

snippet: MessageWithLargePayload

### Using message conventions

NServiceBus also supports defining Claim Check properties by convention. This allows data properties to be sent without using `ClaimCheckProperty<T>`, thus removing the need for a dependency on NServiceBus from the message types.

In the configuration of the endpoint include:

snippet: DefineMessageWithLargePayloadUsingConvention

Set the type of the property as `byte[]`, or any other serializable type:

snippet: MessageWithLargePayloadUsingConvention
