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

### Operational considerations

NHibernate is a strong choice for saga persistence because many organizations will already have plenty of in-house experience with SQL Server or Oracle. This includes troubleshooting, backups, index maintenance, etc., sometimes by a dedicated staff of database administrators.

Of course, NHibernate will require schema to be applied to the database server. While NServiceBus can do this for you as a shortcut in development, it's wise to generate these in a test or QA environment and then manage the database schema migration for a production environment manually.

Additionally, saga data tables, being generated from user code, require schema migrations whenever developers change the shape of the saga data.

When deploying using NHibernate persistence to SQL Server or Oracle, keep in mind that the database becomes a single point of failure, and so the database should be deployed in a highly-available fashion.

Lastly, NHibernate persistence is the "persistence of choice" when using the SQL Transport as the endpoint's queues, persistence, and business data can all be stored in the same database utilizing a single transaction.

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

The NHibernate saga persister takes locks on the saga data table(s) during message processing in order to guarantee that another message processed in parallel does not modify the saga in the meantime. Because of these locks, it's easy for a saga to become a contention point when messages are processed in parallel.

In order to minimize contention, it is important to design sagas such that they are message sending state machines, i.e. they receive a message, make some decisions, and then send/publish other messages or request timeouts. A saga should not query database resources, call web services, or do anything else that will hold the database lock on the saga any longer than absolutely necessary. Instead, the saga should send messages to normal endpoints outside the scope of the saga, which can reply back to the saga with the result if necessary.

A prime example of a contentious saga is one employing the scatter/gather pattern. With the NHibernate persister it is possible to design an append-only saga that takes advantage of the multiple tables created by the NHibernate saga persister to allow multiple messages to process in parallel with minimal locking so that they do not trip over each other.

## RavenDB 

RavenDB persistence, provided through the NServiceBus.NHibernate NuGet package, stores NServiceBus data to a RavenDB database.

### How RavenDB persistence works

RavenDB stores all information as JSON-serialized documents, addressable by a document ID represented as a string. RavenDB has no schema, making a very easy "F5 experience" as it just stores what is needed when asked without having to deal with things like schema migrations, which also makes iterating on saga data structures very fluid as well.

Because Raven can support any schema in documents and does not have to deal with rows and columns, many things are more straightforward, especially saga persistence, which can be represented as a single document. However there are other considerations that must be taken into account.

### Operational considerations

The primary advantage of RavenDB persistence is the lack of friction during development time, free from the need to update database schema.

However, while many organizations already have deep knowledge of SQL Server in house, they may not have operational familiarity with RavenDB. Before choosing RavenDB, this needs to be identified as a risk.

An advantage of RavenDB is that it need not be centralized, thus opening up the possibility for more distributed architecture with no single point of failure. By default, an NServiceBus endpoint will connect to the RavenDB server at `http://localhost:8080` using a database name matching the endpoint name. With this configuration, it's possible for endpoints on a given node to process messages and access their persistence locally, even if other parts of the system are down.

RavenDB persistence can also be used in the cloud via a hosted offering from [RavenHQ](http://ravenhq.com), which can be an attractive alternative to Azure Storage Persistence.

It's important to remember that data in RavenDB, especially saga data, is important and valuable business data, so it's important to create a backup strategy for any RavenDB databases on a similar schedule as any other business data.

#### Pointer documents

Raven includes the transactional document store, where documents can be loaded and stored by the document ID under a full ACID transaction. It also contains a query store based on Lucene search technology to enable querying for data inside documents, but the query store is *not* transactional, and is updated asynchronously a short time after documents are committed.

This means that in order to have a transactional "unique index" on data within a document, RavenDB needs to employ **pointer documents**, which are especially needed to support lookups of a saga's correlation properties.

When a saga is persisted, the document ID for the saga document itself is based on the SagaId of the saga itself. This is used to look up the saga when the SagaId is known, such as when processing a saga timeout message.

To find a saga by OrderId, for example, an additional document (the pointer) is stored to RavenDB and given a document ID based on the OrderId. The pointer document includes the id of the saga document it points to. When loading the saga, Raven will ask for the pointer document but is able to return both documents in the same request.

It's important to realize that the RavenDB persister will create these "extra" documents, and also helpful to understand what they do should anything go wrong.

#### Saga contention

With the RavenDB persister, any modification of saga data requires loading the document from the database, making changes, and then saving the entire document back to the database, including serialization and deserialization of the document to the JSON stored in the database.

RavenDB uses optimistic concurrency by storing version numbers for each document. If two messages try to update a saga's data concurrently, one will succeed, while the other will fail with a `ConcurrencyException` and be forced to retry.

Because of the document structure, it's impossible to escape contention if many messages are being processed simultaneously. Scatter/gather scenarios are especially problematic when using the RavenDB persister.

One strategy that can be useful when dealing with contentious sagas using the RavenDB persister is to batch items and delegate to child sagas. A scatter/gather scenario with 100 items, for example, could subdivide into 10 batches of 10 to be handled by child sagas. The individual saga document is the point of contention, so spreading out the work so no more than 10 messages can ever be in contention can reduce the number of concurrency exceptions thrown and thus increase throughput.

# Outside this article

* Scatter gather guidance
* Append-only saga sample
