---
title: Roslyn analyzers for Azure Functions
component: AzureFunctions
summary: Details of the Roslyn analyzers included with Azure Functions hosting.
reviewed: 2026-06-04
---

The package includes [Roslyn analyzers](https://learn.microsoft.com/en-us/visualstudio/code-quality/roslyn-analyzers-overview) that enforce required patterns for Azure Functions hosting.

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
