### Storage selection

#if-version [7,)

_Environment variable:_ `STORAGETYPE`

Selects the storage provider for the instance. This setting is required; there is no default. Follow the provider's configuration guide to prepare the storage before running setup.

| Provider | `STORAGETYPE` value | Configuration |
| --- | --- | --- |
| RavenDB | `RavenDB` | [RavenDB container setup](/servicecontrol/storage/ravendb/containers.md) |
| SQL Server | `SQLServer` | [Configure SQL Server storage](/servicecontrol/storage/sqlserver/) |
| PostgreSQL | `PostgreSQL` | [Configure PostgreSQL storage](/servicecontrol/storage/postgresql/) |

The chosen storage configuration must be present during both setup and runtime. Follow the linked provider guide for database preparation and permissions requirements. See the [storage overview](/servicecontrol/storage/#storage-providers) for external message body storage requirements that apply to SQL-backed Error and Audit instances.

#end-if

#if-version (,7)

_Environment variable:_ `RAVENDB_CONNECTIONSTRING`

Provides the URL of the RavenDB database container that stores the instance's data. The database container should be exclusive to the instance and not shared by other ServiceControl instances. See [RavenDB container setup](/servicecontrol/storage/ravendb/containers.md) for database container deployment, required settings, and external server configuration.

#end-if