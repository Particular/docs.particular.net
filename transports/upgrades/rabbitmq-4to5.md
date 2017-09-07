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


## ConnectionStringName

`ConnectionStringName` has been deprecated and can be replaceds with a combination of [ConfigurationManager.ConnectionStrings](https://msdn.microsoft.com/en-us/library/system.configuration.configurationmanager.connectionstrings.aspx) and [setting via code](/transports/rabbitmq/connection-settings.md#specifying-the-connection-string-via-code)

snippet: 4to5CustomConnectionStringName


## UseDirectRoutingTopology convention changes

When using [UseDirectRoutingTopology](/transports/rabbitmq/routing-topology.md#direct-routing-topology-enabling-direct-routing-topology) the address and the event Type are no longer passed to `exchangeNameConvention`.

snippet: 4to5usedirectroutingtopology


## Members added to IRoutingTopology

Extra members have been added to `IRoutingTopology`

 * `Initialize`
 * `BindToDelayInfrastructure`

See [custom routing topology](/transports/rabbitmq/routing-topology.md#custom-routing-topology) for updated implementation instructions.