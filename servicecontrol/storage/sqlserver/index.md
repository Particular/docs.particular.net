---
title: ServiceControl SQL Server Storage
summary: Configure SQL Server storage for ServiceControl Error and Audit instances.
reviewed: 2026-10-29
component: ServiceControl
versions: '[7,)'
---

From ServiceControl 7, Error and Audit instances can use SQL Server instead of the default [RavenDB storage](/servicecontrol/storage/ravendb/). Select SQL Server when an instance should use an existing SQL Server deployment. Storage is configured separately for each instance.

> [!IMPORTANT]
> When using SQL Server, Error instances require [external message body storage](/servicecontrol/storage/external-body/) in addition to the database, even when message bodies fit inline. Audit instances do not require it.

## Supported SQL Server versions

ServiceControl supports the SQL Server versions that are in mainstream support; see the [SQL Server lifecycle](https://learn.microsoft.com/en-us/lifecycle/products/?products=sql-server). Full-Text Search must be installed; see [Prepare the database](#prepare-the-database).

## Prepare the database

Create a database before configuring the instance as ServiceControl does not create the database. Setup creates the instance's schema if it does not exist. If the identity that runs setup cannot create schemas, create the schema in advance and grant that identity rights on it. Error and Audit instances can use the same database but not the same schema; see [sharing a database](/servicecontrol/storage/#sharing-a-database).

> [!IMPORTANT]
> SQL Server Full-Text Search must be installed on the server. Error and Audit instance setup fail without it. The `mcr.microsoft.com/mssql/server` container images do not include it; on Linux, including custom container images, install the `mssql-server-fts` package as described in [Install SQL Server Full-Text Search on Linux](https://learn.microsoft.com/en-us/sql/linux/install-upgrade/setup-full-text-search).

Message body search is always enabled for SQL Server storage, regardless of the [`EnableFullTextSearchOnBodies`](/servicecontrol/servicecontrol-instances/configuration.md#performance-tuning-servicecontrolenablefulltextsearchonbodies) setting.

## Database permissions

Grant each identity the following rights on the instance's database:

| Identity | Required rights |
| --- | --- |
| The identity that runs setup | Membership in `db_ddladmin`, `db_datareader`, and `db_datawriter`, or in `db_owner`. Setup creates the schema, tables, indexes, and the full-text catalog and index, and records the applied migrations. |
| The identity the instance runs as | Membership in `db_datareader` and `db_datawriter`, or `SELECT`, `INSERT`, `UPDATE`, and `DELETE` on the instance's schema. Normal operation does not change the schema. |

When the same connection string is used for setup and normal operation, as the ServiceControl Management Utility and PowerShell do, that identity needs the setup rights.

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

To share the ingestion load across several processes, see [Scaling out error ingestion](/servicecontrol/servicecontrol-instances/scaling-out-ingestion.md). Workers use this instance's database, schema, and message body store, and only this instance runs setup.

## Audit instance configuration

Set the persistence type to `SQLServer`, provide the database connection string, and configure the retention period.

include: sc-audit-persistencetype

include: sc-audit-database-connectionstring

include: sc-audit-database-schema

include: sc-audit-auditretentionperiod

Audit instances do not use an external message body store; they store text message bodies in the database.

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

## Run setup

> [!IMPORTANT]
> Run the instance setup process (`--setup`) before first use and after every upgrade. Setup applies the database migrations; the instance does not apply them at startup.

Setup applies database migrations for both Error and Audit instances. For Error instances, setup also provisions the external message body store. See [database permissions](#database-permissions) for the rights setup and normal operation need. See the [Error container setup](/servicecontrol/servicecontrol-instances/deployment/containers.md#initial-setup) or [Audit container setup](/servicecontrol/audit-instances/deployment/containers.md#initial-setup) instructions when deploying in containers.

The ServiceControl Management Utility (SCMU) and the PowerShell module run setup when they create or upgrade an instance, and can create SQL Server-backed Error and Audit instances:

- **SCMU:** choose `SQL Server` as the **STORAGE TYPE** and enter the **DATABASE CONNECTION STRING**. For an Error instance, also choose the **MESSAGE BODY STORAGE** and fill in its settings.
- **PowerShell:** pass `-PersistenceType SQLServer` and `-DatabaseConnectionString` to `New-ServiceControlInstance` or `New-ServiceControlAuditInstance`, and leave out `-DBPath` and `-DatabaseMaintenancePort`, which apply only to RavenDB. For an Error instance, `-MessageBodyStorageType` selects `FileSystem` (the default), `AzureBlob`, or `S3`. See the [Error instance](/servicecontrol/servicecontrol-instances/deployment/powershell.md) and [Audit instance](/servicecontrol/audit-instances/deployment/powershell.md) PowerShell guides.

Neither tool sets `Database/Schema`, so instances they create use the default schema.

## Verify the setup

Setup logs `SQL Server database migration completed` for an Error instance, or `Audit database migration completed` for an Audit instance, and exits without an error. After the instance starts, check that no storage [custom checks](/servicecontrol/servicecontrol-instances/#notifications-health-monitoring) report a failure in ServicePulse.

## Troubleshooting

- **Setup fails with "ServiceControl requires the SQL Server Full-Text Search feature":** install Full-Text Search on the server, then run setup again.
- **Setup fails with a permission error:** grant the rights in [database permissions](#database-permissions), or create the schema in advance.
- **Setup fails because "A primary instance and an audit instance cannot share a schema":** set a different `Database/Schema` for one of the instances. See [sharing a database](/servicecontrol/storage/#sharing-a-database).
- **The instance does not start with "Maintenance mode is not supported":** set `MaintenanceMode` to `false`. Maintenance mode is only available with embedded RavenDB storage. To perform database maintenance, stop the ServiceControl instance and access SQL Server directly.
- **The ServiceControl Retention custom check fails:** check the instance log. A body store that refuses deletes stops failed-message retention; see [external message body storage operations](/servicecontrol/storage/external-body/#operations).
- **Messages show without their bodies after a restore:** restore the database and the external message body store from the same point in time. See [backup and restore](/servicecontrol/storage/#backup-and-restore).

## Next steps

- For Error instances, [choose and configure an external message body store](/servicecontrol/storage/external-body/).
- Plan [backup and restore](/servicecontrol/storage/#backup-and-restore) of the database and the message bodies together; RavenDB-specific procedures do not apply.
- To deploy in containers, see the [Error instance](/servicecontrol/servicecontrol-instances/deployment/containers.md) and [Audit instance](/servicecontrol/audit-instances/deployment/containers.md) container guides.
