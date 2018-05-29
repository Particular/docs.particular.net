In the multi-instance mode queue tables exist in multiple instances of SQL Server. Given two endpoints, Sales and Billing, here's the mapping of the endpoint names to the queue table names:

| Endpoint | Queue table name                                                      |
|----------|-----------------------------------------------------------------------|
| Sales    | **`[sql_server_instance_01]`**.`[nsb_database]`.`[nsb_schema]`.`[Sales]`       |
| Billing  | **`[sql_server_instance_02]`**.`[nsb_database]`.`[nsb_schema]`.`[Billing]`     |

In order to use this mode the [connection information](/transports/sql/connection-settings.md?version=SqlTransport_3#multiple-connection-strings) has to be provided for each instance.

To ensure *exactly-once* message processing the transport must be set to `TransactionScope` [transaction mode](/transports/transactions.md) and Distributed Transaction Coordinator (DTC) has to be set up to allow distributed transaction that span multiple SQL Server instances.