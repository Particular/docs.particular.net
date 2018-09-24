---
title: Microsoft.Extensions.DependencyInjection
summary: How to configure NServiceBus to use Microsoft.Extensions.DependencyInjection for dependency injection.
component: MSDependencyInjection
reviewed: 2018-09-05
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/aspnetcore
---

NServiceBus can be configured to use [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/) for dependency injection.

### Default usage

snippet: msdependencyinjection

### Dependency Lifecycle mapping

The [dependency lifecycle](/nservicebus/dependency-injection/#dependency-lifecycle) maps in the following way.

| DependencyLifecycle                                                                                             | Microsoft.Extensions.DependencyInjection Equivalent                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-call) | [ServiceLifetime.Transient](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?#service-lifetimes)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-unit-of-work)                    | [ServiceLifetime.Scoped](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?#service-lifetimes) |
| [SingleInstance](/nservicebus/dependency-injection/#dependency-lifecycle-single-instance)                                  | [ServiceLifetime.Singleton](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?#service-lifetimes)                          |
