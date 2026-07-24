---
title: Deployment options
reviewed: 2024-11-06
component: SqlTransport
related:
 - nservicebus/operations
redirects:
 - nservicebus/sqlserver/multiple-databases
 - nservicebus/sqlserver/deployment-options
 - transports/sqlserver/deployment-options
---

The SQL Server Transport offers several deployment options for the queue tables.

## Default

In the default mode all tables for all endpoints exist in a single schema of a single catalog. Given two endpoints, Sales and Billing, here's the mapping of the endpoint names to the queue table names:

| Endpoint | Queue table name                                                      |
|----------|-----------------------------------------------------------------------|
| Sales    | `[sql_server_instance_01]`.`[nsb_database]`.`[nsb_schema]`.`[Sales]`  |
| Billing  | `[sql_server_instance_01]`.`[nsb_database]`.`[nsb_schema]`.`[Billing]`|

This mode does not require any transport-specific addressing configuration so it is easy to set up. It also does not require Distributed Transaction Coordinator (DTC) to ensure *exactly-once* message processing.

The snapshot (backup) of the entire system state can be done by backing up a single database. It is especially useful if business data is also stored in the same database.

## Multi-schema

In the multi-schema mode queue tables exist in a single catalog and may belong to different schemas. A schema can contain queues of one or more endpoints. Given two endpoints, Sales and Billing, here's the mapping of the endpoint names to the queue table names:

| Endpoint | Queue table name                                                      |
|----------|-----------------------------------------------------------------------|
| Sales    | `[sql_server_instance_01]`.`[nsb_database]`.**`[sales_schema]`**.`[Sales]`     |
| Billing  | `[sql_server_instance_01]`.`[nsb_database]`.**`[billing_schema]`**.`[Billing]` |

In order to use this mode transport-specific [addressing](/transports/sql/addressing.md) information has to be provided to map endpoints and/or queues to schemas.

The multi-schema mode does not require Distributed Transaction Coordinator (DTC) to ensure *exactly-once* message processing. The snapshot (backup) of the entire system state can be done by backing up a single database. It is especially useful if business data is also stored in the same database.

## Multi-catalog

In the multi-catalog mode queue tables exist in multiple catalogs provided all these catalogs are in the same SQL Server instance. Given two endpoints, Sales and Billing, here's the mapping of the endpoint names to the queue table names:

| Endpoint | Queue table name                                                      |
|----------|-----------------------------------------------------------------------|
| Sales    | `[sql_server_instance_01]`.**`[sales_database]`**.`[nsb_schema]`.`[Sales]`     |
| Billing  | `[sql_server_instance_01]`.**`[billing_database]`**.`[nsb_schema]`.`[Billing]` |

In order to use this mode the transport-specific [addressing](/transports/sql/addressing.md) information has to be provided to map endpoints and/or queues to catalogs.

The multi-catalog mode does not require Distributed Transaction Coordinator (DTC) to ensure *exactly-once* message processing.

The multi-catalog mode can be combined with the multi-schema mode so that groups of endpoints are assigned catalogs and, within each catalog, each endpoint (or a sub-group of endpoints) has its own schema.