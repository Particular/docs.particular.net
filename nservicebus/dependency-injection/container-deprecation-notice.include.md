> [!WARNING]
> Starting with NServiceBus version 7, the dependency injection container adapter packages are no longer required. NServiceBus directly supports the `Microsoft.Extensions.DependencyInjection` model and third party containers can be integrated using the [NServiceBus.Extensions.DependencyInjection package](/nservicebus/dependency-injection/extensions-dependencyinjection.md) or via the [.NET Generic Host](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host).
>
> In NServiceBus version 8, NServiceBus will no longer provide adapters for external dependency injection containers.
>
> As of NServiceBus version 10.2, [hosting with Microsoft.Extensions.Hosting](/nservicebus/hosting/core-hosting.md) is built in, which includes `Microsoft.Extensions.DependencyInjection`.
>
> Visit the [dependency injection upgrade guide](/nservicebus/upgrades/7to8/dependency-injection.md) for further information.
