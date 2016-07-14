## Error Queue

Each endpoint specifies an [error queue](/nservicebus/errors/):

```cs
endpointConfiguration.SendFailedMessagesTo("error");
```

This defines where messages are sent when they cannot be processed due to repetitive exceptions during message processing.

NOTE: It is also possible to [configure the error queue in an App.config file](/nservicebus/errors/) or the the `IProvideConfiguration` interface using [override app.config settings](/nservicebus/hosting/custom-configuration-providers.md), which allows sharing the same configuration across all endpoints.