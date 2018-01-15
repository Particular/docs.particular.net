In the default mode all tables for all endpoints exist in a single schema of a single catalog. Given two endpoints, Sales and Billing, here's the mapping of the endpoint names to the queue table names:

| Endpoint | Queue table name                                                      |
|----------|-----------------------------------------------------------------------|
| Sales    | `[sql_server_instance_01]`.`[nsb_database]`.`[nsb_schema]`.`[Sales]`  |
| Billing  | `[sql_server_instance_01]`.`[nsb_database]`.`[nsb_schema]`.`[Billing]`|

This mode does not require any transport-specific addressing configuration so it is easy to set up. It also does not require Distributed Transaction Coordinator (DTC) to ensure *exactly-once* message processing.

The snapshot (backup) of the entire system state can be done by backing up a single database. It is especially useful if business data is also stored in the same database.