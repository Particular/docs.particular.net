
Settings can be overridden by adding to the `web.config` or the `app.config` files a configuration section called `AzureQueueConfig`:

snippet:AzureStorageQueueConfig

Note that the connection string can be also configured by specifying a value for connection string called `NServiceBus/Transport`, however this value will be overridden if another is provided in `AzureServiceBusQueueConfig`:

snippet: AzureStorageQueueConnectionStringFromAppConfig

