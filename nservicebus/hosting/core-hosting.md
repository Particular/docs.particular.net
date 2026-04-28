---
title: Recommended hosting model
summary: Use AddNServiceBusEndpoint with Microsoft.Extensions.Hosting for the recommended single-endpoint and multi-endpoint hosting model.
component: Core
versions: '[10,)'
reviewed: 2026-04-27
related:
  - nservicebus/upgrades/10to11
  - samples/hosting/generic-host
---

Starting in NServiceBus version 10.2, the recommended way to host NServiceBus is through `Microsoft.Extensions.Hosting` by registering endpoints with `IServiceCollection` using `AddNServiceBusEndpoint`. This aligns endpoint startup, dependency injection, and logging with the standard .NET hosting model, and it supports both the common single-endpoint case and advanced scenarios that host multiple endpoints in one process.

## Choosing an integration path

NServiceBus provides two integration paths with `Microsoft.Extensions.Hosting`:

- **Built-in integration** (recommended): use `AddNServiceBusEndpoint` on `IServiceCollection`. Available starting in NServiceBus 10.2.
- **Package-based integration** (legacy): the [`NServiceBus.Extensions.Hosting`](https://www.nuget.org/packages/NServiceBus.Extensions.Hosting) package and its `UseNServiceBus` method are no longer the recommended path. Existing applications should plan to migrate; new applications should use the built-in integration.

For details on the `UseNServiceBus` approach, see [NServiceBus.Extensions.Hosting](/nservicebus/hosting/extensions-hosting.md).

## Hosting a single endpoint

Register the endpoint on the host's service collection. For new development, this is the recommended alternative to the `UseNServiceBus` pattern in `NServiceBus.Extensions.Hosting`.

snippet: AddNServiceBusEndpointSingle

The endpoint starts and stops with the host's lifecycle. `IMessageSession` is available in dependency injection for components that need to send or publish from outside a message handler.

For most applications, a single endpoint per process is the simplest place to start.

## Hosting multiple endpoints

NServiceBus also supports hosting multiple logical endpoints in one process. This is typically useful when a single-endpoint host is not the right fit. Common scenarios:

- Multi-tenant systems where each tenant requires an isolated endpoint.
- Partitioned throughput where each partition is an endpoint sharing a host.
- Co-located infrastructure endpoints that do not justify a separate process.

Compared to a single-endpoint host, each additional endpoint adds registration, startup overhead, and coordination within the shared process. Keep the configuration and endpoint identities explicit so the host remains easy to reason about.

Each endpoint is registered with its own `EndpointConfiguration`. Pass an identifier string as the second argument to distinguish them in dependency injection:

snippet: AddNServiceBusEndpointMulti

The endpoint name is the recommended identifier. A distinct identifier is only required when the same endpoint definition is hosted more than once with different per-instance configuration — for example, a per-tenant deployment where the endpoint name is composed at runtime:

snippet: AddNServiceBusEndpointPerTenant

In this case the endpoint name distinguishes each runtime instance for routing, while the tenant key serves as the DI identifier so callers can resolve a specific tenant's `IMessageSession` and keyed services.

### Endpoint-scoped dependencies

When a shared service needs its own per-endpoint instance, register it as a [keyed service](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#keyed-services) using the previously chosen (e.g. endpoint name) identifier as a key:

snippet: AddNServiceBusEndpointKeyedServices

Each endpoint resolves its own instance as keyed services using the previously chosen identifier as a key. Services that do not vary per endpoint are registered normally (non-keyed) on `IServiceCollection` and every endpoint resolves the same instance.

It is still possible to use `EndpointConfiguration.RegisterComponents` and the API will internally automatically use the right approach regardless whether a single endpoint or multiple endpoints are used. The API is obsoleted with a warning and it is recommended to migrate to explicit registrations shown above. For more migration guidance see the [NServiceBus 10 to 11 upgrade guide](/nservicebus/upgrades/10to11/).

## Endpoint identity and the DI identifier

- **Endpoint name** — the NServiceBus identity set on `EndpointConfiguration`. Drives the input queue, routing, diagnostics, and log context. Unique across the system.
- **Endpoint identifier** — the dependency-injection slot used to resolve this endpoint's `IMessageSession` and keyed dependencies. Scoped to a single process. Defaults to the endpoint name.

In a single-endpoint host, the two are the same value and the distinction does not matter. In a multi-endpoint host, each endpoint retains its own input queue, `IMessageSession`, and per-endpoint configuration.

## Logging

NServiceBus uses [`Microsoft.Extensions.Logging`](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/) as its built-in logging infrastructure. Log events flow through the host's configured `ILoggerFactory` with endpoint context attached. The `NServiceBus.Extensions.Logging` package is no longer required.

For migration from `LogManager.Use<T>`, `LogManager.UseFactory`, `DefaultFactory`, and `LogManager.GetLogger`, see the [Logging section of the NServiceBus 10 to 11 upgrade guide](/nservicebus/upgrades/10to11/).

## Migrating from self-hosted endpoints

Self-hosting an endpoint with `Endpoint.Create()` or `Endpoint.Start()` is deprecated, along with `IEndpointInstance`, `IStartableEndpoint`, and `NServiceBus.Installer`. `EndpointConfiguration.RegisterComponents` is also obsolete; dependency registrations now flow through the host's `IServiceCollection` directly, or through keyed services when per-endpoint scoping is required.

See the [NServiceBus 10 to 11 upgrade guide](/nservicebus/upgrades/10to11/) for the full migration surface.