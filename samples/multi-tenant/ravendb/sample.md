---
title: RavenDB Persistence in multi-tenant system
summary: Configure RavenDB Persistence to support multi-tenant scenarios.
reviewed: 2019-02-15
component: Raven
related:
- persistence/ravendb
- nservicebus/outbox
---

include: dtc-warning

This sample demonstrates how to configure RavenDB Persistence to store tenant-specific data in separate databases. The tenant-specific information includes the saga state, the business documents that are accessed using [RavenDB-managed session](/persistence/ravendb/#shared-session), and the outbox records.

This sample uses the [Outbox](/nservicebus/outbox/) to guarantee consistency between the saga state and the business entity.

The sample assumes that the tenant information is passed as a custom message header `tenant_id`.

downloadbutton


## Prerequisites

The databases created by this sample are:

 * `MultiTenantSamples`
 * `MultiTenantSamples-A`
 * `MultiTenantSamples-B`

include: persistence-running

include: persistence-code


#### Outbox cleanup

The built-in Outbox cleanup does not work in a multi-tenant environment because it executes in the context of the shared database while the Outbox documents are stored in the tenant databases. For that reason it needs to be disabled.

snippet: DisableOutboxCleanup

Alternative solution needs to be configured. The simplest way to ensure that the dispatched Outbox documents are removed is to use the RavenDB [Document expiration](https://ravendb.net/docs/article-page/3.5/Csharp/server/bundles/expiration) bundle. Following code ensures that tenant databases are created with this bundle enabled:

snippet: CreateDatabase

To make sure the expiration bundle removes old Outbox documents they need to be marked for expiry. This task is performed by a document store listener.

snippet: DocumentStoreListener

This component is executed each time the RavenDB session is about to send data modifications to the server. It checks if the updated document is an Outbox record and if it has been marked as dispatched. In that case it marks it for expiry after 10 days.


#### Connecting to the tenant database

To allow for database isolation between the tenants the connection to the database needs to be created based on the message being processed. RavenDB persistence offers an extension point which allows to customize the database name to be used when opening a session.

snippet: DetermineDatabase

The code above ensures that when the `tenant_id` header is present, the session will point to the tenant database.

include: persistence-propagation
