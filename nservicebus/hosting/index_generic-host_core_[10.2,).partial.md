## Microsoft.Extensions.Hosting

The [Microsoft Generic Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host) is the standard way to host NServiceBus in modern .NET applications. Endpoints are registered on `IServiceCollection` using `AddNServiceBusEndpoint` — see the [recommended hosting model](/nservicebus/hosting/core-hosting.md).

For existing applications, [NServiceBus.Extensions.Hosting](/nservicebus/hosting/extensions-hosting.md) and the `UseNServiceBus` integration remain supported.