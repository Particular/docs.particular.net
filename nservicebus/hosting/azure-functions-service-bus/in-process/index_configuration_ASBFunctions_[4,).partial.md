`ServiceBusTriggeredEndpointConfiguration` loads certain configuration values from the Azure Function host environment in the following order:

- `IConfiguration` passed in via the constructor
- Environment variables

### ServiceBus connection

When using the `NServiceBusTriggerAttribute`, the connection to Azure Service Bus can be configured in multiple ways:

- Using just a `<ConnectionName>` key, with the value set to the connection string for the ServiceBus namespace to connect to.
- Using a `<CONNECTION_NAME_PREFIX>__fullyQualifiedNamespace` along with other connection properties prefixed by the same `<CONNECTION_NAME_PREFIX>`, as specified in the [Identity Based Connections](https://learn.microsoft.com/en-us/azure/azure-functions/functions-reference?tabs=blob&pivots=programming-language-csharp#common-properties-for-identity-based-connections) for Azure Functions.

If both a connection string and Identity Based connection values are specified, the connection string will take precedence. The default `<ConnectionName>` or `<CONNECTION_NAME_PREFIX>` is `AzureWebJobsServiceBus`, however an alternate value can be supplied to the `Connection` property on the `NServiceBusTriggerAttribute`

Example of a connection string based connection:

include: asb-connection-string-local-setting-file

Example of an Identity Based connection:

include: asb-identity-local-setting-file

snippet: asb-function-isolated-identity-connection

### Other Configuration

| Key                      | Value      | Notes     |
|--------------------------|------------|-----------|
| `ENDPOINT_NAME`          | The name of the NServiceBus endpoint to host | A value can be provided directly to the constructor. |
| `NSERVICEBUS_LICENSE`    | The NServiceBus license | Can also be provided via `serviceBusTriggeredEndpointConfig.EndpointConfiguration.License(...)`. |
| `WEBSITE_SITE_NAME`      | The name of the Azure Function app. Provided when hosting the function in Azure. | Used to set the NServiceBus [host identifier](/nservicebus/hosting/override-hostid.md). Local machine name is used if not set. |

For local development, use `local.settings.json`. In Azure, specify a Function setting using the environment variable as the key.

include: license-file-local-setting-file
