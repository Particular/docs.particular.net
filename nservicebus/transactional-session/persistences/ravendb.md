---
title: Transactional Session with RavenDB Persistence
summary: How to configure the transactional session with RavenDB Persistence
component: TransactionalSession.RavenDB
reviewed: 2025-01-20
redirects:
related:
- persistence/ravendb
- nservicebus/transactional-session
---

To use the [transactional session feature](/nservicebus/transactional-session/) with RavenDB persistence, add a reference to the `NServiceBus.RavenDB.TransactionalSession` NuGet package.

## Configuration

To enable the transactional session feature:

snippet: enabling-transactional-session-ravendb

## Opening a session

To open a RavenDB transactional session:

snippet: open-transactional-session-ravendb

### Multi-tenancy support

The specific tenant database name is retrieved from message headers as configured in the [`SetMessageToDatabaseMappingConvention`-method](/persistence/ravendb/#multi-tenant-support).
This header needs to be set in the options to make the necessary information available when storing operations and interacting with the outbox.

snippet: open-transactional-session-ravendb-multitenant

## Transactions usage

Message and database operations made via the transactional session are committed together once the session is committed:

snippet: use-transactional-session-raven

For further details about using the transaction, see the [RavenDB shared session documentation](/persistence/ravendb/#shared-session).

include: ts-outbox-warning
