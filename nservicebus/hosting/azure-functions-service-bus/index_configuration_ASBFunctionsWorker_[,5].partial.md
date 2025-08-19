## Configuration

`ServiceBusTriggeredEndpointConfiguration` can be used to configure the endpoint. It exposes details of the transport via the `Transport` property. To access the regular `EndpointConfiguration` object (i.e., to configure persistence), use the `AdvancedConfiguration` property.

`ServiceBusTriggeredEndpointConfiguration` loads certain configuration values from the Azure Function host environment in the following order:

1. `IConfiguration`
2. Environment variables

| Key                      | Value      | Notes     |
|--------------------------|------------|-----------|
| `AzureWebJobsServiceBus` | Connection string for the Azure ServiceBus namespace to connect to | This value is required for `ServiceBusTriggerAttribute`. |
| `NSERVICEBUS_LICENSE`    | The NServiceBus license | Can also be provided via `serviceBusTriggeredEndpointConfig.AdvancedConfiguration.License(...)`. |
| `ENDPOINT_NAME`          | The name of the NServiceBus endpoint to host | Optional. By default, the endpoint name is derived from the `NServiceBusTriggerFunction` attribute. |
| `WEBSITE_SITE_NAME`      | The name of the Azure Function app. Provided when hosting the function in Azure. | Optional. Used to set the NServiceBus [host identifier](/nservicebus/hosting/override-hostid.md). Local machine name is used if not set. |

For local development, use the `local.settings.json` file. In Azure, specify a Function setting using the environment variable as the key.

include: license-file-local-setting-file
