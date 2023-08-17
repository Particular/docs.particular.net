---
title: Consistency
summary: Achieving consistency in distributed, message-driven systems
reviewed: 2023-07-18
---

## Transactions

Transaction processing is designed to keep the system in a consistent state at any point in time. This is achieved by ensuring that interdependent operations are completed as a single [unit of work](https://en.wikipedia.org/wiki/Unit_of_work). If any single operation fails, then the entire transaction fails and the system is reverted to the state it was in before the transaction started.

### Transaction limitations

There are several challenges and limitations when relying on transactions:

#### Transaction scalability

Scalable systems need to be capable to handle high levels of concurrency. Transactions need concurrency control mechanisms to ensure proper isolation between transactions trying to access the same resource. These concurrency control mechanisms can severely impact a resource's performance and introduce bottlenecks that complicate scaling.

When using horizontal scaling, additional mechanisms are required to ensure the desired level of consistency across all scale units. This typically involves complicated distributed consensus-algorithms. Depending on the consistency guarantees required, many or even all nodes of a scaled component need to participate in the transaction, negatively impacting latency and availability.

#### Transaction costs

Besides the performance impact of transactions, transactional operations may even be charged extra on certain managed cloud services. Broad usage of transactional consistency may therefore incur significant costs. Carefully read cloud vendor's pricing models to understand the financial impact of transactional APIs.

#### Transaction timeout

Transactions will impact concurrent operations on a locked resource. This may block other actors and consume additional resources. Therefore transactions should complete within a short time window to avoid issues. Cloud services typically restrict transaction lifetimes further than traditional on-premises technologies.

#### Transaction scope

Some cloud services like Azure Table Storage, Azure Cosmos DB Table API, and Azure Cosmos DB SQL API offer transactions, but they are usually scoped to a single partition key. When transactions are scoped to a single partition key, all storage operations that need to be atomic have to share the same partition key on all read, update and delete operations to achieve atomicity.


### Cross-Entity transactions

Databases and queueing technologies might support atomic transactions to a single entity (e.g. a message queue, or a database document) or multiple entities (sometimes limited by the partition as well). Without cross-entity transactions, queueing systems cannot atomically acknowledge (consume) a message together with sending outgoing messages, which might lead to duplicate messages when recoverability retries a failed message.

[**Compare message queue transaction capabilities →**](/transports/transactions.md)

### Distributed transactions

Distributed transactions (transactions that span multiple services, e.g. a database and a message queue) are required to ensure strong consistency of all operations within the shared transaction. On Windows, the [Distributed Transaction Coordinator (DTC)](https://en.wikipedia.org/wiki/Microsoft_Distributed_Transaction_Coordinator) is able coordinate transactions across multiple compatible participants using a [two-phase commit protocol](https://en.wikipedia.org/wiki/Two-phase_commit_protocol). Each participating resource of a distributed transaction must explicitly support the two-phase commit protocol. 

{{WARN:
While traditional on-premises focused services like MSMQ or MSSQL server support the DTC, managed cloud services do not support DTC transactions, enforcing different strategies to achieve consistency across transactional resources. Strategies to replace distributed transactions include:
* Using the [Outbox pattern](#transactions-outbox-pattern) to leverage database capabilities to achieve atomic consistency for data and message operations
* Using [stateful workflows](workflows.md) to supervise the successful completion of all involved resources, or manage the rollback process to achieve eventual consistency.
}}


### Outbox Pattern

The [transactional Outbox Pattern](https://microservices.io/patterns/data/transactional-outbox.html) is a pattern to achieve atomically consitent database and message queueing operations. Due to the lack of distributed transactions in many modern application environments, the outbox pattern is a commonly used pattern to solve strong consistency requirements.

The outbox pattern is implemented by storing outgoing messages into the same database as the business operations. This allows the use of database native transaction capabilities to achieve atomic consistency. In a second step, the persisted messages are dispatched to tbe message queue.

The nature of the outbox pattern might cause outgoing messages to be dispatched multiple times. This should be taken into consideration by leveraging [idempotency mechanisms](#idempotency) on the receiver.

Note: Implementing the outbox pattern is very risky and error-prone. Small mistakes can lead to unintended behavior and message or data loss. Consider the the [NServiceBus Outbox feature](/nservicebus/outbox/) which implements the Outbox pattern, including built-in message deduplication when receiving messages. The NServiceBus Outbox is used in production by hundreds of customers, thoroughly tested and well documented.


## Idempotency

[Idempotence](https://en.wikipedia.org/wiki/Idempotence) ensures that processing a message multiple times has the same effect as only processing it once. This means that processing a duplicate message shouldn’t cause any unintended side effects.

Idempotency is an approach to mitigate the lack of distributed transactions. Without transactions, [recoverability mechanisms](/architecture/recoverability.md) that prevent message losses in failure scenarios may produce duplicate messages. With idempotency, processing the same message multiple times produces the same result as processing it once. There are multiple ways to achieve idempotency, implemented at different levels:

### Natural idempotency

Many operations can be designed in a naturally idempotent way. For example, `TurnOnTheLights` is an idempotent operation because it will have the same effect no matter the previous state and how many times the operation is executed. `FlipTheLightSwitch` however is not naturally idempotent because the results will vary depending on the initial state and the number of times it was executed. Changes to naturally idempotent code should be carefully reviewed to ensure that idempotency is retained.

Using natural idempotency is recommended whenever possible.

### Message deduplication

Message deduplication is the easiest way to detect if a message has been processed already. Every message that has been processed so far is stored. When a new message comes in, it is compared to the set of already processed messages (usually by comparing their unique identifiers). If the message is identical to one of the stored messages, it is a duplicate and the new message won't be processed.

One advantage of this approach is its simplicity; however it has downsides. As every message needs to be stored and searched for, it can reduce message throughput because of the additional lookups. Deduplication storages cannot store information infinitely, limiting the deduplication guarantees of this approach to the provided storage capacity. Identifiers for deduplication typically operate on technical IDs (e.g. message ID), rendering this approach useless against duplicate message content.

Note: Implementing message deduplication is risky and error-prone. Small mistakes can lead to unintended behavior or message loss. The the [NServiceBus Outbox feature](/nservicebus/outbox/) implements message deduplication mechanism out of the box. The NServiceBus Outbox is used in production by hundreds of customers, thoroughly tested and well documented.

### Side effect checks

Message deduplication assumes a generic deduplication mechanism that works for all incoming messages, regardless of the specific message content. When this is not possible, duplicate message processing can be avoided by checking for expected side effects. These side effects are only known for a specific message type and it's attached business logic. For example, when `TheFireIsHot` flag is set to true, then there is no need to `TurnOnTheFire`. This approach is less reusable and requires in-depth understanding of the business logic to deal with duplicate messages.

Additional metadata might be attached to existing side effects to achieve deduplication. For example, a message timestamp (provided by the author of the message) is attached to database record that is stored due to processing the message.

## Best Practices

### Break business logic into smaller steps

Business logic triggered by incoming messages may coordinate actions and state across multiple resources, like databases, web APIs, and other domain services. As every interaction with any service may fail for various expected and unexpected reasons, the whole process has to be repeated again. By breaking workflows into smaller steps that include a message queue + one additional resource, the impact of a failure inside the workflow can be greatly reduced and idempotency patterns will be easier to apply.

[**Blog: Autosave for business processes →**](https://particular.net/blog/autosave-for-your-business)

### Delegate idempotency responsibility

If avoiding a side-effect (e.g. a HTTP request, or an outgoing message) is very challenging, it might be easier to delegate the responsibility of dealing with a duplicate side-effect to the receiver. For example, adding an unique but stable identifier to an HTTP request header can allow the receiving API to apply deduplication checks instead.

### Use workflows

Stateful workflows can coordinate multiple resources to achieve (eventual) consistency across all of them. See the [workflows documentation](workflows.md) for more details.