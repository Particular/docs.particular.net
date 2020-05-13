---
title: SQL Persistence Upgrade Version 4.0.0 to 4.1.1
summary: Recommendations on the upgrade to SQL Persistence version 4.1.1
reviewed: 2020-05-12
component: SqlPersistence
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Pessimistic concurrency

Up to and including version 4.1, SQL persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when updating or deleting saga data. In almost all cases pessimistic concurrency control will improve performance, but in some edge cases optimistic concurrency control can actually be much faster. It is recommended to performance test if upgrading might cause issues.
