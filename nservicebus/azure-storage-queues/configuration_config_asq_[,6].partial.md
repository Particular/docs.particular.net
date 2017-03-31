-NOTE: `QueueName` and `QueuePerInstance` are obsoleted. Instead, use bus configuration object to specify the endpoint name and select a scale out option.

Settings can be overridden by adding to the `web.config` or the `app.config` files a configuration section:

snippet: AzureStorageQueueConfig

Note that the connection string can be also configured by specifying a value for connection string called `NServiceBus/Transport`, however this value will be overridden if another is provided in the configuration section:

snippet: AzureStorageQueueConnectionStringFromAppConfig

