---
title: Sql Persistence
component: SqlPersistence
related:
 - samples/sql-persistence/simple
 - samples/sql-persistence/transitioning-correlation-ids
 - samples/saga/sql-sagafinder
 - samples/saga/migration
 - persistence/upgrades/sql-2to3
 - persistence/upgrades/sql-1to2
 - persistence/upgrades/sql-1.0.0-1.0.1
reviewed: 2018-01-02
---


The SQL Persistence uses [Json.NET](http://www.newtonsoft.com/json) to serialize data and store in a SQL database.


## Supported SQL implementations

partial: supportedimpls


## NuGet Packages

The SQL Persistence consists of several [Nuget Packages](https://www.nuget.org/packages?q=NServiceBus.Persistence.Sql).


### [NServiceBus.Persistence.Sql.MsBuild](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.MsBuild/)

This packages installs into the [MSBuild](https://docs.microsoft.com/en-us/visualstudio/msbuild/msbuild) pipeline and generates all SQL installation scripts at compile time. It does this by interrogating types (in the target assembly) and attributes (from the `NServiceBus.Persistence.Sql` NuGet package) to infer what scripts to create. It is required for any project where those SQL installation scripts are required. For Saga scripts it will be any project that contains Saga classes. For Timeouts, Subscriptions and Outbox scripts it will be the endpoint hosting project. This package has a dependency on the `NServiceBus.Persistence.Sql` NuGet package


### [NServiceBus.Persistence.Sql](https://www.nuget.org/packages/NServiceBus.Persistence.Sql/)

This package contains several parts

 * APIs for manipulating `EndPointConfiguration` at configuration time.
 * Runtime implementations of Saga, Timeouts, Subscriptions and Outbox persisters.
 * Attribute definitions used to define certain compile time configuration settings. These attributes are then interrogated by the NServiceBus.Persistence.Sql.MsBuild NuGet Package when generating SQL installation scripts
 * Optionally runs SQL installation scripts at endpoint startup for development purposes. See [Installer Workflow](installer-workflow.md).


### [NServiceBus.Persistence.Sql.ScriptBuilder](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.ScriptBuilder/)

This package contains all APIs that enable the generation of SQL installation scripts using code, i.e. without using the NServiceBus.Persistence.Sql.MsBuild NuGet package.

DANGER: NServiceBus.Persistence.Sql.ScriptBuilder is currently not ready for general usage. It has been made public, and deployed to NuGet, primarily to enable the generation of documentation in a repeatable way. For example it is used to generate the SQL scripts in the [MS SQL Server Scripts](/persistence/sql/sqlserver-scripts.md), [MySql Scripts](/persistence/sql/mysql-scripts.md), and [Oracle Scripts](/persistence/sql/oracle-scripts.md) pages. In future releases, the API may evolve in ways that do not follow the standard of [Release Policy - Semantic Versioning](/nservicebus/upgrades/release-policy.md#semantic-versioning). Raise an issue in the [NServiceBus.Persistence.Sql Repository](https://github.com/Particular/NServiceBus.Persistence.Sql/issues) to discuss this in more detail.


## Script creation

SQL installation scripts are created at compile time by the `NServiceBus.Persistence.Sql.MsBuild` NuGet package. To learn more see [controlling script generation](/persistence/sql/controlling-script-generation.md).