---
title: Oracle dialect
component: SqlPersistence
related:
reviewed: 2018-01-02
versions: "[2,)"
redirects:
 - nservicebus/sql-persistence/oracle-caveats
 - nservicebus/sql-persistence/oracle-design
---

{{WARNING: This persistence will run on the free version of the above engines, i.e. [Oracle XE](http://www.oracle.com/technetwork/database/database-technologies/express-edition/overview/index.html). However it is strongly recommended to use commercial versions for any production system. It is also recommended to ensure that support agreements are in place. See [Oracle Support](https://www.oracle.com/support/index.html) for details.
}}


## Supported database versions

SQL persistence supports [Oracle 11g Release 2](https://docs.oracle.com/cd/E11882_01/readmes.112/e41331/chapter11204.htm) and above.


## Usage

include: usage

Using the [Oracle.ManagedDataAccess NuGet Package](https://www.nuget.org/packages/Oracle.ManagedDataAccess).

snippet: SqlPersistenceUsageOracle

In Versions 2.2.0 and above it's possible to specify custom schema using the following code:

```
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.SqlVariant(SqlVariant.Oracle);
persistence.Schema("custom_schema");
```

NOTE: The ODP.NET managed driver requires the `Enlist=false` or `Enlist=dynamic` setting in the [Oracle connection string](https://docs.oracle.com/database/121/ODPNT/featConnecting.htm) to allow the persister to enlist in a [Distributed Transaction](https://msdn.microsoft.com/en-us/library/windows/desktop/ms681205.aspx) at the correct moment.


## Unicode support

include: unicode-support

Refer to the dedicated [Oracle documentation](https://docs.oracle.com/cd/B19306_01/server.102/b14225/ch2charset.htm) for details.


## Schema support

The notion of schemas is slightly different than in other databases, notable SQL Server and PostgreSQL. By default, when schema is not specified, SQL persistence uses logged-in user context when referring to database objects. When schema is specified, that schema is used instead of logged-in user when referring to database tables. 

snippet: OracleSchema


## Supported name lengths

include: name-lengths

The SQL Persistence provides autonomy between endpoints by using separate tables for every endpoint based on the endpoint name. However, due to Oracle's 30-character limit on table names and index names in [Oracle 12.1 and below](https://docs.oracle.com/database/121/SQLRF/sql_elements008.htm#SQLRF00223), the SQL Persistence must make some compromises.

include: name-length-validation-on


### Table Names

NOTE: For a complete example of the schema created by the SQL Persistence for Oracle, see [Oracle Scripts](oracle-scripts.md).

For storing subscriptions, timeouts, and outbox data, SQL Persistence will reserve 24 characters for the endpoint name, leaving 3 characters for the persistence type, and additional 3 characters for an index type. Names are then constructed as `{EndpointName}{PersistenceTypeSuffix}{KeyType}`.

The following table shows table names created for an endpoint named `My.Endpoint`:

| Persistence type | Suffix |    Table Name    |       Indexes       |
|------------------|:------:|:----------------:|:-------------------:|
| Subscriptions    |  `_SS` | `MY_ENDPOINT_SS` | `MY_ENDPOINT_SS_PK` |
| Timeouts         |  `_TO` | `MY_ENDPOINT_TO` | `MY_ENDPOINT_TO_PK`<br/>`MY_ENDPOINT_TK`<br/>`MY_ENDPOINT_SK` |
| Outbox           |  `_OD` | `MY_ENDPOINT_OD` | `MY_ENDPOINT_OD_PK`<br/>`MY_ENDPOINT_IX` |

If an endpoint name is longer than 24 characters, an exception will be thrown, and a substitute [table prefix](/persistence/sql/install.md#table-prefix) must be specified in the endpoint configuration:

snippet: TablePrefix


### Sagas

Tables generated for sagas reserve 27 characters for the saga name, leaving 3 characters for the `_PK` suffix for the table's primary key.

In order to accommodate as many characters for the saga name as possible, the [table prefix](/persistence/sql/install.md#table-prefix) is omitted from the saga table name.

| Saga Class Name |   Table Name  |    Primary Key   |
|-----------------|:-------------:|:----------------:|
| OrderPolicy     | `ORDERPOLICY` | `ORDERPOLICY_PK` |

A 3-character suffix is not enough to uniquely identify multiple correlation properties in a deterministic way, so unfortunately index names for sagas cannot be named after the owner table in the same way as for other persistence types.

Index names for correlation properties are constructed using the prefix `SAGAIDX_` plus a deterministic hash of the saga name and correlation property name. The owning table for a particular index can be discovered by querying the database:

```sql
select TABLE_NAME
from ALL_INDEXES
where INDEX_NAME = 'SAGAIDX_525D1D4DC0C3DCD96947E1';
```

NOTE: If saga name or correlation property name change, the name of the index will also change.

If a saga name is longer than 27 characters, an exception will be thrown, and a [substitute table name must be specified](saga.md#table-structure-table-name).


### Custom Finders

Because Oracle 11g does not support any JSON query capability, custom saga finders that locate saga data by querying into the JSON payload of the saga are not supported by SQL Persistence when using Oracle.
