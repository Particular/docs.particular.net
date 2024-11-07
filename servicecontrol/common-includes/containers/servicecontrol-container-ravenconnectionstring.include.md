### RavenDB connection string

_Environment variable:_ `RAVENDB_CONNECTIONSTRING`

Provides the URL to connect to the database container that stores the audit instance's data. The database container should be exclusive to the error instance, and not shared by any other ServiceControl instances.

If the [storage requirements for the RavenDB container](/servicecontrol/ravendb/containers.md#required-settings) cannot be met by the container hosting infrastructure, especially in cloud-hosted environments, an externally-hosted and separately-licensed RavenDB instance can also be used starting with ServiceControl version 6.0.

In this case, the RavenDB Major.Minor version must match the version expected by ServiceControl as shown in this table:

include: servicecontrol-ravendb-versions