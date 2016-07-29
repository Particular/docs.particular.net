## Error Queue

Each endpoint configures [recoverability](/nservicebus/recoverability/):

```cs
endpointConfiguration.SendFailedMessagesTo("error");
```

This defines where messages are sent when they cannot be processed due to repetitive exceptions during message processing.

NOTE: It is also possible to [recoverability](/nservicebus/recoverability/) in an App.config file or the the `IProvideConfiguration` interface using [override app.config settings](/nservicebus/hosting/custom-configuration-providers.md), which allows sharing the same configuration across all endpoints.