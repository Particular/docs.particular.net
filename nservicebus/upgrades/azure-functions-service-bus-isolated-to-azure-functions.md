---
title: Migrate Azure Functions (Isolated Worker) to new package
summary: How to migrate from NServiceBus.AzureFunctions.Worker.ServiceBus to NServiceBus.AzureFunctions.AzureServiceBus
component: AzureFunctions
reviewed: 2026-05-25
related:
  - nservicebus/hosting/azure/functions
  - nservicebus/hosting/azure-functions-service-bus
  - nservicebus/hosting/azure-functions-service-bus/custom-triggers
isUpgradeGuide: true
---

This guide explains how to migrate from `NServiceBus.AzureFunctions.Worker.ServiceBus` to `NServiceBus.AzureFunctions.AzureServiceBus`.

The most important change is the hosting model:

- The old worker package supports a single NServiceBus endpoint per Azure Functions project.
- The new package uses explicit endpoint declarations and can host multiple receiving and send-only endpoints in the same function app.

For most migrations, the safest approach is to keep the current behavior first:

1. Migrate the existing endpoint as a single endpoint in the new package.
2. Keep the same queue names and routing where possible.

## Before you start

Before changing code, identify:

- the current endpoint name and queue name
- any HTTP, timer, or other Azure Functions that send or publish messages
- any use of `IFunctionEndpoint`
- any custom trigger setup
- any transport, routing, recoverability, audit, or diagnostics customizations
- whether the endpoint relied on default serializer behavior

## Update the project file

Replace old package references with the new package:

```xml
<ItemGroup>
    <!-- before -->
    <PackageReference Include="NServiceBus.AzureFunctions.Worker.ServiceBus" Version="<old-version>" />
    <PackageReference Include="NServiceBus.Transport.AzureServiceBus" Version="<old-version>" />

    <!-- after -->
    <PackageReference Include="NServiceBus.AzureFunctions.AzureServiceBus" Version="<version>" />
</ItemGroup>
```

If preferred, model this as a remove/add change:

```xml
<ItemGroup>
    <PackageReference Remove="NServiceBus.AzureFunctions.Worker.ServiceBus" />
    <PackageReference Remove="NServiceBus.Transport.AzureServiceBus" />
  <PackageReference Include="NServiceBus.AzureFunctions.AzureServiceBus" Version="<version>" />
</ItemGroup>
```

In practice:

- Remove `NServiceBus.AzureFunctions.Worker.ServiceBus`.
- Remove `NServiceBus.Transport.AzureServiceBus` if it is still referenced explicitly from the old setup.
- Add `NServiceBus.AzureFunctions.AzureServiceBus`.

Do not add `NServiceBus.AzureFunctions.Common` directly. The Azure Service Bus package is the user-facing package for this migration.

A direct `NServiceBus` package reference is optional. Keep it only when intentionally pinning the NServiceBus Core version separately.

## Update host startup

The old package is configured with `builder.AddNServiceBus(...)` and an assembly-level `NServiceBusTriggerFunction` attribute, for example `[assembly: NServiceBusTriggerFunction("Sales")]`.

The new package registers the Azure Functions integration once at startup:

```csharp
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.AddNServiceBusFunctions();

var host = builder.Build();

await host.RunAsync();
```

Remove the `[assembly: NServiceBusTriggerFunction(...)]` attribute. The new package does not use the old single-endpoint trigger generation model.

## Migrate the receiving endpoint

With the old package, a project maps to a single endpoint, and the queue-triggered function is generated from `NServiceBusTriggerFunction`, for example, `[assembly: NServiceBusTriggerFunction("Sales")]`.

With the new package, the receiving endpoint is declared explicitly in code. A minimal one-to-one migration looks like this:

### Select the transport topology explicitly

In the old worker package, the effective Azure Service Bus topology could be configured through the worker integration configuration. In the new package, topology selection is explicit in the transport instance passed to `UseTransport(...)`.

When migrating, select the same topology that the endpoint used before the migration so that queue, topic, and subscription behavior remains consistent. For details, see [Topology configuration](/nservicebus/hosting/azure-functions-service-bus/#preparing-the-azure-service-bus-namespace-topology-configuration).

Trigger queue names and connection setting names can continue to use [Azure Functions binding expressions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-expressions-patterns) such as `%BillingPrefix%-api`.

Serialization must now be configured explicitly in endpoint configuration.
For migrations from `NServiceBus.AzureFunctions.Worker.ServiceBus`, `SystemJsonSerializer` preserves the old default serializer behavior.

```csharp
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

[NServiceBusFunction]
public partial class SalesEndpoint
{
    [Function("Sales")]
    public partial Task Sales(
        [ServiceBusTrigger("sales", Connection = "AzureWebJobsServiceBus", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureSales(EndpointConfiguration configuration)
    {
        // Azure Functions endpoints must use AzureServiceBusServerlessTransport.
        configuration.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));

        // Serializer must be set explicitly
        configuration.UseSerialization<SystemJsonSerializer>();

        // Handlers must be added explicitly because assembly scanning is not available.
        configuration.AddHandler<SubmitOrderHandler>();
    }
}
```

The endpoint method, and any containing class, must be declared `partial` so the source generator can emit the trigger body.

For additional registration approaches, see [Explicit handler and saga registration](/nservicebus/hosting/azure/functions#explicit-handler-and-saga-registration).

## Move endpoint configuration next to the endpoint

The old worker package centralizes configuration in `builder.AddNServiceBus(configuration => { ... })`.

The new package moves endpoint-specific configuration into a static `Configure<FunctionName>` method next to the endpoint. The method always takes `EndpointConfiguration` and can also take `IServiceCollection`, `IConfiguration`, and `IHostEnvironment` as needed.

For the full configure-method model and parameter options, see [The configure method](/nservicebus/hosting/azure/functions#the-configure-method).

## Migrate usages of IFunctionEndpoint

With the old worker package, Azure Functions that send messages outside handlers typically inject `IFunctionEndpoint`.

With the new package, inject `IMessageSession` via [.NET keyed services](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#keyed-services).

There are two valid migration patterns.

### Pattern 1: Reuse the receiving endpoint session

For one-to-one migrations, it is often simplest to inject the keyed session of the receiving endpoint:

```csharp
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

class SalesApi([FromKeyedServices("Sales")] IMessageSession session)
{
    [Function("SalesApi")]
    public async Task Run()
    {
        await session.Send(new SubmitOrder());
    }
}
```

### Pattern 2: Add an explicit send-only endpoint

Use this pattern when send-only traffic should be isolated from the receiving endpoint's configuration, queue identity, or routing policy.

Register the send-only endpoint in `Program.cs`, then inject the keyed `IMessageSession` into the sending function. Use the `AddSendOnlyNServiceBusEndpoint` overload that also takes `IServiceCollection` when endpoint-specific services need to be registered and later resolved via keyed services.

For the send-only registration patterns and keyed-service examples, see [Send-only endpoints](/nservicebus/hosting/azure/functions#send-only-endpoints).

The key used in `[FromKeyedServices("client")]` must match the name passed to `AddSendOnlyNServiceBusEndpoint("client", ...)`.

For pattern 1, the key used in `[FromKeyedServices("Sales")]` must match the receiving endpoint name.

## Migrate custom trigger scenarios

The migration depends on what "custom trigger" means in the existing app.

### Custom Azure Functions that send messages

If the app has HTTP, timer, or other Azure Functions that send messages, keep those functions as normal Azure Functions and migrate them to use a send-only endpoint plus `IMessageSession`.

### Manually declared Service Bus trigger functions

If the app manually declares the Service Bus trigger instead of using the generated trigger, move that code to an explicit endpoint method marked with `[NServiceBusFunction]`.

The new package no longer relies on `NServiceBusTriggerFunction` for this scenario. The `ServiceBusTrigger(...)` definition is part of the endpoint method itself.

## Recoverability

The old worker package exposed `DoNotSendMessagesToErrorQueue()` as the way to stop forwarding failed messages to the error queue and let Azure Service Bus dead-lettering handle them instead. See the [old error queue documentation](/nservicebus/hosting/azure-functions-service-bus/#configuration-error-queue).

In the new package, use the [explicit dead-letter support](/transports/azure-service-bus/configuration.md#dead-lettering).

```csharp
configuration.Recoverability()
    .MoveErrorsToAzureServiceBusDeadLetterQueue();
```

> [!NOTE]
> The new package automatically enables [DLQ forwarding](/transports/azure-service-bus/configuration.md#dead-lettering-forward-dead-lettered-messages-to-the-error-queue) to allow dead-lettered messages to be managed by the platform.

## Host ID

The old package used the `WEBSITE_SITE_NAME` environment variable as the NServiceBus Host ID. This resulted in instances not being visible in ServicePulse as the function was scaled up or down. In the new package, [`WEBSITE_INSTANCE_ID`](https://learn.microsoft.com/en-us/azure/app-service/reference-app-settings?tabs=kudu%2Cdotnet#scaling) is used to ensure that all instances are identifiable.

> [!NOTE]
> It's recommended to [configure ServicePulse not to track heartbeats for these instances](/monitoring/heartbeats/in-servicepulse.md#configuration-do-not-track-instances) to avoid false negatives.

The logic falls back to [`CONTAINER_NAME` when running in Azure container apps](https://learn.microsoft.com/en-us/azure/container-apps/environment-variables?tabs=portal#apps) and `Environment.MachineName` for local development.

If needed, use the guidance in [Overriding the host identifier](/nservicebus/hosting/override-hostid.md) to take full control over the host ID and keep it stable across restarts and deployments.

## Recommended migration sequence

1. Remove `NServiceBus.AzureFunctions.Worker.ServiceBus` and add `NServiceBus.AzureFunctions.AzureServiceBus`.
2. Replace `builder.AddNServiceBus(...)` with `builder.AddNServiceBusFunctions()`.
3. Remove `NServiceBusTriggerFunction`.
4. Migrate the existing receiving endpoint to an explicit `[NServiceBusFunction]` endpoint method.
5. Move endpoint configuration to `Configure<FunctionName>` methods.
6. Replace any `IFunctionEndpoint` usage with keyed `IMessageSession` (either from the receiving endpoint or from an explicit send-only endpoint).
7. Configure serialization explicitly (typically `UseSerialization<SystemJsonSerializer>()` for worker-package parity).
8. Verify queue names, routing, recoverability, and diagnostics behavior.

## Multiple endpoints in one function app

The new package can host multiple endpoints in one function app, but whether that is a good next step depends heavily on the business scenario.

For most teams, it is better to:

1. complete a one-to-one migration first
2. validate routing, recoverability, and operational behavior
3. consider splitting the app into multiple endpoints as a separate change
