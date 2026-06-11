---
title: Migrate Azure Functions (Isolated Worker) to multi-endpoint package
summary: How to migrate from NServiceBus.AzureFunctions.Worker.ServiceBus to NServiceBus.AzureFunctions.AzureServiceBus
component: AzureFunctions
reviewed: 2026-06-04
related:
  - nservicebus/hosting/azure/functions
  - nservicebus/hosting/azure-functions-service-bus
  - nservicebus/hosting/azure-functions-service-bus/custom-triggers
isUpgradeGuide: true
---

This guide explains how to migrate from `NServiceBus.AzureFunctions.Worker.ServiceBus` to `NServiceBus.AzureFunctions.AzureServiceBus`.

The most important change is the hosting model:

- The previous worker package supports a single NServiceBus endpoint per Azure Functions project.
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
    <PackageReference Include="NServiceBus.AzureFunctions.AzureServiceBus" Version="<latest-version>" />
    <PackageReference Include="NServiceBus.Transport.AzureServiceBus" Version="<latest-version>" />
</ItemGroup>
```

In practice:

- Remove `NServiceBus.AzureFunctions.Worker.ServiceBus`.
- Add `NServiceBus.AzureFunctions.AzureServiceBus`.

Do not add `NServiceBus.AzureFunctions.Common` directly. The Azure Service Bus package is the user-facing package for this migration.

## Update host startup

The previous package is configured with `builder.AddNServiceBus(…)` and an assembly-level `NServiceBusTriggerFunction` attribute, for example `[assembly: NServiceBusTriggerFunction("Sales")]`.

The new package registers the Azure Functions integration once at startup:

```csharp
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.AddNServiceBusFunctions();

var host = builder.Build();

await host.RunAsync();
```

Remove the `[assembly: NServiceBusTriggerFunction(…)]` attribute. The new package does not use the previous single-endpoint trigger generation model.

## Migrate the receiving endpoint

With the previous package, a project maps to a single endpoint, and the queue-triggered function is generated from `NServiceBusTriggerFunction`, for example, `[assembly: NServiceBusTriggerFunction("Sales")]`.

With the new package, the receiving endpoint is declared explicitly in code. A minimal one-to-one migration looks like this:

```csharp
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using NServiceBus;

public partial class SalesEndpoint
{
    [Function("Sales")]
    [NServiceBusFunction]
    public partial Task Sales(
        [ServiceBusTrigger("sales", AutoCompleteMessages = false)]
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

The endpoint method must be marked with `[NServiceBusFunction]`, and both the method and its containing class must be declared `partial` so the source generator can emit the trigger body.

For the new package's queue-name and connection-setting behavior, see [Connection configuration](/nservicebus/hosting/azure/functions/configuration.md#connection-configuration) and [Queue name resolution](/nservicebus/hosting/azure/functions/configuration.md#queue-name-resolution).

### Move endpoint configuration to the configure method

The previous worker package centralizes configuration in `builder.AddNServiceBus(configuration => { … })`.

The new package moves endpoint-specific configuration into a static `Configure<FunctionName>` method next to the endpoint. The method always takes `EndpointConfiguration` and can also take `IServiceCollection`, `IConfiguration`, and `IHostEnvironment` as needed.

For the full configure-method model and parameter options, see [The configure method](/nservicebus/hosting/azure/functions/configuration.md#the-configure-method).

### Handlers and sagas

Due to assembly scanning not being available message handlers and sagas needs to be registered explicitly using `configuration.AddHandler`, `configuration.AddSaga` or `configuration.Handlers`.

For additional registration approaches, see [Explicit handler and saga registration](/nservicebus/hosting/azure/functions/configuration.md#explicit-handler-and-saga-registration).

### Serialization

Serialization must now be configured explicitly in endpoint configuration.
For migrations from `NServiceBus.AzureFunctions.Worker.ServiceBus`, `SystemJsonSerializer` preserves the previous default serializer behavior.

### Select the transport topology explicitly

In the previous worker package, the effective Azure Service Bus topology could be configured through the worker integration configuration. In the new package, topology selection is explicit in the transport instance passed to `UseTransport(…)`.

When migrating, select the same topology that the endpoint used before the migration so that queue, topic, and subscription behavior remains consistent. For the supported transport settings in this hosting model, see [Transport configuration](/nservicebus/hosting/azure/functions/configuration.md#transport-configuration). For topology details, see [Azure Service Bus topology](/transports/azure-service-bus/topology.md).

## Migrate usages of IFunctionEndpoint

With the previous worker package, Azure Functions that send messages outside handlers typically inject `IFunctionEndpoint`.

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

Declare the send-only endpoint using a static `Configure{EndpointName}` method decorated with `[NServiceBusSendOnlyFunction("client")]`, then inject the keyed `IMessageSession` into the sending function. Add `IServiceCollection` to the configure method signature when endpoint-specific services need to be registered and later resolved via keyed services.

For the send-only declaration pattern and keyed-service examples, see [Send-only endpoints](/nservicebus/hosting/azure/functions#send-only-endpoints). For connection-setting behavior, see [Connection configuration](/nservicebus/hosting/azure/functions/configuration.md#connection-configuration).

The key used in `[FromKeyedServices("client")]` must match the name passed to `[NServiceBusSendOnlyFunction("client")]`.

## Migrate custom trigger scenarios

The migration depends on what "custom trigger" means in the existing app.

### Custom Azure Functions that send messages

If the app has HTTP, timer, or other Azure Functions that send messages, keep those functions as normal Azure Functions and migrate them to use a send-only endpoint plus `IMessageSession`.

### Manually declared Service Bus trigger functions

If the app manually declares the Service Bus trigger instead of using the generated trigger, move that code to an explicit endpoint method with `[NServiceBusFunction]` on the method.

The new package no longer relies on `NServiceBusTriggerFunction` for this scenario. The `ServiceBusTrigger(…)` definition is part of the endpoint method itself.

## Recoverability

The previous worker package exposed `DoNotSendMessagesToErrorQueue()` as the way to stop forwarding failed messages to the error queue and let Azure Service Bus dead-lettering handle them instead. See the [previous error queue documentation](/nservicebus/hosting/azure-functions-service-bus/#configuration-error-queue).

In the new package, use the [explicit dead-letter support](/transports/azure-service-bus/configuration.md#dead-lettering). For the hosting-model-specific defaults, see [Recoverability](/nservicebus/hosting/azure/functions/configuration.md#recoverability).

```csharp
configuration.Recoverability()
    .MoveErrorsToAzureServiceBusDeadLetterQueue();
```

> [!NOTE]
> The new package automatically enables [DLQ forwarding](/transports/azure-service-bus/configuration.md#dead-lettering-forward-dead-lettered-messages-to-the-error-queue) to allow dead-lettered messages to be managed by the platform.

## Host ID

The previous package used `WEBSITE_SITE_NAME` as the NServiceBus Host ID. The new package uses a different Host ID strategy that is better suited to scaled-out and containerized deployments.

For the new package behavior and override guidance, see [Host ID](/nservicebus/hosting/azure/functions/configuration.md#host-id).

## Recommended migration sequence

1. Remove `NServiceBus.AzureFunctions.Worker.ServiceBus` and add `NServiceBus.AzureFunctions.AzureServiceBus`.
2. Replace `builder.AddNServiceBus(…)` with `builder.AddNServiceBusFunctions()`.
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
