---
title: NServiceBus.Extensions.Hosting
summary: NServiceBus integration with Microsoft.Extensions.Hosting
component: Extensions.Hosting
reviewed: 2019-11-08
tags:
 - Hosting
---

The `NServiceBus.Extensions.Hosting` provides easy integration with the [.NET Core Generic Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host).

## Configuration 

An NServiceBus endpoint can be hosted within the generic host with the `UseNServiceBus` extension method:

snippet: extensions-host-configuration

This code will register the endpoint with the hosting infrastructure and automatically start and stop it based on the host's application lifetime.

WARNING: `UseNServiceBus` must be specified before any other service (e.g. `ConfigureWebHostDefaults`) which requires access to the `IMessageSession`.

## Dependency injection integration

NServiceBus endpoints hosted as part of the generic host automatically use the provided `IServiceCollection` and `IServiceProvider` dependency injection infrastructure. Message handlers can resolve dependencies which are registered in the `IServiceCollection`.

`UseNServiceBus` automatically registers an `IMessageSession` with the container which can be resolved from the `IServiceProvider` or via dependency injection during runtime.
