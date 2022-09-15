---
title: Transactional Session with SQL Persistence
summary: How to configure the transactional session with SQL Persistence
component: TransactionalSession
reviewed: 2022-09-12
redirects:
---

In order to use the TransactionalSession feature with SQL Persistence, add a reference to the `NServiceBus.Persistence.Sql.TransactionalSession` NuGet package.

## Configuration

To enable the TransactionalSession feature:

snippet: enabling-transactional-session-sqlp

## Opening a session

To open a SQL Persistence transactional session:

snippet: open-transactional-session-sqlp

## Multi-tenancy support

The specific tenant id that is used to construct the connection string is retrieved from message headers as configured in the [`MultiTenantConnectionBuilder`-method](/persistence/sql/multi-tenant.md).
This header needs to be set in the options so that the method has the necessary information available when storing operations.

snippet: open-transactional-session-sqlp-multitenant