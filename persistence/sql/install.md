---
title: Installation and deployment
component: SqlPersistence
related:
 - persistence/sql/installer-workflow
reviewed: 2017-10-29
---


SQL persistence outputs scripts to create needed database assets. It's recommended to run those scripts as part of the deployment process. See [Installer Workflow](installer-workflow.md) for more information.


## Script execution during development

To streamline development SQL persistence will execute generated scripts at endpoint startup if installers are enbled.

snippet: ExecuteAtStartup

NOTE: Note this is only recommended during development.


## Script execution in higher enviroments

In non-development environments, where the SQL persistence installation scripts have been executed as part of the deployment, it may be necessary to explicitly disable the SQL persistence installers if [standard installers](/nservicebus/operations/installers.md) needs to be used for other purposes.

snippet: DisableInstaller

partial: script-runner


## Table Prefix

Table prefix is the string that is prefixed to every table name, i.e. Saga, Outbox, Subscription and Timeout tables.

The default TablePrefix is [Endpoint Name](/nservicebus/endpoints/specify-endpoint-name.md) with all periods (`.`) replaced with underscores (`_`).

A Table Prefix is used at runtime and install time. 

NOTE: While the above default Table Prefix at runtime can be inferred by code, it cannot be inferred when [running deployment scripts manually](#manual-installation) and as such much be passed in as a parameter.

When using the default (execute at startup) approach to installation the value configured in code will be used.

snippet: TablePrefix


## Database Schema

When using a database that supports schemas, a schema value other than default can be defined in the configuration API. Consult the documentation of selected SQL dialect for details. 

NOTE: The same value will need to be passed to the installation scripts as a parameter.


## Manual installation

When performing a custom script execution the TablePrefix is required. See also [Installer Workflow](installer-workflow.md).

Note that `scriptDirectory` can be either the root directory for all scripts for, alternatively, the specific locations for a given storage type i.e. Saga, Outbox, Subscription and Timeout scripts.


### SQL Server

snippet: ExecuteScriptsSqlServer


### MySQL

snippet: ExecuteScriptsMySql

partial: executescriptsoracle


partial: executescriptspostgresql
