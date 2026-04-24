---
title: Hosting endpoints with AddNServiceBusEndpoint
summary: Register NServiceBus endpoints on IServiceCollection using the built-in Core hosting model.
component: Core
reviewed: 2026-04-24
related:
 - nservicebus/upgrades/10to11/index
 - samples/hosting/generic-host
---

NServiceBus Core integrates with `Microsoft.Extensions.Hosting` through the `AddNServiceBusEndpoint` extension on `IServiceCollection`. It is the entry point for both single-endpoint hosting and hosting multiple isolated endpoints in one process.

## Hosting a single endpoint

Register the endpoint on the host's service collection. This is the direct replacement for the `UseNServiceBus` pattern in `NServiceBus.Extensions.Hosting`.

```csharp
var builder = Host.CreateApplicationBuilder();

var endpointConfiguration = new EndpointConfiguration("Sales");
// configure transport, persistence, etc.

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

var host = builder.Build();
await host.RunAsync();
```

The endpoint starts and stops with the host's lifecycle. `IMessageSession` is available in dependency injection for components that need to send or publish from outside a message handler.

## Hosting multiple endpoints

Multi-endpoint hosting is used when multiple logical endpoints run in one process. Each additional endpoint adds registration and configuration the application must manage. Common scenarios:

- Multi-tenant systems where each tenant requires an isolated endpoint.
- Partitioned throughput where each partition is an endpoint sharing a host.
- Co-located infrastructure endpoints that do not justify a separate process.

Before moving from one endpoint to multiple endpoints in the same process, verify that the operational tradeoff is intentional:

- Each endpoint adds startup and registration overhead, so standardize configuration early.
- Adopt a clear endpoint identity convention to keep multi-endpoint setups easy to reason about.
- Operations remain endpoint-centric, but deployment and resource isolation are now process-shared.

Each endpoint is registered with its own `EndpointConfiguration`. Pass an identifier string as the second argument to distinguish them in dependency injection:

```csharp
var salesConfig = new EndpointConfiguration("Sales");
var billingConfig = new EndpointConfiguration("Billing");

builder.Services.AddNServiceBusEndpoint(salesConfig, "Sales");
builder.Services.AddNServiceBusEndpoint(billingConfig, "Billing");
```

The endpoint name is the recommended identifier. A distinct identifier is only required when the same endpoint definition is hosted more than once with different per-instance configuration — for example, a per-tenant deployment where the endpoint name is composed at runtime.

### Endpoint-scoped dependencies

When a shared service needs its own per-endpoint instance, register it as a [keyed dependency](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#keyed-services) using the endpoint name as the key:

```csharp
var salesDb = new DatabaseService("sales-db");
var billingDb = new DatabaseService("billing-db");

builder.Services.AddKeyedSingleton<DatabaseService>(salesConfig.EndpointName, salesDb);
builder.Services.AddKeyedSingleton<DatabaseService>(billingConfig.EndpointName, billingDb);

builder.Services.AddNServiceBusEndpoint(salesConfig, "Sales");
builder.Services.AddNServiceBusEndpoint(billingConfig, "Billing");
```

Each endpoint resolves its own instance from the keyed slot. Services that do not vary per endpoint are registered normally on `IServiceCollection` and every endpoint resolves the same instance.

For the migration path from `EndpointConfiguration.RegisterComponents`, see the [NServiceBus 10 to 11 upgrade guide](/nservicebus/upgrades/10to11/index.md).

## Endpoint identity and the DI identifier

- **Endpoint name** — the NServiceBus identity set on `EndpointConfiguration`. Drives the input queue, routing, diagnostics, and log context. Unique across the system.
- **Endpoint identifier** — the dependency-injection slot used to resolve this endpoint's `IMessageSession` and keyed dependencies. Scoped to a single process. Defaults to the endpoint name.

In a single-endpoint host, the two are the same value and the distinction does not matter. In a multi-endpoint host, each endpoint retains its own input queue, `IMessageSession`, and per-endpoint configuration.

## Logging

Core uses [`Microsoft.Extensions.Logging`](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/) as its built-in logging infrastructure. Log events flow through the host's configured `ILoggerFactory` with endpoint context attached. The `NServiceBus.Extensions.Logging` package is no longer required.

For migration from `LogManager.Use<T>`, `LogManager.UseFactory`, `DefaultFactory`, and `LogManager.GetLogger`, see the [Logging section of the NServiceBus 10 to 11 upgrade guide](/nservicebus/upgrades/10to11/index.md).

## Migrating from self-hosted endpoints

Self-hosting an endpoint with `Endpoint.Create()` or `Endpoint.Start()` is deprecated, along with `IEndpointInstance`, `IStartableEndpoint`, and `NServiceBus.Installer`. `EndpointConfiguration.RegisterComponents` is also obsolete; dependency registrations now flow through the host's `IServiceCollection` directly, or through keyed services when per-endpoint scoping is required.

See the [NServiceBus 10 to 11 upgrade guide](/nservicebus/upgrades/10to11/index.md) for the full migration surface.