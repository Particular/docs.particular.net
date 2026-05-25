---
title: Azure Functions hosting
component: AzureFunctions
summary: Hosting NServiceBus endpoints in Azure Functions with the AzureServiceBus package
reviewed: 2026-05-25
related:
  - transports/azure-service-bus
  - nservicebus/hosting/startup-diagnostics
  - nservicebus/hosting/override-hostid
---

NServiceBus endpoints can be hosted in an Azure Functions app using the [`NServiceBus.AzureFunctions.AzureServiceBus`](https://www.nuget.org/packages/NServiceBus.AzureFunctions.AzureServiceBus) package:

snippet: azure-functions-basic-endpoint

An endpoint is a partial class composed of three parts:

- The class is decorated with `[NServiceBusFunction]`.
- A partial method declares the Azure Service Bus trigger. The source generator emits the body, which forwards each incoming message to the NServiceBus pipeline.
- A static `Configure{FunctionName}` method registers handlers and configures the endpoint.

The trigger must set `AutoCompleteMessages = false`. The NServiceBus pipeline takes responsibility for completing or abandoning each message based on handler outcomes; the `NSBFUNC006` analyzer enforces this requirement.

> [!NOTE]
> A single Functions app can host one or more endpoints.

## The configure method

The static `Configure{FunctionName}` method is discovered by the source generator and called once per endpoint at host startup. Parameters are matched by type, in any combination:

| Parameter | Use |
|---|---|
| `EndpointConfiguration` | Required. Configures handlers, recoverability, audit, and other endpoint settings. |
| `IServiceCollection` | Optional. Registers endpoint-scoped services. |
| `IConfiguration` | Optional. Reads application configuration. |
| `IHostEnvironment` | Optional. Inspects the hosting environment, for example to differentiate development and production. |

Declare only the parameters needed:

snippet: azure-functions-configure-with-services

## Explicit handler and saga registration

Assembly scanning is not available in this hosting model, so handlers and sagas must be registered explicitly. See [Registering Handlers and Sagas](/nservicebus/handlers-and-sagas-registration.md) for the available registration approaches in NServiceBus 10.2, and [Disable assembly scanning](/nservicebus/hosting/assembly-scanning.md#disable-assembly-scanning) for the caveats and additional manual registration requirements. Handlers that are not registered will not be invoked.

## Wiring the host

The Functions host is bootstrapped in `Program.cs`. Calling `AddNServiceBusFunctions` discovers every `[NServiceBusFunction]` in the application and registers each endpoint with the Functions runtime:

snippet: azure-functions-program-builder

> [!NOTE]
> `AddNServiceBusFunctions` is a source-generated extension on `FunctionsApplicationBuilder` declared in the project's default namespace. Ensure `Program.cs` imports that namespace. At least one function needs to be defined for the namespace to be available.

## Hosting multiple endpoints

Multiple `[NServiceBusFunction]`-decorated members can co-exist in one Functions app. Each is registered as an independent NServiceBus endpoint with its own queue, handlers, and configure method. Two functions hosted from the same partial class:

snippet: azure-functions-multiple-endpoints

Each endpoint has its own `Configure{FunctionName}` method; the source generator routes each function to its matching configure method. Endpoints share the host's service provider but maintain independent message-handling pipelines.

## Send-only endpoints

A [send-only endpoint](/nservicebus/endpoints/#send-only) can be registered for components that need to dispatch messages without listening for incoming traffic, for example a function fronting an HTTP API:

snippet: azure-functions-sendonly-registration

To send or publish from another function (for example, an HTTP trigger), inject `IMessageSession` keyed by the same name used at registration:

snippet: azure-functions-sendonly-usage

The key passed to `[FromKeyedServices(...)]` must match the name passed to `AddSendOnlyNServiceBusEndpoint(...)`.

## Connection configuration

The [connection string](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues#connection-string) for Azure Service Bus is read from the Functions configuration. The same setting name is referenced in two places:

- The `Connection` parameter on the `[ServiceBusTrigger]` attribute.
- The `ConnectionName` property on `AzureServiceBusServerlessTransport` when registering a send-only endpoint.

Any setting name can be used; the examples on this page use `ServiceBusConnection`.

## Transactions

Azure Functions endpoints use [`TransportTransactionMode.ReceiveOnly`](/transports/transactions.md#transaction-modes-transport-transaction-receive-only). [`TransportTransactionMode.SendsAtomicWithReceive`](/transports/transactions.md#transaction-modes-transport-transaction-sends-atomic-with-receive) is not supported in this hosting model.

## Recoverability

In addition to the standard recoverability provided by NServiceBus the package also automatically enables [DLQ forwarding to the error queue](/transports/azure-service-bus/configuration.md#dead-lettering-forward-dead-lettered-messages-to-the-error-queue), so dead-lettered messages can be managed alongside other failed messages by tools such as ServicePulse.

For details on the dead-letter behavior and configuration options, see [dead-lettering](/transports/azure-service-bus/configuration.md#dead-lettering) in the Azure Service Bus transport documentation.

## Provisioning the namespace

Provision queues and other entities in the Azure Service Bus namespace using the [`asb-transport` command-line tool](/transports/azure-service-bus/operational-scripting.md).

> [!WARNING]
> The transport defaults `MaxDeliveryCount` to 100. When provisioning entities with the `asb-transport` tool or by hand, set `MaxDeliveryCount` to match (for example, pass `--maximum-delivery-count 100` to the CLI tool) so the namespace and the transport agree. Also make sure to include dead-letter queue forwarding using `--forward-dlq-to error`

## Logging and diagnostics

NServiceBus log output is forwarded to the Azure Functions logging pipeline automatically. Configure log levels and providers through the standard Functions logging configuration; no additional NServiceBus log setup is required.

Startup diagnostics are not written by default in this hosting model. When opted in, they appear in the Functions host log stream and in any configured destination such as Application Insights.

### Custom diagnostics writer

For full control over the diagnostic output (for example, to persist diagnostics beyond the function's execution lifetime or to aggregate diagnostic data from multiple function instances), configure a [custom diagnostics writer](/nservicebus/hosting/startup-diagnostics.md) on the endpoint configuration.

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
| `NSBFUNC007` | Error | The function method has an invalid signature or is missing its `Configure{FunctionName}` method. |

A code fix is provided for `NSBFUNC007` to add missing trigger parameters and generate a `Configure{FunctionName}` method stub when one is not present.
