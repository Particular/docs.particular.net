---
title: Transactional Session with RavenDB Persistence
summary: How to configure the transactional session with RavenDB Persistence
component: TransactionalSession
reviewed: 2022-09-12
redirects:
---

In order to use the TransactionalSession feature with RavenDB Persistence, add a reference to the `NServiceBus.RavenDB.TransactionalSession` NuGet package.

## Configuration

To enable the TransactionalSession feature:

snippet: enabling-transactional-session-ravendb

## Opening a session

To open a RavenDB transactional session:

snippet: open-transactional-session-ravendb

## Multi-tenancy support

The specific tenant database name is retrieved from message headers as configured in the [`SetMessageToDatabaseMappingConvention`-method](/persistence/ravendb).
This header needs to be set in the options so that the method has the necessary information available when storing operations.

snippet: open-transactional-session-ravendb-multitenant