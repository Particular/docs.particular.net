Because MSMQ lacks a mechanism for sending delayed messages, the MSMQ transport uses an external store for delayed messages. Messages that are to be delivered later (e.g. [saga timeouts](/nservicebus/sagas/timeouts.md) or [delayed retries](/nservicebus/recoverability/configure-delayed-retries.md)) are persisted in the delayed message store until they are due. When a message is due, it is retreived from the store and dispatched to its destination.

The MSMQ transport requires explicit configuration to enable delayed message delivery. For example:

snippet: delayed-delivery

The SQL Server delayed message store (`SqlServerDelayedMessageStore`) is the only delayed message store that ships with the MSMQ transport.

### How it works

A delayed message store implements the `IDelayedMessageStore` interface. Delayed message delivery has two parts:

### Storing of delayed messages

A delayed message is stored using the `Store` method.

### Polling and dispatching of delayed messages

The message store is polled for due delayed messages in a background task which periodically calls `FetchNextDueTimeout`. If the method returns a message, the message is sent (see next paragraph), and the method is immediately called again. If the method returns `null`, `Next` is called, which returns either a `DateTimeOffset` indicating when the next message will be due, or `null` if there are no delayed messages. If another delayed message is persisted in the meantime, the `Store` method wakes up the polling thread.

When a due delayed message is returned by `FetchNextDueTimeout`, the message is sent to the destination queue and then removed from the store using the `Remove` method. In case of an unexpected exception during forwarding the failure is registered using `IncrementFailureCount`. If the configured number of retries is exhausted the message is forwarded to the configured `error` queue.

## Configuration

The settings described in this section allow changing the default behavior of the built-in delayed delivery store.

### NumberOfRetries

Number of retries when trying to forward due delayed messages.

Defaults to `0`.

### TimeToTriggerStoreCircuitBreaker

Time to wait before triggering the circuit breaker that monitors the storing of delayed messages in the database.

Defaults to `30` seconds.

### TimeToTriggerFetchCircuitBreaker

Time to wait before triggering the circuit breaker that monitors the fetching of due delayed messages from the database.

Defaults to `30` seconds.

### TimeToTriggerDispatchCircuitBreaker

Time to wait before triggering the circuit breaker that monitors the dispatching of due delayed messages to the destination.

Defaults to `30` seconds.

### MaximumRecoveryFailuresPerSecond

Maximum number of failed attempts per second to increment the per-message failure counter that triggers the recovery circuit breaker.

Defaults to `1` per sec.

## Using a custom delayed message store

Create a class which implements the `IDelayedMessageStore` interface and pass an instance to the `DelayedDeliverySettings` constructor.

If the custom store needs to set up some infrastructure (create tables, etc.) then it must implement `IDelayedMessageStoreWithInfrastructure`. This interface extends `IDelayedMessageStore` with a `SetupInfrastructure()` method. `SetupInfrastructure()` is called before `Initialize()`.

### Consistency

In `TransactionScope` [transaction mode](/transports/transactions.md), the delayed message store is expected to enlist in the `TransactionScope` to ensure **exactly once** behavior. `FetchNextDueTimeout`, `Remove`, and sending messages to their destination queues are all executed in a single distributed transaction. The built-in SQL Server store supports this mode of operation.

In lower transaction modes the dispatch behavior is **at least once**. `FetchNextDueTimeout` and `Remove` are executed in the same `TransactionScope` but sending messages to their destination queues is executed in a separate (inner) transport scope. If `Remove` fails, the message will be sent to the destination queue multiple times and the destination endpoint must handle the duplicates, using either the [outbox feature](/nservicebus/outbox/) or a custom de-duplication mechanism.

The built-in SQL Server delayed message store takes a pessimistic lock on the delayed message row in the `FetchNextDueTimeout` operation to prevent other physical instances of the same logical endpoint from delivering the same delayed message. A custom delayed message store must also take some kind of lock to prevent this from happening. For example, a delayed message store using Azure Blog Storage may take a lease lock.

### IDelayedMessageStore

> [!NOTE]
> The following samples are adapted from the `SqlServerDelayedMessageStore` class provided with the MSMQ transport, and are provided as a guide for adapting to whatever persistence technology is required.

When creating a custom message store, the class can either implement `IDelayedMessageStore`:

snippet: dms-without-infrastructure

or `IDelayedMessageStoreWithInfrastructure`:

snippet: dms-with-infrastructure

The only difference between the two interfaces is the `SetupInfrastructure` method, which must be implemented for `IDelayedMessageStoreWithInfrastructure` to create the required storage tables if they don't yet exist. With `IDelayedMessageStore`, it is expected that the storage tables already exist:
      
snippet: dms-setup-infrastructure

In the above example, `TimeoutTableCreator` is responsible for executing the script against the database. For SQL Server, the script is:

snippet: dms-sql-create-table

With both interfaces, the `Initialize` method is called with the name of the endpoint being initialized. The storage implementation should throw an exception if it doesn't support the specified transaction mode. For example, `TransactionScope` mode requires the storage to enlist in a distributed transaction managed by the DTC:

snippet: dms-initialise

snippet: dms-sql-crud

The remaining methods implement the logic required for the message store.

`Store` stores a delayed message:

snippet: dms-store

`Remove` removes a delayed message that has been dispatched to its destination. It must return `true` if the removal succeeded or `false` if there was nothing to remove because the delayed message was already removed:
   
snippet: dms-remove

`IncrementFailureCount` increments the count of failures for a given delayed message. It must return `true` if the increment succeeded or `false` if the delayed message was already removed:
   
snippet: dms-increment-failure-count

`Next` returns the date and time when the next delayed message is due or `null` if there are no delayed messages:
   
snippet: dms-next

`FetchNextDueTimeout` returns the next delayed message that will be due at a specified date and time or `null` if there will be no delayed messages at that date and time:
   
snippet: dms-fetch-next-duetimeout
