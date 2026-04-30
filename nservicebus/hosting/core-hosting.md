---
title: Hosting with Microsoft.Extensions.Hosting
summary: Host NServiceBus endpoints with Microsoft.Extensions.Hosting using AddNServiceBusEndpoint, for both single-endpoint and multi-endpoint scenarios.
component: Core
versions: '[10,)'
reviewed: 2026-04-27
related:
  - nservicebus/upgrades/10to11
  - samples/hosting/generic-host
---

NServiceBus endpoints are hosted with [`Microsoft.Extensions.Hosting`](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host) by registering them on `IServiceCollection` using `AddNServiceBusEndpoint`. Endpoint startup, dependency injection, and logging align with the standard .NET hosting model, and the same registration approach supports both single-endpoint and multi-endpoint hosts.

> [!NOTE]
> Earlier versions of NServiceBus relied on the [`NServiceBus.Extensions.Hosting`](https://www.nuget.org/packages/NServiceBus.Extensions.Hosting) package and the `UseNServiceBus` extension method on `IHostBuilder`. That package is still supported in NServiceBus 10 but is no longer the recommended approach for new development. For details, see [NServiceBus.Extensions.Hosting](/nservicebus/hosting/extensions-hosting.md).

## Hosting a single endpoint

Register the endpoint on the host's service collection:

snippet: AddNServiceBusEndpointSingle

The endpoint starts and stops with the host's lifecycle. `IMessageSession` is registered on the host's `IServiceCollection` and can be resolved from the service provider:

snippet: AddNServiceBusEndpointGetSession

Or injected into controllers, background services, or any other component that needs to send or publish from outside a message handler or other NServiceBus extension point:

snippet: AddNServiceBusEndpointInjectSession

For most applications, a single endpoint per process is the simplest place to start.

## Hosting multiple endpoints

NServiceBus also supports hosting multiple logical endpoints in one process. Common scenarios:

- Multi-tenant systems where each tenant requires an isolated endpoint.
- Modular monoliths where each module owns its own endpoint within a shared host.
- Partitioned throughput where each partition is an endpoint sharing a host.
- Co-located infrastructure endpoints that do not justify a separate process.

Compared to a single-endpoint host, each additional endpoint adds registration, startup overhead, and coordination within the shared process.

Each endpoint is registered with its own `EndpointConfiguration`. The second argument to `AddNServiceBusEndpoint` is the DI identifier — a [service key](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#keyed-services) used to distinguish that endpoint's `IMessageSession` and per-endpoint keyed services:

snippet: AddNServiceBusEndpointMulti

The endpoint name is the recommended DI identifier. A distinct identifier is only required when the same endpoint definition is hosted more than once with different per-instance configuration — for example, a per-tenant deployment where each tenant reads from its own input queue:

snippet: AddNServiceBusEndpointPerTenant

Each tenant shares the `Sales` endpoint name but [overrides its local address](/nservicebus/endpoints/specify-endpoint-name.md#input-queue) for queue isolation. The tenant key serves as the DI identifier so callers can resolve a specific tenant's `IMessageSession` and keyed services.

### Endpoint-scoped dependencies

When each endpoint requires a different implementation of a shared service, register it as a [keyed service](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#keyed-services) using the chosen DI identifier (typically the endpoint name) as the key:

snippet: AddNServiceBusEndpointKeyedServices

Each endpoint resolves its own `DatabaseService` instance as keyed services using the previously chosen identifier as a key. Services that do not vary per endpoint are registered normally (non-keyed) on `IServiceCollection` and every endpoint resolves the same instance.

### Resolving services per endpoint

Inside NServiceBus extension points — message handlers, sagas, features, and installers — per-endpoint services resolve automatically from the right scope. `[FromKeyedServices]` is not needed in those contexts.

Outside extension points (for example, controllers, background services, or any host-level component that needs to send or publish), use `GetRequiredKeyedService<T>(identifier)` from the service provider or `[FromKeyedServices(identifier)]` for constructor injection:

snippet: AddNServiceBusEndpointGetKeyedSession

snippet: AddNServiceBusEndpointInjectKeyedSession

Global (non-keyed) services and per-endpoint keyed services can be mixed in the same constructor:

snippet: AddNServiceBusEndpointInjectMixed

## Understanding endpoint identity

Two identifiers describe an endpoint:

|                | Endpoint name                                       | DI identifier                                                              |
| -------------- | --------------------------------------------------- | -------------------------------------------------------------------------- |
| Identifies     | The logical endpoint (queue, routing, log context)  | The DI registration for `IMessageSession` and per-endpoint keyed services  |
| Set via        | `new EndpointConfiguration(name)`                   | Second argument to `AddNServiceBusEndpoint(config, identifier)`            |
| Required       | Always                                              | When more than one endpoint is registered on the same `IServiceCollection` |
| Unique across  | Logical endpoints in the system                     | Endpoint registrations in the process                                      |

When a DI identifier is needed, the endpoint name is the recommended value. A different value is only needed when the same endpoint definition is hosted more than once in one process — for example, a per-tenant deployment. See [Hosting multiple endpoints](#hosting-multiple-endpoints) for the registration patterns.

## Logging

NServiceBus uses [`Microsoft.Extensions.Logging`](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/) as its built-in logging infrastructure. Log events flow through the host's configured `ILoggerFactory` with endpoint context attached. The `NServiceBus.Extensions.Logging` package is no longer required.