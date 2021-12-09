For a description of each feature, see the [persistence at a glance legend](/persistence/#persistence-at-a-glance).

|Feature                    |   |
|:---                       |---
|Supported storage types    |Sagas, Outbox, Subscriptions
|Transactions               |via `IDocumentSession.SaveChanges()` or cluster-wide transactions
|Concurrency control        |Pessimistic concurrency, optional optimistic concurrency
|Scripted deployment        |Not supported
|Installers                 |None. Required indexes are created in the database as needed.