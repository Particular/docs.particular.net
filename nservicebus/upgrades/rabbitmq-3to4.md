---
title: RabbitMQ Transport Upgrade Version 3 to 4
summary: Instructions on how to upgrade RabbitMQ Transport Version 3 to 4.
reviewed: 2016-03-24
tags:
 - upgrade
 - migration
related:
- nservicebus/rabbitmq
- nservicebus/upgrades/5to6
---


## CallbackReceiverMaxConcurrency

`CallbackReceiverMaxConcurrency` is obsolete and `LimitMessageProcessingConcurrencyTo` should be used instead.

snippet: 3to4rabbitmq-config-callbackreceiver-thread-count

See [Tuning](/nservicebus/operations/tuning.md).


## [Connection String Options](/nservicebus/rabbitmq/configuration-api.md)


### PrefetchCount


[PrefetchCount](/nservicebus/rabbitmq/configuration-api.md#configuring-rabbitmq-transport-to-be-used-prefetchcount) is obsolete and `EndpointConfiguration.LimitMessageProcessingConcurrencyTo` should be used instead. See [Tuning](/nservicebus/operations/tuning.md).


### DequeueTimeout

[DequeueTimeout](/nservicebus/rabbitmq/configuration-api.md#configuring-rabbitmq-transport-to-be-used-dequeuetimeout) is deprecated as the RabbitMQ Transport message pump does not require a timeout.

### MaxWaitTimeForConfirms

[MaxWaitTimeForConfirms](/nservicebus/rabbitmq/configuration-api.md#configuring-rabbitmq-transport-to-be-used-maxwaittimeforconfirms) is deprecated as with current implementation this setting is not used.

### Added Settings

Several new settings have been added related to Transport Layer Security (TLS) connection. See [TLS Configuration](/nservicebus/rabbitmq/configuration-api.md#specifying-the-connection-string-transport-layer-security-support) for more information.


## Circuit Breaker

Xml configuration options for controlling lost connection behavior were replaced by code equivalent.

snippet:3to4rabbitmq-custom-breaker-settings

snippet:3to4rabbitmq-custom-breaker-settings-code


## Routing


### UseDirectRoutingTopology

When using `UseDirectRoutingTopology` method parameter's type was changed from `Address` to `string`.

snippet:3to4rabbitmq-config-usedirectroutingtopology


### IRoutingTopology

When [changing routing topology](/nservicebus/rabbitmq/configuration-api.md#configuring-rabbitmq-transport-to-be-used-changing-routing-topology) some changes were introduced to `IRoutingTopology` interface.

 * `message` parameter type changed from `TransportMessage` to `OutgoingMessage`
 * `address` parameter type changed from `Address` to `string`

`IRoutingTopology` was moved to different namespace, old namespace was: `NServiceBus.Transports.RabbitMQ.Routing`, current namespace is: `NServiceBus.Transport.RabbitMQ`. 