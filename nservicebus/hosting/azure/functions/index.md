---
title: Azure Functions hosting
component: AzureFunctions
summary: Hosting NServiceBus endpoints in Azure Functions with the AzureServiceBus package
reviewed: 2026-05-05
---

NServiceBus endpoints can be hosted in an Azure Functions app using the [`NServiceBus.AzureFunctions.AzureServiceBus`](https://www.nuget.org/packages/NServiceBus.AzureFunctions.AzureServiceBus) package:

```csharp
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

[NServiceBusFunction]
public partial class OrdersEndpoint
{
    [Function(nameof(Orders))]
    public partial Task Orders(
        [ServiceBusTrigger("orders", Connection = "ServiceBusConnection", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureOrders(EndpointConfiguration endpoint)
    {
        endpoint.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));
        endpoint.UseSerialization<SystemJsonSerializer>();
        endpoint.AddHandler<PlaceOrderHandler>();
    }
}
```

An endpoint is a partial class composed of three parts:

- The class is decorated with `[NServiceBusFunction]`.
- A partial method declares the Azure Service Bus trigger. The source generator emits the body, which forwards each incoming message to the NServiceBus pipeline.
- A static `Configure{FunctionName}` method registers handlers and configures the endpoint.

The trigger must set `AutoCompleteMessages = false`. The NServiceBus pipeline takes responsibility for completing or abandoning each message based on handler outcomes; the `NSBFUNC006` analyzer enforces this requirement.

A single Functions app can host one or more endpoints.

## The configure method

The static `Configure{FunctionName}` method is discovered by the source generator and called once per endpoint at host startup. Parameters are matched by type, in any combination:

| Parameter | Use |
|---|---|
| `EndpointConfiguration` | Required. Configures handlers, recoverability, audit, and other endpoint settings. |
| `IServiceCollection` | Optional. Registers endpoint-scoped services. |
| `IConfiguration` | Optional. Reads application configuration. |
| `IHostEnvironment` | Optional. Inspects the hosting environment, for example to differentiate development and production. |

Declare only the parameters needed:

```csharp
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

[NServiceBusFunction]
public partial class ShippingEndpoint
{
    [Function("Shipping")]
    public partial Task Shipping(
        [ServiceBusTrigger("shipping", Connection = "ServiceBusConnection", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureShipping(EndpointConfiguration configuration, IServiceCollection services)
    {
        configuration.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));
        configuration.UseSerialization<SystemJsonSerializer>();
        services.AddSingleton(new MyComponent("Shipping"));
        configuration.AddHandler<ShipOrderHandler>();
    }
}
```

## Handler registration

Handlers must be registered explicitly with `EndpointConfiguration.AddHandler<T>`. Assembly scanning is not available in this hosting model; handlers that are not registered will not be invoked.

## Wiring the host

The Functions host is bootstrapped in `Program.cs`. Calling `AddNServiceBusFunctions` discovers every `[NServiceBusFunction]` in the application and registers each endpoint with the Functions runtime:

```csharp
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);
builder.AddNServiceBusFunctions();

var host = builder.Build();
await host.RunAsync();
```

> [!NOTE]
> `AddNServiceBusFunctions` is a source-generated extension on `FunctionsApplicationBuilder` declared in the project's default namespace. Ensure `Program.cs` imports that namespace.

## Hosting multiple endpoints

Multiple `[NServiceBusFunction]`-decorated members can co-exist in one Functions app. Each is registered as an independent NServiceBus endpoint with its own queue, handlers, and configure method. Two functions hosted from the same partial class:

```csharp
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

public partial class BillingFunctions
{
    [Function("BillingApi")]
    [NServiceBusFunction]
    public partial Task BillingApi(
        [ServiceBusTrigger("billing-api", Connection = "ServiceBusConnection", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureBillingApi(EndpointConfiguration configuration)
    {
        configuration.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));
        configuration.UseSerialization<SystemJsonSerializer>();
        configuration.AddHandler<ProcessPaymentHandler>();
    }

    [Function("BillingBackend")]
    [NServiceBusFunction]
    public partial Task BillingBackend(
        [ServiceBusTrigger("billing-backend", Connection = "ServiceBusConnection", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureBillingBackend(EndpointConfiguration endpointConfiguration, IConfiguration configuration, IHostEnvironment environment)
    {
        endpointConfiguration.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.AddHandler<PaymentChargedHandler>();

        if (environment.IsProduction())
        {
            endpointConfiguration.AuditProcessedMessagesTo(configuration["audit-queue"] ?? "audit");
        }
    }
}
```

Each endpoint has its own `Configure{FunctionName}` method; the source generator routes each function to its matching configure method. Endpoints share the host's service provider but maintain independent message-handling pipelines.

## Send-only endpoints

A send-only endpoint can be registered for components that need to dispatch messages without listening for incoming traffic, for example a function fronting an HTTP API:

```csharp
builder.AddSendOnlyNServiceBusEndpoint("client", (configuration, services) =>
{
    var transport = new AzureServiceBusServerlessTransport(TopicTopology.Default)
    {
        ConnectionName = "ServiceBusConnection"
    };
    var routing = configuration.UseTransport(transport);
    routing.RouteToEndpoint(typeof(SubmitOrder), "sales");
    configuration.UseSerialization<SystemJsonSerializer>();
});
```

To send or publish from another function (for example, an HTTP trigger), inject `IMessageSession` keyed by the same name used at registration:

```csharp
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

class SalesApi([FromKeyedServices("client")] IMessageSession session)
{
    [Function("SalesApi")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request)
    {
        await session.Send(new SubmitOrder());
        return request.CreateResponse(HttpStatusCode.OK);
    }
}
```

The key passed to `[FromKeyedServices(...)]` must match the name passed to `AddSendOnlyNServiceBusEndpoint(...)`.

## Connection configuration

The connection string for Azure Service Bus is read from the Functions configuration. The same setting name is referenced in two places:

- The `Connection` parameter on the `[ServiceBusTrigger]` attribute.
- The `ConnectionName` property on `AzureServiceBusServerlessTransport` when registering a send-only endpoint.

Any setting name can be used; the examples on this page use `ServiceBusConnection`.

## Logging and diagnostics

NServiceBus log output is forwarded to the Azure Functions logging pipeline automatically. Configure log levels and providers through the standard Functions logging configuration; no additional NServiceBus log setup is required.

Startup diagnostics are written when the host starts. In Azure they appear in the Functions host log stream and in any configured destination such as Application Insights.

## Analyzers

The package includes Roslyn analyzers that enforce required patterns:

| ID | Severity | Rule |
|---|---|---|
| `NSBFUNC001` | Error | A class with `[NServiceBusFunction]` must be `partial`. |
| `NSBFUNC002` | Warning | A function class should not implement `IHandleMessages<T>`. |
| `NSBFUNC003` | Error | A method with `[NServiceBusFunction]` must be `partial`. |
| `NSBFUNC004` | Warning | The application must call `builder.AddNServiceBusFunctions()`. |
| `NSBFUNC005` | Error | Only one `Configure{FunctionName}` method is allowed per function. |
| `NSBFUNC006` | Error | A `ServiceBusTrigger` must set `AutoCompleteMessages = false`. |
| `NSBFUNC007` | Error | The function method signature must match the required trigger shape. |

Code fixes are provided for the partial-modifier and signature errors.