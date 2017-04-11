---
title: Oracle Caveats
component: SqlPersistence
related:
reviewed: 2017-04-06
---

The SQL Persistence provides true autonomy between endpoints by using separate tables for every endpoint based on the endpoint name. However, due to Oracle's 30-character limit on table names and index names, the SQL Persistence must make some compromises.

## Table Names

NOTE: For a complete example of the schema created by the SQL Persistence for Oracle, see [Oracle Scripts](oracle-scripts.md).

For storing subscriptions, timeouts, and outbox data, SQL Persistence will reserve 24 characters for the endpoint name, leaving 3 characters for the persister type, and and additional 3 characters for an index type.Names are then constructed as `{EndpointName}{PersisterTypeSuffix}{KeyType}`.

The following table shows table names created for an endpoint named `My.Endpoint`:

| Persister     | Suffix |    Table Name    |       Indexes       |
|---------------|:------:|:----------------:|:-------------------:|
| Subscriptions |  `_SS` | `MY_ENDPOINT_SS` | `MY_ENDPOINT_SS_PK` |
| Timeouts      |  `_TO` | `MY_ENDPOINT_TO` | `MY_ENDPOINT_TO_PK`<br/>`MY_ENDPOINT_TK`<br/>`MY_ENDPOINT_SK` |
| Outbox        |  `_OD` | `MY_ENDPOINT_OD` | `MY_ENDPOINT_OD_PK`<br/>`MY_ENDPOINT_IX` |

If an endpoint name is longer than 24 characters, an exception will be thrown, and a substitute [table prefix](/nservicebus/sql-persistence/#installation-table-prefix) must be specified in the endpoint configuration:

snippet: TablePrefix

### Sagas

Tables generated for sagas reserve 27 characters for the saga name, leaving 3 characters for the `_PK` suffix for the table's primary key.

In order to accommodate as many characters for the saga name as possible, the [table prefix](/nservicebus/sql-persistence/#installation-table-prefix) is not represented in the resulting table name.

| Saga Class Name |   Table Name  |    Primary Key   |
|-----------------|:-------------:|:----------------:|
| OrderPolicy     | `ORDERPOLICY` | `ORDERPOLICY_PK` |

A 3-character suffix is not enough to uniquely identify multiple correlation properties in a deterministic way, so unfortunately index names for sagas cannot be named after the owner table in the same way as for other persisters.

Index names for correlation properties are constructed using the prefix `SAGAIDX_` plus the first 22 characters of a SHA1 hash of the saga name and correlation property name. The owning table for a particular index can be discovered by querying the database:

```sql
select TABLE_NAME
from ALL_INDEXES
where INDEX_NAME = 'SAGAIDX_525D1D4DC0C3DCD96947E1';
```

If a saga name is longer than 27 characters, an exception will be thrown, and a [substitute table name must be specified](saga.md#table-structure-table-name).

## Custom Finders

Because Oracle 11g does not support any JSON query capability, custom saga finders that locate saga data by querying into the JSON payload of the saga are not supported by SQL Persistence when using Oracle.