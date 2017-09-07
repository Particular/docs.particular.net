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

## Mandatory topology selection.

Specifying a routing topology is now mandatory. For backwards compatiblility, the recommendation is to use the `ConventionalRoutingTopology` which was the previous default. 

See the [transport configuration documentation](https://docs.particular.net/transports/rabbitmq/#topology-selection) for further details.

## ConnectionStringName

`ConnectionStringName` has been deprecated and can be replaced with a combination of [ConfigurationManager.ConnectionStrings](https://msdn.microsoft.com/en-us/library/system.configuration.configurationmanager.connectionstrings.aspx) and [setting via code](/transports/rabbitmq/connection-settings.md#specifying-the-connection-string-via-code)

snippet: 4to5CustomConnectionStringName


## UseDirectRoutingTopology convention changes

When using [UseDirectRoutingTopology](/transports/rabbitmq/routing-topology.md#direct-routing-topology-enabling-direct-routing-topology) the address and the event Type are no longer passed to `exchangeNameConvention`.

snippet: 4to5usedirectroutingtopology


## Members added to IRoutingTopology

Members have changed in `IRoutingTopology`:

 * `Initialize` has had extra parameters added.
 * `BindToDelayInfrastructure` method has been added.

See [custom routing topology](/transports/rabbitmq/routing-topology.md#custom-routing-topology) for updated implementation instructions.
