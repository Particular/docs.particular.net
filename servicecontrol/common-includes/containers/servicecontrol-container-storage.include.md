### Storage selection

_Added in version 7.0.0_

_Environment variable:_ `STORAGETYPE`

Selects the storage for the instance. This setting is required and has no default. Each storage type needs the variables listed in its own section. Use the same storage variables during setup and normal operation.

| Storage | `STORAGETYPE` value | Variables |
| --- | --- | --- |
| SQL Server | `SQLServer` | [SQL Server storage](#required-settings-sql-server-storage) |
| PostgreSQL | `PostgreSQL` | [PostgreSQL storage](#required-settings-postgresql-storage) |
| RavenDB | `RavenDB` | [RavenDB storage](#required-settings-ravendb-storage) |

### SQL Server storage

_Added in version 7.0.0_

_Environment variables:_ `STORAGETYPE`, `DATABASE_CONNECTIONSTRING`

```shell
-e STORAGETYPE=SQLServer \
-e DATABASE_CONNECTIONSTRING="Server=my-server;Database=my-database;User ID=...;Password=...;Encrypt=True" \
```

The database must exist before setup runs. See [SQL Server storage](/servicecontrol/storage/sqlserver/) for how to prepare the database, the permissions it needs, and the authentication options. Also set the [message body storage](#required-settings-message-body-storage) variables.

### PostgreSQL storage

_Added in version 7.0.0_

_Environment variables:_ `STORAGETYPE`, `DATABASE_CONNECTIONSTRING`

```shell
-e STORAGETYPE=PostgreSQL \
-e DATABASE_CONNECTIONSTRING="Host=my-server;Database=my-database;Username=...;Password=..." \
```

The database must exist before setup runs. See [PostgreSQL storage](/servicecontrol/storage/postgresql/) for how to prepare the database, the permissions it needs, and the authentication options. Also set the [message body storage](#required-settings-message-body-storage) variables.

### RavenDB storage

_Environment variables:_ `STORAGETYPE`, `RAVENDB_CONNECTIONSTRING`

```shell
-e STORAGETYPE=RavenDB \
-e RAVENDB_CONNECTIONSTRING="http://servicecontrol-db:8080" \
```

Set `RAVENDB_CONNECTIONSTRING` to the URL of the RavenDB database container that stores the instance's data. In production, the database container should be exclusive to the instance and not shared by other ServiceControl instances. See [RavenDB container setup](/servicecontrol/storage/ravendb/containers.md). RavenDB storage does not use the message body storage variables.

Versions before 7 support only RavenDB storage and do not have the `STORAGETYPE` setting. Leave it out and set only `RAVENDB_CONNECTIONSTRING`.

### Message body storage

_Added in version 7.0.0_

_Environment variable:_ `MESSAGEBODY_STORAGETYPE`

SQL Server and PostgreSQL storage keep large and binary message bodies in an [external message body store](/servicecontrol/storage/external-body/). An Error instance using those storage types does not start without this setting, and an Audit instance starts but [does not store large or binary bodies](/servicecontrol/storage/external-body/#storage-providers-servicecontrol-auditmessagebodystoragetype). The setting selects the store:

| Store | `MESSAGEBODY_STORAGETYPE` value | Required variables | Configuration |
| --- | --- | --- | --- |
| File system | `FileSystem` | `MESSAGEBODY_FILESYSTEM_STORAGEPATH`, with a [volume](#volumes) mounted at that path | [File system storage](/servicecontrol/storage/external-body/file.md) |
| Azure Blob Storage | `AzureBlob` | `MESSAGEBODY_AZURE_CONNECTIONSTRING` or `MESSAGEBODY_AZURE_SERVICEURI` | [Azure Blob Storage](/servicecontrol/storage/external-body/azure-blob.md) |
| Amazon S3 | `S3` | `MESSAGEBODY_S3_BUCKETNAME` | [Amazon S3](/servicecontrol/storage/external-body/amazon-s3.md) |

Give each instance its own directory, container, or key prefix, unless the instances share a database; see [sharing a message body store](/servicecontrol/storage/external-body/#sharing-a-message-body-store).
