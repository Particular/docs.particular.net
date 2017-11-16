---
title: Asp.NET Core Dependency Injection Integration
component: core
reviewed: 2017-11-14
tags:
 - dependency injection
related:
 - nservicebus/dependency-injection
---

[Asp.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) comes with an integrated [Dependency Injection (DI) container](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection). When hosting NServiceBus endpoints inside an Asp.NET Core app, components registered for DI might need to be shared between Asp.NET components and NServiceBus message handlers. 

It is possible to use an external container to satisfy both requirements.

Note: The integrated Asp.NET Core Dependency Injection container does not support all the features required by NServiceBus for dependency injection, making it is necessary to use a third party container to share the dependency injection configuration between Asp.NET Core and NServiceBus. The sample here uses [Autofac](/nservicebus/dependency-injection/autofac.md), but the same can be achieved by using most other [DI libraries](/nservicebus/dependency-injection/).


### Configuring to use shared DI

snippet: ContainerConfiguration

The [ConfigureServices method](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup#the-configureservices-method) is called by .NET Core infrastructure [at application startup time](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup) and returns a `IServiceProvider` instance that will be used by the infrastructure to resolve components. The same container instance is shared with the NServiceBus endpoint.

When the sample is run a web application is started, all web requests received will trigger a message send:

snippet: RequestHandling

Message handlers will have dependencies injected at runtime by the configured Inversion of Control container:

snippet: InjectingDependency
