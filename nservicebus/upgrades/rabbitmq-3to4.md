---
title: RabbitMQ Transport Upgrade Version 3 to 4
summary: Instructions on how to upgrade RabbitMQ Transport Version 3 to 4.
reviewed: 2016-09-09
tags:
 - upgrade
 - migration
related:
 - nservicebus/rabbitmq
 - nservicebus/upgrades/5to6
 - nservicebus/rabbitmq/connection-settings
---


## Connection string options

When upgrading, there are several [connection string options](/nservicebus/rabbitmq/connection-settings.md) that should be removed from any existing connection strings.


### PrefetchCount

The [consumer prefetch count](http://www.rabbitmq.com/amqp-0-9-1-reference.html#basic.qos.prefetch-count) is no longer controlled by the `PrefetchCount` setting. Instead, to better integrate with the new concurrency model, the value passed to `EndpointConfiguration.LimitMessageProcessingConcurrencyTo` is used to control the prefetch count. See [Tuning](/nservicebus/operations/tuning.md).

snippet:3to4rabbitmq-config-prefetch-count-replacement


### DequeueTimeout

The `DequeueTimeout` setting has been removed because the message pump no longer polls for incoming messages, so there is no need for a timeout on how long it should block while waiting for a new message.


### MaxWaitTimeForConfirms

The `MaxWaitTimeForConfirms` setting has been removed because the transport no longer requires a timeout for how long it should block while waiting for publisher confirmation messages.

### UsePublisherConfirms

The `UsePublisherConfirms` setting has been removed from connection string and added as a code API `EndpointConfiguration.UsePublisherConfirms` method.

snippet:3to4rabbitmq-use-publisher-confirms

## Callback support

[Callbacks](/nservicebus/rabbitmq/callbacks.md) are no longer directly managed by the RabbitMQ transport, so the settings related to the callback receiver queue have been removed.


### DisableCallbackReceiver

This setting has been removed because the RabbitMQ transport no longer directly creates a callback receiver queue.


### CallbackReceiverMaxConcurrency

This setting has been removed because the RabbitMQ transport no longer directly creates a callback receiver queue. When callbacks have been enabled by installing the `NServiceBus.Callbacks` NuGet package, the maximum concurrency is no longer separately controlled. The value passed to `EndpointConfiguration.LimitMessageProcessingConcurrencyTo` will be used for the callbacks queue in addition to the main queue.

snippet:3to4rabbitmq-config-callbackreceiver-thread-count


## Providing a custom connection manager

The ability to [provide a custom connection manager](/nservicebus/rabbitmq/connection-settings.md) via the `IManageRabbitMqConnections` interface has been removed. Connections are now managed internally by the transport in a way that is not extensible.


## Controlling behavior when the broker connection is lost

The XML configuration options for [controlling lost connection behavior](/nservicebus/rabbitmq/connection-settings.md#controlling-behavior-when-the-broker-connection-is-lost) have been removed.


### TimeToWaitBeforeTriggering

The 'TimeToWaitBeforeTriggering` setting can now be configured via the following:

snippet:3to4rabbitmq-custom-breaker-settings-time-to-wait-before-triggering


### DelayAfterFailure

The `DelayAfterFailure` setting has been removed because the message pump no longer polls for incoming messages, so there is no inner loop that needs to pause when a connection failure is detected.


## Routing topology

The changes to the RabbitMQ transport's [routing topologies](/nservicebus/rabbitmq/routing-topology.md) are listed below.


### [Direct Routing Topology](/nservicebus/rabbitmq/routing-topology.md#direct-routing-topology)

The `UseDirectRoutingTopology` method's `exchangeNameConvention` parameter's type was changed from `Func<Address, Type, string>` to `Func<string, Type, string>`.


### [Custom Routing Topology](/nservicebus/rabbitmq/routing-topology.md#custom-routing-topology)

The following changes have been made to the `IRoutingTopology` interface:

 * The interface's namespace was changed from `NServiceBus.Transports.RabbitMQ.Routing` to `NServiceBus.Transport.RabbitMQ`.
 * The `Publish` method's `message` parameter's type changed from `TransportMessage` to `OutgoingMessage`.
 * The `Send` method's `message` parameter's type changed from `TransportMessage` to `OutgoingMessage`.
 * The `Send` method's `address` parameter's type changed from `Address` to `string`.
 * The `RawSendInCaseOfFailure` method was added to allow for forwarding messages that cannot be deserialized to the error queue.
