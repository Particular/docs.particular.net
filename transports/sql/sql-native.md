---
title: SQL Server Transport - Native
summary: Provides low level access to the SQL Server Transport with no NServiceBus reference required.
reviewed: 2018-04-23
component: SqlTransportNative
---

Provides low-level access to the [SQL Server Transport](/transports/sql/) with no NServiceBus or SQL Transport reference required.


## Usage scenarios

 * **Error or Audit queue handling**: Since NServiceBus requires the message type it is not suitable for use when processing an error or audit queue. SQL Native allows manipulation or consumption of messages without the need of message types.
 * **Corrupted or malformed messages**: This project operates as close to the SQL database as possible. As such message headers and body are treated as a raw string and byte array. This means corrupted or malformed messages can be read and manipulated so as to correct any problems.
 * **Deployment or decommission**: Running [installers](/nservicebus/operations/installers.md) requires starting a full endpoint. This is not always ideal during the execution of a deployment or decommission. SQL Native allows creating or deleting of queues with no running endpoint, and with significantly less code. This also makes it a better candidate for usage in deployment scripting languages like PowerShell.
 * **Bulk operations**: SQL Native supports sending and receiving of multiple messages within a single SQLConnection and SQLTransaction.
 * **Explicit connection and transaction management**: NServiceBus abstracts the SQLConnection and SQLTransaction creation and management. SQL Native allows any consuming code to manage the scope and settings of both the SQLConnection and SQLTransaction.
 * **Message pass through**: In message pass through scenarios (such as used in the [HTTP Message Pass Through Sample](/samples/web/owin-pass-through/) where no pipeline or mutators are required.


{{NOTE:

Some notes on the below snippets:

 * All methods that return a [Task](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.aspx) also accept an optional [CancellationToken](https://msdn.microsoft.com/en-us/library/system.threading.cancellationtoken.aspx).
 * While a string `SqlConnection` is used in all snippets for simplicity, an overload that takes a `SqlTransaction` also exists.
 * All snippets show variants that target and use byte arrays. There are also overloads that target and use a [Stream](https://msdn.microsoft.com/en-us/library/system.io.stream.aspx). Depending on the message size and performance requirements using the Stream-based overload may result in less memory pressure on the system and better overall throughput.

}}


## Queue management


### Main queue

Queue management for the [native delayed delivery](/transports/sql/native-delayed-delivery.md) functionality.

See also [SQL Server Transport - SQL statements](/transports/sql/sql-statements.md#installation).


#### Create

The queue can be created using the following:

snippet: CreateQueue


#### Delete

The queue can be deleted using the following:

snippet: DeleteQueue


### Delayed queue

Queue management for the [native delayed delivery](/transports/sql/native-delayed-delivery.md) functionality.

See also [SQL Server Transport - SQL statements](/transports/sql/sql-statements.md#create-delayed-queue-table).


#### Create

The queue can be created using the following:

snippet: CreateDelayedQueue


#### Delete

The queue can be deleted using the following:

snippet: DeleteDelayedQueue


## Sending a message

A variety of [message sending](/nservicebus/messaging/send-a-message.md) functionality is provided.
 

### Main queue

Sending from the main transport queue.


#### Single

Sending a single message.

snippet: Send


#### Batch

Sending a batch of message.

snippet: SendBatch


### Delayed queue

Sending from the [native delayed delivery](/transports/sql/native-delayed-delivery.md) queue.


#### Single

Sending a single message.

snippet: SendDelayed


#### Batch

Sending a batch of message.

snippet: SendDelayedBatch


## Reading messages

"Reading" a message returns the data from the database without deleting it.


#### Single

Reading a single message.

snippet: Read


#### Batch

Reading a batch of messages.

snippet: ReadBatch


### RowVersion tracking

For many scenarios, it is likely to be necessary to keep track of the last message `RowVersion` that was read. A lightweight implementation of the functionality is provided by `RowVersionTracker`. `RowVersionTracker` stored the current `RowVersion` in a table containing a single column and row.

snippet: RowVersionTracker

Note that is is only one possible implementation of storing the current `RowVersion`.


### Processing loop

For scenarios where continual processing (reading and executing some code with the result) of incoming messages is required, `MessageProcessingLoop` can be used. 

An example use case is monitoring an [error queue](/nservicebus/recoverability/configure-error-handling.md). Some action should be taken when a message appears in the error queue, but it should remain in that queue in case it needs to be retried. 

Note that in the below snippet, the above `RowVersionTracker` is used for tracking the current `RowVersion`.

snippet: ProcessingLoop


## Consuming messages

"Consuming" a message returns the data from the database and also deletes that message.


#### Single

Consume a single message.

snippet: Consume


#### Batch

Consuming a batch of messages.

snippet: ConsumeBatch


### Consuming loop

For scenarios where continual consumption (consuming and executing some code with the result) of incoming messages is required, `MessageConsumingLoop` can be used.

An example use case is monitoring an [audit queue](/nservicebus/operations/auditing.md). Some action should be taken when a message appears in the audit queue, and it should be purged from the queue so as to free up the storage space. 

snippet: ConsumeLoop


## Headers

There is a headers helpers class `NServiceBus.Transport.SqlServerNative.Headers`.

It contains several [header](/nservicebus/messaging/headers.md) related utilities:


### JSON headers


#### Serialization

Serialize a `Dictionary<string, string>` to a JSON string.

snippet: Serialize


#### Deserialization

Deserialize a JSON string to a `Dictionary<string, string>`.

snippet: Deserialize


### Copied header constants

Contains all the string constants copied from `NServiceBus.Headers`.

 
### Duplicated timestamp functionality

A copy of the [timestamp format methods](/nservicebus/messaging/headers.md#timestamp-format) `ToWireFormattedString` and `ToUtcDateTime`. 


## ConnectionHelpers

The APIs of this extension target either a SQLConnection and SQLTransaction. Given that in configuration those values are often expressed as a connection string, `ConnectionHelpers` supports converting that string to a SQLConnection or SQLTransaction. It provides two methods `OpenConnection` and `BeginTransaction` with the effective implementation of those methods being:

snippet: ConnectionHelpers
