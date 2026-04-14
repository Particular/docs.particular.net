---
title: Upgrade Version 10 to 11
summary: Instructions on how to upgrade NServiceBus from version 10 to version 11.
reviewed: 2026-04-14
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 10
 - 11
---

include: upgrade-major

## Self-hosted endpoints

With the ubiquity of the .NET Generic Host as the entry point for an application's hosting, dependency injection, and logging needs, it no longer makes sense to self-host NServiceBus endpoints using `Endpoint.Create()` or `Endpoint.Start()`. Instead, NServiceBus endpoints can be added to the `IServiceCollection` which will cause them to start along with the host's lifecycle.

Instead of:

```csharp
var endpointInstance = await Endpoint.Start(endpointConfiguration);

// or

var startableEndpoint = await Endpoint.Create(endpointConfiguration);
var endpointInstance = await startableEndpoint.Start();
```

…the endpoint can be started through the .NET Generic Host:

```csharp
var builder = Host.CreateApplicationBuilder();

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

var host = builder.Build();

await host.RunAsync();
```

In addition, the following APIs related to creating and starting endpoints with the self-hosting API are deprecated and no longer necessary when using the .NET Generic Host:

- `NServiceBus.Endpoint`
- `NServiceBus.Installer`
- `NServiceBus.IEndpointInstance`
- `NServiceBus.IStartableEndpoint`
- `NServiceBus.IStartableEndpointWithExternallyManagedContainer`

### Endpoint-specific dependency injection

The `RegisterCompoments(Action<IServiceCollection> registration)` method on `EndpointConfiguration` is obsolete and must be replaced. Originally this method was meant to allow dependency injection registrations when self-hosting, but is no longer necessary without self-hosted endpoints. It is better practice to manage dependency injection registrations using standard .NET idioms through the Generic Host.

Instead of:

```csharp
var endpointConfiguration = new EndpointConfiguration("EndpointName");
endpointConfiguration.RegisterComponents(registrations =>
{
    registrations.AddSingleton<EndpointSpecificService>();
});
```

…the service can be added to the global `IServiceCollection` when only one NServiceBus endpoint is defined, and the endpoint will resolve the dependency from the global collection:

```csharp
var endpointConfiguration = new EndpointConfiguration("EndpointName");

builder.Services.AddSingleton<EndpointSpecificService>();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
```

When multiple endpoints are hosted in the same process, each endpoint can receive its own configured dependency using [keyed services](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection#keyed-services), where the key matches the endpoint name by default:

```csharp
var salesConfig = new EndpointConfiguration("Sales");
var billingConfig = new EndpointConfiguration("Billing");

var salesDb = new DatabaseService("sales-db");
var billingDb = new DatabaseService("billing-db");

builder.Services.AddKeyedSingleton<DatabaseService>("Sales", salesDb);
builder.Services.AddKeyedSingleton<DatabaseService>("Billing", billingDb);

builder.Services.AddNServiceBusEndpoint(salesConfig, "Sales");
builder.Services.AddNServiceBusEndpoint(billingConfig, "Billing");
```

### Logging

NServiceBus now uses [Microsoft.Extensions.Logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/) as its built-in logging infrastructure. When no other logging providers are configured, NServiceBus provides opinionated defaults: a rolling file logger and a colored console logger, just like the previous `DefaultFactory` did, but now powered by the `Microsoft.Extensions.Logging` pipeline under the covers. As soon as other logging providers are registered on the host, these built-in providers automatically disable themselves so that the externally configured providers take over without any manual opt-out.

The legacy NServiceBus logging configuration APIs have been deprecated and will produce compiler warnings. These APIs will cause compile errors in NServiceBus version 11 and will be removed in NServiceBus version 12.

#### Migrating from LogManager.GetLogger to ILogger<T>

`LogManager.GetLogger` is not deprecated yet, but it will be deprecated in a future version. It should be replaced with `Microsoft.Extensions.Logging`, which offers two approaches depending on the application structure. For more details, see [Logging in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/overview).

##### Dependency injection (recommended)

In applications that use dependency injection or a host, `ILogger<T>` should be obtained through constructor injection. This is the primary and recommended pattern in `Microsoft.Extensions.Logging`.

Instead of:

```csharp
public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger(typeof(MyHandler));

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info("Handling message");
        return Task.CompletedTask;
    }
}
```

use constructor-injected `ILogger<T>`:

```csharp
public class MyHandler(ILogger<MyHandler> logger) : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Handling message");
        return Task.CompletedTask;
    }
}
```

> [!NOTE]
> `Microsoft.Extensions.Logging` is designed as a dependency-injection-first framework. The category name is derived from the type parameter of `ILogger<T>`, which determines how log events are filtered and routed. Using `ILogger<T>` via constructor injection ensures that logger instances are properly scoped and configured by the host.

##### Static logging (non-hosted contexts)

In scenarios where dependency injection is not available, such as before the host is built or in static contexts, an `ILoggerFactory` can be created directly following the guidance in [Get started with .NET logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/overview?tabs=command-line#get-started):

```csharp
using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = factory.CreateLogger("MyCategory");
logger.LogInformation("Message logged before the host is built.");
```

This approach is suitable only for trivial scenarios. In non-trivial applications, `ILoggerFactory` and `ILogger` should be obtained from the DI container rather than created directly, as described in [Integration with hosts and dependency injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/overview#integration-with-hosts-and-dependency-injection).

##### High-performance logging

For high-performance scenarios, the [`LoggerMessage`](https://learn.microsoft.com/en-us/dotnet/core/extensions/loggermessage-generator) source generator creates strongly-typed logging methods that avoid unnecessary allocations and string formatting overhead.

#### Using custom logging providers

Instead of `LogManager.Use<T>()` or `LogManager.UseFactory(...)`, register custom logging providers directly with the `Microsoft.Extensions.Logging` infrastructure on the host:

```csharp
var builder = Host.CreateApplicationBuilder();

builder.Logging.AddSerilog();
```

> [!NOTE]
> Consider transitioning to the standard logging infrastructure provided by `Microsoft.Extensions.Logging`, for example, using providers like Serilog, NLog, or the built-in console provider. This offers richer filtering, structured logging, and integration with the broader .NET ecosystem, and avoids dependency on NServiceBus-specific logging infrastructure.

#### Configuring the rolling file logger

The `RollingLoggerProviderOptions` section is only relevant when relying on NServiceBus default logging. If the host already has an external logging provider configured, the built-in rolling file and console log providers are automatically disabled and these options have no effect.

Instead of using `DefaultFactory`, configure the rolling file logger through the options pattern:

```csharp
var builder = Host.CreateApplicationBuilder();

builder.Services.Configure<RollingLoggerProviderOptions>(options =>
{
    options.Directory = "C:/logs";
    options.LogLevel = LogLevel.Debug;
    options.NumberOfArchiveFilesToKeep = 10;
    options.MaxFileSizeInBytes = 10L * 1024 * 1024;
});
```

#### Direct logger retrieval from DefaultFactory

Calling `DefaultFactory.GetLoggingFactory().GetLogger(...)` is no longer supported and will throw an exception at runtime. Use `ILogger<T>` via dependency injection instead.

#### Deprecated APIs

The following APIs are deprecated:

| Deprecated API | Replacement |
| --- | --- |
| `LogManager.Use<T>()` | Register an `ILoggerProvider` via `IServiceCollection` |
| `LogManager.UseFactory(ILoggerFactory)` | Configure `Microsoft.Extensions.Logging` directly on the host |
| `DefaultFactory` | `services.Configure<RollingLoggerProviderOptions>()` |
| `DefaultFactory.Directory(string)` | `RollingLoggerProviderOptions.Directory` |
| `DefaultFactory.Level(LogLevel)` | `RollingLoggerProviderOptions.LogLevel` |
| `LoggingFactoryDefinition` | Implement `ILoggerProvider` and register via `services.AddSingleton<ILoggerProvider, YourProvider>()` |
