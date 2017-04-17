---
title: Understanding Transactionality in Azure
tags:
- Azure
- Transactions
- Idempotency
- DTC
redirects:
 - nservicebus/transactions
 - nservicebus/understanding-transactions-in-windows-azure
reviewed: 2016-04-20
---

The Azure Platform and NServiceBus complement each other. Azure is a distributed, scalable and flexible platform. NServiceBus provides high level abstractions and features that make development in Azure easier.

There are a few things to keep in mind when developing for Azure. The most important one is the lack of (distributed) transactions.


## No transactions is the default

Transaction processing is designed to maintain systems integrity (typically a database or some modern filesystems and services), i.e. to always keep them in a consistent state. That is achieved by ensuring that interdependent operations are either all completed successfully or all canceled. This article focuses on Azure databases and storage services.

To guarantee integrity the database engine, or service, must lock a certain number of records inside the transaction when updating values. Which records and how many of them are locked depends, among others, on the selected isolation level.

It is really important to understand, especially in the context of cloud services, that other transactions cannot work with locked records at the same time. In a cloud or self-service environment such locks become a trust issue because external parties can leverage them to perform a denial of service attack (sometimes not even intentionally).

This is the primary reason why many Azure hosted services do not support transactions at all or are very aggressive when it comes to the lock duration, for example:

 * Azure Storage Services officially do not participate in transactions. If a transactional behavior is required, it needs to be implemented in the client system as atomic operations within the limits imposed by Azure Storage Services on atomicity.
 * The Azure SQL Server supports local transactions (with .NET 4.6.1 and higher), but only grants locks on resources for 20 seconds (when requested by a system task) or 24 hours (otherwise). See [Azure SQL Database resource limits](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-resource-limits) for more details.

## Understanding distributed transactions and the two-phase commit protocol

When both the database management system and client are under the same ownership, e.g. when SQL Server is deployed to the virtual machine, then transactions are available and the lock duration can be controlled. 

Even in the above scenario distributed transactions must be used carefully.

When multiple transaction-aware resources are involved in a single transaction, then this transaction automatically is promoted to a distributed transaction. That means that handling the unit of work is performed outside the database system by the so-called Global Transaction Manager, or Distributed Transaction Coordinator (DTC). 

The coordinator is a service on the machine where the transaction started. It communicates with similar services, called resource managers, which are running on other machines involved in the transaction. They communicate using the two-phase commit protocol.

As illustrated in the diagram, the two-phase commit protocol consists of two phases. During the preparation phase the Global Transaction Manager instructs all resource managers to get ready to commit. Then all resource managers need to inform the Global Transaction Manager whether they approve the transaction. After collecting that information the Global Transaction Manager instructs all resource managers to either complete the commit or to rollback.

![Two Phase Commit](two-phase-commit.png)

Note that this protocol requires two communication steps for each resource manager added to the transaction and requires a response from each of them to be able to continue. 

**Both of these conditions are problematic in a huge data center such as Azure** because:

 * Two extra communication steps per each resource manager result in an exponential explosion of additional communication: 2 resources - 4 network calls, 4 resources - 16 calls, 100 resources - 10000 calls, etc. This limits scalability.
 * Azure data centers consist of hundreds of thousands of machines. That means that failure needs to be expected and all systems must be able to deal with network partitions. Network partitions result in slow or [in doubt](https://msdn.microsoft.com/en-us/library/ms681727.aspx) transactions. Therefore the requirement to wait for responses from all resource managers is problematic even if the communication overhead is manageable.

The latter is the primary reason why none of the Azure services supports distributed transactions, and the recommendation is not to use them in new designs even if it's technically possible.
