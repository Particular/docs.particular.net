---
title: Roslyn analyzers for Azure Functions
component: AzureFunctions
summary: Details of the Roslyn analyzers included with Azure Functions hosting.
reviewed: 2026-06-04
---

The package includes [Roslyn analyzers](https://learn.microsoft.com/en-us/visualstudio/code-quality/roslyn-analyzers-overview) that enforce required patterns for Azure Functions hosting.

## Diagnostics

| ID | Severity | Description |
|---|---|---|
| `NSBFUNC001` | Error | A class containing a method with `[NServiceBusFunction]` must be `partial`. |
| `NSBFUNC002` | Warning | A function class should not implement `IHandleMessages<T>`. Register handlers separately in endpoint configuration. |
| `NSBFUNC003` | Error | A method with `[NServiceBusFunction]` must be `partial`. |
| `NSBFUNC004` | Warning | The project contains generated NServiceBus registrations but does not call `builder.AddNServiceBusFunctions()`. |
| `NSBFUNC005` | Error | Only one `Configure{FunctionName}` method is allowed for a function. |
| `NSBFUNC006` | Error | The Service Bus trigger must explicitly set `AutoCompleteMessages = false`. |
| `NSBFUNC007` | Error | The function method is invalid, for example because required trigger parameters are missing or the matching `Configure{FunctionName}` method is missing or invalid. |
| `NSBFUNC008` | Error | Unsupported endpoint configuration API is used in a `Configure…` method for a receiving or send-only endpoint. |
| `NSBFUNC009` | Error | Unsupported `SendOptions` or `ReplyOptions` API is used for Azure Functions endpoints. |
| `NSBFUNC010` | Error | `EndpointConfiguration.UseTransport(...)` does not use `AzureServiceBusServerlessTransport`. |
| `NSBFUNC011` | Error | A method marked with `[NServiceBusSendOnlyFunction]` is not a valid send-only endpoint declaration. |

## Unsupported endpoint configuration APIs

Because the Azure Functions runtime is responsible for fetching messages from the broker, some endpoint configuration APIs are not supported in this hosting model. `NSBFUNC008` is reported when a `Configure…` method uses one of these unsupported APIs.

Unsupported APIs include:

- `EndpointConfiguration.PurgeOnStartup`
- `EndpointConfiguration.LimitMessageProcessingConcurrencyTo`
- `EndpointConfiguration.DefineCriticalErrorAction`
- `EndpointConfiguration.SetDiagnosticsPath`
- `EndpointConfiguration.MakeInstanceUniquelyAddressable`
- `EndpointConfiguration.UniquelyIdentifyRunningInstance`
- `EndpointConfiguration.OverrideLocalAddress`

For send-only endpoints, some of these APIs are invalid because send-only endpoints do not receive messages.

## Unsupported send and reply options APIs

`NSBFUNC009` is reported when code uses APIs that try to route to a specific instance, which is not supported for Azure Functions because instances are ephemeral.

Unsupported APIs:

- `SendOptions.RouteToThisInstance`
- `ReplyOptions.RouteReplyToThisInstance`

## Required transport

`NSBFUNC010` ensures `EndpointConfiguration.UseTransport(…)` uses `AzureServiceBusServerlessTransport`.

For the supported transport configuration in this hosting model, see [Azure Functions configuration](/nservicebus/hosting/azure/functions/configuration.md#transport-configuration).

## Send-only endpoint validation

`NSBFUNC011` is reported when a method marked with `[NServiceBusSendOnlyFunction]` does not match the required shape.

The method must:

- be `static`
- be named `Configure{EndpointName}` (case-insensitive)
- take `EndpointConfiguration` as the first parameter
- only use `IServiceCollection`, `IConfiguration`, and `IHostEnvironment` as additional optional parameters

## Code fixes

The package currently includes one code fix.

### `NSBFUNC007`

The code fix can:

- add missing trigger-related parameters such as `FunctionContext` and `CancellationToken`
- add missing additional trigger parameters required by the function signature
- generate a missing `Configure{FunctionName}` method stub
