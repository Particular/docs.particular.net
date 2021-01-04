---
title: Installation and deployment
summary: Things to consider when installing and deploying endpoints that use SQL persistence
component: SqlPersistence
related:
 - persistence/sql/installer-workflow
reviewed: 2021-01-04
---

The SQL persistence package generates scripts to create necessary database assets. It's recommended to run those scripts as part of the deployment process. See [Installer Workflow](installer-workflow.md) for more information.


## Script execution during development

To streamline development, SQL persistence will execute generated scripts at endpoint startup if installers are enabled.

snippet: ExecuteAtStartup

NOTE: Automatically executing generated scripts is recommended only in development environments.


## Script execution in non-development environments

In non-development environments, where the SQL persistence installation scripts have been executed as part of the deployment, it may be necessary to explicitly disable the SQL persistence installers if [standard installers](/nservicebus/operations/installers.md) need to be used for other purposes.

snippet: DisableInstaller

partial: script-runner


## Table prefix

The *table prefix* is the string that is prefixed to every table name, e.g. Saga, Outbox, Subscription and Timeout tables.

The default table prefix is [Endpoint Name](/nservicebus/endpoints/specify-endpoint-name.md) with all periods (`.`) replaced by underscores (`_`).

A table prefix is used at runtime and install time. 

NOTE: While the default table prefix can be inferred by code at runtime, it cannot be inferred when [running deployment scripts manually](#manual-installation) and as such must be passed in as a parameter.

When using the default approach to installation (execute at startup), the value configured in code will be used.

snippet: TablePrefix


## Database schema

When using a database that supports schemas, a schema value other than default can be defined in the configuration API. Consult the documentation of the selected SQL dialect for details. 

NOTE: The same value must be passed to the installation scripts as a parameter.


## Manual installation

When performing a custom script execution the table prefix is required. See also [Installer Workflow](installer-workflow.md).

Note that `scriptDirectory` can be either the root directory for all scripts, or the specific locations for a given storage type i.e. Saga, Outbox, Subscription and Timeout scripts.


### SQL Server

snippet: ExecuteScriptsSqlServer


### MySQL

snippet: ExecuteScriptsMySql

partial: executescriptsoracle

partial: executescriptspostgresql