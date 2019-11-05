---
title: NServiceBus.Extensions.Hosting
summary: NServiceBus integration for Microsoft.Extensions.Hosting
component: Extensions.Hosting
reviewed: 2019-11-04
tags:
 - Hosting
related:
 - samples/dependency-injection/aspnetcore
 - samples/web/send-from-aspnetcore-webapi
---

The `NServiceBus.Extensions.Hosting` provides easy integration with the [.NET Core Generic Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host).

## Configuration 

An NServiceBus endpoint can be hosted within the generic host with the `AddNServiceBus` extension method:

snippet: extensions-host-configuration

This code will register the endpoint with the hosting infrastructure and automatically start and stop it based on the hosts application lifetime.

### ASP.NET Core

`AddNServiceBus` can also be used within the `ConfigureServices` method when using a `Startup` class in a ASP.NET Core web application. For more details, see the [ASP.NET Core sample](/samples/dependency-injection/aspnetcore).


## Dependency injection integration

NServiceBus endpoints hosted as part of the generic host using the `AddNServiceBus` extension method automatically use the provided `IServiceCollection` and `IServiceProvider` dependency injection infrastructure. Message handlers can resolve dependencies which are registered in the `IServiceCollection`.

`AddNServiceBus` automatically registers a `IMessageSession` with the container which can be resolved from the `IServiceProvider` or via dependency injection during runtime.

