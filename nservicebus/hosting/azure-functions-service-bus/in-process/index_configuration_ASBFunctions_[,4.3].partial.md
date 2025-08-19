`ServiceBusTriggeredEndpointConfiguration` loads certain configuration values from the Azure Function host environment in the following order:

- `IConfiguration` passed in via the constructor
- Environment variables

| Key                      | Value      | Notes     |
|--------------------------|------------|-----------|
| `AzureWebJobsServiceBus` | Connection string for the Azure ServiceBus namespace to connect to | This value is required for `ServiceBusTriggerAttribute`. An alternative key can be passed into the constructor. |
| `ENDPOINT_NAME`          | The name of the NServiceBus endpoint to host | A value can be provided directly to the constructor. |
| `NSERVICEBUS_LICENSE`    | The NServiceBus license | Can also be provided via `serviceBusTriggeredEndpointConfig.EndpointConfiguration.License(...)`. |
| `WEBSITE_SITE_NAME`      | The name of the Azure Function app. Provided when hosting the function in Azure. | Used to set the NServiceBus [host identifier](/nservicebus/hosting/override-hostid.md). Local machine name is used if not set. |

For local development, use `local.settings.json`. In Azure, specify a Function setting using the environment variable as the key.

include: license-file-local-setting-file