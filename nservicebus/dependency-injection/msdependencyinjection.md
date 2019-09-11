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

If there is the need to share the DI container between NServiceBus and the hosting .NET Core application, the following code can be used:

snippet: msdependencyinjectionsharedcontainer

### DependencyLifecycle mapping

[`DependencyLifecycle`](/nservicebus/dependency-injection/#internally-managed-mode-built-in-default-container) maps to [`Microsoft.Extensions.DependencyInjection` service lifetimes](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2) as follows:

| `DependencyLifecycle`                                                                                             | `Microsoft.Extensions.DependencyInjection` service lifetime                                                                                                        |
|-----------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| [InstancePerCall](/nservicebus/dependency-injection/#internally-managed-mode-built-in-default-container-instance-per-call) | [Transient](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?#service-lifetimes)         |
| [InstancePerUnitOfWork](/nservicebus/dependency-injection/#internally-managed-mode-built-in-default-container-instance-per-unit-of-work)                    | [Scoped](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?#service-lifetimes) |
| [SingleInstance](/nservicebus/dependency-injection/#internally-managed-mode-built-in-default-container-single-instance)                                  | [Singleton](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?#service-lifetimes)                          |
