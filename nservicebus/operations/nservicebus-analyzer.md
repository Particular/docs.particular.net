---
title: NServiceBus Analyzer
summary: How to use the NServiceBus analyzer to avoid missing awaits
component: Core
versions: '[6,]'
---

Asynchronous APIs bring great performance improvements. However, they introduce new risks for bugs due to missing `await` operators. Starting with Versions 6.5 and 7.1, NServiceBus comes with a built-in Roslyn analyzer to detect missing awaits on NServiceBus APIs.

## Functionality

The analyzer points out missing `await` operators when using asynchronous methods on the following classes and interfaces:
* `IMessageHandlerContext`
* `IMessageSession`
* `Saga`
* `IEndpointInstance`
* `Endpoint`
* `IUniformSession` (from the [UniformSession package](/nservicebus/messaging/uniformsession.md))

## Limitations

This analyzer inspects only the APIs contained on the classes and interfaces as described in the previous section. Other asynchronous NServiceBus APIs are not analyzed.

The analyzer checks for ignored `Task`s returned from the scanned methods. If the `Task` is assigned to a variable, passed to another method, or has one of its members accessed, the analyzer will not evaluate this `Task`'s usage any further.


## Enabling the analyzer

The analyzer is part of the NServiceBus NuGet package and will be automatically installed when referencing the NServiceBus package in a project.


## Requirements

The analyzer requires Visual Studio 2015 Update 2 or newer.

The analyzer also works when building a .NET Core application using `dotnet build` commands, allowing the analyzer to highlight missing `await` operators in other development environments.


## Disabling the analyzer

The analyzer can be disabled in several ways. However, disabling the analyzer is not recommended as it highlights missing `await` operators, which can cause duplicate messages, message loss, transaction issues, and more.


### Disable analyzer warning for a single call site

```
#pragma warning disable NSB0001
context.Send(message);
#pragma warning restore NSB0001
```

### Disable analyzer warning project-wide

Add a `<NoWarn>NSB0001</NoWarn>` element to the csproj file.


## Additional recommendations

### Treat warnings as errors

The C# compiler already contains a set of inspections which can warn about incorrect usage of `async` and `Task`-based APIs. It is recommended to treat these warnings as errors to ensure they are not missed accidentally. This feature can be enabled by the project settings or by adding `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` to the `csproj` file directly.
