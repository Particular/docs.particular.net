---
title: Azure Functions hosting
component: AzureFunctions
summary: Hosting NServiceBus endpoints in Azure Functions with the AzureServiceBus package
reviewed: 2026-06-03
related:
  - transports/azure-service-bus
  - nservicebus/hosting/startup-diagnostics
  - nservicebus/hosting/override-hostid
  - samples/azure-functions/service-bus
---

NServiceBus endpoints can be hosted in an Azure Functions app using the [`NServiceBus.AzureFunctions.AzureServiceBus`](https://www.nuget.org/packages/NServiceBus.AzureFunctions.AzureServiceBus) package:

snippet: azure-functions-basic-endpoint

An endpoint is declared as a partial method inside a partial class and composed of two parts:

- A partial method decorated with a `[NServiceBusFunction]` and a `[Function("MyFunction")]` attribute declaring a [Azure Service Bus trigger](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus-trigger).
  - The trigger must set `AutoCompleteMessages = false` since NServiceBus pipeline takes responsibility for completing or abandoning each message based on handler outcomes; the `NSBFUNC006` analyzer enforces this requirement.
  - A source generator will emit the method body that forwards each incoming message to the NServiceBus pipeline.
- A static `Configure{FunctionName}` method that configures the NServiceBus endpoint for the function.

> [!NOTE]
> A single Functions app can host one or more endpoints.

## The configure method

The static `Configure{FunctionName}` method is discovered by the source generator and called once per endpoint at host startup. Parameters are matched by type:

| Parameter | Use |
|---|---|
| `EndpointConfiguration` | Required. Configures the endpoint. |
| [`IServiceCollection`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection) | Optional. Registers endpoint-scoped services. |
| [`IConfiguration`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) | Optional. Reads application configuration. |
| [`IHostEnvironment`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostenvironment) | Optional. Inspects the hosting environment, for example to differentiate development and production. |

Declare only the parameters needed, `EndpointConfiguration` must be first:

snippet: azure-functions-configure-with-services

### Transport configuration

Azure Functions endpoints must use `AzureServiceBusServerlessTransport`. Other transport definitions are currently not supported in this hosting model.

Supported options:

| Option | Notes |
|---|---|
| `ConnectionName` | Sets the connection setting name used by the transport, only relevant for [send-only endpoints](#send-only-endpoints). For receiving endpoints, the queue and connection are declared on `[ServiceBusTrigger]`. |
| [`HierarchyNamespaceOptions`](/transports/azure-service-bus/configuration.md#entity-creation-hierarchy-namespace) | Applies hierarchy prefixes to transport addresses and entities created by the transport. |
| [`EnablePartitioning`](/transports/azure-service-bus/configuration.md#entity-creation) | Applies when the transport creates queues and topics. |
| [`EntityMaximumSize`](/transports/azure-service-bus/configuration.md#entity-creation) | Applies when the transport creates queues and topics. |
| [`AutoForwardDeadLetteredMessagesToErrorQueue`](/transports/azure-service-bus/configuration.md#dead-lettering-forward-dead-lettered-messages-to-the-error-queue) | Enabled by default. Applies to transport-created receive queues. |
| [`MaxDeliveryCount`](/transports/azure-service-bus/configuration.md#entity-creation) | Applies when the transport creates queues. In this hosting model, `AzureServiceBusServerlessTransport` defaults `MaxDeliveryCount` to `100`, not `int.MaxValue` like `AzureServiceBusTransport`. |
| [`ThrowOnMissingTopicWhenPublishing`](/transports/azure-service-bus/configuration.md#entity-creation) | Controls whether publishing to a non-existent topic throws after logging a warning. |

Other receive-side Azure Service Bus transport settings, such as [prefetch count](/transports/azure-service-bus/configuration.md#controlling-the-prefetch-count) and [lock renewal](/transports/azure-service-bus/configuration.md#lock-renewal), are controlled by Azure Functions rather than by `AzureServiceBusServerlessTransport` in this hosting model.

## Explicit handler and saga registration

Assembly scanning is not available in this hosting model, so handlers and sagas must be registered explicitly. See [Registering Handlers and Sagas](/nservicebus/handlers-and-sagas-registration.md) for the available registration approaches in NServiceBus 10.2, and [Disable assembly scanning](/nservicebus/hosting/assembly-scanning.md#disable-assembly-scanning) for the caveats and additional manual registration requirements. Handlers that are not registered will not be invoked.

## Wiring the host

The Functions host is bootstrapped in `Program.cs`. Calling `AddNServiceBusFunctions` registers and configures each `[NServiceBusFunction]` with the Functions runtime:

snippet: azure-functions-program-builder

## Hosting multiple endpoints

Multiple methods decorated with `[NServiceBusFunction]` can co-exist in one Functions app. Each is registered as an independent NServiceBus endpoint with its own queue, handlers, and configure method. Two functions hosted from the same partial class:

snippet: azure-functions-multiple-endpoints

Each endpoint has its own `Configure{FunctionName}` method; the source generator routes each function to its matching configure method. Endpoints share the host's service provider but maintain independent message-handling pipelines.

## Send-only endpoints

A [send-only endpoint](/nservicebus/endpoints/#send-only) can be registered for components that need to dispatch messages without listening for incoming traffic, for example a function fronting an HTTP API:

snippet: azure-functions-sendonly-registration

`AddSendOnlyNServiceBusEndpoint(...)` has overloads for endpoint-only configuration and for configuration plus endpoint-scoped service registration.

To send or publish from another function (for example, an HTTP trigger), inject `IMessageSession` [keyed by the same name](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#keyed-services) used at registration. Any endpoint-scoped services registered in the send-only endpoint can be resolved the same way:

snippet: azure-functions-sendonly-usage

The key passed to `[FromKeyedServices(...)]` must match the name passed to `AddSendOnlyNServiceBusEndpoint(...)`.

## Connection configuration

The Azure Service Bus connection is read from the Functions configuration. The named setting can be either a [connection string](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues#connection-string) or a configuration section containing `fullyQualifiedNamespace` for token-credential authentication. The same setting name is referenced in two places:

- The `Connection` parameter on the `[ServiceBusTrigger]` attribute.
- The `ConnectionName` property on `AzureServiceBusServerlessTransport` when registering a send-only endpoint.

If the default setting name `AzureWebJobsServiceBus` is used, the `Connection` parameter can be omitted on `[ServiceBusTrigger]`, and `ConnectionName` does not need to be set on `AzureServiceBusServerlessTransport`.

The connection name in `[ServiceBusTrigger]` can also use Azure Functions [binding expressions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-expressions-patterns).

## Queue name resolution

The queue that the endpoint receives from is taken from the `queueName` passed to the `[ServiceBusTrigger]` attribute. No separate receive queue name is configured in `Configure{FunctionName}`.

The queue name in `[ServiceBusTrigger]` can use Azure Functions [binding expressions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-expressions-patterns).

## Transactions

Azure Functions endpoints only supports [`TransportTransactionMode.ReceiveOnly`](/transports/transactions.md#transaction-modes-transport-transaction-receive-only).

## Recoverability

In addition to the standard recoverability provided by NServiceBus the package also automatically enables [DLQ forwarding to the error queue](/transports/azure-service-bus/configuration.md#dead-lettering-forward-dead-lettered-messages-to-the-error-queue), so dead-lettered messages can be managed alongside other failed messages by tools such as ServicePulse.

For details on the dead-letter behavior and configuration options, see [dead-lettering](/transports/azure-service-bus/configuration.md#dead-lettering) in the Azure Service Bus transport documentation.

## Provisioning the namespace

Provision queues and other entities in the Azure Service Bus namespace using the [`asb-transport` command-line tool](/transports/azure-service-bus/operational-scripting.md).

> [!WARNING]
> The transport defaults `MaxDeliveryCount` to 100. When provisioning entities with the `asb-transport` tool or by hand, set `MaxDeliveryCount` to match (for example, pass `--maximum-delivery-count 100` to the CLI tool) so the namespace and the transport agree. Also make sure to include dead-letter queue forwarding using `--forward-dlq-to error`

## Startup diagnostics

Startup diagnostics are not written by default in this hosting model. Use `LogDiagnostics()` to write them to the Functions host log stream and any configured destination such as Application Insights. For the available `LogDiagnostics()` behavior and options, see [Writing diagnostics to the log](/nservicebus/hosting/startup-diagnostics.md#writing-diagnostics-to-the-log).

### Custom diagnostics writer

For full control over the diagnostic output (for example, to persist diagnostics beyond the function's execution lifetime or to aggregate diagnostic data from multiple function instances), configure a [custom diagnostics writer](/nservicebus/hosting/startup-diagnostics.md) on the endpoint configuration.

## Analyzers

The package includes Roslyn analyzers that enforce required patterns:

| ID | Severity | Rule |
|---|---|---|
| `NSBFUNC001` | Error | A class containing a method with `[NServiceBusFunction]` must be `partial`. |
| `NSBFUNC002` | Warning | A function class should not implement `IHandleMessages<T>`. |
| `NSBFUNC003` | Error | A method with `[NServiceBusFunction]` must be `partial`. |
| `NSBFUNC004` | Warning | The application must call `builder.AddNServiceBusFunctions()`. |
| `NSBFUNC005` | Error | Only one `Configure{FunctionName}` method is allowed per function. |
| `NSBFUNC006` | Error | A `ServiceBusTrigger` must set `AutoCompleteMessages = false`. |
| `NSBFUNC007` | Error | The function method has an invalid signature or is missing its `Configure{FunctionName}` method. |

A code fix is provided for `NSBFUNC007` to add missing trigger parameters and generate a `Configure{FunctionName}` method stub when one is not present.
