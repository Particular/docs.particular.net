---
title: RabbitMQ Transport Upgrade Version 6 to 7
summary: Migration instructions on how to upgrade RabbitMQ Transport from Version 6 to 7.
reviewed: 2020-11-09
component: Rabbit
related:
- transports/rabbitmq
- nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

## Timeout manager

The [timeout manager is removed from core](/nservicebus/upgrades/7to8/#timeout-manager-removed) which makes timeout manager backwards compatibility mode obsolete. If backwards compatibility mode was enabled these APIs must be removed.
