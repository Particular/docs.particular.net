---
title: Upgrade Version 10 to 11
summary: Instructions on how to upgrade NServiceBus from version 10 to version 11.
reviewed: 2026-06-16
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

builder.Services.AddKeyedSingleton<DatabaseService>(salesConfig.EndpointName, salesDb);
builder.Services.AddKeyedSingleton<DatabaseService>(billingConfig.EndpointName, billingDb);

builder.Services.AddNServiceBusEndpoint(salesConfig, "Sales");
builder.Services.AddNServiceBusEndpoint(billingConfig, "Billing");
```

### Logging

NServiceBus now uses [Microsoft.Extensions.Logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/) as its built-in logging infrastructure. When no other logging providers are configured, NServiceBus provides opinionated defaults: a rolling file logger and a colored console logger, just like the previous `DefaultFactory` did, but now powered by the `Microsoft.Extensions.Logging` pipeline. As soon as other logging providers are registered on the host, these built-in providers automatically disable themselves so that the externally configured providers take over without any manual opt-out.

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

Instead of `LogManager.Use<T>()` or `LogManager.UseFactory(…)`, register custom logging providers directly with the `Microsoft.Extensions.Logging` infrastructure on the host:

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

## Host identifier algorithm change

In version 11, the default algorithm for generating deterministic host identifiers changes from MD5 to XxHash128 (RFC 9562 version 8 GUIDs). This produces different host identifiers, which affects how endpoints are identified in [ServicePulse](/servicepulse/) and [ServiceControl](/servicecontrol/). Changing the algorithm will cause existing known endpoints to appear inactive in the ServicePulse [heartbeats](/monitoring/heartbeats/in-servicepulse.md) and [monitoring](/monitoring/metrics/in-servicepulse.md) views, while new instances (with the changed host identifiers) appear in their place.

### Rationale

This change provides a path for customers who require **FIPS-compliant** host identifiers (see [FIPS compliance](/nservicebus/compliance/fips.md)). The legacy MD5-based algorithm is not FIPS-compliant; by moving to the new `XxHash128` algorithm, the framework uses a compliant standard by default.

To ensure a predictable transition, this is designed as a multi-phase migration:

| NServiceBus Versions | Hashes Available | Default Hash | App Switch |
|:-:|:-:|:-:|:-:|
| <= 10.2 | MD5 Only | MD5 | - |
| >= 10.2 && < 11.0 | MD5 + XxHash128 | MD5 | Can opt in |
| >= 11.0 && < 12.0 | MD5 + XxHash128 | XxHash128 | Can opt out |
| >= 12.0 | XxHash128 Only | XxHash128 | - |


In version 11, XxHash128 becomes the default. By making this an explicit switch rather than an automatic change, there is a clear "escape hatch" to preserve legacy host IDs if correlation with older monitoring data (such as ServicePulse) must be maintained during the transition.

This approach allows the framework to move toward a compliant default while providing the necessary flexibility to manage existing integrations before the legacy algorithm is removed in version 12.

### Impact

After upgrading, endpoints will receive new host identifiers. This causes endpoints to appear as new entries in ServicePulse, while the previous instances become stale and must be [removed from the monitoring view](/monitoring/metrics/in-servicepulse.md#disconnected-endpoints-removing-disconnected-endpoints).

### Preserving the legacy host identifier

To preserve the existing MD5-based host identifier after upgrading, set the following AppContext switch before endpoint startup:

```csharp
AppContext.SetSwitch("NServiceBus.Core.Hosting.UseV2DeterministicGuid", false);
```

Or via environment variable:

```text
DOTNET_NServiceBus_Core_Hosting_UseV2DeterministicGuid=false
```

Or via MSBuild in the project file:

```xml
<ItemGroup>
  <RuntimeHostConfigurationOption Include="NServiceBus.Core.Hosting.UseV2DeterministicGuid" Value="false" />
</ItemGroup>
```

> [!NOTE]
> The legacy MD5-based host identifier algorithm and the `UseV2DeterministicGuid` AppContext switch will be removed in version 12.

## OpenTelemetry

### Context propagation

In version 11, NServiceBus propagates the [W3C Trace Context](https://www.w3.org/TR/trace-context/) and [W3C Baggage](https://www.w3.org/TR/baggage/) using the built-in .NET `DistributedContextPropagator` instead of the custom propagation logic used in version 10. This aligns the on-the-wire format with the W3C specifications and improves interoperability with standard OpenTelemetry tooling and non-NServiceBus systems that participate in the same trace. See [OpenTelemetry](/nservicebus/operations/opentelemetry.md) for an overview of the feature.

#### Trace correlation is unaffected

The `traceparent` and `tracestate` headers continue to be emitted in the W3C format. Distributed traces still correlate correctly between version 10 and version 11 endpoints in both directions, so upgrading does not break trace continuity.

#### Baggage serialization change

The change affects how the `baggage` header is serialized on the wire:

- Version 10 emitted a compact form with no optional whitespace (`key1=value1,key2=value2`) and percent-encoded baggage values aggressively.
- Version 11 emits the W3C form with optional whitespace around the delimiters (`key1 = value1, key2 = value2`) and percent-encodes only the characters that are structurally significant (such as `,`, `;`, and `%`).

Both versions decode percent-encoding when reading, so a baggage value written by one version is generally decoded correctly by the other — with the exception described below.

#### Mixed-version incompatibility

> [!WARNING]
> When [baggage](https://www.w3.org/TR/baggage/) is used, a **version 11 endpoint sending to a version 10 endpoint corrupts every baggage value by prepending a single space**. Version 11 writes baggage using the W3C optional-whitespace format (`key = value`), and the version 10 reader does not trim that whitespace from the value when parsing. The opposite direction (a version 10 endpoint sending to a version 11 endpoint) is not affected.

This only matters when both of the following are true:

- The application adds baggage to activities. Baggage is opt-in; endpoints that do not use it are unaffected, and `traceparent`/`tracestate` correlation works regardless.
- Version 10 and version 11 endpoints exchange messages during a rolling upgrade.

To avoid the problem, upgrade message **receivers before senders** so that no version 10 endpoint receives baggage produced by a version 11 endpoint.

#### Baggage

##### Whitespace in values

Contrary to version 10, version 11 of NServiceBus does not preserve leading or trailing whitespace in a baggage value. The W3C propagator treats such whitespace as insignificant optional whitespace and trims it when reading, whereas version 10 percent-encoded it. For example, a value of `" tenant"` is read back as `"tenant"`. This applies even when both endpoints run version 11. If exact leading or trailing whitespace must be retained, encode it into the value (for example, percent-encode it) before adding it to baggage and decode it after reading.

##### Empty values are no longer propagated

Version 10 preserved a baggage item that had an empty value: a header such as `key1=value1,key3=` was read back with `key3` present and set to an empty string. Version 11 discards baggage members that have an empty value when reading, so `key3` is not added to the activity at all. The propagator also stops parsing at the first empty-valued member, so members listed after it can be dropped as well.

This is the behavior of the underlying .NET `DistributedContextPropagator`, which on this point is stricter than the [W3C Baggage](https://www.w3.org/TR/baggage/) specification (the specification permits empty values). Avoid relying on empty or null baggage values; if an item only needs to signal presence, give it a non-empty value such as `true` or `1`. Note that a `null` and an empty baggage value are indistinguishable on the wire — both serialize to `key=` — so neither survives.

#### Trace state must conform to the W3C format

Version 10 copied the `tracestate` value onto outgoing messages verbatim, without validation. Version 11 validates `tracestate` against the [W3C Trace Context](https://www.w3.org/TR/trace-context/#tracestate-header) format and drops any content that does not conform. As a result, a non-conformant trace state set on an ambient activity — for example, free-form text such as `my custom state`, or a member whose key contains uppercase letters — is no longer propagated to the message spans.

To retain custom trace state, ensure it is a comma-separated list of `key=value` members with lowercase keys, for example `vendorkey=vendorvalue`. Values may contain mixed case; only keys are restricted to lowercase letters, digits, and `_`, `-`, `*`, `/`, `@`.
