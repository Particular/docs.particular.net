---
title: Microsoft.Extensions.DependencyInjection
summary: How to configure NServiceBus to use Microsoft.Extensions.DependencyInjection for dependency injection.
component: MSDependencyInjection
reviewed: 2018-12-05
tags:
 - Dependency Injection
related:
 - samples/dependency-injection/aspnetcore
---

NServiceBus can be configured to use [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/) for dependency injection.

### Usage

snippet: msdependencyinjection

### DependencyLifecycle mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/#dependency-lifecycle) maps to [`Microsoft.Extensions.DependencyInjection` service lifetimes](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2) as follows:

| `DependencyLifecycle`                                                                                             | `Microsoft.Extensions.DependencyInjection` service lifetime                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-call) | [Transient](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?#service-lifetimes)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#dependency-lifecycle-instance-per-unit-of-work)                    | [Scoped](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?#service-lifetimes) |
| [SingleInstance](/nservicebus/dependency-injection/#dependency-lifecycle-single-instance)                                  | [Singleton](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?#service-lifetimes)                          |
