---
title: NServiceBus.Extensions.DependencyInjection
summary: Provides integration with the Microsoft.Extensions.DependencyInjection abstraction.
reviewed: 2020-02-17
component: Extensions.DependencyInjection
tags:
 - Dependency Injection
---

The `NServiceBus.Extensions.DependencyInjection` package provides integration with the `Microsoft.Extensions.DependencyInjection` dependency injection abstraction.

## Usage with ServiceCollection

The following snippet shows how to configure NServiceBus to use Microsoft's built-in dependency injection container:

snippet: usecontainer-servicecollection


## Usage with third party containers

NServiceBus can also be configured to work with any third party dependency injection which implements the `Microsoft.Extensions.DependencyInjection` abstraction. To use a third party dependency injection container, pass the specfic container's `IServiceProviderFactory` to the `UseContainer` configuration method. The following snippet shows this approach, using Autofac as an example:

snippet: usecontainer-thirdparty
