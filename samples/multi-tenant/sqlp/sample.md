---
title: SQL Persistence in multi-tenant system
summary: Configure SQL Persistence to support multi-tenant scenarios.
reviewed: 2024-03-05
component: SqlPersistence
related:
- persistence/sql
- nservicebus/outbox
- samples/multi-tenant/nhibernate
---

This sample demonstrates how to configure SQL Persistence to store tenant-specific data in separate databases, one for each tenant. The tenant-specific information includes saga state, and business entities that are accessed using [NServiceBus-managed session](/persistence/sql/accessing-data.md). In addition, the [Outbox](/nservicebus/outbox/) is used to guarantee consistency between the saga state and the business entity. Outbox data is also stored in the tenant-specific database.

The sample assumes that the tenant information is passed as a custom message header `tenant_id`.

downloadbutton


## Prerequisites

include: sql-prereq

The databases created by this sample are:

 * `SqlMultiTenantA`
 * `SqlMultiTenantB`

include: persistence-running

include: persistence-code

#### Creating the schema

The default SQL Persistence installers create all schema objects in a single database. In multi-tenant scenarios schema objects need to be created manually. The `ScriptRunner` class provides the required APIs to run schema creation scripts.

This code snippet makes sure that business entity and saga tables are created in the tenant databases.

snippet: CreateSchema

Due to the Outbox tables being stored in multiple databases (one per tenant), SQL Persistence is not able to automatically clean Outbox entries. This setting must be confirmed by disabling Outbox cleanup:

snippet: DisablingOutboxCleanup

The Outbox tables on each tenant database must be [cleaned by an outside process like SQL Agent](/persistence/sql/multi-tenant.md#disabling-outbox-cleanup).


#### Connecting to the tenant database

To allow for database isolation between the tenants the connection to the database needs to be created based on the message being processed. This requires cooperation of two components:

 * [Pipeline behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md) to extract the tenant information from a message header and ensures that it is propagated to any outgoing messages generated during processing
 * A [tenant-aware connection factory](/persistence/sql/multi-tenant.md#specifying-connections-per-tenant) for SQL Persistence

The connection factory retrieves the value of the `tenant_id` header and builds a connection string based on the header value.

snippet: ConnectionFactory

When SQL Persistence needs to open a connection, the connection factory is called using the value extracted from the message. As an alternative, other connection factory options exist that allow [consulting multiple headers to extract tenant information](/persistence/sql/multi-tenant.md#specifying-connections-per-tenant).

include: persistence-propagation
