## TypeNameHandling

For backward compatibility, the `NewtonsoftSerializer` serializer is using `TypeNameHandling.Auto` by default. This allows Json.NET to include additional type information for serialization and deserialization (see [Inferring message type from $type for more information](#inferring-message-type-from-type)).

Warn: `TypeNameHandling.Auto` should be used with caution and it is recommended to use `TypeNameHandling.None` via [custom settings](#usage-custom-settings) instead. When using `TypeNameHandling.Auto`, consider a [custom SerializationBinder](https://www.newtonsoft.com/json/help/html/SerializeSerializationBinder.htm) to limit the allowed deserialization types.

`TypeNameHandling.Auto` can be a security risk as it allows the message payload to control the deserialization target type. See [CA2326: Do not use TypeNameHandling values other than None](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2326) for further details on this vulnerability. By [configuring custom `JsonSerializerSettings` settings](#usage-custom-settings), the `TypeNameHandling` setting can be explicitly controlled (the default setting on the new `NewtonsoftJsonSerializer` is `TypeNameHandling.None`).
