---
title: DTC not supported for RavenDB Persistence
component: raven
reviewed: 2017-10-17
---

RavenDB's implementation of distributed transactions contains a bug that can cause an endpoint, in certain (rare) conditions, to lose data. The RavenDB team [has no further plans to address this issue](http://issues.hibernatingrhinos.com/issue/RavenDB-4431). Starting with RavenDB 4.0, RavenDB will not support the Distributed Transaction Coordinator (DTC), making this bug have no practical relevance beyond RavenDB 3.5.

Although the chances of the data loss bug occurring are slim, the use of DTC transactions with RavenDB is not recommended and not supported.


## Affected versions

All customers using RavenDB persistence with distributed transactions, which are enabled by default, are affected.

Customers using RavenDB with local transactions only, and who have disabled the Distributed Transaction Coordinator on the servers running endpoints, are not affected.

All affected versions of the NServiceBus.RavenDB package have been patched to log a warning if an unsafe configuration is detected. In Versions 5.0 and above, the configuration will not be supported and will throw an exception if detected.


## Cause of data loss

RavenDB uses a client-side DTC implementation, where the RavenDB client library stores the results of in-flight transactions in local storage. During the Prepare phase of the transaction, RavenDB creates a file in the local storage and writes transaction recovery information into it. During the Commit phase, the session commits the transaction to the server and then deletes the recovery file.

The Commit phase does not occur on the thread running the transaction scope. Instead, Commit occurs on a separate ThreadPool thread, after the TransactionScope has been completed and disposed. This means that if a failure occurs during the Commit phase, perhaps due to a temporary network issue or database restart, the calling code has no way to know the failure occurred and it has already moved on to subsequent instructions after the end of the transaction.

The data for the pending transaction has not yet been sent to the server at the end of the Prepare phase. As such it is possible for the transaction to be effectively lost if the RavenDB database becomes unavailable between the execution of the Prepare and Commit phases. In this circumstance, no exception is thrown on the main thread, so NServiceBus has no way to know that the transaction is effectively lost.


## Recommendations

Using the RavenDB persistence with distributed transactions enabled is a significant risk, as there is always a possibility that a transaction could be dropped, resulting in lost data.

All customers using RavenDB persistence are recommended to either:

 1. Disable the Distributed Transaction Coordinator altogether and use the [Outbox feature](/nservicebus/outbox/) to manage consistency between message transport and persistence
 1. Migrate to a different persistence technology altogether


### Disabling DTC + Outbox

In NServiceBus, the DTC is used to guarantee consistency between messaging operations and the data persistence. Messaging operations includes the message being processed as well as any messages being sent out as a result. Data persistence includes any business data persisted, as well as any NServiceBus saga or timeout data stored in the database. The DTC ensures that all these operations either complete successfully together, or are all rolled back.

Instead of the DTC, the [Outbox feature](/nservicebus/outbox/) can be used to mimic this level of consistency without the need for distributed transactions. It does this by first storing any outgoing messages in the database, taking advantage of same (non-distributed) local transaction to ensure that the messaging operations are stored atomically with any changes to business data and NServiceBus data. Once that transaction is successfully committed, the data for the outgoing messages are dispatched to the message transport separately.

{{WARNING:
The Outbox relies upon the local RavenDB database transaction, this means that all business data and NServiceBus data for the transaction must be contained within the same RavenDB database.

*Due to the one-database rule, the Outbox feature cannot guarantee consistency in a mixed-database environment. For example, when storing NServiceBus persistence data in RavenDB while storing business data in SQL Server, consistency cannot be achieved using the Outbox feature. In this type of situation, transitioning to a different persistence as described below is required.*
}}

To migrate away from DTC, refer to the [Outbox documentation](/nservicebus/outbox/), especially the section on [converting from DTC to Outbox](/nservicebus/outbox/#converting-from-dtc-to-outbox), as well as the [Outbox with RavenDB persistence](/persistence/ravendb/outbox.md) article, especially the section on the [effect of the Outbox on a RavenDB DocumentStore](/persistence/ravendb/outbox.md?version=raven_4#effect-on-ravendb-documentstore), being sure to use the *Switch Version* button just below the article titles to customize the content for the versions of NServiceBus and NServiceBus.RavenDB currently in use.


### Transitioning to a different persistence

The path to transition from RavenDB to a different persistence vary by version of NServiceBus.


#### NServiceBus 6.x

To maintain use of the DTC, the best solution is to transition away from RavenDB persistence toward a different solution. The Outbox only works if all business data and NServiceBus data is stored in the same RavenDB database. Therefore, if message endpoints use RavenDB for NServiceBus persistence, but also modify data in a SQL Server database, then a non-DTC solution like the Outbox will not work.

In this situation, consider switching to the [SQL Persistence](/persistence/sql/) library, with data stored in Microsoft SQL Server or Oracle, both of which support distributed transactions.

Because it stores saga data as JSON blobs in much the same way as RavenDB, SQL persistence also provides a smooth data migration path from existing RavenDB data. Contact [support@particular.net](mailto:support@particular.net) to pursue this option.

Before beginning a migration, remember that different persistence can be used by different endpoints within the same solution. Therefore, any new endpoint added to a current solution should use the new persistence from the start.


#### NServiceBus 5.x and lower

SQL persistence is only available for NServiceBus 6 and above. Therefore, customers using NServiceBus 5 or lower that cannot use the Outbox method described above will need to first [upgrade to NServiceBus 6](/nservicebus/upgrades/5to6/) with RavenDB persistence. After that upgrade is complete, a transition can be made to SQL Persistence as described above.

It is also possible to migrate to [NHibernate Persistence](/persistence/nhibernate/), however due to mismatches in how RavenDB and NHibernate Persistence store saga data, this type of migration can be much more cumbersome.


## Summary

Going forward, DTC transactions with RavenDB will not be supported, although support will be given to existing customers to migrate to a different solution using one of the alternatives above. Contact [support@particular.net](mailto:support@particular.net) for a more thorough discussion of a migration path.
