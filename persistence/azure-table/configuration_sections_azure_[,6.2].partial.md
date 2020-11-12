In NServiceBus.Azure the behavior of the `AzureStoragePersister` can be controlled by working with the appropriate configuration section(s) in the `app.config` or by using the code via the [`IProvideConfiguration` adapter](/nservicebus/hosting/custom-configuration-providers.md). Both approaches to configuring the persister can access the same configuration options.

### Configuration with Configuration Section

In NServiceBus.Azure configuration can be performed using configuration sections and properties in the `app.config` file.

snippet: AzurePersistenceFromAppConfig