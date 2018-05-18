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

This analyzer does only inspect the APIs contained on the classes and interfaces as described in the previous chapter. Other asynchronous operations on NServiceBus APIs are not analyzed.

The analyzer only checks for ignored tasks returned from the scanned methods. If the task is assigned to a variable or passed to another method, the analyzer will not analyze this task's usage any further.

Cases which are already handled by existing compiler warnings (e.g. [CS4014](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs4014)) are ignored by the NServiceBus analyzer.

## Enabling the analyzer

The analyzer is part of the NServiceBus NuGet package and will be automatically installed when referencing the NServiceBus package in a project.


## Requirements

The analyzer requires VisualStudio 2015 version 15.3 or newer.

The analyzer also works when building a .NET Core application using `dotnet build` commands, allowing the analyzer highlighting missing awaits in other development environments.

## Disabling the analyzer

The analyzer warning can be disabled in several ways:

### Disable analyzer warning project wide

Add a `<NoWarn>NSB0001</NoWarn>` element to the csproj file.

### Temporarily disable analyzer warning

```
#pragma warning disable NSB0001
context.Send(message);
#pragma warning restore NSB0001
```

