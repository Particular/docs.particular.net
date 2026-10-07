---
title: Upgrade Version 10 to 11
summary: Instructions on how to upgrade NServiceBus from version 10 to version 11.
reviewed: 2026-06-26
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 10
 - 11
---

> [!TIP]
> This is an upgrade guide for a version of NServiceBus that has not yet been released. It can currently be used to proactively adjust for changes introduced as warnings, but which will not be required until NServiceBus version 11 is released.

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

This change avoids using MD5 for default host identifier generation, which prevents FIPS policy enforcement from blocking endpoint startup for this code path (see [FIPS compliance](/nservicebus/compliance/fips.md)). The legacy MD5-based algorithm is not appropriate for this non-cryptographic use case.

To ensure a predictable transition, this is designed as a multi-phase migration:

| NServiceBus Versions | Hashes Available | Default Hash | App Switch |
|:-:|:-:|:-:|:-:|
| <= 10.2 | MD5 Only | MD5 | - |
| >= 10.2 && < 11.0 | MD5 + XxHash128 | MD5 | Can opt in |
| >= 11.0 && < 12.0 | MD5 + XxHash128 | XxHash128 | Can opt out |
| >= 12.0 | XxHash128 Only | XxHash128 | - |


In version 11, XxHash128 becomes the default. The opt-out switch is intended as a temporary migration aid when operational dashboards, log queries, audit processing, or monitoring processes need more time to move from the legacy generated host identifiers to the new identifiers.

This approach allows the framework to move away from MD5-based host identifier generation while providing flexibility to manage existing integrations before the legacy algorithm is removed in version 12.

### Impact

After upgrading, endpoints that rely on the default generated host identifier will receive new host identifiers. This causes endpoints to appear as new entries in ServicePulse, while the previous instances become stale and must be [removed from the monitoring view](/monitoring/metrics/in-servicepulse.md#disconnected-endpoints-removing-disconnected-endpoints).

The changed host identifier also affects any custom logging, audit processing, dashboards, or queries that use generated host identifier headers, such as `$.diagnostics.hostid` or `$.diagnostics.originating.hostid`. Endpoint names, queues, message processing, and explicitly configured host identifiers are not affected.

ServiceControl stores and displays the resulting host identifier value. It does not know whether two different host identifiers were generated from the same endpoint path and machine name by different algorithms.

### Temporarily preserving the legacy generated host identifier

To temporarily preserve the existing MD5-based generated host identifier after upgrading to version 11, set the following AppContext switch before endpoint startup:

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

If an endpoint must keep a specific host identifier beyond version 11, configure the host identifier explicitly instead of relying on the legacy algorithm switch. For example, an endpoint can be configured with its existing host identifier to keep the value stable after the legacy algorithm is removed. See [Overriding the host identifier](/nservicebus/hosting/override-hostid.md).

## OpenTelemetry

Version 11 changes what the OpenTelemetry instrumentation of NServiceBus emits. Span names, the parent of the process span, some span attributes, the baggage wire format and one metric tag all change. These changes break dashboards, alerts and queries that match on the old output.

Version 10 keeps the version 10 output. Every change below is available on version 10 behind a single opt-in switch, so the new output can be adopted and verified before the upgrade to version 11.

### Adopting the version 11 behavior on version 10

Set the following AppContext switch before endpoint startup:

```csharp
AppContext.SetSwitch("NServiceBus.Core.OpenTelemetry.UseV11Behavior", true);
```

Or via environment variable:

```text
DOTNET_NServiceBus_Core_OpenTelemetry_UseV11Behavior=true
```

Or via MSBuild in the project file:

```xml
<ItemGroup>
  <RuntimeHostConfigurationOption Include="NServiceBus.Core.OpenTelemetry.UseV11Behavior" Value="true" />
</ItemGroup>
```

| NServiceBus version | Behaviors available | Default | App switch |
|:-:|:-:|:-:|:-:|
| 10.x before the switch | version 10 only | version 10 | - |
| 10.x with the switch | version 10 + version 11 | version 10 | Can opt in |
| >= 11.0 | version 11 only | version 11 | - |

The switch enables every change in this section at once. It cannot enable them one at a time, because version 11 does not offer them one at a time either. The switch is read once, before the endpoint starts, and applies to the whole process, so two endpoints in one process cannot use different modes.

> [!NOTE]
> Version 11 has no switch to restore the version 10 output. Verify the changes on version 10 with the switch enabled before upgrading.

### ActivitySources

In version 11, NServiceBus emits spans from three ActivitySources:

| Source | Spans |
|---|---|
| `NServiceBus.Core` | Pipeline spans (send, publish, process) |
| `NServiceBus.Core.Handler` | Handler invocation spans |
| `NServiceBus.Core.Recoverability` | Recoverability action spans |

In version 10, handler spans are emitted from `NServiceBus.Core`. In version 11, they are emitted from `NServiceBus.Core.Handler`.

Any OpenTelemetry configuration that only subscribes to `NServiceBus.Core` will no longer receive handler spans after upgrading. Update the tracer configuration to subscribe to all required sources:

```csharp
Sdk.CreateTracerProviderBuilder()
    .AddSource("NServiceBus.Core")
    .AddSource("NServiceBus.Core.Handler")
    .AddSource("NServiceBus.Core.Recoverability")
    // ...
    .Build();
```

#### ActivitySource version

The three sources report version `1.0.0` in version 11, instead of `0.1.0` in version 10. The version identifies which set of span names and attributes a span belongs to. Monitoring configuration that filters on the source version must be updated.

### Process span parent with instrumented transport SDKs

When a transport SDK, such as the Azure Service Bus, RabbitMQ, or Amazon SQS client, emits its own receive span and the endpoint subscribes to that ActivitySource, version 11 creates the NServiceBus process span as a child of the SDK receive span and links it to the NServiceBus send span. In version 10, the process span is a child of the NServiceBus send span regardless of the SDK span.

Dashboards or queries that assume the parent of the process span is the NServiceBus send span need updating for endpoints that subscribe to a transport SDK's ActivitySource. The send span remains reachable through the span link.

Endpoints that do not subscribe to a transport SDK's ActivitySource are not affected. Without a listener there is no SDK span, and the process span stays a child of the NServiceBus send span.

### Span names

In version 10, span names are generic, such as `process message` and `publish event`. In version 11 they follow the OpenTelemetry messaging semantic convention format `{operation} {target}`. The target is the queue for receive and recoverability spans, and the message type name for outgoing spans.

| Operation | Version 10 | Version 11 |
|---|---|---|
| Process | `process message` | `process {receiveAddress}` |
| Send | `send message` | `send message {MessageType}` |
| Publish | `publish event` | `publish {EventType}` |
| Reply | `reply` | `reply {MessageType}` |
| Subscribe | `subscribe event` | `subscribe event {EventType}` |
| Unsubscribe | `unsubscribe event` | `unsubscribe event {EventType}` |
| Immediate retry | `immediate retry` | `immediate retry {receiveAddress}` |
| Delayed retry | `delayed retry` | `delayed retry {receiveAddress}` |
| Move to error | `move to error` | `move to {errorQueue}` |
| Discard | `discard` | `discard` |

Message type names are short names, not full type names. A subscribe span for several event types lists the type names separated by spaces.

Alerts, dashboards and trace searches that match on a span name must be updated. A name match fails silently: the query returns no results instead of an error.

### Renamed span attributes

#### nservicebus.outbox.deduplicated_message

The outbox deduplication attribute is renamed from `nservicebus.outbox.deduplicate-message` to `nservicebus.outbox.deduplicated_message`. The [OpenTelemetry attribute naming rules](https://opentelemetry.io/docs/specs/semconv/general/naming/) ask for snake_case inside a dot-delimited name, and do not allow hyphens.

#### Array-valued type attributes

The `nservicebus.enclosed_message_types` attribute on message spans and the `nservicebus.event_types` attribute on subscribe and unsubscribe spans are arrays of type names in version 11. In version 10 they are single delimited strings.

> [!NOTE]
> An array-valued attribute is only visible through `Activity.TagObjects`. `Activity.Tags` returns string values only and skips these attributes. Custom enrichers, processors and tests that read `Activity.Tags` must move to `Activity.TagObjects`.

The `nservicebus.enclosed_message_types` *metric* tag is unchanged. It remains a single delimited string.

### Removed span attributes

#### otel.status_code and otel.status_description

Earlier versions of NServiceBus set `otel.status_code` and `otel.status_description` as explicit span attributes on failed spans, in addition to setting the span status via the OpenTelemetry API. These are NServiceBus-specific tags that predate reliable support for `Activity.SetStatus` in .NET. They are now redundant: the standard `Activity.SetStatus` call is the canonical way to convey span status, and exporters surface it correctly without these extra attributes.

In version 10, these attributes are still emitted for backward compatibility. They are removed in version 11.

If dashboards, alerts, or queries rely on `otel.status_code` or `otel.status_description` span attributes set by NServiceBus, migrate to using the span status provided by the OpenTelemetry exporter before upgrading to version 11.

#### exception.escaped

The `exception.escaped` attribute on exception span events is deprecated in the [OpenTelemetry semantic conventions](https://opentelemetry.io/docs/specs/semconv/attributes-registry/exception/). The spec notes that it is no longer recommended to record exceptions that are handled and do not escape the scope of a span.

In version 10, `exception.escaped` is still included in exception events for backward compatibility. It is removed in version 11.

### Removed span events

NServiceBus adds two span events to the incoming message pipeline span whenever outgoing messages are dispatched during message processing:

- `"Start dispatching"` - emitted before the outgoing messages are handed to the transport, with a `message-count` event tag indicating how many messages are being dispatched.
- `"Finished dispatching"` - emitted after the dispatch completes.

In version 10, these events are always emitted when OpenTelemetry instrumentation is enabled. In version 11, they are removed. There is no option to keep them. Consumers that read the events lose them; the dispatch itself is still visible through the outgoing message spans.

### Context propagation

In version 11, NServiceBus propagates the [W3C Trace Context](https://www.w3.org/TR/trace-context/) and [W3C Baggage](https://www.w3.org/TR/baggage/) using the built-in .NET `DistributedContextPropagator` instead of the custom propagation logic used in version 10. This aligns the on-the-wire format with the W3C specifications and improves interoperability with standard OpenTelemetry tooling and non-NServiceBus systems that participate in the same trace. See [OpenTelemetry](/nservicebus/operations/opentelemetry.md) for an overview of the feature.

#### Trace correlation is unaffected

The `traceparent` and `tracestate` headers continue to be emitted in the W3C format. Distributed traces still correlate correctly between version 10 and version 11 endpoints in both directions, so upgrading does not break trace continuity.

#### Baggage serialization change

The change affects how the `baggage` header is serialized on the wire:

- Version 10 emitted a compact form with no optional whitespace (`key1=value1,key2=value2`) and percent-encoded baggage values aggressively.
- Version 11 emits the W3C form with optional whitespace around the delimiters (`key1 = value1, key2 = value2`) and percent-encodes only the characters that are structurally significant (such as `,`, `;`, and `%`).

Both versions decode percent-encoding when reading, so a baggage value written by one version is generally decoded correctly by the other - with the exception described below.

#### Mixed-version incompatibility

> [!WARNING]
> When [baggage](https://www.w3.org/TR/baggage/) is used, a **version 11 endpoint sending to a version 10 endpoint corrupts every baggage value by prepending a single space**. Version 11 writes baggage using the W3C optional-whitespace format (`key = value`), and the version 10 reader does not trim that whitespace from the value when parsing. The opposite direction (a version 10 endpoint sending to a version 11 endpoint) is not affected.

This only matters when both of the following are true:

- The application adds baggage to activities. Baggage is opt-in; endpoints that do not use it are unaffected, and `traceparent`/`tracestate` correlation works regardless.
- Version 10 and version 11 endpoints exchange messages during a rolling upgrade.

To avoid the problem, upgrade message **receivers before senders** so that no version 10 endpoint receives baggage produced by a version 11 endpoint.

The same applies on version 10 to an endpoint that has the `UseV11Behavior` switch enabled. Such an endpoint writes the version 11 baggage format, so enable the switch on receivers before senders.

#### Baggage

##### Whitespace in values

Contrary to version 10, version 11 of NServiceBus does not preserve leading or trailing whitespace in a baggage value. The W3C propagator treats such whitespace as insignificant optional whitespace and trims it when reading, whereas version 10 percent-encoded it. For example, a value of `" tenant"` is read back as `"tenant"`. This applies even when both endpoints run version 11. If exact leading or trailing whitespace must be retained, encode it into the value (for example, percent-encode it) before adding it to baggage and decode it after reading.

##### Empty values are no longer propagated

Version 10 preserved a baggage item that had an empty value: a header such as `key1=value1,key3=` was read back with `key3` present and set to an empty string. Version 11 discards baggage members that have an empty value when reading, so `key3` is not added to the activity at all. The propagator also stops parsing at the first empty-valued member, so members listed after it can be dropped as well.

This is the behavior of the underlying .NET `DistributedContextPropagator`, which on this point is stricter than the [W3C Baggage](https://www.w3.org/TR/baggage/) specification (the specification permits empty values). Avoid relying on empty or null baggage values; if an item only needs to signal presence, give it a non-empty value such as `true` or `1`. Note that a `null` and an empty baggage value are indistinguishable on the wire - both serialize to `key=` - so neither survives.

#### Trace state must conform to the W3C format

Version 10 copied the `tracestate` value onto outgoing messages verbatim, without validation. Version 11 validates `tracestate` against the [W3C Trace Context](https://www.w3.org/TR/trace-context/#tracestate-header) format and drops any content that does not conform. As a result, a non-conformant trace state set on an ambient activity - for example, free-form text such as `my custom state`, or a member whose key contains uppercase letters - is no longer propagated to the message spans.

To retain custom trace state, ensure it is a comma-separated list of `key=value` members with lowercase keys, for example `vendorkey=vendorvalue`. Values may contain mixed case; only keys are restricted to lowercase letters, digits, and `_`, `-`, `*`, `/`, `@`.

### Metrics

#### New performance metrics

Version 11 adds six new histograms to the `NServiceBus.Core.Pipeline.Incoming` meter source:

| Metric | Description |
|---|---|
| `nservicebus.messaging.deserialize_time` | Time to deserialize an incoming message |
| `nservicebus.messaging.serialize_time` | Time to serialize an outgoing message |
| `nservicebus.sagas.fetch_time` | Time to load saga data from the persister |
| `nservicebus.outbox.fetch_time` | Time to query outbox storage for deduplication |
| `nservicebus.outbox.store_time` | Time to store a message in outbox storage |
| `nservicebus.persistence.commit_time` | Time to complete the synchronized storage session |

These metrics are emitted automatically when the meter source is subscribed. No additional configuration is required. See [OpenTelemetry metrics](/nservicebus/operations/opentelemetry.md#meters-emitted-meters) for the full tag reference.

#### Meter source version

The `NServiceBus.Core.Pipeline.Incoming` meter source version has been updated to `0.4.0`. If any monitoring configuration references this version string explicitly, update it accordingly.

#### execution.result tag is removed

The `execution.result` tag, which carries `"success"` or `"failure"`, is removed in version 11. There is no option to keep it. In version 10 it is emitted on `nservicebus.messaging.successes`, `nservicebus.messaging.failures`, `nservicebus.messaging.processing_time`, `nservicebus.messaging.critical_time`, `nservicebus.messaging.handler_time`, `nservicebus.messaging.deserialize_time`, `nservicebus.messaging.serialize_time`, and `nservicebus.sagas.fetch_time`.

The tag duplicated information that is already available: `nservicebus.messaging.successes` and `nservicebus.messaging.failures` are separate instruments, and the histograms carry `error.type` when the operation fails. Removing the tag also lowers metric cardinality and ingestion cost.

Update any dashboard or alert that groups or filters on `execution.result`. Use the separate success and failure instruments, or `error.type`, instead.
