In the multi-schema mode queue tables exist in a single catalog but might belong to different schemas. A schema can contain queues of one or more endpoints. Given two endpoints, Sales and Billing, here's the mapping of the endpoint names to the queue table names:

| Endpoint | Queue table name                                                      |
|----------|-----------------------------------------------------------------------|
| Sales    | `[sql_server_instance_01]`.`[nsb_database]`.**`[sales_schema]`**.`[Sales]`     |
| Billing  | `[sql_server_instance_01]`.`[nsb_database]`.**`[billing_schema]`**.`[Billing]` |

In order to use this mode the transport-specific [addressing](/transports/sql/addressing.md) information has to be provided to map endpoints and/or queues to schemas. 

The multi-schema mode does not require Distributed Transaction Coordinator (DTC) to ensure *exactly-once* message processing. The snapshot (backup) of the entire system state can be done by backing up a single database. It is especially useful if business data is also stored in the same database.