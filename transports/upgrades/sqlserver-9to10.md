---
title: SQL Server Transport Upgrade Version 9 to 10
summary: Migration instructions on how to migrate the SQL Server transport from version 9 to version 10
reviewed: 2026-04-16
component: SqlTransport
related:
- transports/sql
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 10
---

## Microsoft.Data.SqlClient upgraded to version 7.0.0

The `Microsoft.Data.SqlClient` dependency has been upgraded from version 6.x to 7.0.0.

In version 7.0.0, Entra ID (Azure Active Directory) authentication support was removed from the core `Microsoft.Data.SqlClient` package. Applications using Entra ID authentication must now install the `Microsoft.Data.SqlClient.Extensions.Azure` NuGet package separately.
