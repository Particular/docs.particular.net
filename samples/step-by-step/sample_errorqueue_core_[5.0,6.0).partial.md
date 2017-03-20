## Error Queue

In the Shared project, create a class named `ConfigErrorQueue`:

snippet: ConfigErrorQueue

Each endpoint needs to [configure recoverability](/nservicebus/recoverability/). This defines where messages are sent when they cannot be processed due to repetitive exceptions during message processing.

NOTE: While it is possible to [configure the error queue in an App.config file](/nservicebus/recoverability/), the `IProvideConfiguration` interface can be used to [override app.config settings](/nservicebus/hosting/custom-configuration-providers.md), which allows sharing the same configuration across all endpoints.
