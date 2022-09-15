---
title: Transactional Session with RavenDB Persistence
summary: How to configure the transactional session with RavenDB Persistence
component: TransactionalSession
reviewed: 2022-09-12
redirects:
related:
- persistence/ravendb
---

In order to use the TransactionalSession feature with RavenDB persistence, add a reference to the `NServiceBus.RavenDB.TransactionalSession` NuGet package.

## Configuration

To enable the TransactionalSession feature:

snippet: enabling-transactional-session-ravendb

## Opening a session

To open a RavenDB transactional session:

snippet: open-transactional-session-ravendb

### Multi-tenancy support

The specific tenant database name is retrieved from message headers as configured in the [`SetMessageToDatabaseMappingConvention`-method](/persistence/ravendb/#multi-tenant-support).
This header needs to be set in the options so that the method has the necessary information available when storing operations.

snippet: open-transactional-session-ravendb-multitenant

## Transaction usage

Message and database operations made via the the transactional session are committed together once the session is committed:

snippet: use-transactional-session-raven

See the [RavenDB shared session documentation](/persistence/ravendb/#shared-session) for further details about using the transaction.

WARN: In order to guarantee atomic consistency across message and database operations, the [Outbox](/nservicebus/outbox) needs to be enabled. Otherwise `Commit` will execute all operations in a best-effort fashion.