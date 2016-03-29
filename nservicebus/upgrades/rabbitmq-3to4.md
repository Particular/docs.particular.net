---
title: Upgrade RabbitMQ Transport Version 3 to 4
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


`PrefetchCount` is obsolete and `EndpointConfiguration.LimitMessageProcessingConcurrencyTo` should be used instead. See [Tuning](/nservicebus/operations/tuning.md).


### DequeueTimeout

`DequeueTimeout` is deprecated as the RabbitMQ Transport message pump does not require a timeout.


### Added Settings

Several new settings have been added related to TLS and Certs. See [Connection String Options](/nservicebus/rabbitmq/configuration-api.md).


## Circuit Breaker

Xml configuration options for controlling lost connection behavior were replaced by code equivalent.

snippet:3to4rabbitmq-custom-breaker-settings

snippet:3to4rabbitmq-custom-breaker-settings-code