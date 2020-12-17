---
title: Outbox
summary: Reliable messaging without distributed transactions
reviewed: 2019-11-25
component: Core
isLearningPath: true
versions: "[5,)"
redirects:
- nservicebus/no-dtc
related:
- samples/outbox
- persistence/nhibernate/outbox
- persistence/service-fabric/outbox
- persistence/ravendb/outbox
- persistence/sql/outbox
---

Most message queues, and some data stores, do not support distributed transactions. This may cause problems when message handlers modify business data. Business data and messages may become inconsistent in the form of **phantom records** or **ghost messages** (see below).

The NServiceBus **outbox** feature ensures consistency between business data and messages. It simulates an atomic transaction, distributed across both the data store used for business data and the message queue used for messaging.

Note: Messages dispatched to the transport as part of the Outbox dispatch stage will not be batched and each message is sent in isolation.

## The consistency problem

Consider a message handler that creates a `User` in the business database, and also publishes a `UserCreated` event. If a failure occurs during the execution of the message handler, two scenarios may occur, depending on the order of operations.

1. **Phantom record**: The handler creates the `User` in the database first, then publishes the `UserCreated` event. If a failure occurs between these two operations:
   * The `User` is created in the database, but the `UserCreated` event is not published.
   * The message handler does not complete, so the message is retried, and both operations are repeated. This results in a duplicate `User` in the database, known as a phantom record, which is never announced to the rest of the system.
2. **Ghost message**: The handler publishes the `UserCreated` event first, then creates the `User` in the database. If a failure occurs between these two operations:
   * The `UserCreated` event is published, but the `User` is not created in the database.
   * The rest of the system is notified about the creation of the `User`, but the `User` does not exist in the database. This causes further errors in other message handlers which expect the `User` to exist in the database.

To avoid these problems, developers of distributed systems have two options:

1. Ensure all message handlers are [idempotent](https://en.wikipedia.org/wiki/Idempotence). This means each message handler can handle the same message multiple times without adverse side effects. This is often very difficult to achieve.
2. Implement infrastructure which guarantees consistency between business data and messages. This avoids the need for all messages handlers to be idempotent.

The outbox feature is the infrastructure described in the second option.

## How it works

The outbox feature guarantees that each message is processed once and once only, using the database transaction used to store business data.

Returning to the earlier example of a message handler which creates a `User` and then publishes a `UserCreated` event, the following process occurs. Detailed descriptions are beneath the diagram.

```mermaid
graph TD
  receiveMessage(1. Receive incoming message)
  createTx(2. Begin transaction)
  dedupe{3. Deduplication}
  handler(4. Execute handler)
  storeOutbox(5. Store outgoing<br/>messages in outbox)
  commitTx(6. Commit transaction)
  areSent{7. Have outgoing<br/>messages been<br/>sent?}
  send(8. Send messages)
  updateOutbox(9. Set as sent)
  ack(10. Acknowledge incoming message)

  receiveMessage-->createTx
  createTx-->dedupe
  dedupe-- No record found -->handler
  handler-->storeOutbox
  storeOutbox-->commitTx
  commitTx-->areSent
  send-->updateOutbox
  updateOutbox-->ack

  dedupe-- Record found -->commitTx
  areSent-- No -->send
  areSent-- Yes -->ack
```

More detail on each stage of the process:

1. Receive the incoming message from the queue.
   * Do not acknowledge receipt of the message yet, so that if processing fails, the message will be delivered again.
2. Begin a transaction in the database.
3. Check outbox storage in the database to see if the incoming message has already been processed. This is called **deduplication**.
   * If the message has already been processed, skip to **step 6**.
   * If the message has not yet been processed, continue to **step 4**.
4. Execute the message handler for the incoming message
   * Any outgoing messages are not immediately sent.
5. Store any outgoing messages in outbox storage in the database.
6. Commit the transaction in the database.
   * This is the operation that ensures consistency between messaging and database operations.
7. Check if the outgoing messages have already been sent.
   * If the outgoing messages have already been sent, the incoming message is a duplicate, so skip to **step 10**.
   * If the outgoing messages have not yet been sent, continue to **Step 8**.
8. Send the outgoing messages to the queue.
   * If processing fails at this point, duplicate messages may be sent to the queue. Any duplicates will have the same `MessageId`, so they will be deduplicated by the outbox feature (in **step 3**) in the endpoint that receives them.
9. Update outbox storage to show that the outgoing messages have been sent.
10. Acknowledge (ACK) receipt of the incoming message so that it is removed from the queue and will not be delivered again.

## Important design considerations

* For best performance, outbox data should be stored in the same database as business data. For more information, see [_Transaction scope_](#important-design-considerations-transaction-scope) below.
* The outbox works only in an NServiceBus message handler.
* Because deduplication is done using `MessageId`, messages sent outside of an NServiceBus message handler (i.e. from a Web API) cannot be deduplicated unless they are sent with the same `MessageId`.
* The outbox is _expected to_ generate duplicate messages from time to time, especially if there is unreliable communication between the endpoint and the message broker.
* Endpoints using the outbox feature should not send messages to endpoints using DTC (see below) as the DTC-enabled endpoints will treat duplicates coming from Outbox-enabled endpoints as multiple messages.

### Transaction scope

The performance of the outbox feature depends on the scope of the transactions used to update business data and outbox data. Transactions may be scoped to a single database, multiple databases on a single server, or multiple databases on multiple servers.

* Transactions scoped to a single database are supported by all persisters and usually have the best performance, so it is usually best to store outbox data in the same database as business data.
* Transactions scoped to multiple databases on a single server, also known as _cross-database transactions_, are supported by some persisters, such as those which use SQL Server. These transactions may have reasonably good performance but they may introduce other concerns. For example, [cross-database transactions are not supported by all types of tables in SQL Server](https://docs.microsoft.com/en-us/sql/relational-databases/in-memory-oltp/cross-database-queries).
* Transactions scoped to multiple databases on multiple servers, also known as _distributed transactions_, are supported by persisters which use SQL Server, but they require the use of MSDTC and should generally be avoided for the same reasons as those listed in the [comparison with MSDTC](#comparison-with-msdtc) below.

## Comparison with MSDTC

The [Microsoft Distributed Transaction Coordinator (DTC)](https://en.wikipedia.org/wiki/Microsoft_Distributed_Transaction_Coordinator) is a Windows technology that enlists multiple local transactions (called resource managers) within a single distributed `TransactionScope`. All enlisted transactions either complete successfully as a set or are all rolled back.

MSDTC uses a very chatty protocol, due to the need for multiple resource managers to continually check on each other to make sure they are prepared to commit their results. An example of where this works well is a distributed transaction including consuming messages from an MSMQ message queue and storing business data in SQL Server. The communication with MSMQ is local and has very low latency, and there are only two resource managers to coordinate, with only one communication path between them. Early versions of NServiceBus primarily used MSMQ and SQL Server and systems built with those versions tended to use MSDTC.

The more resource managers involved and/or the higher the latency between them, the more poorly MSDTC performs. Cloud environments, in particular, have much higher latency and the message queue and the data store are probably not even located in the same server rack.

The rise of cloud infrastructure and [decline of MSMQ](https://particular.net/blog/msmq-is-dead) have contributed to the overall decline in use of MSDTC in the software development industry.

The outbox feature is designed to provide the same level of consistency between business data and messages provided by MSDTC, without the need to coordinate multiple resource managers.

## Enabling the outbox

partial: enable-outbox

Each NServiceBus persistence package may contain specific configuration options, such as a time to keep deduplication data and a deduplication data cleanup interval. For details, refer to the specific page for each persistence package in the [persistence section](#persistence) below.

## Converting from DTC to outbox

When converting a system from using the DTC to the outbox, care must be taken to ensure the system does not process duplicate messages incorrectly.

Because the outbox feature uses an "at least once" consistency guarantee at the transport level, endpoints that enable the outbox may occasionally send duplicate messages. These duplicate messages will be properly handled by deduplication in other Outbox-enabled endpoints, but will be processed more than once by endpoints which still use the DTC.

In order to gradually convert an entire system from the DTC to the outbox:

1. Enable the outbox on any endpoints that receive messages but do not send or publish any messages.
1. Enable the outbox on any endpoints that only send or publish messages to already-converted endpoints, where the outbox will be able to properly handle any duplicate messages.
1. Progress outward until all endpoints are converted.

WARNING: When verifying outbox functionality, it can sometimes be helpful to temporarily [stop the MS DTC Service](https://technet.microsoft.com/en-us/library/cc770732.aspx). This ensures that the outbox is working as expected, and no other resources are enlisting in distributed transactions.

## Message identity

The outbox only uses the incoming [message identifier](/nservicebus/messaging/message-identity.md) as a unique key for deduplicating messages. If the sender does not use outbox when sending messages, it is responsible for ensuring that the message identifier value is consistent when that message is sent multiple times.

## Outbox expiration duration

To determine if a message has been processed before, the identification data for each outbox record is retained. The duration that this data is retained for will vary depending on the persistence chosen for the outbox. The default duration, as well as the frequency of data removal, can be overridden for all outbox persisters.

After the outbox data retention period has lapsed, a retried message will be seen as the first of its kind and will be reprocessed. It is important to ensure that the retention period of outbox data is longer than the maximum time the message can be retried, including delayed retries and manual retries via ServicePulse.

Depending on the throughput of the system's endpoints, the outbox cleanup interval may need to be run more frequently. Increased frequency will allow each cleanup operation to purge the fewest records possible each time it runs. Purging fewer records will make the purge operation run faster which will ensure that it completes before the next purge operation is due to start.

## Storage requirements

The amount of storage space required for the outbox can be calculated as follows:

`Total outbox records = Message throughput per second * Deduplication period in seconds`

A single outbox record, after all transport operations have been dispatched, usually requires less than 50 bytes, most of which are taken for storing the original message ID as this is a string value.

NOTE: If the system is processing a high volume of messages, having a long deduplication time frame will increase the amount of storage space that is required by outbox.

## Persistence

The outbox feature requires persistence in order to perform deduplication and to store outgoing downstream messages.

For more information on the outbox persistence options available refer to the dedicated persistence pages:

* [NHibernate](/persistence/nhibernate/outbox.md)
* [RavenDB](/persistence/ravendb/outbox.md)
* [SQL](/persistence/sql/outbox.md)
* [ServiceFabric](/persistence/service-fabric/outbox.md)
* [MongoDB](/persistence/mongodb/#outbox-cleanup)
