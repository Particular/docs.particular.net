---
title: SQL Server dialect
component: SqlPersistence
related:
reviewed: 2017-11-23
---

{{WARNING: This persistence will run on the free version of the database engine i.e. [SQL Server Express](https://www.microsoft.com/en-au/sql-server/sql-server-editions-express). However it is strongly recommended to use commercial versions for any production system. It is also recommended to ensure that support agreements are in place. See [Microsoft Premier Support](https://www.microsoft.com/en-us/microsoftservices/support.aspx) for details.
}}


## Supported database versions

SQL persistence supports [SQL Server Version 2012](https://docs.microsoft.com/en-us/sql/release-notes/sql-server-2012-release-notes). It does not work with lower versions due to the use of the [THROW functionality](https://docs.microsoft.com/en-us/sql/t-sql/language-elements/throw-transact-sql).


## Usage

include: usage

snippet: SqlPersistenceUsageSqlServer

include: mssql-dtc-warning


## Unicode support

include: unicode-support

Refer to the dedicated [SQL Server documentation](https://docs.microsoft.com/en-us/sql/relational-databases/collations/collation-and-unicode-support) for details.


## Supported name lengths

include: name-lengths

SQL Server supports [max. 128 characters](https://docs.microsoft.com/en-us/sql/sql-server/maximum-capacity-specifications-for-sql-server).

include: name-length-validation-off


## Schema support

The SQL Server dialect supports multiple schemas. By default, when schema is not specified, it uses `dbo` schema when referring to database objects.

snippet: MsSqlSchema



