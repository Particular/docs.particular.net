---
title: RabbitMQ Transport Upgrade Version 4 to 5
summary: Instructions on how to upgrade RabbitMQ Transport Version 4 to 5.
reviewed: 2017-09-07
component: Rabbit
related:
 - nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

## Mandatory routing topology selection

Specifying a routing topology is now mandatory. For backwards compatibility, the recommendation is to use the `ConventionalRoutingTopology`, which was the previous default. 

See the [routing topology](/transports/rabbitmq/routing-topology.md) for further details.


## ConnectionStringName

`ConnectionStringName` has been deprecated and can be replaced with a combination of [ConfigurationManager.ConnectionStrings](https://msdn.microsoft.com/en-us/library/system.configuration.configurationmanager.connectionstrings.aspx) and [setting via code](/transports/rabbitmq/connection-settings.md#specifying-the-connection-string-via-code)

snippet: 4to5CustomConnectionStringName


## Direct routing topology changes

The convention for overriding the name of the exchange used when publishing events has changed. The address and the event type are no longer passed to the `exchangeNameConvention` parameter of the [UseDirectRoutingTopology](/transports/rabbitmq/routing-topology.md#direct-routing-topology-enabling-direct-routing-topology) method.

snippet: 4to5usedirectroutingtopology


## Members added to IRoutingTopology

Members have changed in `IRoutingTopology`:

 * `Initialize` has had extra parameters added.
 * `BindToDelayInfrastructure` method has been added.

See [custom routing topology](/transports/rabbitmq/routing-topology.md#custom-routing-topology) for updated implementation instructions.
