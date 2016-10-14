
### Custom Content Key

When using [additional deserializers](/nservicebus/serialization/#specifying-additional-deserializers) or transitioning between different versions of the same serializer it can be helpful to take explicit control over the content type a serializer passes to NServiceBus (to be used for the [ContentType header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-contenttype)).