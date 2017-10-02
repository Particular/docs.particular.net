---
title: Asp.NET Core Dependency Injection Integration
component: Autofac
reviewed: 2017-10-02
tags:
 - dependency injection
related:
 - nservicebus/dependency-injection
 - nservicebus/dependency-injection/autofac
---

Asp.NET Core comes with an integrated Inversion of Control container,. When hosting NServiceBus endpoints inside an Asp.NET Core app components registered for DI might need to be shared between Asp.NET components and NServiceBus message handlers.

It is possible to use an external container to satisfy both world dependency injection needs

### Configuring to use Autofac

snippet: ContainerConfiguration

The `ConfigureServices` method is called by .NET Core infrastructure at application startup time an can return a `IServiceProvider` instance that will be used by the infrastructure to resolve components. The same container instance is shared with the NServiceBus endpoint.

When the sample is run a web application is started, all web requests received will trigger a message send:

snippet: RequestHandling

Message handlers will have dependencies injected at runtime by the configured Inversion of Control container:

snippet: InjectingDependency