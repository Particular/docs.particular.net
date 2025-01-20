---
title: ASP.NET Core Dependency Injection Integration
component: Extensions.Hosting
reviewed: 2025-01-14
related:
 - nservicebus/dependency-injection
---

[ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) has an integrated [dependency injection (DI) feature](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection). When hosting NServiceBus endpoints inside an ASP.NET Core app, sharing components registered for DI between ASP.NET components and NServiceBus message handlers may be necessary. Use the [NServiceBus.Extensions.Hosting package](https://www.nuget.org/packages/NServiceBus.Extensions.Hosting) to host an endpoint as part of an ASP.NET Core application.

### Configuring an endpoint to use built-in DI

snippet: ContainerConfiguration

When the sample is run, a web application is started. All web requests received will trigger a message send operation:

snippet: RequestHandling

Message handlers will have dependencies injected at runtime by the configured Inversion of Control container:

snippet: InjectingDependency

### Configuring to use shared DI with Autofac

It is also possible to configure ASP.NET Core to use a specific container and still share components between ASP.NET and NServiceBus message handlers. This sample demonstrates how to do this with Autofac using the `Autofac.Extensions.DependencyInjection` package

> [!NOTE]
> This can also be done using other [DI libraries](/nservicebus/dependency-injection/).

First, ASP.NET Core is instructed to use a custom container:

snippet: ServiceProviderFactoryAutofac

Then, `Startup` can use Autofac natively to configure services:

snippet: ContainerConfigurationAutofac

The [`ConfigureServices` method](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup#the-configureservices-method) is called by .NET Core at [application startup time](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup) to register additional services. The `ConfigureContainer` method registers the components in the container using the container's native APIs.
