
### Specifying additional deserializers

To support sending and receiving messages between endpoints using different serializers, additional deserialization capability may be specified. It is possible to register additional deserializers to process incoming messages. Additionally, if a deserializer requires custom settings, they can be provided during its registration.

snippet: AdditionalDeserializers

Note: When using multiple deserializers make sure that there's only one type registered per given `ContentType`.