---
title: RabbitMQ Transport Upgrade Version 3 to 4
summary: Instructions on how to upgrade the RabbitMQ transport from version 3 to 4.
reviewed: 2018-05-17
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

When upgrading, there are several [connection string options](/transports/rabbitmq/connection-settings.md?version=rabbit_3#connection-string-options) that should be removed from existing connection strings.


### DequeueTimeout

The `DequeueTimeout` setting has been removed because the message pump no longer polls for incoming messages; there is no need for a timeout on how long it should block while waiting for a new message.


### PrefetchCount

The [consumer prefetch count](http://www.rabbitmq.com/amqp-0-9-1-reference.html#basic.qos.prefetch-count) is no longer controlled by the `PrefetchCount` setting. Instead, the prefetch count is calculated by setting it to a multiple of the [maximum concurrency](/nservicebus/operations/tuning.md#tuning-concurrency) value. The multiplier used in the calculation can be changed.

snippet: 3to4rabbitmq-config-prefetch-multiplier

Alternatively, the calculation can be overridden and prefetch count can be set directly.

snippet: 3to4rabbitmq-config-prefetch-count


### UsePublisherConfirms

The `UsePublisherConfirms` setting has been replaced by the following: 

snippet: 3to4rabbitmq-use-publisher-confirms


### MaxWaitTimeForConfirms

The `MaxWaitTimeForConfirms` setting has been removed because the transport no longer requires a timeout for how long it should block while waiting for publisher confirmation messages.


## Callback support

[Callbacks](/transports/rabbitmq/callbacks.md?version=rabbit_3) are no longer directly managed by the RabbitMQ transport; the settings related to the callback receiver queue have been removed.


### DisableCallbackReceiver

This setting has been removed because the RabbitMQ transport no longer directly creates a callback receiver queue.


### CallbackReceiverMaxConcurrency

The `CallbackReceiverMaxConcurrency` setting has been removed because the RabbitMQ transport no longer directly creates a callback receiver queue. When callbacks have been enabled by installing the `NServiceBus.Callbacks` NuGet package, the maximum concurrency is no longer separately controlled. The value passed to `EndpointConfiguration.LimitMessageProcessingConcurrencyTo` will be used for the callbacks queue in addition to the main queue.

snippet: 3to4rabbitmq-config-callbackreceiver-thread-count


## Providing a custom connection manager

The ability to [provide a custom connection manager](/transports/rabbitmq/connection-settings.md?version=rabbit_3#providing-a-custom-connection-manager) via the `IManageRabbitMqConnections` interface has been removed. Connections are now managed internally by the transport in a way that is not extensible.


## Controlling behavior when the broker connection is lost

The XML configuration options for [controlling lost connection behavior](/transports/rabbitmq/connection-settings.md?version=rabbit_3#controlling-behavior-when-the-broker-connection-is-lost) have been removed.


### TimeToWaitBeforeTriggering

The `TimeToWaitBeforeTriggering` setting can now be configured as follows:

snippet: 3to4rabbitmq-custom-breaker-settings-time-to-wait-before-triggering


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
