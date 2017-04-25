---
title: SQL Persistence
component: SqlPersistence
related:
 - samples/sql-persistence/simple
 - samples/sql-persistence/transitioning-correlation-ids
 - samples/saga/sql-sagafinder
 - samples/saga/migration
 - nservicebus/upgrades/sqlpersistence-1to2
 - nservicebus/upgrades/sqlpersistence-1.0.0-1.0.1
reviewed: 2016-11-29
---


The SQL Persistence uses [Json.NET](http://www.newtonsoft.com/json) to serialize data and store in a SQL database.


## Supported SQL Implementations

 * [SQL Server](https://www.microsoft.com/en-au/sql-server/) ([Version 2012](https://docs.microsoft.com/en-us/sql/release-notes/sql-server-2012-release-notes) and above due to the use of the [THROW functionality](https://msdn.microsoft.com/en-us/library/ee677615.aspx)).
 * [MySQL](https://www.mysql.com/) ([Version 5.7](https://dev.mysql.com/doc/relnotes/mysql/5.7/en/) and above due to the use of [JSON Data Type](https://dev.mysql.com/doc/refman/5.7/en/json.html)).
 * [Oracle](https://www.oracle.com/database/index.html) ([Version 11g Release 2](https://docs.oracle.com/cd/E11882_01/readmes.112/e41331/chapter11204.htm) and above).
   * Due to the 30-character limit on table, key, and index names, there are some [Oracle-specific caveats to consider](oracle-caveats.md).

WARNING: Although this persistence will run on the free version of the above engines, i.e. [SQL Server Express](https://www.microsoft.com/en-au/sql-server/sql-server-editions-express), [MySQL Community Edition](https://www.mysql.com/products/community/), and [Oracle XE](http://www.oracle.com/technetwork/database/database-technologies/express-edition/overview/index.html), it is strongly recommended to use commercial versions for any production system. It is also recommended to ensure that support agreements are in place from either [Microsoft Premier Support](https://www.microsoft.com/en-us/microsoftservices/support.aspx), [MySQL support](https://www.mysql.com/support/), [Oracle Support](https://www.oracle.com/support/index.html), or another third party support provider.


## Usage

Install the [NServiceBus.Persistence.Sql](https://www.nuget.org/packages/NServiceBus.Persistence.Sql/) and [NServiceBus.Persistence.Sql.MsBuild](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.MsBuild/) NuGet packages.


### SQL Server

snippet: SqlPersistenceUsageSqlServer


### MySQL

Using the [MySql.Data NuGet Package](https://www.nuget.org/packages/MySql.Data/).

snippet: SqlPersistenceUsageMySql

{{Note: The following settings are required for [MySQL connections string](https://dev.mysql.com/doc/connector-net/en/connector-net-connection-options.html).

 * `AllowUserVariables=True`: since the Persistence uses [user variables](https://dev.mysql.com/doc/refman/5.7/en/user-variables.html).
 * `AutoEnlist=false`: To prevent auto enlistment in a [Distributed Transaction](https://msdn.microsoft.com/en-us/library/windows/desktop/ms681205.aspx) which the MySql .net connector does not currently support.}}

partial: usageoracle


## NuGet Packages

The SQL Persistence consists of several [Nuget Packages](https://www.nuget.org/packages?q=NServiceBus.Persistence.Sql).


### [NServiceBus.Persistence.Sql.MsBuild](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.MsBuild/)

This packages installs into the MSBuild pipeline and generates all SQL installation scripts at compile time. It does this by interrogating types (in the target assembly) and attributes (from the `NServiceBus.Persistence.Sql` NuGet package) to infer what scripts to create. It is required for any project where those SQL installation scripts are required. For Saga Scripts it will be any project that contains Saga classes. For Timeouts, Subscriptions and Outbox Scripts it will be the endpoint hosting project. This package has a dependency on the `NServiceBus.Persistence.Sql` NuGet package


### [NServiceBus.Persistence.Sql](https://www.nuget.org/packages/NServiceBus.Persistence.Sql/)

This package contains several parts

 * APIs for manipulating `EndPointConfiguration` at configuration time.
 * Runtime implementations of Saga, Timeouts, Subscriptions and Outbox Persisters.
 * Attribute definitions used to define certain compile time configuration settings. These attributes are then interrogated by the NServiceBus.Persistence.Sql.MsBuild NuGet Package when generating SQL installation scripts
 * Optionally runs SQL installation scripts at endpoint startup for development purposes. See [Installer Workflow](installer-workflow.md).


### [NServiceBus.Persistence.Sql.ScriptBuilder](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.ScriptBuilder/)

This package contains all APIs that enable the generation of SQL installation scripts using code, i.e. without using the NServiceBus.Persistence.Sql.MsBuild NuGet package.

DANGER: NServiceBus.Persistence.Sql.ScriptBuilder is currently not ready for general usage. It has been made public, and deployed to NuGet, primarily to enable the generation of documentation in a repeatable way. For example it is used to generate the SQL scripts in the [MS SQL Server Scripts](/nservicebus/sql-persistence/sqlserver-scripts.md), [MySql Scripts](/nservicebus/sql-persistence/mysql-scripts.md), and [Oracle Scripts](/nservicebus/sql-persistence/oracle-scripts.md) pages. In future releases, the API may evolve in ways that do not follow the standard of [Release Policy - Semantic Versioning](/nservicebus/upgrades/release-policy.md#semantic-versioning). Raise an issue in the [NServiceBus.Persistence.Sql Repository](https://github.com/Particular/NServiceBus.Persistence.Sql/issues) to discuss this in more detail.


## Script Creation

SQL installation scripts are created at compile time by the `NServiceBus.Persistence.Sql.MsBuild` NuGet package.

Scripts will be created in the directory format of `[CurrentProjectDebugDir]\NServiceBus.Persistence.Sql\[SqlVariant]`.

For example for a project named `ClassLibrary` build in Debug mode the following directories will be created.

 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\MsSqlServer`
 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\MySql`
 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\Oracle`

Scripts will also be included in the list of project output files. So this means those files produced will be copied to the output directory of any project that references it.

Scripts creation can configured via the use of `[SqlPersistenceSettings]` applied to the target assembly.


### To Produce All scripts

snippet: AllSqlScripts


### To produce only MS SQL Server scripts

snippet: SqlServerScripts


### To produce only MySQL scripts

snippet: MySqlScripts

partial: scriptsoracle

partial: promote


## Installation

The SQL persistence enables creation of scripts that can be run as a part of a deployment process instead of as part of endpoint startup as with [standard installers](/nservicebus/operations/installers.md). See [Installer Workflow](installer-workflow.md) for more information.

To streamline development SQL persistence installers are, by default, executed at endpoint startup, in the same manner as all other installers. However in higher level environment scenarios, where standard installers are being run, but the SQL persistence installation scripts have been executed as part of a deployment, it may be necessary to explicitly disable the SQL persistence installers executing while leaving standard installers enabled.

snippet: DisableInstaller


### Table Prefix

Table prefix is the string that is prefixed to every table name, i.e. Saga, Outbox, Subscription and Timeout tables.

The default TablePrefix is [Endpoint Name](/nservicebus/endpoints/specify-endpoint-name.md) with all periods (`.`) replaced with underscores (`_`).

A Table Prefix is required at runtime and install time.

When using the default (execute at startup) approach to installation the value configured in code will be used.

snippet: TablePrefix


#### Database Schema

A database schema can be defined in the configuration API as follows:

snippet: Schema

Note that the same value will need to be passed to the SQL installation scripts as a parameter.


#### Manual installation

When performing a custom script execution the TablePrefix is required. See also [Installer Workflow](installer-workflow.md).

Note that `scriptDirectory` can be either the root directory for all scripts for, alternatively, the specific locations for a given storage type i.e. Saga, Outbox, Subscription and Timeout scripts.


##### SQL Server

snippet: ExecuteScriptsSqlServer


##### MySQL

snippet: ExecuteScriptsMySql

partial: executescriptsoracle


## SqlStorageSession

The current [DbConnection](https://msdn.microsoft.com/en-us/library/system.data.common.dbconnection.aspx) and [DbTransaction](https://msdn.microsoft.com/en-us/library/system.data.common.dbtransaction.aspx) can be accessed via the current context.


### Using in a Handler

snippet: handler-sqlPersistenceSession


### Using in a Saga

include: saga-business-data-access

snippet: saga-sqlPersistenceSession
