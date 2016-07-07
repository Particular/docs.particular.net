---
title: Transactions in Azure
summary: Understanding transactions in Azure and NServiceBus.
tags:
- Azure
- Cloud
- Transactions
- Idempotency
- DTC
redirects:
 - nservicebus/understanding-transactions-in-windows-azure
reviewed: 2016-04-20
---

The Azure Platform and NServiceBus complement each other. Azure is a distributed, scalable and flexible platform. NServiceBus provides high level abstractions and features that make development in Azure easier.

There are a few things to keep in mind when developing for Azure. The most important one is the lack of (distributed) transactions.


## Understanding transactions

Transaction processing is designed to maintain systems integrity (typically a database or some modern filesystems and services), i.e. to always keep them in a consistent state. That is achieved by ensuring that interdependent operations are either all completed successfully or all canceled. This article focuses on Azure databases and storage services.

In order to guarantee integrity the database engine must lock a certain number of records inside the transaction when updating values. Which records and how many of them are locked depends, among others, on the selected isolation level.

It is really important to understand, especially in the context of cloud services, that other transactions cannot work with locked records at the same time. In a cloud or self-service environment such locks become a trust issue, because external parties can use them to perform a denial of service attack (sometimes not even intentionally).

This is the primary reason why many Azure hosted services do not support transactions at all or are very aggressive when it comes to the lock duration, for example:

 * Azure Storage Services officially do not participate in transactions. If the transactional behavior is required, it needs to be implemented in the specific system.
 * The Azure SQL Server supports local transactions (with .NET 4.6.1 and higher), but only grants locks on resources for 20 seconds (when requested by a system task) or 24 hours (otherwise). See [Azure SQL Database resource limits](https://azure.microsoft.com/en-us/documentation/articles/sql-database-resource-limits/) for more details.

The lock duration can be directly controlled when both the database management system and client are under the same ownership, e.g. when SQL Server is deployed to the virtual machine. But even in that scenario distributed transactions must be used carefully.


## Understanding distributed transactions and the two-phase commit protocol

When multiple transaction-aware resources are involved in a single transaction, then this transaction automatically is promoted to a distributed transaction. That means that handling the unit of work is performed outside the database system by the so-called Global Transaction Manager, or Distributed Transaction Coordinator (DTC). The coordinator is a service on the machine where the transaction started. It communicates with similar services, called resource managers, which are running on other machines involved in the transaction. They communicate using the two-phase commit protocol.

As illustrated in the diagram, the two-phase commit protocol consists of two phases. During the preparation phase the Global Transaction Manager instructs all resource managers to get ready to commit. Then all resource managers need to inform the Global Transaction Manager whether they approve the transaction. After collecting that information the Global Transaction Manager instructs all resource managers to either complete the commit or to rollback.

![Two Phase Commit](two-phase-commit.png)

Note that this protocol requires two communication steps for each resource manager added to the transaction and requires a response from each of them to be able to continue. Both of these conditions are problematic in a huge data center such as Azure because:

 * Two extra communication steps per each resource manager result in an exponential explosion of additional communication: 2 resources - 4 network calls, 4 resources - 16 calls, 100 resources - 10000 calls, etc.
 * Azure data centers are huge and consist of thousands of machines. That means the failure needs to be expected and all systems must be able to deal with network partitions. The network partition results in slow or [in doubt](https://msdn.microsoft.com/en-us/library/ms681727.aspx) transactions. Therefore the requirement to wait for responses from all resource managers is problematic even if the communication overhead is manageable.

The latter is the primary reason why none of the Azure services supports distributed transactions, and the recommendation is not to use them even if it's technically possible.


## Recommendations for using NServiceBus in Azure

In case of other transports (MSMQ, SQL Server) NServiceBus by default relies on DTC to make distributed system reliable and ensure consistency. But in the Azure environment DTC shouldn't be used, as explained above.

There are a few possible approaches to deal with that limitation. The rest of this article lists the options and discusses their advantages and disadvantages. Which approach would be the best depends on the specific system.

The possible approaches:

 * Share local transaction
 * Unit of work with batching and transport retries
 * Atomic operations with transport retries
 * Sagas and compensation logic
 * Routing slips and compensation logic


### Sharing local transactions

To prevent promoting transaction to the distributed transaction, one can reuse a single local transaction between resources. The outermost local transaction/unit of work, started by the receiving transport is passed to the rest of the application (e.g. to an ORM). This way only a single transaction is used for both receiving and handling a message and its result.


#### Advantages

 * It is possible to prevent escalating transaction to a distributed transaction. The only change is injecting the transport level transaction into other parts of the application.


#### Disadvantages

 * In this approach there can be only a single transactional resource in the entire system. The technique can only be applied if the application fits the limitations of this transactional resource. As some Azure services throttle quite aggressively, sometimes on behavior of other tenants, capacity planning might become an issue.
 * Injecting the transaction might be a challenge in some parts of the system, e.g. when using third-party libraries.


### Atomic operations and transport retries

If a resource does not support transactions, to ensure consistency one can use atomic operations combined with automatic retries. The idea is that every operation is *transactional* (atomic), meaning that the whole operation either succeeds or fails as a single unit. If all operations conform to that rule then transactions are not needed anymore.

One of operations that fit this criteria is a unit of work pattern with batching. With some restrictions, it can be used to emulate a transaction. Azure Storage Services allow to group a number of operations into a single batch in order to make the whole set of operations atomic. However, it works only for Azure Storage Tables and only when the partition key for all operations is the same.

Another important consideration is that regular transactions also have a *rollback* mechanism that will allow the message receiver to retry processing the original message later without causing unintended side-effects. When using transport that has automatic retry functionality, it is necessary to also support this kind of rollback semantics.


#### Advantages

 * The consistency can be achieved without transactions.
 * Message processing can be retried automatically after transient exceptions.


#### Disadvantages

 * The application must ensure that operations related to business logic are atomic, i.e. have a single insert, update or delete statement per operation. That often requires changes in programs structure.
 * Operations related to business logic have to be idempotent. That guarantees that automatic retries don't cause unintended side-effects. [The need for idempotency](#the-need-for-idempotency) section discusses techniques to achieve idempotency.
 * The retry behavior is usually combined with timeouts. The timeouts cause retries not only if the operation fails, but also when it is too slow. This can lead to situations where the same operation executes multiple times in parallel, even though it hasn't failed.


### Sagas and compensation logic

Sagas in NServiceBus are essentially a stateful set of message handlers that can be used to track and orchestrate the transaction. The handlers communicate with each other, each of them performs a part of the transaction and then notifies whether it succeeded or failed. Depending on the partial results, the saga decides what needs to happen to the rest of the transaction; whether to continue the transaction or to roll it back. The latter is often referred to as *compensation*, as it tries to compensate the failure at a business logic level.

In essence, using sagas is implementing a Distributed Transaction Coordinator that operates on business logic level instead of using two-phase commit protocol.


#### Advantages

 * The consistency can be achieved without transactions.
 * This approach is extremely flexible and maps very well to the business domain.


#### Disadvantages

 * Considering and implementing all possible variations and error conditions in a transaction is a non-trivial amount of work.
 * Effective implementation requires a good understanding of the business requirements. The implementation details are driven by business decisions more than technical considerations, so it's recommended to collaborate closely with business experts when designing sagas.
 * Sagas are stateful therefore the operation cannot be atomic. This needs to be taken into consideration when sagas are used in combination with atomic operations that cannot be batched. Operations against a Store should never be executed directly inside the saga itself. Instead, they should be executed by another handler and queued in between, to cater for idempotency at all levels.


### Routing slips and compensation logic

Instead of using a central coordinator that orchestrates the work in a transaction, this approach uses a chain of independent handlers. It can be thought of as a linked list of message handlers that can send messages back and forward within the list.

The first handler composes a *routing slip* that defines what needs to be done in the transaction, and passes it to the next handler in the chain. The next handler processes the message according to the information in the routing slip, and passes the message to the next one in the chain and so forth, until all handlers have been invoked and the transaction was completed.

If at any point in time a handler fails, it sends the message back in the chain to execute compensation logic. The slip is continuously sent back until it reaches the original handler again. At that point the transaction is rolled back.


#### Advantages

 * The consistency can be achieved without transactions.
 * The conceptual model is more "linear" than sagas, so it might be easier to analyze and implement.
 * It doesn't require maintaining a state in the data store. The state information can be passed in the routing slip.


#### Disadvantages

 * Considering and implementing all possible variations and error conditions in a transaction is a non-trivial amount of work.
 * The handlers can't be executed in parallel, therefore the implementation is often slower than when using sagas.
 * This approach is less flexible than sagas.


## The need for idempotency

Every approach involving automatic retries will result in `at least once` delivery semantics. In other words, the same message can be processed multiple times. This has to be taken into account when designing the business logic, by ensuring that every operation is idempotent.

There are multiple ways to achieve idempotency, some at the technical level, others built into the business logic:

 * Message deduplication
 * Natural idempotency
 * Entities and messages with version information
 * Side effect checks
 * Partner state machines
 * Accept uncertainty


### Message deduplication

Message deduplication is the easiest way to detect if a message has been executed already. Every message that has been processed so far is stored. When the new message comes in, it is compared to the set of already processed messages (usually by comparing only their unique identifiers). If the message is identical to one of the stored messages, then it means it is a duplicate and the new message won't be processed again.

The main advantage of this approach is its simplicity, however it also has downsides. As every message needs to be stored and searched for, it can reduce message throughput because of the additional lookups. That can potentially cause high contention on the message store.


### Natural idempotency

Many operations can be designed in a naturally idempotent way. `TurnOnTheLights` is by default an idempotent operation, because it will have the same effect no matter what was the previous state and how many times the operation was executed. `FlipTheLightSwitch` however is not naturally idempotent, because the results will vary depending on the initial state and the number of times it was executed.

Using natural idempotency is recommended whenever possible.


### Entities and messages with version information

Idempotency can be also achieved by adding versioning information to the entities. Typically it is in the form of a timestamp or a version number.

The versioning information is included in each command that alters the state of the entity. This way when the command is received, the handler can compare the versioning information on both the entity and the message and decide whether this logic needs to be executed or not.

The downside of this approach is that the version of the entity can be change by different commands, and may therefore cause unexpected outcomes when unrelated commands arrive in a different order than logically sent. When using this approach, one has to consider how such conflicts will be resolved.


### Partner state machines

Ultimately the only non-idempotent messages are sent when one entity issues a command to another. The good practice in messaging solutions is to have only `one master`, i.e. there should only be one logical endpoint sending a given command. As a consequence it is possible to organize the entity's state as a miniature state machine inside the entity.

The state machine represents the progression of the relationship between endpoints that communicate with each other. That approach avoids versioning conflicts by allowing only for valid state transitions. As a result, it is possible to ensure each command is executed only once.


### Side effect checks

In some situations it is possible to verify if a command has been executed by checking its indirect side effects, for example when `TheFireIsHot` flag is set to true, then there is no need to `TurnOnTheFire`.

Arguably it's a risky approach, that can easily lead to subtle errors. Although it's useful in the real world, it has to be used very carefully and only if no other approach can be used.


### Accept uncertainty

In some systems it is possible to accept uncertainty and potential inaccuracies caused by non-idempotent messages. In some cases the data doesn't have to be consistent at all times. In other systems there might be mechanisms that allow for dealing with inconsistencies afterwards.

Although that might seem unacceptable for many programmers, in the end it is a business decision. It's always recommended to talk to business experts and double-check what are their expectations.