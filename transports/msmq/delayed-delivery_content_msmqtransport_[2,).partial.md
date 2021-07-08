Because MSMQ lacks a mechanism for sending delayed messages, the MSMQ transport uses an external store for delayed messages. Messages that are meant to be delivered later (e.g. [saga timeouts](/nservicebus/sagas/timeouts.md) or [delayed retries](/nservicebus/recoverability/configure-delayed-retries.md)) are persisted in the delayed message store until these are due. At that point the due messages are fetched from the store and dispatched to their ultimate destinations.

To configure the MSMQ transport to enable delayed message delivery use the following API:

snippet: delayed-delivery

The code above sets up a SQL Server delayed message store. The SQL Server store is the only store that ships with the MSMQ transport. By default the SQL Server store uses a table named after then endpoint with `.delayed` suffix but that table name can be customized. 


## How to provide a custom delayed message store

A custom delayed message store can be provided by implementing the `IDelayedMessageStore` interface. 

This interface  The delayed delivery handling mechanism has two parts. First, a delayed message is stored using the `Store` method. 

The second part is polling the store for due delayed messages. This is accomplished by periodically calling `FetchNextDueTimeout`. As long as this method returns a message, the mechanism continues to pull more messages. If there are no more due messages, `Next` is invoked to determine how much time is left until the next due timeout. The polling mechanism pauses until that time. If another delayed message is persisted in the meantime, the `Store` method wakes up the polling thread.

When a due delayed message is returned by `FetchNextDueTimeout`, the message is forwarded to the destination and then removed from the store using the `Remove` method. In case of an unexpected exception during forwarding the failure is registered using `IncrementFailureCount`. If the configured number of retries is exhausted the message is forwarded to the configured `error` queue.

### Consistency

In the `TransactionScope` [transaction mode](/transports/transactions.md) the delayed message store is expected to enlist in the `TransactionScope` to ensure exact-once semantics. The `FetchNextDueTimeout` and `Remove` storage operations and the dispatch to the destination queue transport operation are executed in a single distributed transaction. The built-in SQL Server store supports this mode of operation.```

In lower transaction modes the dispatch behavior is **At-least-once**. The `FetchNextDueTimeout` and `Remove` storage operations are executed in the same storage `TransactionScope` but the dispatching is executed in a separate (inner) transport scope. In case of failure in `Remove`, the message will be sent to the destination multiple times. The destination endpoint has to handle the duplicates, either via the [Outbox](/nservicebus/outbox/) or custom deduplication mechanism.

The built-in SQL Server store applies a pessimistic lock on the delayed message row in the `FetchNextDueTimeout` operation to prevent multiple instances of the same endpoint from attempting to deliver the same due delayed message. Custom implementation of the store are also expected to use some form of a lock mechanism (e.g. Blob Storage lease locks).
