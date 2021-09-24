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

## Transport configuration

NServiceBus version 8 comes with a new transport configuration API. Instead of the generic-based `UseTransport<TTransport>` method, create an instance of the transport's configuration class and pass it to the the `UseTransport` method. For example, instead of:

```csharp
var transport = endpointConfiguration.UseTransport<MyTransport>();

transport.Transactions(TransportTransactionMode.ReceiveOnly);
var routing = t.Routing();
routing.RouteToEndpoint(typeof(MyMessage), "DestinationEndpoint");
```

use:

```csharp
var transport = new MyTransport{
    TransportTransactionMode = TransportTransactionMode.ReceiveOnly
};

var routing = endpointConfiguration.UseTransport(transport);
routing.RouteToEndpoint(typeof(MyMessage), "DestinationEndpoint");
```

See the [transport upgrade guide](/nservicebus/upgrades/7to8/transport.md) for further details.

Note: The existing API surface is supported for NServiceBus version 8 via a [shim API](https://en.wikipedia.org/wiki/Shim_(computing)) to ease migration to the new version. However, it is recommended to switch to the new transport configuration API to prepare for future upgrades of NServiceBus.

## Dependency injection

NServiceBus no longer provides adapters for external dependency injection containers. Instead, NServiceBus version 8 directly provides the ability to use any container that conforms to the `Microsoft.Extensions.DependencyInjection` container abstraction. Visit the dedicated [dependency injection changes](/nservicebus/upgrades/7to8/dependency-injection.md) section of the upgrade guide for further information.

## Support for external logging providers

Support for external logging providers is no longer provided by NServiceBus adapters for each logging framework. Instead, the [`NServiceBus.Extensions.Logging` package](/nservicebus/logging/extensions-logging.md) provides the ability to use any logging provider that conforms to the `Microsoft.Extensions.Logging` abstraction.

The following provider packages will no longer be provided:

* [Common.Logging](/nservicebus/logging/common-logging.md)
* [Log4net](/nservicebus/logging/log4net.md)
* [NLog](/nservicebus/logging/nlog.md)

## CancellationToken support

NServiceBus version 8 supports [cooperative cancellation](/nservicebus/hosting/cooperative-cancellation.md) using `CancellationToken` parameters. Where appropriate, optional `CancellationToken` parameters have been added to public methods. This includes the abstract classes and interfaces required to implement a message transport or persistence library, and other extension points like `IDataBus`, `FeatureStartupTask`, and `INeedToInstallSomething`. Implementers can be updated by adding an optional `CancellationToken` parameter to the end of method signatures. The change also includes callbacks that customize the behavior of NServiceBus:

* when a [critical error](/nservicebus/hosting/critical-errors.md) is encountered
* when a message is retried
* when a message is moved to the error queue
* when a message is processed

## Shutdown behavior

In all versions of NServiceBus, `endpoint.Stop()` immediately stops receiving new messages but waits for a period of time for currently running message handlers to complete.

In NServiceBus version 7 and below, the MSMQ, Azure Service Bus, RabbitMQ, Azure Storage Queues, and SQL transports allow handlers up to 30 seconds to complete before forcing an endpoint shutdown. In NServiceBus version 8, all transports block shutdown until all handlers complete or the [transport transaction](/transports/transactions.md) times out.

The `CancellationToken` available on the `IMessageHandlerContext` in NServiceBus version 8 is triggered when the host forces shutdown. For example, by default, the .NET Generic Host signals the cancellation token after 5 seconds.

However, if handlers do not observe the cancellation token, they will be allowed to complete before the endpoints shuts down. This means that if, for example, a handler calls `Task.Delay(TimeSpan.Infinite)`, the endpoint will never shut down.

Therefore, it is recommended that all message handlers observe the cancellation token to enable forced shutdown when required.

## New gateway persistence API

The NServiceBus gateway has been moved to a separate `NServiceBus.Gateway` package and all gateway public APIs in NServiceBus are obsolete and will produce the following warning:

> Gateway persistence has been moved to the NServiceBus.Gateway dedicated package. Will be treated as an error from version 8.0.0. Will be removed in version 9.0.0.

### How to upgrade

* Install the desired gateway persistence package. Supported packages are:
  * [NServiceBus.Gateway.Sql](https://www.nuget.org/packages/NServiceBus.Gateway.Sql)
  * [NServiceBus.Gateway.RavenDB](https://www.nuget.org/packages/NServiceBus.Gateway.RavenDB)
* Configure the gateway API by invoking the `endpointConfiguration.Gateway(...)` method, passing as an argument the selected storage configuration instance:
  * [Documentation for NServiceBus.Gateway.Sql](/nservicebus/gateway/sql/)
  * [Documentation for NServiceBus.Gateway.RavenDB](/nservicebus/gateway/ravendb/)

## Error notification events

In NServiceBus version 7.2, error notification events for `MessageSentToErrorQueue`, `MessageHasFailedAnImmediateRetryAttempt`, and `MessageHasBeenSentToDelayedRetries` using .NET events were deprecated in favor of `Task`-based callbacks. In NServiceBus version 8 and above, the event-based notifications will throw an error.

Error notifications can be set with the `Task`-based callbacks through the recoverability settings:

snippet: SubscribeToErrorsNotifications-UpgradeGuide

## Disabling subscriptions

In previous versions, users sometimes disabled the `MessageDrivenSubscriptions` feature to remove the need for a subscription storage on endpoints that do not publish events, which could cause other unintended consequences.

While NServiceBus still supports message-driven subscriptions for transports that do not have native publish/subscribe capabilities, the `MessageDrivenSubscriptions` feature itself has been deprecated.

To disable publishing on an endpoint, the declarative API should be used instead:

snippet: DisablePublishing-UpgradeGuide

## Change to license file locations

 NServiceBus version 8 will no longer attempt to load the license file from the `appSettings` section of an app.config or web.config file, in order to create better alignment between .NET Framework 4.x and .NET Core.

In NServiceBus version 7 and below, the license path could be loaded from the `NServiceBus/LicensePath` app setting, or the license text itself could be loaded from the `NServiceBus/License` app setting.

Starting in NServiceBus version 8, one of the [other methods of providing a license](/nservicebus/licensing/?version=core_8) must be used.

## Support for message forwarding

NServiceBus no longer natively supports forwarding a copy of every message processed by an endpoint. Instead, create a custom behavior to forward a copy of every processed message as described in [message forwarding sample](/samples/routing/message-forwarding).

## NServiceBus Host

The `NServiceBus.Host` package is deprecated. See the [NServiceBus Host upgrade guide](/nservicebus/upgrades/host-7to8.md) for details and alternatives.

## NServiceBus Azure Host

The `NServiceBus.Hosting.Azure` and `NServiceBus.Hosting.Azure.HostProcess` are deprecated.See the [NServiceBus Azure Host upgrade guide](/nservicebus/upgrades/acs-host-7to8.md) for details and alternatives.

## Overriding the host machine name

`RuntimeEnvironment.MachineNameAction` is deprecated. The host machine name of an endpoint instance may be overridden using:

```csharp
endpointConfiguration.UniquelyIdentifyRunningInstance()
    .UsingHostName("NewMachineName");`
```

## DateTimeOffset instead of DateTime

Usage of `DateTime` can result in numerous issues caused by misalignment of time zone offsets, which can lead to time calculation errors. Although a `DateTime.Kind` property exists, it is often ignored during DateTime math and it is up to the user to ensure values are aligned in their offset. The `DateTimeOffset` type fixes this. It does not contain any time zone information, only an offset, which is sufficient to get the time calculations right.

[> These uses for DateTimeOffset values are much more common than those for DateTime values. As a result, DateTimeOffset should be considered the default date and time type for application development."](https://docs.microsoft.com/en-us/dotnet/standard/datetime/choosing-between-datetime)

In NServiceBus version 8, all APIs have been migrated from `DateTime` to `DateTimeOffset`.

## NServiceBus Scheduler

In NServiceBus version 8, the Scheduler API has been deprecated in favor of options like [sagas](/nservicebus/sagas/) and production-grade schedulers such as Hangfire, Quartz, and FluentScheduler.

It is recommended to create a .NET Timer with the same interval as the scheduled task and use `IMessageSession.SendLocal` to send a message to process. Using message processing has the benefit of using recoverability and uses a transactional context. If these benefits are not needed then do not send a message at all and directly invoke logic from the timer.

INFO: The behavior in NServiceBus version 7 is to **not** retry the task on failures, so be sure to wrap the business logic in a `try-catch` statement to get the same behavior in NServiceBus version 8.

See the [scheduling with .NET Timers sample](/samples/scheduling/timer) for more details.

## Meaningful exceptions when stopped

NServiceBus version 8 throws an `InvalidOperationException` when invoking message operations on `IMessageSession` when the endpoint instance is stopping or stopped to indicate that the instance can no longer be used.

## Non-durable messaging

Support for non-durable messaging has been moved to the transports that can support it, which as of November 2020 is only the RabbitMQ transport. When using another transport, use of `[Express]` or message conventions to request non-durable delivery can safely be removed.

RabbitMQ user should use the new [`options.UseNonPersistentDeliveryMode()` API provided by `NServiceBus.RabbitMQ` Version 7](/transports/rabbitmq/#controlling-delivery-mode)

## Non-durable persistence

Support for non-durable persistence (previously known as `InMemoryPersistence`) has been removed from the `NServiceBus` package to a separate `NServiceBus.Persistence.NonDurable` package. To continue using it, add a reference to the new package and update the configuration code.

```csharp
endpointConfiguration.UsePersistence<NonDurablePersistence>();
```

## Timeout manager removed

With all currently-supported transports now supporting native delayed delivery, the [timeout manager](/nservicebus/messaging/timeout-manager.md) is no longer needed. Any calls to `EndpointConfiguration.TimeoutManager()` and `EndpointConfiguration.UseExternalTimeoutManager()` can safely be removed.

### Data migration

Using a transport that previously relied on the timeout manager may require a migration of existing timeouts. Use the [timeouts migration tool](/nservicebus/tools/migrate-to-native-delivery.md) to detect and migrate timeouts as needed.

The following transports might need migration:

* RabbitMQ
* Azure Storage Queues
* SQL Transport
* SQS
* MSMQ

### IManageUnitOfWork

`IManageUnitOfWork` interface is no longer recommended. The unit of work pattern is more straightforward to implement in a pipeline behavior, where the using keyword and try/catch blocks can be used.

[Custom unit of work sample](/samples/pipeline/unit-of-work/) is an example of the the recommended approach.

## Outbox configuration

NServiceBus version 8 requires the transport transaction mode to be set explicitly to `ReceiveOnly` when using the [outbox](/nservicebus/outbox/) feature:

```csharp
var transport = endpointConfiguration.UseTransport<MyTransport>();

transport.TransportTransactionMode = TransportTransactionMode.ReceiveOnly;

endpointConfiguration.EnableOutbox();
```

## Message mutators with Dependancy Injection (DI)

Message mutators that operate on serialized messages (`IMutateIncomingTransportMessages` and `IMutateOutgoingTransportMessages`) in NServiceBus version 8 represent the message payload as `ReadOnlyMemory<byte>` instead of `byte[]`. Therefore, it is no longer possible to change individual bytes. Instead, a modified copy of the payload must be provided.

In NServiceBus version 7 and below message mutators could be registered in two ways: using a dedicated `endpointConfiguration.RegisterMessageMutator` API and via a dependency injection container. In version 8 only the dedicated API is supported. Mutators registered in the container are ignored.

## AbortReceiveOperation

`ITransportReceiveContext.AbortReceiveOperation` has been deprecated in favor of throwing an [`OperationCanceledException`](https://docs.microsoft.com/en-us/dotnet/api/system.operationcanceledexception). This will preserve the NServiceBus version 7 behavior of immediately retrying the message without invoking [recoverability](/nservicebus/recoverability).

## Renamed extension method types

The following static extension method types were renamed:

| Old name                       | New name                      |
|--------------------------------|-------------------------------|
| `IEndpointInstanceExtensions`  | `EndpointInstanceExtensions`  |
| `IMessageProcessingExtensions` | `MessageProcessingExtensions` |
| `IMessageSessionExtensions`    | `MessageSessionExtensions`    |
| `IPipelineContextExtensions`   | `PipelineContextExtensions`   |

All references to the old types must be changed to the new types, although usually these types are not referenced, since they only contain extension methods.

## Removing a behavior from the pipeline is obsolete

The `Remove` command is no longer available in `PipelineSettings`. In order to disable a behavior, [replace the behavior](/nservicebus/pipeline/manipulate-with-behaviors.md?version=core_8#disable-an-existing-step) with an empty one.

## Gateway in-memory deduplication

The `InMemoryDeduplicationConfiguration` type within the NServiceBus.Gateway package has been renamed to `NonDurableDeduplicationConfiguration`.

## Pipeline delivery constraints

The `TryGetDeliveryConstraint` method on the `NServiceBus.Extensibility.ContextBag` property has been removed. In order to access delivery constraints from within the pipeline, use `NServiceBus.Extensibility.ContextBag.TryGet` instead.

```csharp
var extensions = context.Extensions;
if (extensions.TryGet(out DiscardIfNotReceivedBefore constraint))
{
    timeToBeReceived = constraint.MaxTime;
}
```

## Dependency on System.Memory package for .NET Framework

Memory allocations for incoming and outgoing messages bodies are reduced by using the low allocation memory types via the System.Memory namespace. These type are available on .NET Framework via the **System.Memory** package. The NServiceBus build that targets .NET Framework has this dependency added.

### Pipeline context types changes

Throughput the pipeline all contexts have been updated to use `ReadOnlyMemory<byte>` (e.g. context for behaviors, stage connectors, and message mutators) and affects the following types:

* MutateIncomingTransportMessageContext
* MutateOutgoingTransportMessageContext
* ConnectorContextExtensions
* IIncomingPhysicalMessageContext
* IncomingPhysicalMessageContext
* IOutgoingPhysicalMessageContext
* OutgoingPhysicalMessageContext
* SerializeMessageConnector

## Message bodies only available in scope of incoming or outgoing messages

Messages bodies available via `ReadOnlyMemory<byte>` are only valid while in scope of incoming or outgoing message processing. Once out of scope the data referenced is no longer valid.

If access is still required outside the processing scope it is required to copy the message body while in scope.

### Message Mutators: Updating of message bodies

The message mutator API for changing the message body has changed. Instead, of `UpdateMessage(byte[] body)` method `MutateIncomingTransportMessageContext` and `MutateOutgoingTransportMessageContext` expose `Body` property of type `ReadOnlyMemory<byte>`.

Message mutators that replace the whole message body might be better of to be converted to a behavior. With a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) it is possible to reduce allocations via [`ArrayPool<byte>`](https://docs.microsoft.com/en-us/dotnet/api/system.buffers.arraypool-1) or packages like [RecyclableMemoryStream](https://github.com/Microsoft/Microsoft.IO.RecyclableMemoryStream).

### Serializers: Deserialization using ReadOnlyMemory of byte

Deserialization updated to use `ReadOnlyMemory<byte>` instead of `Stream` with  on `Deserialize` API method on `IMessageSerializer`.

```csharp
object[] Deserialize(ReadOnlyMemory<byte> body, IList<Type> messageTypes = null);
```

If serializers do not support `ReadOnlyMemory<byte>` usage of `.ToArray` must be avoided as this will copy the data and increases memory allocations. Instead include a shim that provides read-only access to the underlying data or use the `ReadOnlyMemoryExtensions.AsStream` from the [Microsoft.Toolkit.HighPerformance](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.highperformance.extensions.readonlymemoryextensions.asstream).

## Transports

### Transports: Outgoing message TransportOperation API

The outgoing message body passed to the transport via `TransportOperation` has a new constructor:

```csharp
public TransportOperation(string messageId, DispatchProperties properties, ReadOnlyMemory<byte> body, Dictionary<string, string> headers)
```

### Transports: Incoming message MessageContext API

A message body passed by the transport to the core using `ReadOnlyMemory<byte>` instead of `byte[]`. The `MessageContext` becomes:

```csharp
public MessageContext(string nativeMessageId, Dictionary<string, string> headers, ReadOnlyMemory<byte> body, TransportTransaction transportTransaction, ContextBag context)
```

For transports that use low allocation types, this allows passing message body without additional memory allocations. Secondly, this ensures that the body passed by the transport cannot be changed by code executing in the pipeline (immutable message body). 

## Persisters

### Persister Outbox API: TransportOperation based on `ReadOnlyMemory<byte>`

The outbox `TransportOperation` is using `ReadOnlyMemory<byte>` instead of `byte[]`.
