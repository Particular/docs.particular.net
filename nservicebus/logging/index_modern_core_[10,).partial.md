NServiceBus endpoints integrate with the standard .NET logging infrastructure. [Microsoft.Extensions.Logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/) provides rich filtering, structured logging, and [works seamlessly with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/), the Generic Host, and many third-party providers.


## Using `ILogger<T>` in handlers

The recommended way to write log entries inside message handlers is to inject `ILogger<T>` through the constructor:

snippet: InjectingILoggerInterface

`ILogger<T>` is automatically wired into the dependency injection container when hosting with the .NET Generic Host.

## Configuring the default rolling file logger

When no external logging providers are registered, NServiceBus supplies backward-compatible defaults: a colored console provider and a rolling file provider. Consider registering standard [Microsoft logging providers](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/providers) such as Console, Debug, or EventLog for new applications.

If continuing with the built-in NServiceBus providers, configure the rolling file logger through `RollingLoggerProviderOptions`:

snippet: ConfiguringRollingFileLogger

When external logging providers are added to the host, the built-in providers automatically disable themselves so the external configuration takes over without any manual opt-out.

## Using custom logging providers

Register custom logging frameworks directly with the `Microsoft.Extensions.Logging` infrastructure on the host:

```csharp
var builder = Host.CreateApplicationBuilder();

builder.Logging.AddSerilog();
```

## High-performance logging

For scenarios that require minimal allocations, use the [`LoggerMessage`](https://learn.microsoft.com/en-us/dotnet/core/extensions/loggermessage-generator) source generator:

snippet: UsingLoggerMessageSourceGenerator

> [!WARNING]
> The NServiceBus-specific logging APIs described in the next section below are still functional but produce obsolete warnings starting in version 10.2 and will be removed in version 12. Use the `Microsoft.Extensions.Logging` patterns shown earlier for new development.
