---
title: RabbitMQ Transport Upgrade Version 3 to 4
summary: Instructions on upgrading the RabbitMQ transport from version 3 to 4.
reviewed: 2022-10-31
component: Rabbit
related:
 - nservicebus/upgrades/5to6
 - transports/rabbitmq/connection-settings
redirects:
 - nservicebus/upgrades/rabbitmq-3to4
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Connection string options

When upgrading, there are several connection string options that should be removed from existing connection strings.


### DequeueTimeout

The `DequeueTimeout` setting has been removed because the message pump no longer polls for incoming messages; there is no need for a timeout on how long it should block while waiting for a new message.


### PrefetchCount

The [consumer prefetch count](https://www.rabbitmq.com/amqp-0-9-1-reference.html#basic.qos.prefetch-count) is no longer controlled by the `PrefetchCount` setting. Instead, the prefetch count is calculated by setting it to a multiple of the [maximum concurrency](/nservicebus/operations/tuning.md) value. The multiplier used in the calculation can be changed.

```csharp
endpointConfiguration.LimitMessageProcessingConcurrencyTo(10);
var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
// Prefetch count set to 10 * 4 = 40
transport.PrefetchMultiplier(4);
```

Alternatively, the calculation can be overridden and prefetch count can be set directly.

```csharp
var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
transport.PrefetchCount(100);
```


### UsePublisherConfirms

The `UsePublisherConfirms` setting has been replaced by the following:

```csharp
var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
transport.UsePublisherConfirms(true);
```


### MaxWaitTimeForConfirms

The `MaxWaitTimeForConfirms` setting has been removed because the transport no longer requires a timeout for how long it should block while waiting for publisher confirmation messages.


## Callback support

Callbacks are no longer directly managed by the RabbitMQ transport. The settings related to the callback receiver queue have been removed.


### DisableCallbackReceiver

This setting has been removed because the RabbitMQ transport no longer directly creates a callback receiver queue.


### CallbackReceiverMaxConcurrency

The `CallbackReceiverMaxConcurrency` setting has been removed because the RabbitMQ transport no longer directly creates a callback receiver queue. When callbacks have been enabled by installing the `NServiceBus.Callbacks` NuGet package, the maximum concurrency is no longer separately controlled. The value passed to `EndpointConfiguration.LimitMessageProcessingConcurrencyTo` will be used for the callbacks queue in addition to the main queue.

```csharp
// For RabbitMQ Transport version 4.x
endpointConfiguration.LimitMessageProcessingConcurrencyTo(10);

// For RabbitMQ Transport version 3.x
var transport = busConfiguration.UseTransport<RabbitMQTransport>();
transport.CallbackReceiverMaxConcurrency(10);
```


## Providing a custom connection manager

The ability to provide a custom connection manager via the `IManageRabbitMqConnections` interface has been removed. Connections are now managed internally by the transport in a way that is not extensible.


## Controlling behavior when the broker connection is lost

The XML configuration options for controlling lost connection behavior have been removed.


### TimeToWaitBeforeTriggering

The `TimeToWaitBeforeTriggering` setting can now be configured as follows:

```csharp
var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
transport.TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(2));
```


### DelayAfterFailure

The `DelayAfterFailure` setting has been removed because the message pump no longer polls for incoming messages; there is no inner loop that needs to pause when a connection failure is detected.


## Routing topology

The changes to the RabbitMQ transport's [routing topologies](/transports/rabbitmq/routing-topology.md) are listed below.


### [Direct routing topology](/transports/rabbitmq/routing-topology.md#direct-routing-topology)

The type for the `UseDirectRoutingTopology` method's `exchangeNameConvention` parameter was changed from `Func<Address, Type, string>` to `Func<string, Type, string>`.


### [Custom routing topology](/transports/rabbitmq/routing-topology.md#custom-routing-topology)

The following changes have been made to the `IRoutingTopology` interface:

 * The interface's namespace was changed from `NServiceBus.Transports.RabbitMQ.Routing` to `NServiceBus.Transport.RabbitMQ`.
 * The type for the `Publish` method's `message` parameter changed from `TransportMessage` to `OutgoingMessage`.
 * The type for the `Send` method's `message` parameter changed from `TransportMessage` to `OutgoingMessage`.
 * The type for the `Send` method's `address` parameter changed from `Address` to `string`.
 * A `RawSendInCaseOfFailure` method was added to allow for forwarding messages that cannot be deserialized to the error queue.
