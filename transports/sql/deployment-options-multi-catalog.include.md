In the multi-catalog mode queue tables exist in multiple catalogs provided all these catalogs are in the same SQL Server instance. Given two endpoints, Sales and Billing, here's the mapping of the endpoint names to the queue table names:

| Endpoint | Queue table name                                                      |
|----------|-----------------------------------------------------------------------|
| Sales    | `[sql_server_instance_01]`.**`[sales_database]`**.`[nsb_schema]`.`[Sales]`     |
| Billing  | `[sql_server_instance_01]`.**`[billing_database]`**.`[nsb_schema]`.`[Billing]` |

In order to use this mode the transport-specific [addressing](/transports/sql/addressing.md) information has to be provided to map endpoints and/or queues to catalogs.

The multi-schema mode does not require Distributed Transaction Coordinator (DTC) to ensure *exactly-once* message processing.

The multi-catalog mode can be combined with the multi-schema mode so that groups of endpoints are assigned catalogs and, within each catalogs, each endpoint (or a sub-group of endpoints) has its own schema.