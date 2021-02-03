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

## Dependency injection

Support for external dependency injection containers is no longer provided by NServiceBus adapters for each container library. Instead, NServiceBus version 8 directly provides the ability to use any container that conforms to the `Microsoft.Extensions.DependencyInjection.Abstractions` container abstraction. Visit the dedicated [dependency injection changes](/nservicebus/upgrades/7to8/dependency-injection.md) section of the upgrade guide for further information.


## Support for external logging providers

Support for external logging providers is no longer provided by NServiceBus adapters for each logging framework. Instead, the [`NServiceBus.Extensions.Logging` package](/nservicebus/logging/extensions-logging.md) provides the ability to use any logging provider that conforms to the `Microsoft.Extensions.Logging` abstraction.

The following provider packages will no longer be provided:

* [Common.Logging](/nservicebus/logging/common-logging.md)
* [Log4net](/nservicebus/logging/log4net.md)
* [NLog](/nservicebus/logging/nlog.md)

## CancellationToken support

NServiceBus version 8 supports cooperative cancellation using `CancellationToken`.

### Non-message-handling contexts

Methods on extension points are updated to include a mandatory `CancellationToken` parameter. This includes abstract classes and interfaces needed to implement a message transport or persistence libary, as well as other extension points like `IDataBus`, `FeatureStartupTask`, `INeedToInstallSomething`. Implementors can be updated by adding the `CancellationToken` parameter to the end of the method signature.

Methods used outside the message processing pipeline now include an optional `CancellationToken` parameter, including methods for starting and stopping endpoints, and methods to Send/Publish messages from outside a message processing pipeline, such as from within a web application.

It is recommended to forward a `CancellationToken` within a method to any method that accepts one. For example, within a web application controller:

```csharp
public class TestController
{
    private IMessageSession session;

    public TestController(IMessageSession session)
    {
        this.session = session;
    }

    [HttpGet("/test")]
    public async Task<string> Get(CancellationToken cancellationToken)
    {
        // Forward the cancellation token to methods that accept one
        await Task.Delay(10_000, cancellationToken);

        return "Finished 10s delay";
    }
}
```

[Enabling .NET source code analysis](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview) (enabled by default in projects targeting .NET 5 or above) is also recommended so that [CA2016: Forward the CancellationToken parameter to methods that take one](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2016) can identify locations where the cancellation token should be forwarded. This rule is presesented as an informational message only, but the [analyzer severity](https://docs.microsoft.com/en-us/visualstudio/code-quality/use-roslyn-analyzers#configure-severity-levels) can be upgraded to a warning using an [.editorconfig file](https://editorconfig.org/):

```ini
[*.cs]
dotnet_diagnostic.CA2016.severity = warning
```

### Message handlers and pipeline

Inside a message handler, a cancellation token from the incoming message processing pipeline is available on the `IMessageHandlerContext` as the property `context.CancellationToken`. The cancellation token was added to the context parameter to avoid making a breaking change to `IHandleMessages` affecting all message handlers and sagas.

Similarly, [pipeline behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md) also contain a `CancellationToken` property on their respective `context` parameters.

Methods on `IMessageHandlerContext` such as `Send()` and `Publish()` do not accept a cancellation token, as the token from the incoming message pipeline will be routed to the outgoing operations transparently.

It is recommended to forward the `context.CancellationToken` to any other method that accepts one. A new analyzer identifies locations where the token with a build warning, for example:

```csharp
public class SampleHandler : IHandleMessages<TestMessage>
{
    public async Task Handle(TestMessage message, IMessageHandlerContext context)
    {
        // Analyzer Warning NSB0002: Forward `context.CancellationToken` to the `Delay` method.
        await Task.Delay(10_000);
    }
}
```

The analyzer also offers a code fix that will update the code to forward the token using the "light bulb" menu. ( <kbd>Ctrl</kbd> + <kbd>.</kbd> )

If cancellation is not a major concern, the [analyzer severity](https://docs.microsoft.com/en-us/visualstudio/code-quality/use-roslyn-analyzers#configure-severity-levels) can be downgraded using an [.editorconfig file](https://editorconfig.org/):

```ini
[*.cs]
dotnet_diagnostic.NSB0002.severity = suggestion
```

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

 NServiceBus version 8 will no longer attempt to load the license file from the `appSettings` section of an app.config or web.config file, in order to create better alignment between .NET Framework 4.x and .NET Core.

In NServiceBus version 7 and below, the license path could be loaded from the `NServiceBus/LicensePath` app setting, or the license text itself could be loaded from the `NServiceBus/License` app setting.

Starting in NServiceBus version 8, one of the [other methods of providing a license](/nservicebus/licensing/?version=core_8) must be used.


## Support for message forwarding

NServiceBus no longer natively supports forwarding a copy of every message processed by an endpoint. Instead, create a custom behavior to forward a copy of every procesed message as described in [this sample](/samples/routing/message-forwarding).


## NServiceBus Host

The `NServiceBus.Host` package is deprecated. See the [NServiceBus Host upgrade guide](/nservicebus/upgrades/host-7to8.md) for details and alternatives.


## NServiceBus Azure Host

The `NServiceBus.Hosting.Azure` and `NServiceBus.Hosting.Azure.HostProcess` are deprecated.See the [NServiceBus Azure Host upgrade guide](/nservicebus/upgrades/acs-host-7to8.md) for details and alternatives.

## DateTimeOffset instead of DateTime

Usage of `DateTime` can result in numerous issues caused by misalignment of timezone offsets, which can lead to time calculation errors. Although a `DateTime.Kind` property exists, it is often ignored during DateTime math and it is up to the user to ensure values are aligned in their offset. The `DateTimeOffset` type fixes this. It does not contain any timezone information, only an offset, which is sufficient to get the time calculations right. 

[> These uses for DateTimeOffset values are much more common than those for DateTime values. As a result, DateTimeOffset should be considered the default date and time type for application development."](https://docs.microsoft.com/en-us/dotnet/standard/datetime/choosing-between-datetime)

In NServiceBus version 8, all APIs have been migrated from `DateTime` to `DateTimeOffset`.

## NServiceBus Scheduler

In NServiceBus version 8, the Scheduler API has been deprecated in favor of options like [sagas](/nservicebus/sagas/) and production-grade schedulers such as Hangfire, Quartz, and FluentScheduler.

It is recommended to create a .NET Timer with the same interval as the scheduled task and use `IMessageSession.SendLocal` to send a message to process. Using message processing has the benefit of using recoverability and uses a transactional context. If these benefits are not needed then do not send a message at all and directly invoke logic from the timer.

INFO: The behavior in NServiceBus version 7 is to **not** retry the task on failures, so be sure to wrap the business logic in a `try-catch` statement to get the same behavior in NServicebus version 8.

See the [scheduling with .NET Timers sample](/samples/scheduling/timer) for more details.

## Meaningful exceptions when stopped

NServiceBus version 8 throws an `InvalidOperationException` when invoking message opererations on `IMessageSession` when the endpoint instance is stopping or stopped to indicate that the instance can no longer be used.

## Non-durable messaging

Support for non-durable messaging has been moved to the transports that can support it, which as of November 2020 is only the RabbitMQ transport. When using another transport, use of `[Express]` or message conventions to request non-durable delivery can safely be removed.

RabbitMQ user should use the new [`options.UseNonPersistentDeliveryMode()` API provided by `NServiceBus.RabbitMQ` Version 7](/transports/rabbitmq/#controlling-delivery-mode)

## Timeout manager removed

With all currently-supported transports now supporting native delayed delivery, the [timeout manager](/nservicebus/messaging/timeout-manager.md) is no longer needed. Any calls to `EndpointConfiguration.TimeoutManager()` and `EndpointConfiguration.UseExternalTimeoutManager()` can safely be removed.

### Data migration

Using a transport that previously relied on the timeout manager may require a migration of existing timeouts. Use the [timeouts migration tool](/nservicebus/tools/migrate-to-native-delivery.md) to detect and migrate timeouts as needed.

The following transports might need migration:

* RabbitMQ
* Azure Storage Queues
* SQL Transport
* SQS
