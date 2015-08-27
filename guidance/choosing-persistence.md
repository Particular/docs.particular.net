---
title: Choosing Persistence
summary: Guidance for choosing a persistence library for NServiceBus data.
---

Several NServiceBus features require storing data in persistent storage. We provide multiple different libraries that can provide this persistence, and so you must choose one to use within your NServiceBus project. While NServiceBus makes this choice largely transparent from an API level, the choice of technology does have ramifications to your project that you should consider before making a decision, and will have lasting ramifications on your project in the long term.

No persistence is inherently better or worse than any other. They all have their strengths and weaknesses. You should consider these differences carefully and choose the persistence that makes the most sense for your team.

## What does NServiceBus store?

Before we discuss differences between persistence choices, we should first consider what features in NServiceBus require persistence.

* **Subscriptions** - Some transports that do not natively support publish/subscribe accomplish it instead by storing a list of subscribers interested in each event type.
* **Sagas** - Sagas represent state that is shared between multiple message handlers. An object graph must be loaded before a message handler is executed, and then changes to that graph must be persisted after the message handler completes.
* **Timeouts** - In order to defer messages for long periods of time, the mesage content and due date are persisted to be dispatched to the message transport later on.
* **Outbox** - In order to support exactly-once processing for message transports that do not support the Distributed Transaction Coordinator (DTC), the Outbox stores the results of executing a message handler so that duplicate messages (containing the same message ID) are not processed more than once.
* **Gateway** - Because HTTP communication is unreliable, the Gateway component employs automatic retries, and must deduplicate on the receiving end to ensure that duplicate messages are not dispatched to the message transport.

Of all these features, Saga storage creates the largest persistence challenge. Subscriptions, timeouts, outbox, and gateway storage are all fairly simple data structures, but the object graph for a Saga can contain child objects, or even collections of objects - nearly anything that can be represented in an in-memory .NET object.

## In-Memory Persistence

In-memory persistence is built into the NServiceBus Core assembly, and stores all its data in volatile RAM. As a result, everything is forgotten when the process restarts.

As a result, in-memory persistence is (for the most part) recommended only during development. During development, it can be advantageous to have persistence that forgets, as it is easier to iterate quickly without previous "mistakes" getting in the way.

The forgetfulness of in-memory persistence can be unsettling, however, in certain circumstances. Attempting to debug a saga can be difficult when the debugging demands many small code tweaks and restarts to the process, as this can result in messages arriving from the transport that refer to Saga data that cannot be found, because it has been forgotten.

Additionally, volatile subscription storage can cause confusion, because the subscription storage will be empty until the subscriber endpoint sends a new subscription request message. This makes it difficult to debug a publishing endpoint in isolation, as its subscription storage will be empty every time it starts up.

**Trying to think of a reason you might use in-memory for production for raw speed, but I can't come up with a single concrete use case.**

## NHibernate

NHibernate persistence, provided through the [NServiceBus.NHibernate NuGet package](https://www.nuget.org/packages/NServiceBus.NHibernate), uses the NHibernate library to store data in an ADO.NET data store. Particular supports Microsoft SQL Server and Oracle, and runs a suite of acceptance tests targeted to both databases as a part of each release.

### How NHibernate persistence works

Since SQL Server and Oracle are both relational databases, NHibernate persistence aims to store all of its data in terms of tables, columns, and rows.

For subscriptions, timeouts, outbox, and gateway, the data structures are quite simple, so NHibernate is able to represent the data for these features with one table each, whose schemas are stable and unchanging.

Saga storage becomes more complex, as developers can create any data structure they like, and NHibernate has to try to represent that in relational form. This may require the use of multiple tables per saga, and also places some limitations on how those classes must be designed.

#### Saga virtual properties

One peculiarity of NHibernate is that it loads data using dynamic proxy classes, which require all of your saga data properties to be marked as `virtual`.

Why is this? Consider the following saga data class:

```
public class OrderSagaData : ContainSagaData
{
    public virtual int OrderId { get; set; }
    public virtual List<OrderItem> Items { get; set; }
}
```

At runtime, NHibernate will dynamically create and compile a class that inherits from the saga data class, similar in concept to the following code:

```
public class  OrderSagaDataProxy : OrderSagaData
{
    public override OrderId
    {
        get { return base.OrderId; }
        set { base.OrderId = value; }
    }
    
    public override List<OrderItem> Items
    {
        get { return base.Items; }
        set [ base.Items = value; ]
    }
}
```

Except, the dynamic proxy will be a little more complex because it will include NHibernate magic in each of the intercepted properties, which give it the ability to lazy-load data from the OrderItem table, for example, in order to execute queries against the database only when necessary.

If properties were not marked as `virtual`, NHibernate would be unable to override them in its generated proxy class. So instead, NHibernate persistence will throw an exception at startup time to warn you that non-virtual properties are not allowed.

NOTE: Wherever possible, we endeavor to present all Saga samples and documentation using `virtual` properties, in order to accommodate the NHibernate persister.

#### Saga collections

Because NHibernate must store its data in a relational data store, certain class patterns that don't map well cannot be supported.

```
public class BatchSagaData : ContainSagaData
{
    // Will not work with NHibernate
    public virtual List<int> ProcessedIds { get; set; }
}
```

The NHibernate persister is not able to map `BatchSagaData` to a relational model, as it is not clear what to do with the `List<int>`.

Instead, this concept must be expressed in a different way:

```
public class BatchSagaData : ContainSagaData
{
    public virtual List<BatchId> ProcessedIds { get; set; }
}

public class BatchId
{
    public virtual Guid Id { get; set; }
    public virtual int ProcessedId { get; set; }
}
```

Now, it is clear to the NHibernate persister that it should create two tables, `BatchSagaData` and `BatchId`, and how to relate them together.

#### Saga contention

* Explain saga contention, especially scatter/gather
* Explain how sagas take locks
* Explain possibility for append-only sagas. Link to sample?

### Operational considerations

* Org may have plenty of in-house experience for SQL Server or Oracle. (Backups, tuning, troubleshooting, etc.)
* Stress single point of failure, high availability
* Persistence of choice if using SQL Server transport.

## RavenDB 

Stub

### How RavenDB persistence works

#### Pointer documents

* Describe doc store ACID vs. index store updated async
* Describe pointer docs as unique index
* Explain single unique property limit.

#### Saga contention

* Explain saga contention on documents, especially scatter/gather
* Explain optimistic concurrency / concurrency exception

### Operational considerations

* Org may not have operational knowledge of Raven, which is a risk.
* Explain more distributed architecture with Ravens on each node.
* Stress backup strategy, on similar schedule as business databases.
