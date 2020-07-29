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


## Error notification events

In NServiceBus version 7.2, error notification events for `MessageSentToErrorQueue`, `MessageHasFailedAnImmediateRetryAttempt`, and `MessageHasBeenSentToDelayedRetries` using .NET events were deprecated in favor of `Task`-based callbacks. In NServiceBus version 8 and above, the event-based notifications will throw an error.

Error notifications can be set with the `Task`-based callbacks through the recoverability settings:

snippet: SubscribeToErrorsNotifications-UpgradeGuide


## Disabling subscriptions

In previous versions, users sometimes disabled the `MessageDrivenSubscriptions` feature to remove the need for a subscription storage on endpoints that do not publish events, which could cause other unintended consequences.

While NServiceBus still supports message-driven subscriptions for transports that do not have native publish/subscribe capabilities, the `MessageDrivenSubscriptions` feature itself has been deprecated.

To disable publishing on an endpoint, the declarative API should be used instead:

snippet: DisablePublishing-UpgradeGuide


## Connection strings

Configuring a transport's connection using `.ConnectionStringName(name)`, which was removed for .NET Core in NServiceBus version 7, has been removed all platforms in NServiceBus version 8. To continue to retrieve the connection string by the named value in the configuration, first retrieve the connection string and then pass it to the `.ConnectionString(value)` configuration.

A connection string named `NServiceBus/Transport` will also **no longer be detected automatically** on any platform. The connection string value must be configured explicitly using `.ConnectionString(value)`.


## Change to license file locations

 will no longer attempt to load the license file from the `appSettings` section of an app.config or web.config file, in order to create better alignment between .NET Framework 4.x and .NET Core.

In NServiceBus version 6 and below, the license path could be loaded from the `NServiceBus/LicensePath` app setting, or the license text itself could be loaded from the `NServiceBus/License` app setting.

Starting in NServiceBus version 8, one of the [other methods of providing a license](/nservicebus/licensing/?version=core_8) must be used.