---
title: Subscription Persister
component: SqlPersistence
reviewed: 2026-09-30
redirects:
 - nservicebus/sql-persistence/subscriptions
---

partial: caching

## Connection

The subscription persister can be configured to use a dedicated connection builder. For example, it may be used for creating subscription tables in a separate database.

snippet: SqlPersistenceSubscriptionConnection
