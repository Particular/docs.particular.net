---
title: NHibernate persistence in multi-tenant systems
summary: Configure NHibernate persistence to support multi-tenant scenarios.
reviewed: 2020-12-22
component: NHibernate
related:
- persistence/nhibernate
- nservicebus/outbox
---

This sample demonstrates how to configure NHibernate persistence to store tenant-specific data in separate catalogs for each tenant. The tenant-specific information includes saga state and business entities that are accessed using [NServiceBus-managed session](/persistence/nhibernate/accessing-data.md).

This sample uses [Outbox](/nservicebus/outbox/) to guarantee consistency between the saga state and the business entity. Outbox data as well as timeout data are stored in a dedicated catalog shared by all tenants.

The sample assumes that the tenant information is passed as a custom message header `tenant_id`.

downloadbutton


## Prerequisites

include: sql-prereq

The databases created by this sample are:

 * `NHibernateMultiTenantReceiver`
 * `NHibernateMultiTenantA`
 * `NHibernateMultiTenantB`

include: persistence-running

include: persistence-code

#### Creating the schema

The default NHibernate persistence installers create all schema objects in a single catalog. In a multi-tenant scenario schema objects need to be created manually. The `ScriptGenerator` class provides APIs required to generate schema creation scripts.

snippet: CreateSchema

The above code makes sure that business entity and saga tables are created in the tenant databases while the timeouts and outbox -- in the shared database.

Because the outbox table is stored in the shared catalog, the NHibernate persistence is not able to access it when using the tenant connection string. Synonyms (a feature of SQL Server) provide a way to solve this problem. The following code creates synonyms for the `OutboxRecord` table in both tenant databases. These synonyms instruct the query processor to use the outbox table in the shared database whenever it encounters a reference to `OutboxRecord`.

snippet: Synonyms


#### Configuring NHibernate persistence to recognize business entities

In order to be able to use [NServiceBus-managed session](/persistence/nhibernate/accessing-data.md) to retrieve and store business entities, NHibernate configuration used by NServiceBus needs to be configured properly.

snippet: NHibernateConfiguration


#### Connecting to the tenant database

To allow database isolation between tenants, the connection to the database needs to be created based on the message being processed. This requires cooperation of two components:

 * A behavior that inspects an incoming message and extracts the tenant information 
 * A custom `ConnectionProvider` for NHibernate

The custom connection provider has to be registered with NHibernate

snippet: ConnectionProvider

The behavior retrieves the value of the `tenant_id` header and builds a connection string based on the header value. Then it stores the connection string in the async context via `AsyncLocal`.

snippet: PutConnectionStringToContext

The behavior has to be registered in the pipeline configuration

snippet: ExtractTenantConnectionStringBehavior

When NHibernate needs to open a connection, the custom connection provider retrieves the connection string value from the async context and, if present, opens a connection to the tenant database. Otherwise it opens a connection to the shared database.

snippet: GetConnectionFromContext

include: persistence-propagation
