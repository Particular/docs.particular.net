---
title: SQL Persistence in multi-tenant system
summary: Configure SQL Persistence to support multi-tenant scenarios.
reviewed: 2019-02-12
component: SqlPersistence
related:
- persistence/sql
- nservicebus/outbox
---

This sample demonstrates how to configure SQL Persistence to store tenant-specific data in separate catalogs, for each tenant. The tenant-specific information include the saga state and the business entities that are accessed using [NServiceBus-managed session](/persistence/sql/accessing-data.md).

This sample uses the [Outbox](/nservicebus/outbox/) to guarantee consistency between the saga state and the business entity. The outbox data as well as timeout data is stored a dedicated catalog shared by all tenants.

The sample assumes that the tenant information is passed as a custom message header `tenant_id`.

downloadbutton


## Prerequisites

include: sql-prereq

The databases created by this sample are:

 * `SqlMultiTenantReceiver`
 * `SqlMultiTenantA`
 * `SqlMultiTenantB`

include: persistence-running

include: persistence-code

#### Creating the schema

The default SQL Persistence installers create all schema objects in a single catalog. In the multi-tenant mode the schema objects need to be created manually. The `ScriptRunner` class provides APIs required to run schema creation scripts.

snippet: CreateSchema

The above code makes sure that business entity and saga tables are created in the tenant databases while the timeouts and outbox -- in the shared database.

Because the Outbox table is stored in the shared catalog, SQL Persistence is not able to access it when using the tenant connection string. Synonyms (a feature of SQL Server) provide a way to solve this problem. The following code creates synonyms for the `OutboxData` table in both tenant databases. These synonyms instruct the query processor to use the Outbox table in the shared database whenever it encounters a reference to `OutboxData`.

snippet: Synonyms


#### Connecting to the tenant database

To allow for database isolation between the tenants the connection to the database needs to be created based on the message being processed. This requires cooperation of two components:

 * A behavior that inspects an incoming message and extracts the tenant information 
 * Custom connection factory for SQL Persistence

The connection factory has to be registered with SQL Persistence

snippet: ConnectionFactory

The behavior retrieves the value of the `tenant_id` header and builds a connection string based on the header value. Then it stores the connection string value in the async context via `AsyncLocal`.

snippet: PutConnectionStringToContext

The behavior has to be registered in the pipeline configuration

snippet: ExtractTenantConnectionStringBehavior

When SQL Persistence needs to open a connection, the custom connection factory retrieves the connection string value from the async context and, if present, opens a connection to the tenant database. Otherwise it opens a connection to the shared database.

snippet: GetConnectionFromContext

include: persistence-propagation
