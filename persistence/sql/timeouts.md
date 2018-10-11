---
title: Timeouts Persister
component: SqlPersistence
reviewed: 2018-05-23
versions: '[4,)'
---

## Connection

The timeouts persister can be configured to use a dedicated connection builder. For example it may be used for creating timeouts tables in a separate database.

snippet: SqlPersistenceTimeoutConnection