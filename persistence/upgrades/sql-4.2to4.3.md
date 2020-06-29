---
title: SQL Persistence Upgrade Version 4.2 to 4.3
summary: Instructions on how to upgrade to SQL Persistence version 4.3
reviewed: 2020-06-29
component: SqlPersistence
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

In SQL Persistence version 4.3, the [NServiceBus.Persistence.Sql.MsBuild](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.MsBuild) NuGet package is deprecated, and the generation of SQL installation scripts is merged into the [NServiceBus.Persistence.Sql](https://www.nuget.org/packages/NServiceBus.Persistence.Sql) package.

Version 4.3.0 of NServiceBus.Persistence.Sql.MsBuild will be the last release of this package. When included in a project, it will produce this warning at compile time:

> The NServiceBus.Persistence.Sql.MsBuild package is deprecated and should be removed from the project. The functionality was merged into the NServiceBus.Persistence.Sql package.

In all projects where the NServiceBus.Persistence.Sql.MsBuild package is in use, it can be removed when upgrading to SQL Persistence 4.3, and scripts will continue to be generated.

For projects where NServiceBus.Persistence.Sql is used without NServiceBus.Persistence.Sql.MsBuild, and where generation of scripts is not desired, script generation can be disabled by including the following in the project file:

snippet: DisableScriptGeneration

For more ways to control script generation, see [Controlling script generation](/persistence/sql/controlling-script-generation.md).
