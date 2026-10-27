---
title: ServiceControl SQL Server Storage
summary: Configure SQL Server storage for ServiceControl Error and Audit instances.
reviewed: 2026-10-27
component: ServiceControl
versions: '[7,)'
---

From ServiceControl 7, Error and Audit instances can use SQL Server instead of the default [RavenDB storage](/servicecontrol/storage/ravendb/). Select SQL Server when an instance should use an existing SQL Server deployment. Storage is configured separately for each instance.

> [!IMPORTANT]
> When using SQL Server, Error instances require [external message body storage](/servicecontrol/storage/external-body/) in addition to the database, even when message bodies fit inline. Audit instances do not require it.

## Prepare the database

Create a database before configuring the instance as ServiceControl does not create the database. Setup creates the instance's schema if it does not exist. If the identity that runs setup cannot create schemas, create the schema in advance and grant that identity rights on it. Error and Audit instances can use the same database but not the same schema; see [sharing a database](/servicecontrol/storage/#sharing-a-database).

> [!IMPORTANT]
> SQL Server Full-Text Search must be installed on the server. Error and Audit instance setup fail without it. The `mcr.microsoft.com/mssql/server` container images do not include it.

Message body search is always enabled for SQL Server storage, regardless of the [`EnableFullTextSearchOnBodies`](/servicecontrol/servicecontrol-instances/configuration.md#performance-tuning-servicecontrolenablefulltextsearchonbodies) setting.

> [!IMPORTANT]
> Run the instance setup process (`--setup`) before first use and after every upgrade. Setup applies the database migrations; the instance does not apply them at startup.

Setup applies database migrations for both Error and Audit instances, so the identity that runs setup must have permissions to modify the schema (create and alter tables, indexes, and full-text catalogs), and to create the schema if it does not exist yet (`CREATE SCHEMA`). For Error instances, setup also provisions the external message body store. See the [Error container setup](/servicecontrol/servicecontrol-instances/deployment/containers.md#initial-setup) or [Audit container setup](/servicecontrol/audit-instances/deployment/containers.md#initial-setup) instructions when deploying in containers.

> [!NOTE]
> The ServiceControl Management Utility and the PowerShell module create only RavenDB instances. Configure SQL Server storage in environment variables or the application configuration file.

## Authentication

Configure database authentication in the connection string for each Error or Audit instance. This is separate from the transport connection string.

ServiceControl uses `Microsoft.Data.SqlClient`. Microsoft Entra ID authentication is supported via the [standard connection string options](https://learn.microsoft.com/en-us/sql/connect/ado-net/sql/azure-active-directory-authentication), including managed identity and service principal authentication.

Choose an authentication method suitable for an unattended service. Ensure the selected identity is available to the ServiceControl host and has the database permissions required for setup and normal operation.

## Error instance configuration

Set the persistence type to `SQLServer`, provide the database connection string, and configure the retention period. Also configure an [external message body store](/servicecontrol/storage/external-body/), which is required even if most bodies fit inline in the database.

include: sc-error-persistencetype

include: sc-error-database-connectionstring

include: sc-error-database-schema

include: sc-error-errorretentionperiod

Set these variables in the Error instance's environment to use file system body storage. Replace the connection string placeholder with credentials supplied by the deployment environment. These storage settings supplement the instance's transport and other required settings.

```text
SERVICECONTROL_PERSISTENCETYPE=SQLServer
SERVICECONTROL_DATABASE_CONNECTIONSTRING="Server=sqlserver;Database=ServiceControl;User Id=sc;Password=..."
SERVICECONTROL_MESSAGEBODY_STORAGETYPE=FileSystem
SERVICECONTROL_MESSAGEBODY_FILESYSTEM_STORAGEPATH=/var/servicecontrol/bodies
SERVICECONTROL_ERRORRETENTIONPERIOD=15
```

See the [Error instance settings reference](/servicecontrol/servicecontrol-instances/configuration.md#storage) for the complete reference.

## Audit instance configuration

Set the persistence type to `SQLServer`, provide the database connection string, and configure the retention period.

include: sc-audit-persistencetype

include: sc-audit-database-connectionstring

include: sc-audit-database-schema

include: sc-audit-auditretentionperiod

Audit instances do not require an external body store; eligible text bodies are stored inline in the database.

include: sc-audit-maxbodysizetostore

> [!NOTE]
> Audit instances using SQL storage store only text message bodies, up to `ServiceControl.Audit/MaxBodySizeToStore`. Binary bodies and larger bodies cannot be viewed in ServicePulse.

> [!NOTE]
> SQL Server and PostgreSQL storage do not support maintenance mode. An instance with `MaintenanceMode` set to `true` does not start.

Example environment variables for an Audit instance:

```text
SERVICECONTROL_AUDIT_PERSISTENCETYPE=SQLServer
SERVICECONTROL_AUDIT_DATABASE_CONNECTIONSTRING="Server=sqlserver;Database=ServiceControl.Audit;User Id=sc;Password=..."
SERVICECONTROL_AUDIT_AUDITRETENTIONPERIOD=30
```

See the [Audit instance settings reference](/servicecontrol/audit-instances/configuration.md#storage) for the complete reference.

## Message bodies and operations

See the [external body storage overview](/servicecontrol/storage/external-body/) for Error-instance inline size, compression, and body retention settings. Follow the [storage backup and restore guidance](/servicecontrol/storage/#backup-and-restore) for database and message body backups; RavenDB-specific procedures do not apply.
