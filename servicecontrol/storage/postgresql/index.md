---
title: ServiceControl PostgreSQL Storage
summary: Configure PostgreSQL storage for ServiceControl Error and Audit instances.
reviewed: 2026-10-29
component: ServiceControl
---

From ServiceControl 7, Error and Audit instances can use PostgreSQL instead of [RavenDB storage](/servicecontrol/storage/ravendb/). Select PostgreSQL when an instance should use an existing PostgreSQL deployment. Storage is configured separately for each instance.

> [!IMPORTANT]
> When using PostgreSQL, Error and Audit instances require [external message body storage](/servicecontrol/storage/external-body/) in addition to the database, even when message bodies fit inline.

## Supported PostgreSQL versions

ServiceControl supports the PostgreSQL major versions that are currently supported by the PostgreSQL Global Development Group; see the [PostgreSQL versioning policy](https://www.postgresql.org/support/versioning/). Use the latest minor release of the chosen major version.

PostgreSQL 18 or later is recommended. In testing by Particular Software, PostgreSQL 18 significantly improved ServiceControl performance. PostgreSQL 18 can [build GIN indexes in parallel](https://www.postgresql.org/docs/18/release-18.html), and ServiceControl uses GIN indexes for full-text message search.

## Prepare the database

Create a database before configuring the instance as ServiceControl does not create the database. Setup creates the instance's schema if it does not exist. If the identity that runs setup cannot create schemas, create the schema in advance and grant that identity rights on it. Error and Audit instances can use the same database but not the same schema; see [sharing a database](/servicecontrol/storage/#sharing-a-database).

PostgreSQL full-text search uses a built-in `tsvector` GIN index and message body search is always enabled for PostgreSQL storage, regardless of the [`EnableFullTextSearchOnBodies`](/servicecontrol/servicecontrol-instances/configuration.md#performance-tuning-servicecontrolenablefulltextsearchonbodies) setting.

## Database permissions

Grant each role the following rights:

| Role | Required rights |
| --- | --- |
| The role that runs setup | `CREATE` on the database, so that setup can create the schema. If the schema already exists, `USAGE` and `CREATE` on it instead. Setup creates the tables and indexes, and for Audit instances the first daily partitions, so this role owns them. |
| The role an Error instance runs as | `USAGE` on the schema, and `SELECT`, `INSERT`, `UPDATE`, and `DELETE` on the tables in it. No sequence privileges are needed. |
| The role an Audit instance runs as | Ownership of the Audit tables: the same role that runs setup, or a member of it. See the warning below. |

An Error instance with no `Database/Schema` uses the connection's current schema, usually `public`. From PostgreSQL 15, only the database owner can create objects in `public` unless `CREATE` on it is granted, so the setup role needs `CREATE` on `public` or ownership of the database.

When the same connection string is used for setup and normal operation, as the ServiceControl Management Utility and PowerShell do, that role needs the setup rights.

> [!WARNING]
> While running, Audit instances create daily partitions up to 48 hours ahead and drop expired ones. PostgreSQL only lets the owner of a [partitioned table](https://www.postgresql.org/docs/current/ddl-partitioning.html) create partitions of it. Setup creates the Audit tables, so they are owned by the role that runs setup. Run setup and normal operation as the same role, or grant the setup role to the runtime role so that the runtime role counts as the owner. Otherwise the **Audit partition provisioning** custom check fails, and Audit ingestion stops when the provisioned partitions run out, within about two days of the last successful provisioning.

## Authentication

Configure database authentication in the connection string for each Error or Audit instance. This is separate from the transport connection string.

ServiceControl uses [Npgsql](https://www.npgsql.org/doc/connection-string-parameters.html). Methods configured entirely in the connection string work, such as a user name and password, a password file (`Passfile`), a client certificate, or Kerberos.

### Token authentication

Azure Database for PostgreSQL and Amazon RDS for PostgreSQL, including Aurora PostgreSQL, can sign in with short-lived access tokens instead of a password. To use them, add `Token Authentication` to the connection string, set `Username` to the database role the token signs in as, and leave out `Password` and `Passfile`.

| `Token Authentication` value | Database | Credentials |
| --- | --- | --- |
| `AzureEntraId` | Azure Database for PostgreSQL with Microsoft Entra authentication | The Azure SDK's `DefaultAzureCredential`. See [Microsoft Entra ID](#authentication-token-authentication-microsoft-entra-id). |
| `AwsRdsIam` | Amazon RDS for PostgreSQL or Aurora PostgreSQL with IAM database authentication | The AWS SDK default credential chain, such as an IAM role. The region comes from the `AWS_REGION` environment variable or the AWS profile. |

```text
Host=my-server.postgres.database.azure.com;Database=ServiceControl;Username=servicecontrol-identity;Token Authentication=AzureEntraId;SSL Mode=Require
Host=my-cluster.cluster-xxxxxxxxxxxx.us-east-1.rds.amazonaws.com;Database=ServiceControl;Username=servicecontrol;Token Authentication=AwsRdsIam;SSL Mode=Require
```

ServiceControl uses a token each time it opens a new database connection:

- **AWS IAM:** ServiceControl signs a new token for every connection.
- **Microsoft Entra ID:** ServiceControl reuses a token until two minutes before it expires, then gets a new one. If getting a new token fails while the current one is still valid, ServiceControl logs a warning, keeps using the current token, and tries again 10 seconds later.

If ServiceControl cannot get a valid token, opening the connection fails with `Could not get a database access token for user <user> on host <host>, database <database>.` The inner exception contains the error from the credential. Neither the error nor the warning includes the token.

The same keywords work in the [PostgreSQL transport connection string](/servicecontrol/transports.md#postgresql).

#### Microsoft Entra ID

ServiceControl gets Microsoft Entra tokens from the Azure SDK's [`DefaultAzureCredential`](https://learn.microsoft.com/en-us/dotnet/azure/sdk/authentication/credential-chains#defaultazurecredential-overview). It supports the following identities:

| Identity | Configuration |
| --- | --- |
| System-assigned managed identity | None |
| User-assigned managed identity | Set `AZURE_CLIENT_ID` to the client ID of the identity. |
| Service principal with a client secret | Set `AZURE_TENANT_ID`, `AZURE_CLIENT_ID`, and `AZURE_CLIENT_SECRET`. |
| Service principal with a certificate | Set `AZURE_TENANT_ID`, `AZURE_CLIENT_ID`, and `AZURE_CLIENT_CERTIFICATE_PATH` to a PEM or PFX file that contains the certificate and its private key. For a password-protected PFX file, also set `AZURE_CLIENT_CERTIFICATE_PASSWORD`. |
| Microsoft Entra Workload ID, for example on Azure Kubernetes Service | The environment variables that the workload identity webhook sets. |

Set these environment variables for the ServiceControl process. See [`EnvironmentCredential`](https://learn.microsoft.com/en-us/dotnet/api/azure.identity.environmentcredential) and [`ManagedIdentityCredential`](https://learn.microsoft.com/en-us/dotnet/api/azure.identity.managedidentitycredential) for the complete list of variables.

`DefaultAzureCredential` tries a chain of credentials in order and uses the first one that returns a token: environment variables (service principals), workload identity, managed identity, and then developer tools such as Visual Studio, the Azure CLI, and Azure PowerShell. On a machine where more than one credential is available, such as a virtual machine with a managed identity where a user is also signed in to the Azure CLI, the first available credential in the chain is used.

To use one credential only, set the `AZURE_TOKEN_CREDENTIALS` environment variable to its name, for example `ManagedIdentityCredential` or `EnvironmentCredential`. To exclude developer tool credentials, set it to `prod`. Setting `AZURE_TOKEN_CREDENTIALS` is recommended in production: the instance always signs in as the expected identity, and an authentication error comes from that credential instead of from the whole chain. See [use a specific credential](https://learn.microsoft.com/en-us/dotnet/azure/sdk/authentication/credential-chains#use-a-specific-credential) for the supported values.

> [!NOTE]
> These environment variables apply to the whole ServiceControl process. They also affect other components of the instance that use `DefaultAzureCredential`, such as [Azure Blob Storage message body storage](/servicecontrol/storage/external-body/azure-blob.md) with a service URI.

ServiceControl works out the token scope from host names in the Azure public cloud, Azure Government, and Azure China. For other host names, such as a custom DNS alias, add `Token Scope` with the server's Microsoft Entra scope, for example `Token Scope=https://ossrdbms-aad.database.windows.net/.default`.

#### AWS IAM

The connection string must list exactly one host, because an IAM token is signed for a single host and port. Grant the database user the `rds_iam` role, and give the identity that ServiceControl runs as the `rds-db:connect` permission for that user. A user with `rds_iam` can no longer sign in with a password.

## Error instance configuration

Set the storage type to `PostgreSQL`, provide the database connection string, and configure the retention period. Also configure an [external message body store](/servicecontrol/storage/external-body/), which is required even if most bodies fit inline in the database.

include: sc-error-storagetype

include: sc-error-database-connectionstring

include: sc-error-database-schema

include: sc-error-errorretentionperiod

Set these variables in the Error instance's environment to use file system body storage. Replace the connection string placeholder with credentials supplied by the deployment environment. These storage settings supplement the instance's transport and other required settings.

```text
SERVICECONTROL_STORAGETYPE=PostgreSQL
SERVICECONTROL_DATABASE_CONNECTIONSTRING="Host=postgres;Database=ServiceControl;Username=sc;Password=..."
SERVICECONTROL_MESSAGEBODY_STORAGETYPE=FileSystem
SERVICECONTROL_MESSAGEBODY_FILESYSTEM_STORAGEPATH=/var/servicecontrol/bodies
SERVICECONTROL_ERRORRETENTIONPERIOD=15
```

See the [Error instance settings reference](/servicecontrol/servicecontrol-instances/configuration.md#storage) for the complete reference.

To share the ingestion load across several processes, see [Scaling out error ingestion](/servicecontrol/servicecontrol-instances/scaling-out-ingestion.md). Workers use this instance's database, schema, and message body store, and only this instance runs setup.

## Audit instance configuration

Set the storage type to `PostgreSQL`, provide the database connection string, and configure the retention period. Also configure an [external message body store](/servicecontrol/storage/external-body/), which is required even if most bodies fit inline in the database. The ServiceControl Management Utility and the PowerShell module both configure it for Audit instances. For manual or container deployments, add the `ServiceControl.Audit/MessageBody` settings to the instance's configuration file or environment.

include: sc-audit-storagetype

include: sc-audit-database-connectionstring

include: sc-audit-database-schema

include: sc-audit-auditretentionperiod

PostgreSQL-backed Audit instances are partitioned by day. The instance creates and drops daily partitions automatically, which requires the runtime role to own the Audit tables; see [database permissions](#database-permissions).

include: sc-audit-maxbodysizetostore

> [!NOTE]
> SQL Server and PostgreSQL storage do not support maintenance mode. An instance with `MaintenanceMode` set to `true` does not start.

> [!WARNING]
> Connect to PostgreSQL directly or through a connection pooler in session mode. A pooler in transaction mode, such as PgBouncer with `pool_mode = transaction`, breaks the advisory lock that the retention process holds, and retention stops running.

Example environment variables for an Audit instance using file system body storage:

```text
SERVICECONTROL_AUDIT_STORAGETYPE=PostgreSQL
SERVICECONTROL_AUDIT_DATABASE_CONNECTIONSTRING="Host=postgres;Database=ServiceControl.Audit;Username=sc;Password=..."
SERVICECONTROL_AUDIT_MESSAGEBODY_STORAGETYPE=FileSystem
SERVICECONTROL_AUDIT_MESSAGEBODY_FILESYSTEM_STORAGEPATH=/var/servicecontrol/audit-bodies
SERVICECONTROL_AUDIT_AUDITRETENTIONPERIOD=30
```

See the [Audit instance settings reference](/servicecontrol/audit-instances/configuration.md#storage) for the complete reference.

## Run setup

> [!IMPORTANT]
> Run the instance setup process (`--setup`) before first use and after every upgrade. Setup applies the database migrations; the instance does not apply them at startup.

Setup applies database migrations for both Error and Audit instances. Setup also provisions the external message body store. See [database permissions](#database-permissions) for the rights setup and normal operation need. See the [Error container setup](/servicecontrol/servicecontrol-instances/deployment/containers.md#initial-setup) or [Audit container setup](/servicecontrol/audit-instances/deployment/containers.md#initial-setup) instructions when deploying in containers.

The ServiceControl Management Utility (SCMU) and the PowerShell module run setup when they create or upgrade an instance, and can create PostgreSQL-backed Error and Audit instances:

- **SCMU:** choose `PostgreSQL` as the **STORAGE TYPE** and enter the **DATABASE CONNECTION STRING**.
- **PowerShell:** pass `-StorageType PostgreSQL` and `-DatabaseConnectionString` to `New-ServiceControlInstance` or `New-ServiceControlAuditInstance`, and leave out `-DBPath` and `-DatabaseMaintenancePort`, which apply only to RavenDB. See the [Error instance](/servicecontrol/servicecontrol-instances/deployment/powershell.md) and [Audit instance](/servicecontrol/audit-instances/deployment/powershell.md) PowerShell guides.

Both tools configure file system message body storage in `%ProgramData%\Particular\ServiceControl\<instance name>\MessageBodies`. To use a different folder, [Azure Blob Storage](/servicecontrol/storage/external-body/azure-blob.md), or [Amazon S3](/servicecontrol/storage/external-body/amazon-s3.md), change the message body settings in the instance's configuration file. Bodies already stored are not moved, so make the change before the instance ingests messages.

Neither tool sets `Database/Schema`, so instances they create use the default schema.

## Verify the setup

Setup logs `PostgreSQL database migration completed` for an Error instance, or `Audit database migration completed` for an Audit instance, and exits without an error. After the instance starts, check that no storage [custom checks](/servicecontrol/servicecontrol-instances/#notifications-health-monitoring) report a failure in ServicePulse.

## Troubleshooting

- **Setup fails with a permission error:** grant the rights in [database permissions](#database-permissions), or create the schema in advance.
- **Setup fails because "A primary instance and an audit instance cannot share a schema":** set a different `Database/Schema` for one of the instances. See [sharing a database](/servicecontrol/storage/#sharing-a-database).
- **The instance does not start with "Maintenance mode is not supported":** set `MaintenanceMode` to `false`. Maintenance mode is only available with embedded RavenDB storage. To perform database maintenance, stop the ServiceControl instance and access PostgreSQL directly.
- **The ServiceControl Retention custom check fails:** check the instance log. A body store that refuses deletes stops failed-message retention; see [external message body storage operations](/servicecontrol/storage/external-body/#operations).
- **The Audit partition provisioning custom check fails:** make sure the Audit instance runs as the role that owns its tables. See [database permissions](#database-permissions).
- **Connections fail with "Could not get a database access token":** check the inner exception for the credential's error. For Microsoft Entra ID, set `AZURE_TOKEN_CREDENTIALS` to the credential the instance should use, so that the error comes from that credential; see [Microsoft Entra ID](#authentication-token-authentication-microsoft-entra-id). For AWS IAM, check that the identity has the `rds-db:connect` permission for the database user and that the region is set.
- **Messages show without their bodies after a restore:** restore the database and the external message body store from the same point in time. See [backup and restore](/servicecontrol/storage/#backup-and-restore).

## Next steps

- [Choose and configure an external message body store](/servicecontrol/storage/external-body/) for each instance.
- Plan [backup and restore](/servicecontrol/storage/#backup-and-restore) of the database and the message bodies together; RavenDB-specific procedures do not apply.
- To deploy in containers, see the [Error instance](/servicecontrol/servicecontrol-instances/deployment/containers.md) and [Audit instance](/servicecontrol/audit-instances/deployment/containers.md) container guides.
