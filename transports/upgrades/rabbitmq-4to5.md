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
