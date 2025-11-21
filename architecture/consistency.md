---
title: Consistency
summary: Ensuring consistency in message-driven systems using transactions, outbox patterns, idempotency, deduplication, workflows, and key trade-offs.
reviewed: 2025-07-15
callsToAction: ['solution-architect']
redirects:
- nservicebus/azure/understanding-transactionality-in-azure
- nservicebus/understanding-transactions-in-windows-azure
---

## Transactions

Transactions are designed to keep a system in a consistent state at any point in time. This is achieved by ensuring interdependent operations are completed as a single [unit of work](https://en.wikipedia.org/wiki/Unit_of_work). If any single operation fails, then the entire transaction fails and the system is reverted to the state it was in before the transaction started.

### Limitations

There are several challenges and limitations when relying on transactions:

#### Scalability

Scalable systems need to handle high levels of concurrency. Transactions need concurrency control mechanisms to ensure proper isolation between transactions trying to access the same resources. These concurrency control mechanisms can severely impact a resource's performance and introduce bottlenecks that complicate scaling.

When using [horizontal scaling](https://learn.microsoft.com/en-us/azure/well-architected/scalability/design-scale#understand-horizontal-and-vertical-scaling), additional mechanisms are required to ensure the desired level of consistency across all scale units. This typically involves complicated distributed consensus algorithms. Depending on the consistency guarantees required, many, or even all, nodes of a scaled component need to participate in the transaction, negatively impacting latency and availability.

#### Cost

Besides the performance impact of transactions, transactional operations may even be charged extra on certain managed cloud services. Broad usage of transactional consistency may therefore incur significant costs. It is important to understand a cloud vendor's pricing models to estimate the financial cost of using transactions.

#### Timeouts

Transactions impact concurrent operations on a locked resource. This may block other actors and consume additional resources. Therefore, transactions should be completed within a short time window to avoid issues. Cloud services typically restrict transaction lifetimes more than traditional on-premises technologies.

#### Scope

Some cloud services like Azure Table Storage, Azure Cosmos DB Table API, and Azure Cosmos DB SQL API offer transactions, but they are usually scoped to a single partition key. When transactions are scoped to a single partition key, atomic storage operations must share the same partition key on all create, read, update, and delete operations.

### Cross-entity transactions

Databases and queueing technologies may support atomic transactions for a single entity (e.g. a message queue, or a database document) or multiple entities (sometimes also restricted to a single partition). Without cross-entity transactions, queueing systems cannot atomically acknowledge (consume) a message together with sending outgoing messages, which might lead to duplicate messages when retrying a failed message.

[**Compare message queue transaction capabilities →**](/transports/transactions.md)

### Distributed transactions

Distributed transactions span multiple technologies, such as a database and a message queue. On Windows, the [Distributed Transaction Coordinator (DTC)](https://en.wikipedia.org/wiki/Microsoft_Distributed_Transaction_Coordinator) coordinates distributed transactions across multiple compatible participants using a [two-phase commit protocol](https://en.wikipedia.org/wiki/Two-phase_commit_protocol). Each participant must explicitly support the two-phase commit protocol.

> [!WARNING]
> While traditional on-premises focused services like MSMQ or Microsoft SQL Server support DTC transactions, managed cloud services do not, and require other strategies for achieving consistency between resources such as:
>
> * The [outbox pattern](#transactions-outbox-pattern), which uses database capabilities to achieve atomic consistency for data and message operations
> * [Stateful workflows](workflows.md), which supervise the successful completion of all involved resources and manage compensating actions to achieve eventual consistency.

### Outbox pattern

The [transactional outbox pattern](https://microservices.io/patterns/data/transactional-outbox.html) provides atomically consistent database and message queue operations. Due to the lack of distributed transactions in modern application environments, the outbox pattern is commonly used to satisfy consistency requirements.

The outbox pattern is implemented by storing outgoing messages in the same database as business data. This allows the use of database transaction capabilities to achieve atomic consistency. In the second step, the persisted messages are dispatched to the message queue.

The nature of the outbox pattern may cause outgoing messages to be dispatched multiple times, which requires [idempotency](#idempotency) in receivers.

> [!NOTE]
> Implementing the outbox pattern is very risky and error-prone. Small mistakes can lead to unintended behavior and message or data loss. The [NServiceBus outbox feature](/nservicebus/outbox/) implements the outbox pattern, including built-in message deduplication when receiving messages, and is thoroughly tested and well-documented.

## Idempotency

[Idempotence](https://en.wikipedia.org/wiki/Idempotence) ensures that processing a message multiple times has the same effect as processing it only once. This means that processing a duplicate message shouldn’t cause any unintended side effects.

Idempotency is an approach used as part of avoiding the use of distributed transactions. Without transactions, [recoverability mechanisms](/architecture/recoverability.md) that prevent message losses in failure scenarios may produce duplicate messages. There are multiple ways to achieve idempotency, implemented at different levels.

[**Blog: What does idempotent mean in software systems? →**](https://particular.net/blog/what-does-idempotent-mean)

### Natural idempotency

Many operations can be designed in a naturally idempotent way. For example, `TurnOnTheLights` is an idempotent operation because it will have the same effect no matter the previous state and how many times the operation is executed. `FlipTheLightSwitch` however is not naturally idempotent because the results will vary depending on the initial state and the number of times it was executed. Changes to naturally idempotent code should be carefully reviewed to ensure that idempotency is retained.

Using natural idempotency is recommended whenever possible.

### Message deduplication

Message deduplication is the easiest way to detect if a message has been processed already. Every message that has been processed so far is stored. When a new message comes in, it is compared to the set of already processed messages (usually by comparing their unique identifiers). If the message is identical to one of the stored messages, it is a duplicate and is not processed again.

One advantage of this approach is its simplicity. However, it has downsides. Every message needs to be stored and searched for, which can reduce message throughput. Deduplication storage is not infinite, limiting the deduplication guarantees of this approach to the provided storage capacity. Identifiers for deduplication typically operate on technical IDs (e.g. message ID), which means this approach does not work for duplicate message _content_.

> [!NOTE]
> Implementing message deduplication is risky and error-prone. Small mistakes can lead to unintended behavior or message loss. The [NServiceBus Outbox feature](/nservicebus/outbox/) implements message deduplication and is thoroughly tested and well-documented.

### Side effect checks

Message deduplication assumes a generic deduplication mechanism that works for all incoming messages, regardless of the specific message content. When this is not possible, duplicate message processing can be avoided by checking for expected side effects. These side effects are only known for a specific message type and its related business logic. For example, when `TheFireIsHot` flag is set to true, then there is no need to `TurnOnTheFire`. This approach is less reusable and requires an in-depth understanding of the business logic to deal with duplicate messages.

Additional metadata may be stored to enable side effect checks. For example, a message timestamp or a unique identifier (provided by the author of a message) is attached to a database record that is stored while processing that message.

## Best practices

### Break business logic into smaller steps

Business logic triggered by incoming messages may coordinate actions and states across multiple resources such as databases and web APIs. Any interaction with any resource may fail for various expected and unexpected reasons, causing the whole process to be repeated. By breaking workflows into smaller steps that each involve only interacting with a message queue and one other resource, the impact of a failure inside the workflow can be greatly reduced, making idempotency patterns easier to apply.

[**Blog: Autosave for business processes →**](https://particular.net/blog/autosave-for-your-business)

### Delegate idempotency responsibility

If avoiding a side-effect (e.g. a HTTP request, or an outgoing message) is very challenging, it may be easier to delegate the responsibility of dealing with a duplicate side-effect to the receiver. For example, adding a unique but stable identifier to an HTTP request header can allow the receiving API to apply deduplication checks instead.

### Use workflows

Stateful [workflows](workflows.md) can coordinate multiple resources to achieve (eventual) consistency.
