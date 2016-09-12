#### SerializeMessageWrapperWith

Azure Storage Queues Transport wraps messages in a transport specific structure containing message metadata. By default, Azure Storage Queues Transport uses the same serializer for the message wrapper as configured for the contained message. In Versions 7 and above, it's possible to configure a different serializer for the wrapper using the `SerializeMessageWrapperWith` option

snippet:SerializerAndMessageWrapperSerializer

Note: All endpoints in the same system must use the same serializer for the message wrapper. This can be achieved by configuring the same message serializer using the `UseSerializer()` or the above `SerializeMessageWrapperWith` API.

