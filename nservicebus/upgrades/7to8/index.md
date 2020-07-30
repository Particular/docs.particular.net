---
title: Upgrade Version 7 to 8
summary: Instructions on how to upgrade NServiceBus from version 7 to version 8.
reviewed: 2020-02-20
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

NOTE: This is a working document; there is currently no timeline for the release of NServiceBus version 8.0.

## Support for external dependency injection containers

Support for external dependency injection containers is no longer provided by NServiceBus adapters for each container library. Instead, the [`NServiceBus.Extensions.DependencyInjection` package](/nservicebus/dependency-injection/extensions-dependencyinjection.md) provides the ability to use any container that conforms to the `Microsoft.Extensions.DependencyInjection` container abstraction.

The following adapter packages will no longer be provided:

* [Autofac](/nservicebus/dependency-injection/autofac.md)
* [Castle](/nservicebus/dependency-injection/castlewindsor.md)
* [StructureMap](/nservicebus/dependency-injection/structuremap.md)
* [Unity](/nservicebus/dependency-injection/unity.md)

### Property injection

NServiceBus container adapters supported [automatic property injection](/nservicebus/dependency-injection/extensions-dependencyinjection.md#property-injection), this is not possible via `Microsoft.Extensions.DependencyInjection` and must be explicitly enabled using the native API of containers that supports it.

## Support for external logging providers

Support for external logging providers is no longer provided by NServiceBus adapters for each logging framework. Instead, the [`NServiceBus.Extensions.Logging` package](/nservicebus/logging/extensions-logging.md) provides the ability to use any logging provider that conforms to the `Microsoft.Extensions.Logging` abstraction.

The following provider packages will no longer be provided:

* [Common.Logging](/nservicebus/logging/common-logging.md)
* [Log4net](/nservicebus/logging/log4net.md)
* [NLog](/nservicebus/logging/nlog.md)

## New gateway persistence API

The NServiceBus gateway has been moved to a separate `NServiceBus.Gateway` package and all gateway public APIs in NServiceBus are obsolete and will produce the following warning:

> Gateway persistence has been moved to the NServiceBus.Gateway dedicated package. Will be treated as an error from version 8.0.0. Will be removed in version 9.0.0.

### How to upgrade

- Install the desired gateway persistence package. Supported packages are:
  - [NServiceBus.Gateway.Sql](https://www.nuget.org/packages/NServiceBus.Gateway.Sql)
  - [NServiceBus.Gateway.RavenDB](https://www.nuget.org/packages/NServiceBus.Gateway.RavenDB)
- Configure the gateway API by invoking the `endpointConfiguration.Gateway(...)` method, passing as an argument the selected storage configuration instance:
  - [Documentation for NServiceBus.Gateway.Sql](/nservicebus/gateway/sql/)
  - [Documentation for NServiceBus.Gateway.RavenDB](/nservicebus/gateway/ravendb/)
