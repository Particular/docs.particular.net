---
title: Async Analyzer
summary: How to use the async analyzer to avoid missing awaits
component: Core
versions: '[6,]'
---

Asynchronous APIs bring great performance improvements. However, they introduce new risks for bugs due to missed `await` statements. NServiceBus comes with a built in Roslyn analyzer to detect those missing awaits.

## Functionality

The analyzer will point out missing `await` statements when using asynchronous methods on the following classes and interfaces:
* `IMessageHandlerContext`
* `IMessageSession`
* `Saga`
* `IEndpointInstance`
* `Endpoint`

## Limitations

This analyzer does only inspect the APIs contained on the classes and interfaces as described in the previous section. Other asynchronous operations on NServiceBus APIs are not analyzed.

The analyzer only checks for ignored tasks returned from the scanned methods. If the task is assigned to a variable or passed to another method, the analyzer will not analyze this task's usage any further.


## Enabling the analyzer

The analyzer is part of the NServiceBus NuGet package and will be automatically installed when referencing the NServiceBus package in a project.


## Requirements

The analyzer requires VisualStudio 2015 or newer.

The analyzer also works when building a .NET Core application using `dotnet build` commands, allowing the analyzer highlighting missing awaits in other development environments.

## Disabling the analyzer

The analyzer errors can be disabled in several ways. However, disabling the analyzer errors is not recommended as they highlight missing awaits which can cause duplicate messages, message loss, transaction issues and more.


### Disable analyzer warning project wide

Add a `<NoWarn>NSB0001</NoWarn>` element to the csproj file.

### Temporarily disable analyzer warning

```
#pragma warning disable NSB0001
context.Send(message);
#pragma warning restore NSB0001
```


## Additional recommendations

### Treat warnings as errors

The C# compiler already contains a set of inspections which can warn about incorrect usage of `async` and task based APIs. We recommend to treat these warnings as errors to ensure they are not missed accidentally. This feature can be enabled by the project settings or by adding `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` to the `csproj` file directly.
