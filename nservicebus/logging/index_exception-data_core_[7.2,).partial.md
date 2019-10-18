## Additional exception data

Starting from NServiceBus version 7.2, exceptions from message processing might contain additional error information in the `Exception.Data` property. While the default logger exposes this information automatically, other loggers might require additional configuration.

[Custom behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md) can provide additional exception data by adding information to the `Exception.Data` property.
