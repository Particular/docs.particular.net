---
title: Asp.NET Core Dependency Injection Integration
component: core
reviewed: 2017-10-02
tags:
 - dependency injection
related:
 - nservicebus/dependency-injection
---

[Asp.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) comes with an integrated [Dependency Injection (DI) feature](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection). When hosting NServiceBus endpoints inside an Asp.NET Core app, components registered for DI might need to be shared between Asp.NET components and NServiceBus message handlers.

It is possible to use an external container to satisfy both requirements.

Note: While this sample illustrates the scenario using [Autofac](/nservicebus/dependency-injection/autofac.md), the same can be achieved by using most other [DI libraries](/nservicebus/dependency-injection/). Using Autofac requires adding reference to the `Autofac.Extensions.DependencyInjection` package.


### Configuring to use shared DI

snippet: ContainerConfiguration

The [ConfigureServices method](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup#the-configureservices-method) is called by .NET Core infrastructure [at application startup time](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup) and returns a `IServiceProvider` instance that will be used by the infrastructure to resolve components. The same container instance is shared with the NServiceBus endpoint.

When the sample is run a web application is started, all web requests received will trigger a message send:

snippet: RequestHandling

Message handlers will have dependencies injected at runtime by the configured Inversion of Control container:

snippet: InjectingDependency
