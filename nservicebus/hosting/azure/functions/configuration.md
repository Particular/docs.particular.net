---
title: Azure Functions configuration
component: AzureFunctions
summary: Configuring Azure Functions-hosted NServiceBus endpoints.
reviewed: 2026-06-04
related:
  - nservicebus/hosting/azure/functions
  - transports/azure-service-bus/configuration
  - nservicebus/handlers-and-sagas-registration
---

## The configure method

The static `Configure{FunctionName}` method is discovered by the source generator and called once per endpoint at host startup. Parameters are matched by type:

| Parameter | Use |
|---|---|
| `EndpointConfiguration` | Required. Configures the endpoint. |
| [`IServiceCollection`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection) | Optional. Registers endpoint-scoped services. |
| [`IConfigurationManager`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfigurationmanager) | Optional. Reads application configuration. |
| [`IHostEnvironment`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostenvironment) | Optional. Inspects the hosting environment, for example to differentiate development and production. |

Declare only the parameters needed, `EndpointConfiguration` must be first:

snippet: azure-functions-configure-with-services

## Transport configuration

Azure Functions endpoints must use `AzureServiceBusServerlessTransport`. Other transport definitions are currently not supported in this hosting model.

Supported options:

| Option | Notes |
|---|---|
| `ConnectionName` | Sets the connection setting name used by the transport, only relevant for [send-only endpoints](/nservicebus/hosting/azure/functions/#send-only-endpoints). For receiving endpoints, the queue and connection are declared on `[ServiceBusTrigger]`. |
| [`HierarchyNamespaceOptions`](/transports/azure-service-bus/configuration.md#entity-creation-hierarchy-namespace) | Applies hierarchy prefixes to transport addresses and entities created by the transport. |
| [`EnablePartitioning`](/transports/azure-service-bus/configuration.md#entity-creation) | Applies when the transport creates queues and topics. |
| [`EntityMaximumSize`](/transports/azure-service-bus/configuration.md#entity-creation) | Applies when the transport creates queues and topics. |
| [`AutoForwardDeadLetteredMessagesToErrorQueue`](/transports/azure-service-bus/configuration.md#dead-lettering-forward-dead-lettered-messages-to-the-error-queue) | Enabled by default. Applies to transport-created receive queues. |
| [`MaxDeliveryCount`](/transports/azure-service-bus/configuration.md#entity-creation) | Applies when the transport creates queues. In this hosting model, `AzureServiceBusServerlessTransport` defaults `MaxDeliveryCount` to `100`, not `int.MaxValue` like `AzureServiceBusTransport`. |
| [`ThrowOnMissingTopicWhenPublishing`](/transports/azure-service-bus/configuration.md#entity-creation) | Controls whether publishing to a non-existent topic throws after logging a warning. |

Other receive-side Azure Service Bus transport settings, such as [prefetch count](/transports/azure-service-bus/configuration.md#controlling-the-prefetch-count) and [lock renewal](/transports/azure-service-bus/configuration.md#lock-renewal), are controlled by Azure Functions rather than by `AzureServiceBusServerlessTransport` in this hosting model.

## Explicit handler and saga registration

Assembly scanning is not available in this hosting model because a single Functions app can host [multiple endpoints](/nservicebus/hosting/core-hosting.md). As a secondary effect, avoiding assembly scanning also improves cold startup times.

Handlers and sagas must therefore be registered explicitly. See [Registering Handlers and Sagas](/nservicebus/handlers-and-sagas-registration.md) for the available registration approaches in NServiceBus 10.2, and [Disable assembly scanning](/nservicebus/hosting/assembly-scanning.md#disable-assembly-scanning) for the caveats and additional manual registration requirements. Handlers that are not registered will not be invoked.

## Connection configuration

The Azure Service Bus [connection](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus-trigger#connections) will by default be read from an appSetting named `AzureWebJobsServiceBus` unless explicitly configured via the connection parameter on the `[ServiceBusTrigger]` or `NServiceBusSendOnlyFunction` attributes.

## Queue name resolution

The queue that the endpoint receives from is taken from the `queueName` passed to the `[ServiceBusTrigger]` attribute. No separate receive queue name is configured in `Configure{FunctionName}`.

The queue name supports [functions binding expressions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-expressions-patterns).

## Transactions

Azure Functions endpoints only supports [`TransportTransactionMode.ReceiveOnly`](/transports/transactions.md#transaction-modes-transport-transaction-receive-only).

## Recoverability

In addition to the standard recoverability provided by NServiceBus the package also automatically enables [dead-letter queue forwarding to the error queue](/transports/azure-service-bus/configuration.md#dead-lettering-forward-dead-lettered-messages-to-the-error-queue), so dead-lettered messages can be managed alongside other failed messages by tools such as ServicePulse.

For details on the dead-letter behavior and configuration options, see [dead-lettering](/transports/azure-service-bus/configuration.md#dead-lettering) in the Azure Service Bus transport documentation.

## Provisioning

Queues and other entities in the Azure Service Bus namespace can be provisioned using [`EnableInstallers()`](/nservicebus/operations/installers.md).

Alternatively, provision entities explicitly using the [`asb-transport` command-line tool](/transports/azure-service-bus/operational-scripting.md).

> [!NOTE]
> The Azure Service Bus transport defaults `MaxDeliveryCount` to 100. When provisioning entities with installers, the `asb-transport` tool, or by hand, set `MaxDeliveryCount` to match so the namespace and the transport agree. When using the CLI, pass `--maximum-delivery-count 100` and include dead-letter queue forwarding using `--forward-dlq-to error`.

## Startup diagnostics

Startup diagnostics are not written by default in this hosting model. Use `LogDiagnostics()` to write them to the Functions host log stream and any configured destination such as Application Insights. For the available `LogDiagnostics()` behavior and options, see [Writing diagnostics to the log](/nservicebus/hosting/startup-diagnostics.md#writing-diagnostics-to-the-log).

### Custom diagnostics writer

For full control over the diagnostic output (for example, to persist diagnostics beyond the function's execution lifetime or to aggregate diagnostic data from multiple function instances), configure a [custom diagnostics writer](/nservicebus/hosting/startup-diagnostics.md) on the endpoint configuration.

## Host ID

The package derives the NServiceBus Host ID from the Azure Functions hosting environment so scaled-out instances can be identified consistently.

The Host ID resolution order is:

1. [`WEBSITE_INSTANCE_ID`](https://learn.microsoft.com/en-us/azure/app-service/reference-app-settings?tabs=kudu%2Cdotnet#scaling) when running in Azure Functions on App Service
2. [`CONTAINER_NAME`](https://learn.microsoft.com/en-us/azure/container-apps/environment-variables?tabs=portal#apps) when running in Azure Container Apps
3. `Environment.MachineName` for local development

> [!NOTE]
> When using `WEBSITE_INSTANCE_ID`, it is recommended to [configure ServicePulse not to track heartbeats for these instances](/monitoring/heartbeats/in-servicepulse.md#configuration-do-not-track-instances) to avoid false negatives.

If needed, use the guidance in [Overriding the host identifier](/nservicebus/hosting/override-hostid.md) to take full control over the Host ID and keep it stable across restarts and deployments.
