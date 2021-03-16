---
title: Saga concurrency
summary: NServiceBus ensures consistency between saga state and messaging.
component: Core
redirects:
- nservicebus/nservicebus-sagas-and-concurrency
reviewed: 2019-09-24
related:
- persistence/nhibernate/saga-concurrency
- persistence/ravendb/saga-concurrency
- persistence/sql/saga-concurrency
---

An [endpoint](/nservicebus/concepts/glossary.md#endpoint) may be configured to allow [concurrent handling of messages](/nservicebus/operations/tuning.md#tuning-concurrency). An endpoint may also be [scaled out](/nservicebus/architecture/scaling.md#scaling-out-to-multiple-nodes) to multiple nodes. In these scenarios, multiple messages may be received simultaneously which correlate to a single saga instance. Handling those messages may cause saga state to be created, updated, or deleted, and may cause new messages to be sent.

NOTE: With respect to sagas, "handling" a message refers to the invocation of any saga method that processes a message, such as `IAmStartedByMessages<T>.Handle()`, `IHandleTimeouts<T>.Timeout()`, etc.

To ensure consistency between saga state and messages, NServiceBus ensures that receiving a message, changing saga state, and sending new messages, occurs as a single, atomic operation, and that two message handlers for the same saga instance do not perform this operation simultaneously.

WARNING: This level of consistency is provided automatically when using combinations of transports and persisters that can enlist in a transaction. When using other combinations of transports and persisters, the [outbox](/nservicebus/outbox/) must be used instead.

Saga state is created when a saga is started by a message. It may be updated when the saga handles another message. It is deleted when the saga handles another message which completes the saga. In any of these cases, multiple messages may be received simultaneously.

## Starting a saga

When messages that start the same saga instance are received simultaneously, NServiceBus ensures that only one message starts the saga.

Using optimistic concurrency, only one message handler is allowed to succeed. The saga state is created and sent messages are dispatched. The other handlers fail, roll back, and their messages enter [recoverability](/nservicebus/recoverability/). When those messages are retried, the existing saga state is found. Handling the messages may involve changing the state or completing the saga. See below for how those scenarios are handled.

partial: unique

## Changes to saga state

When using some persisters, messages received simultaneously that correlate to the same existing saga instance are not handled simultaneously. The persister uses pessimistic locking to ensure that message handlers are invoked one after another.

When using other persisters, and simultaneously handling messages which cause changes to the state of the same existing saga instance, NServiceBus ensures that only one message changes the state. Using optimistic concurrency, only one message handler is allowed to succeed. The saga state is updated, and sent messages are dispatched. The other handlers fail, roll back, and their messages enter [recoverability](/nservicebus/recoverability/). When those messages are retried, the process repeats. One or more of those messages may attempt to complete the saga. See below for how that scenario is handled.

See the [documentation for each persister](/persistence/) for details of which method is used.

## Completing a saga

When using some persisters, messages received simultaneously that correlate to the same existing saga instance are not handled simultaneously. The persister use pessimistic locking to ensure that message handlers are invoked one after another.

When using other persisters, and simultaneously receiving messages which cause changes to the state of the same existing saga instance, and at least one of those messages causes the saga to be completed, NServiceBus ensures that only one message either changes the state or completes the saga. Using optimistic concurrency, only one message handler is allowed to succeed. If the message changes the saga, the saga stated is updated, and sent messages are dispatched. If the message completes the saga, the saga state is deleted, and sent messages are dispatched. The other handlers fail, roll back, and their messages enter [recoverability](/nservicebus/recoverability/).

In both cases, if a message completes the saga, when subsequent messages are received, the saga state is not found and those messages are [discarded](/nservicebus/sagas/saga-not-found.md).

See the [documentation for each persister](/persistence/) for details of which method is used.

## High-load scenarios

Under high load, simultaneous attempts to create, update, or delete the state of a saga instance may lead to decreased performance due to the data contention scenarios described above. In effect, the use of pessimistic locking or optimistic currency control may lead to [block contention](https://en.wikipedia.org/wiki/Block_contention).

In these scenarios, the symptoms of high data contention differ depending on the persister being used. Potential symptoms include:

- High number of retries due to optimistic concurrency control conflicts.
- Exhaustion of retries, leading to messages being moved to the error queue.
- Transaction timeouts due to a large number of simultaneous attempts to acquire a lock on the same data.
- High [processing time](/monitoring/metrics/definitions.md#metrics-captured-processing-time) or [critical time](/monitoring/metrics/definitions.md#metrics-captured-critical-time), compared with the time taken to execute a method handler.
- Unexpected increases in [processing time](/monitoring/metrics/definitions.md#metrics-captured-processing-time) or [critical time](/monitoring/metrics/definitions.md#metrics-captured-critical-time), even though the system isn't fully utilizing resources like CPU, network, RAM, and storage.

These symptoms can be mitigated by the choice of persister, tuning endpoint configuration, how handlers and sagas are deployed, and saga design.

### Choosing a persister

As described above, some [persisters](/persistence/) use pessimistic locking and others use optimistic concurrency control.

#### Pessimistic locking

When a persister uses pessimistic locking it will start a transaction which attempts to obtain an update lock on the saga instance data before the message handler is invoked. When handling messages simultaneously related to a given saga instance, one transaction will obtain the lock, and the others will wait until either the current lock is released or the transaction timeout period is reached. Messages in the queue that are not related to that saga instance may be delayed if the concurrency limit of the endpoints has been reached and all transactions are waiting to obtain a lock.

NOTE: The transaction timeout is usually set between 1 and 10 minutes. Consult the DBA or operations for system- or environment-wide transactions settings.

The following saga persisters use pessimistic locking:

- [NHibernate](/persistence/nhibernate/)
- [MongoDB](/persistence/mongodb/) (since version 2.2.0)
- [Service Fabric](/persistence/service-fabric/) (since version 2.2.0)
- [SQL](/persistence/sql/) (since version 4.1.1)

#### Optimistic concurrency control

When a persister uses optimistic concurrency control (OCC), unlike pessimistic locking, a lock is not obtained before handlers are invoked. After handlers are invoked, attempts to update or delete saga instance data compete with each other. The first writer wins and all others fail. Failed messages enter [recoverability](/nservicebus/recoverability/) and are retried. If all retries fail, they will be moved to the error queue.

Due to recoverability, OCC conflicts in high data contention scenarios may result in multiple attempts to ingest a message from a queue, and multiple attempts to update saga instance data. However, messages in the queue that are not related to that saga instance may be processed faster than with pessimistic locking because they can be ingested while the messages in recoverability are scheduled for delayed retry.

The following saga persisters use OCC:

- [Azure Storage](/persistence/azure-table/)
- [Non-Durable](/persistence/non-durable/)
- [MongoDB](/persistence/mongodb/) (prior to 2.2.0)
- [RavenDB](/persistence/ravendb/)
- [Service Fabric](/persistence/service-fabric/) (prior to 2.2.0)
- [SQL](/persistence/sql/) (prior to 4.1.1)

### Use custom recoverability for OCC conflicts

Because OCC conflicts should eventually be resolved through retries, a [full custom retry policy](/nservicebus/recoverability/custom-recoverability-policy.md#implement-a-custom-policy-full-customization) can be written to prevent moving a message to the error queue too early.

NOTE: The saga concurrency documentation for each persister contains details of the exception type and/or exception message which indicate a concurrency conflict.

### Host the saga in a dedicated endpoint

To avoid impacting the processing of messages which are not related to the saga, one strategy is to host the saga in a dedicated endpoint.

### Decrease the endpoint concurrency limit

The number of OCC conflicts can be reduced by [decreasing the concurrency limit](/nservicebus/operations/tuning.md#tuning-concurrency). Message handling can even be made sequential by setting the concurrency limit to 1.

NOTE: Sequential messaging handling when using OCC is only possible for a single endpoint instance. When an endpoint is [scaled out](/nservicebus/architecture/scaling.md#scaling-out-to-multiple-nodes), message handling cannot be made sequential when all instances are running. An alternative is to have only one instance running at a time, in an active/passive configuration.

The concurrency limit applies to an entire endpoint. If the endpoint hosts many handlers and sagas, they will all be subject to the concurrency limit. When decreasing the concurrency limit to reduce data contention for a given saga, consider hosting the saga in a dedicated endpoint.

### Avoid I/O or CPU bound operations in a saga handler

The longer it takes for a saga handler to execute, the more likely it is to suffer from data contention. When using pessimistic locking, the handler will cause the lock to be held for longer. When using OCC, conflicts are more likely. Ensure saga handlers *only* read and write saga state and send messages. Avoid accessing databases and other resources, and long running CPU bound work. These operations should be performed in [separate message handlers](/nservicebus/sagas/#accessing-databases-and-other-resources-from-a-saga) which are not part of the saga.

### Do not enlist in the transport transaction

Some transports support distributed transactions. If the persister also supports transactions, by default, the persister will enlist in the transport transaction. This increases the duration of the persister transaction, making it more likely to suffer from data contention. This can be prevented by configuring the transport to not use the ["transaction scope" transaction mode](/transports/transactions.md). Messages will now be delivered "at least once", rather than "exactly once". This means that all handlers hosted by the endpoint, whether in a saga or not, must be idempotent, or the [outbox](/nservicebus/outbox/) must be used. If this is not possible, it may be necessary to split the handlers and sagas across multiple endpoints, each with their own transaction mode configuration.

### Partition message processing

The default message processing model is a single queue with one or more [competing consumers](https://www.enterpriseintegrationpatterns.com/patterns/messaging/CompetingConsumers.html) that concurrently process messages. Alternatively, a queue may be partitioned into several queues. Messages are routed to partitions using a partitioning strategy which ideally gives an even distribution of messages across the partitions. For each partitioned queue, messages are consumed by a single endpoint instance configured with a maximum concurrency of one. For example, one queue with a single endpoint instance with a concurrency limit of eight could be replaced with eight queues and eight endpoint instances, each with a concurrency limit of one. By partitioning the processing of messages appropriately, the messages relating to a single saga instance will be processed sequentially, eliminating data contention due to concurrency conflicts. This occurs in parallel with messages relating to other saga instances, which are processed by other endpoint instances.

Note that it isn't necessary to [shard](https://en.wikipedia.org/wiki/Shard_(database_architecture)) or [partition](https://en.wikipedia.org/wiki/Partition_(database)) saga data, although those techniques could be applied to improve performance even further.

A partitioned endpoint instance must be configured to be uniquely addressable using `MakeInstanceUniquelyAddressable`. Messages must be sent to the appropriate endpoint instances using [routing extensibility](/nservicebus/messaging/routing-extensibility.md). This is demonstrated in the [Service Fabric Partition-Aware Routing sample](/samples/azure/azure-service-fabric-routing/).

### Decrease network latency or bandwidth

Saga data storage, and endpoints hosting sagas, often have separate network locations. Networks are typically the slowest component in computer systems. Network latency may be reduced by ensuring storage and endpoints are closely located, potentially with a dedicated network link. Network latency can even be removed completely, by deploying storage and endpoints on the same host.

### Redesign the sagas

#### Minimize saga data

Saga state is retrieved from and submitted to storage for every message received. Saga state should only contain the minimum data required for the saga to make its decisions. Other data, especially large strings or [BLOBs](https://en.wikipedia.org/wiki/Binary_large_object) should not be contained in saga data. Instead this data could be forwarded to another handler for storage or processing, and the saga state could store only a reference to it.

#### Prevent race conditions when starting sagas

When there are a high number of OCC conflicts starting sagas, and the persister supports pessimistic locking, consider finding a different way of starting the saga. For example, the system may be able to send a single message earlier that can start the saga. The processing of later, concurrent messages can then take advantage of pessimistic locking to avoid conflicts.

#### Apply chunking by creating "sub-saga" instances

Sagas that use a [scatter-gather](https://www.enterpriseintegrationpatterns.com/patterns/messaging/BroadcastAggregate.html) pattern typically initiate a large number of requests and aggregate the responses. Those responses could be received simultaneously and cause data contention. Instead of having a single saga sending many requests and aggregating many responses, a tree-like structure could be formed, with sub-sagas that subdivide the work.

For example, instead of a saga creating 1,000 requests, it could split the requests into two groups of 500 and send them to two sub-sagas. Each of those sub-sagas could split the requests into two groups of 250 and send them to two further sub-sagas. This process could continue, until a given sub-saga receives only the details of two requests. That saga then sends the requests, aggregates the responses, and responds to its parent saga with the aggregated data. The end result is that the originating saga receives two responses, each containing the data from 500 requests. Because each saga only sends two requests and aggregates two responses, data contention will be largely eliminated.

In the above example, the requests are split into two groups each time. Depending on the dynamics of the system, splitting them into more groups is also an option.

A simpler approach is to split the request just once and have a single level of sub-sagas sending the requests and aggregating the responses. This will not reduce data contention to the same degree, since the originating saga will be aggregating more multiple responses.

#### Sequential scatter/gather

When using a [scatter-gather](https://www.enterpriseintegrationpatterns.com/patterns/messaging/BroadcastAggregate.html) pattern or similar, simultaneously sending a large number of requests may result in simultaneous processing of a large number of responses, which may lead to data contention. Instead, consider sending requests sequentially. In each iteration, send the next request only after the response from the previous request was processed. This approach removes data contention when processing responses but may increase the overall duration. Also, the size of the saga state may increase, because it may need to contain some of the data from the message which initiated the scatter-gather so that that data can be included in each request.

#### Create an append-only saga data model

Currently, this is only possible with the NHibernate persister and a custom mapping. It requires advanced knowledge of NHibernate. Data added to a collection must be mapped to another entity so that the the master/parent row does not need to be updated. This way there is no data contention on the table row representing the saga instance data root.

#### Further reading

For more methods of redesigning sagas to reduce data contention, see _[Reducing NServiceBus Saga load](https://lostechies.com/jimmybogard/2014/02/27/reducing-nservicebus-saga-load/)_ by Jimmy Bogard.

### Use a custom saga implementation

Sometimes, the methods of reducing data contention described above may not be enough. It may be necessary to write a custom saga implementation, where message processing is specifically optimized for the given saga model. For example, append-only models can only be implemented with a custom NHibernate mapping. With a custom implementation, an append-only model could be implemented with other storage types.

A custom saga implementation could simply be a class with multiple message handlers, each of which manages the storage of state in a way that is optimized for the specific use case.
