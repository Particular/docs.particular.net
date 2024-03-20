---
title: SQL Server Transport Upgrade Version 7 to 8
summary: Migration instructions on how to migrate the SQL Server transport from version 7 to version 8
reviewed: 2024-03-20
component: SqlTransport
related:
- transports/sql
- nservicebus/upgrades/8to9
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
 - 9
---

## Legacy SQL Client package no longer shipped

The `NServiceBus.SqlServer` package using the legacy [System.Data.SqlClient](https://www.nuget.org/packages/System.Data.SqlClient) driver is no longer shipped and the following package changes are needed:

- `NServiceBus.SqlServer` => `NServiceBus.Transports.SqlServer`
- `System.Data.SqlClient` => `Microsoft.Data.SqlClient`
