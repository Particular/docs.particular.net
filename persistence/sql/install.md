---
title: Installation and deployment
component: SqlPersistence
related:
 - persistence/sql/installer-workflow
reviewed: 2017-10-29
---


The SQL persistence enables creation of scripts that can be run as a part of a deployment process instead of as part of endpoint startup as with [standard installers](/nservicebus/operations/installers.md). See [Installer Workflow](installer-workflow.md) for more information.


## Script execution runs by default at endpoint startup

To streamline development SQL persistence installers are, by default, executed at endpoint startup, in the same manner as all other installers. 

snippet: ExecuteAtStartup

NOTE: Note that this is also a valid approach for higher level environments.


## Optionally take control of script execution

However in higher level environment scenarios, where standard installers are being run, but the SQL persistence installation scripts have been executed as part of a deployment, it may be necessary to explicitly disable the SQL persistence installers executing while leaving standard installers enabled.

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
