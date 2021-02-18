For a description of each feature, see the [persistence at a glance legend](/persistence/#persistence-at-a-glance).

|Feature                    |   |
|:---                       |---
|Supported storage types    |Sagas, Outbox, Subscriptions, Timeouts
|Transactions               |via `IDocumentSession.SaveChanges()`
|Concurrency control        |Optimistic concurrency, optional custom pessimistic concurrency for performance
|Scripted deployment        |Not supported
|Installers                 |None. Required indexes are created in the database as needed.