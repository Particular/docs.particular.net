---
title: SQL Persistence Upgrade - Support NServiceBus 5 subscribers
summary: Instructions on how to support subscribers using NServiceBus 5 and lower
component: SqlPersistence
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---


## Summary

This guidance explains how to allow processing of subscription messages sent by endpoints using NServiceBus 5 and lower, as described in the following issue:

- https://github.com/Particular/NServiceBus.Persistence.Sql/issues/112

This issue makes it impossible for subscriber endpoints using NServiceBus 5 and lower to subscribe to events published by endpoints using NServiceBus 6. Those subscription messages are eventually moved to the error queue. This causes message loss as the subscriber never receives events it subscribed for.


## Compatibility

This issue has been resolved in the following patch versions as defined in the NServiceBus [support policy](support-policy.md):

- NServiceBus.Persistence.Sql 2.1.2
- NServiceBus.Persistence.Sql 2.0.1
- NServiceBus.Persistence.Sql 1.0.5

If any of the supported affected minor versions (2.1.x, 2.0.x, or 1.0.x) are used these should be updated to the latest patch release.


## Upgrade steps

Steps:

 * If using Oracle Database, follow the procedure on how to update the database schema
 * Update to latest patch release
 * Deploy the new version


## Updating database schema on Oracle Database

The schema created on Oracle Database does not allow `NULL` in the `ENDPOINT` column and this patch requires that it does. The following script illustrates an example DDL statement for an endpoint named `Publisher`.

NOTE: This procedure does not require any downtime. It is advisable to execute it when affected endpoint instances are not under heavy load.

NOTE: Run this script on a testing or staging environment first to verify that it works as expected.

```sql
alter table PUBLISHER_SS modify ENDPOINT varchar2(200) null;
```
