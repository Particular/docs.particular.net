## TypeNameHandling

The `NewtonsoftJsonSerializer` is using `TypeNameHandling.None` by default.

For backward compatibility, an old `NewtonsoftSerializer` serializer is using `TypeNameHandling.Auto` by default. This allows Json.NET to include additional type information for serialization and deserialization (see [Inferring message type from $type for more information](#inferring-message-type-from-type)). This serializer has been deprecated in favor of the `NewtonsoftJsonSerializer`.

> [!WARNING]
> `TypeNameHandling.Auto` can be a security risk as it allows the message payload to control the deserialization target type. See [CA2326: Do not use TypeNameHandling values other than None](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2326) for further details on this vulnerability.

If `TypeNameHandling.Auto` is required it can be configured via [custom `JsonSerializerSettings` settings](#usage-custom-settings), where the `TypeNameHandling` setting can be explicitly controlled.

When using `TypeNameHandling.Auto`, consider a [custom SerializationBinder](https://www.newtonsoft.com/json/help/html/SerializeSerializationBinder.htm) to limit the allowed deserialization types.
