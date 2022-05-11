---
title: RabbitMQ Transport Upgrade Version 6 to 7
summary: Migration instructions on how to upgrade RabbitMQ Transport from Version 6 to 7.
reviewed: 2022-05-05
component: Rabbit
related:
- transports/rabbitmq
- nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

To make sure that delayed messages can be safely executed version 7 requires all RabbitMQ nodes to be on version `3.10.0` or above. 

See the [minimum broker requirements documentation](/transports/rabbitmq/#broker-compatibility) for more details.
