---
title: Deployment options
reviewed: 2016-08-31
component: SqlTransport
tags:
- Transport
related:
 - nservicebus/operations
redirects:
 - nservicebus/sqlserver/multiple-databases
 - nservicebus/sqlserver/deployment-options
 - transports/sqlserver/deployment-options
---

When using the default configuration, the SQL Server Transport assumes that all tables used for storing messages for endpoints are located in a single catalog and within a single schema. However, the configuration can be changed such that message storage is partitioned between different schemas and catalogs. 

The schemas and catalogs can also be specified at a queue level. For example, the error and the audit queues can be configured to use a different schema and a different database catalog.

The supported deployment options are:

 * **default**: all queues are stored in a single catalog and a single schema.
 * **multi-schema**: queues are stored in a single catalog but in more than one schema.
 * **multi-instance**: queues are stored in multiple catalogs on more than one SQL Server instance.
 * **multi-catalog**: queues are stored in multiple catalogs but on a single SQL Server instance. This mode is indirectly supported by using *multi-instance* option, and requires using DTC. In this document both options are covered under the *multi-instance* term.

NOTE: To properly identify the chosen deployment option, all queues that the endpoint interacts with need to be taken into consideration, including error and audit queues. If either of them are stored in a separate SQL Server instance then the deployment should be treated as a *multi-instance* one.

The transport will route messages to destination based on the configuration. If no specific configuration has been provided for a particular destination, the transport assumes the destination has the same configuration as the sending endpoint (i.e. identical schema, catalog, and instance name). If the destination has a different configuration and it hasn't been provided, then an exception will be thrown when sending a message because the transport wouldn't be able to connect to the destination queue.


### Queues names examples

The following table shows the queue table name for each deployment option, where the SQL server name is `sql_server_instance_01`, the default NServiceBus database name is `nsb_database` and the default NServiceBus schema is `nsb_schema`:

| Deployment option | Endpoint | Queue table name                                                      |
|-------------------|----------|-----------------------------------------------------------------------|
| Default           | Sales    | `[sql_server_instance_01]`.`[nsb_database]`.`[nsb_schema]`.`[Sales]`           |
|                   | Billing  | `[sql_server_instance_01]`.`[nsb_database]`.`[nsb_schema]`.`[Billing]`         |
||||
| Multi-schema      | Sales    | `[sql_server_instance_01]`.`[nsb_database]`.**`[sales_schema]`**.`[Sales]`     |
|                   | Billing  | `[sql_server_instance_01]`.`[nsb_database]`.**`[billing_schema]`**.`[Billing]` |
||||
| Multi-catalog     | Sales    | `[sql_server_instance_01]`.**`[sales_database]`**.`[nsb_schema]`.`[Sales]`     |
|                   | Billing  | `[sql_server_instance_01]`.**`[billing_database]`**.`[nsb_schema]`.`[Billing]` |
||||
| Multi-instance    | Sales    | **`[sql_server_instance_01]`**.`[nsb_database]`.`[nsb_schema]`.`[Sales]`       |
|                   | Billing  | **`[sql_server_instance_02]`**.`[nsb_database]`.`[nsb_schema]`.`[Billing]`     |

Note: The tables mentioned above will serve as the incoming queues for endpoints. NServiceBus will also create additional queues, e.g. retries queue. The names of additional queues are made up of the endpoint name and suffix, e.g. `[endpoint_queue_name].[Retries]`.


## Modes overview


### Default (single schema, single catalog, single SQL Server instance)

 * Has simple configuration and setup.
 * Doesn't require Distributed Transaction Coordinator (DTC).
 * The snapshot (backup) of the entire system state can be done by backing up a single database. It is especially useful if business data is also stored in the same database.


### Multi-schema

 * Has simple configuration and setup.
 * Doesn't require DTC.
 * The snapshot (backup) of the entire system state is done by backing up a single database. It is especially useful if business data is also stored in the same database.
 * Enables security configuration on a schema level.

partial: multi-instance
