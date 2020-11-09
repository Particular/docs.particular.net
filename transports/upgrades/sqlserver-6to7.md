---
title: SQL Server Transport Upgrade Version 6 to 7
summary: Migration instructions on how to upgrade SQL Server Transport from Version 8 to 9.
reviewed: 2020-11-09
component: SqlTransport
related:
- transports/sql
- nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
---

## Timeout manager

The [timeout manager is removed from core](/nservicebus/upgrades/7to8/#timeout-manager-removed) which makes timeout manager backwards compatibility mode obsolete. If backwards compatibility mode was enabled these API's must be removed.
