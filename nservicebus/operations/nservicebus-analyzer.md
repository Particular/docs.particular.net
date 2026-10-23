---
title: NServiceBus Analyzer
summary: How to use the NServiceBus analyzer to avoid missing awaits
reviewed: 2026-10-23
component: Core
versions: '[6,]'
---

[Asynchronous](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/) APIs bring great performance improvements. However, they introduce new risks for bugs due to missing `await` operators. NServiceBus comes with a built-in [Roslyn](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/) analyzer to detect missing awaits on NServiceBus APIs.

## Functionality

The analyzer points out missing `await` operators when using asynchronous methods on the following classes and interfaces:

* `IMessageHandlerContext`
* `IMessageSession`
* `Saga`
* `Endpoint`
* `IStartableEndpoint`
* `IEndpointInstance`
* `IUniformSession` (from the [UniformSession package](/nservicebus/messaging/uniformsession.md))

Failing to `await` or assign the tasks returned by these methods results in the following compile-time error:

> [!WARNING]
> **NSB0001**: A Task returned by an async NServiceBus method is not awaited.

## Limitations

This analyzer inspects only the APIs contained on the classes and interfaces as described in the previous section. Other asynchronous NServiceBus APIs are not analyzed.

The analyzer checks for ignored `Task`s returned from the scanned methods. If the `Task` is assigned to a variable, passed to another method, or has one of its members accessed, the analyzer will not evaluate this `Task`'s usage any further.

## Enabling the analyzer

The analyzer is part of the NServiceBus NuGet package and will be automatically installed when referencing the NServiceBus package in a project.

## Requirements

The analyzer runs as part of the C# compiler, so it reports missing `await` operators in any environment that builds the project, including Visual Studio, JetBrains Rider, Visual Studio Code, and the `dotnet build` command.

#if-version [8,)
The project must reference NServiceBus using `PackageReference` elements. Projects that use a `packages.config` file do not run the analyzer.
#end-if

## Disabling the analyzer

The analyzer can be disabled in several ways. However, it is not recommended to do so. The analyzer helps identify missing `await` operators, which, if not added, can lead to issues such as duplicate messages, message loss, transaction inconsistencies, and more.

### Disable analyzer warning for a single call site

```csharp
#pragma warning disable NSB0001
context.Send(message);
#pragma warning restore NSB0001
```

### Disable analyzer warning project-wide

Add `NSB0001` to the `NoWarn` property in the project file, for example `<NoWarn>$(NoWarn);NSB0001</NoWarn>`. Including `$(NoWarn)` preserves any other suppressed warnings.

## Additional recommendations

### Treat warnings as errors

The C# compiler already contains a set of inspections which can warn about incorrect usage of `async` and `Task`-based APIs. It is recommended to treat these warnings as errors to ensure they are not missed accidentally. This feature can be enabled by the project settings or by adding `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` to the `csproj` file directly.
