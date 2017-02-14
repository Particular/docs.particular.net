---
title: SQL Persistence
component: SqlPersistence
related:
 - samples/sql-persistence
 - samples/saga/migration
reviewed: 2016-11-29
---


The SQL Persistence uses [Json.NET](http://www.newtonsoft.com/json) to serialize data and store in a SQL database.


## Supported SQL Implementations

 * [SQL Server](https://www.microsoft.com/en-au/sql-server/)
 * [MySql](https://www.mysql.com/)


## Usage

Install the [NServiceBus.Persistence.Sql](https://www.nuget.org/packages/NServiceBus.Persistence.Sql/) and [NServiceBus.Persistence.Sql.MsBuild](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.MsBuild/) NuGet packages.


### SQL Server

snippet:SqlPersistenceUsageSqlServer


### MySql

Using the [MySql.Data NuGet Package](https://www.nuget.org/packages/MySql.Data/).

snippet:SqlPersistenceUsageMySql

The following settings are required for [MySql connections string](https://dev.mysql.com/doc/connector-net/en/connector-net-connection-options.html).

 * `AllowUserVariables=True`: since the Persistence uses [user variables](https://dev.mysql.com/doc/refman/5.7/en/user-variables.html).
 * `AutoEnlist=false`: To prevent auto enlistment in a [Distributed Transaction](https://msdn.microsoft.com/en-us/library/windows/desktop/ms681205.aspx) which the MySql .net connector does not currently support.


## Script Creation

SQL installation scripts are created at compile time by the `NServiceBus.Persistence.Sql.MsBuild` NuGet package.

Scripts will be created in the directory format of `[CurrentProjectDebugDir]\NServiceBus.Persistence.Sql\[SqlVariant]`.

For example for a project named `ClassLibrary` build in Debug mode the following directories will be created.

 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\MsSqlServer`
 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\MySql`

Scripts will also be included in the list of project output files. So this means those files produced will be copied to the output directory of any project that references it.

What scripts are created can be controlled via the use of `[SqlPersistenceSettings]` applied to the target assembly.


#### To Produce All scripts

snippet: AllSqlScripts


#### To Produce only MS SQL Server scripts

snippet: SqlServerScripts


#### To Produce only MySql scripts

snippet: MySqlScripts


## Installation

The SQL persistence enables creation of scripts that can be run as a part of a deployment process instead of as part of endpoint startup as with [standard installers](/nservicebus/operations/installers.md). See [installer-workflow](installer-workflow.md) for more information.

To streamline development SQL persistence installation are executed at endpoint startup in the same manner as all other installers. However in higher level environment scenarios, where standard installers are being run but the SQL persistence installation scripts have been executed as part of a deployment, it may be necessary to explicitly disable the SQL persistence installation script executing while leaving standard installers enabled.

snippet: DisableInstaller


### Table Prefix

Table prefix is the string that is prefixed to every table name, i.e. Saga, Outbox, Subscription and Timeout tables.

The default TablePrefix is [Endpoint Name](/nservicebus/endpoints/specify-endpoint-name.md) with all periods (`.`) replaced with underscores (`_`).

A Table Prefix is required at runtime and install time.

When using the default (execute at startup) approach to installation the value configured in code will be used.

snippet: TablePrefix


#### Manual installation

When performing a custom script execution the TablePrefix is required. See also [Installer Workflow](installer-workflow.md).

snippet: ExecuteScripts

Note that `scriptDirectory` can be either the root directory for all scripts for, alternatively, the specific locations for a given storage type i.e. Saga, Outbox, Subscription and Timeout scripts.


## SqlStorageSession

The current [DbConnection](https://msdn.microsoft.com/en-us/library/system.data.common.dbconnection.aspx) and [DbTransaction](https://msdn.microsoft.com/en-us/library/system.data.common.dbtransaction.aspx) can be accessed via the current context.


### Using in a Handler

snippet: handler-sqlPersistenceSession


### Using in a Saga

include: saga-business-data-access

snippet: saga-sqlPersistenceSession
