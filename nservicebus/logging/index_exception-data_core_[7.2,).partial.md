## Additional exception data

Starting from NServiceBus version 7.2, exceptions from failing message handlers might contain additional error information in the `Exception.Data` property. While the default logger exposes those information automatically, other loggers might require additional configuration.
