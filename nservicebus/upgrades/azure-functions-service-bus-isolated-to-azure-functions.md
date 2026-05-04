---
title: Migrating Azure Functions (Isolated Worker) with Service Bus to the new Azure Functions package
summary: How to migrate from NServiceBus.AzureFunctions.Worker.ServiceBus to NServiceBus.AzureFunctions.AzureServiceBus
component: AzureFunctions
reviewed: 2026-04-28
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

## Update the project file

Remove the old package reference and add the new package:

```xml
<ItemGroup>
  <PackageReference Include="NServiceBus.AzureFunctions.AzureServiceBus" Version="<version>" />
</ItemGroup>
```

Do not add `NServiceBus.AzureFunctions.Common` directly. The Azure Service Bus package is the user-facing package for this migration.

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

With the old package, a project maps to a single endpoint and the queue-triggered function is generated from `NServiceBusTriggerFunction`, for example `[assembly: NServiceBusTriggerFunction("Sales")]`.

With the new package, the receiving endpoint is declared explicitly in code. A minimal one-to-one migration looks like this:

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
        configuration.UseSerialization<SystemJsonSerializer>();

        // Handlers must be added explicitly because assembly scanning is not available.
        configuration.AddHandler<SubmitOrderHandler>();
    }
}
```

The endpoint method, and any containing class, must be declared `partial` so the source generator can emit the trigger body.

## Move endpoint configuration next to the endpoint

The old worker package centralizes configuration in `builder.AddNServiceBus(configuration => { ... })`.

The new package moves endpoint-specific configuration into a static `Configure<FunctionName>` method next to the endpoint. The configure method always takes `EndpointConfiguration` and can also take these optional parameters:

- `IServiceCollection`
- `IConfiguration`
- `IHostEnvironment`

This makes it possible to keep shared settings in one place while still applying endpoint-specific behavior:

```csharp
using Microsoft.Extensions.DependencyInjection;

public static void ConfigureSales(EndpointConfiguration configuration, IServiceCollection services)
{
    // Azure Functions endpoints must use AzureServiceBusServerlessTransport.
    configuration.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));
    configuration.UseSerialization<SystemJsonSerializer>();

    // Register services that should only be available to this endpoint.
    services.AddSingleton(new MyComponent("Sales"));

    // Handlers must be added explicitly because assembly scanning is not available.
    configuration.AddHandler<SubmitOrderHandler>();
}
```

## Migrate usages of IFunctionEndpoint

With the old worker package, Azure Functions that send messages outside handlers typically inject `IFunctionEndpoint`.

With the new package, configure an explicit send-only endpoint and inject `IMessageSession` from that endpoint.
This uses [.NET keyed services](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#keyed-services) to select the correct send-only endpoint from dependency injection.

First, register the send-only endpoint in `Program.cs`:

```csharp
using NServiceBus.Transport.AzureServiceBus;

builder.AddSendOnlyNServiceBusEndpoint("client", configuration =>
{
    // Send-only endpoints in Azure Functions also use AzureServiceBusServerlessTransport.
    var transport = new AzureServiceBusServerlessTransport(TopicTopology.Default);
    var routing = configuration.UseTransport(transport);

    // Route outgoing messages from this send-only endpoint to the migrated receiver.
    routing.RouteToEndpoint(typeof(SubmitOrder), "sales");
    configuration.UseSerialization<SystemJsonSerializer>();
});
```

Then inject the message session into the sending function:

```csharp
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

// The key must match the send-only endpoint name registered in Program.cs.
class SalesApi([FromKeyedServices("client")] IMessageSession session)
{
    [Function("SalesApi")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
        HttpRequestData request)
    {
        await session.Send(new SubmitOrder());

        return request.CreateResponse(HttpStatusCode.OK);
    }
}
```

The key used in `[FromKeyedServices("client")]` must match the name passed to `AddSendOnlyNServiceBusEndpoint("client", ...)`.

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

> [!NOTE]
> The new package automatically enables [DLQ forwarding](/transports/azure-service-bus/configuration.md#dead-lettering-forward-dead-lettered-messages-to-the-error-queue) to allow dead-lettered messages to be managed by the platform.

## Startup diagnostics

The old worker package documents an explicit startup diagnostics configuration step. See the [old startup diagnostics documentation](/nservicebus/hosting/azure-functions-service-bus/#configuration-startup-diagnostics).

With the new package, startup diagnostics are automatically forwarded to the logs, so the old `LogDiagnostics()` guidance is no longer needed.

## Host ID

The old package used the `WEBSITE_SITE_NAME` environment variable as the NServiceBus Host ID. This resulted in instances not being visible in ServicePulse as the function was scaled up or down. In the new package, [`WEBSITE_INSTANCE_ID`](https://learn.microsoft.com/en-us/azure/app-service/reference-app-settings?tabs=kudu%2Cdotnet#scaling) is used to ensure that all instances are identifiable.

> [!NOTE]
> It's recommended to [configure ServicePulse not to track heartbeats for these instances](https://docs.particular.net/monitoring/heartbeats/in-servicepulse#configuration-do-not-track-instances) to avoid false negatives.

The logic falls back to [`CONTAINER_NAME` when running in Azure container apps](https://learn.microsoft.com/en-us/azure/container-apps/environment-variables?tabs=portal#apps) and `Environment.MachineName` for local development.

If needed, use the guidance in [Overriding the host identifier](/nservicebus/hosting/override-hostid.md) to take full control over the host ID and keep it stable across restarts and deployments.

## Recommended migration sequence

1. Remove `NServiceBus.AzureFunctions.Worker.ServiceBus` and add `NServiceBus.AzureFunctions.AzureServiceBus`.
2. Replace `builder.AddNServiceBus(...)` with `builder.AddNServiceBusFunctions()`.
3. Remove `NServiceBusTriggerFunction`.
4. Migrate the existing receiving endpoint to an explicit `[NServiceBusFunction]` endpoint method.
5. Move endpoint configuration to `Configure<FunctionName>` methods.
6. Replace any `IFunctionEndpoint` usage with a send-only endpoint and keyed `IMessageSession`.
7. Verify queue names, routing, recoverability, and diagnostics behavior.

## Multiple endpoints in one function app

The new package can host multiple endpoints in one function app, but whether that is a good next step depends heavily on the business scenario.

For most teams, it is better to:

1. complete a one-to-one migration first
2. validate routing, recoverability, and operational behavior
3. consider splitting the app into multiple endpoints as a separate change
