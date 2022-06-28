---
title: RabbitMQ Transport Upgrade Version 6 to 7
summary: Instructions on how to upgrade RabbitMQ Transport from version 6 to 7.
reviewed: 2022-05-05
component: Rabbit
isUpgradeGuide: true
---

Version 7 of the RabbitMQ transport is focused on fully supporting [quorum queues](https://www.rabbitmq.com/quorum-queues.html).

## Minium broker version

Version 7 relies on quorum queue features introduced in RabbitMQ 3.10.0, so all RabbitMQ nodes are required to be on version `3.10.0` or above.

See the [minimum broker requirements documentation](/transports/rabbitmq/#broker-compatibility) for more details.

## New version of the delay infrastructure

The original delay infrastructure could lose messages due to the [lack of safety guarantees when dead-lettering messages with classic queues](https://www.rabbitmq.com/dlx.html#safety).

Version 7 introduces a new v2 delay infrastructure that uses quorum queues instead of classic queues. The v2 infrastructure can exist side-by-side with the previous infrastructure. Any messages in the original infrastructure can be migrated to the new infrastructure with the new `delays migrate` command provided in the new command line tool.

## `UseConventionalRoutingTopology` now has mandatory `QueueType` parameter

To control what type of queue the endpoint will use, a `QueueType` parameter has been added to `UseConventionalRoutingTopology`:

snippet: 6to7conventional

The parameter controls what type of queues the endpoint will create if installers are enabled.

## `UseDirectRoutingTopology` now has mandatory` QueueType` parameter

To control what type of queue the endpoint will use, a `QueueType` parameter has been added to `UseDirectRoutingTopology`:

snippet: 6to7direct

The parameter controls what type of queues the endpoint will create if installers are enabled.




- Quorum queues now full supported
  - Migration instructions
- Tooling in general
