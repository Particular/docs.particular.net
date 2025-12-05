---
title: SQL Persistence - SQL Server dialect
component: SqlPersistence
related:
reviewed: 2024-10-09
---

> [!WARNING]
> This persistence will run on the free version of the database engine i.e. [SQL Server Express](https://www.microsoft.com/en-au/sql-server/sql-server-editions-express). However, it is strongly recommended to use commercial versions for any production system. It is also recommended to ensure that support agreements are in place. See [Microsoft Premier Support](https://www.microsoft.com/en-us/microsoftservices/support.aspx) for details.


## Supported database versions

SQL persistence supports [SQL Server Version 2012](https://docs.microsoft.com/en-us/sql/release-notes/sql-server-2012-release-notes). It does not work with lower versions due to the use of the [THROW functionality](https://docs.microsoft.com/en-us/sql/t-sql/language-elements/throw-transact-sql).


## Usage

Using either the [System.Data.SqlClient](https://www.nuget.org/packages/System.Data.SqlClient/) or [Microsoft.Data.SqlClient](https://www.nuget.org/packages/Microsoft.Data.SqlClient/) NuGet packages.

snippet: SqlPersistenceUsageSqlServer

include: mssql-dtc-warning

## Token-credentials

Microsoft Entra ID authentication is supported via the [standard connection string options](https://learn.microsoft.com/en-us/sql/connect/ado-net/sql/azure-active-directory-authentication).

> [!NOTE]
> Microsoft Entra ID authentication is only supported when using [Microsoft.Data.SqlClient](https://learn.microsoft.com/en-us/sql/connect/ado-net/sql/azure-active-directory-authentication#overview)

## Unicode support

include: unicode-support

Refer to the dedicated [SQL Server documentation](https://docs.microsoft.com/en-us/sql/relational-databases/collations/collation-and-unicode-support) for details.


## Supported name lengths

include: name-lengths

SQL Server supports [max. 128 characters](https://docs.microsoft.com/en-us/sql/sql-server/maximum-capacity-specifications-for-sql-server).

include: name-length-validation-off


## Schema support

The SQL Server dialect supports multiple schemas. By default, when a schema is not specified, it uses the `dbo` schema when referring to database objects.

snippet: MsSqlSchema

## SQL Always Encrypted

The SQL Server dialect has support for [SQL Server Always Encrypted](https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/always-encrypted-database-engine).

> [!NOTE]
> Always Encrypted support works only with `Microsoft.Data.SqlClient`.

The steps to use SQL Always Encrypted are:

1. Make sure SQL Always Encrypted is configured with the correct certificate or key stores on the database engine and the client machines.
1. Encrypt the `Body` column for the saga table that encryption is being enabled for. For more information on how to encrypt columns in SQL Server, refer to the [Microsoft documentation](https://docs.microsoft.com/en-us/sql/connect/ado-net/sql/sqlclient-support-always-encrypted?view=sql-server-ver15#retrieving-and-modifying-data-in-encrypted-columns).
1. Encrypt the `Operations` column for the `OutboxData` table. This contains business data in the form of outgoing messages. There is a separate `OutboxData` table for every endpoint that uses the outbox feature.
1. Ensure the connection string for the endpoint includes the `Column Encryption Setting = Enabled;` connection string parameter.

The `Body` and `Operations` columns will now be readable only by clients that have the correct certificate or key stores configured.

> [!NOTE]
> Encrypting columns requires a few parameters including the type of encryption, the algorithm and the key. Installers currently do not support encryption. Therefore, [installers](/nservicebus/operations/installers.md) cannot be enabled in combination with SQL Server Always Encrypted.

