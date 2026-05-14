### Using Microsoft.Extensions.Hosting

NServiceBus integrates with any web host that supports the [.NET Generic Host](https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host). The [NServiceBus.Extensions.Hosting](/nservicebus/hosting/extensions-hosting.md) package registers `IMessageSession` with the dependency injection container.