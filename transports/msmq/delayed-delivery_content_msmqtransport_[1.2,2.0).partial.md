The MSMQ transport can use the [timeout manager](/nservicebus/messaging/timeout-manager.md) for delayed delivery of messages (e.g. [saga timeouts](/nservicebus/sagas/timeouts.md) or [delayed retries](/nservicebus/recoverability/configure-delayed-retries.md)) as well as an external store for delayed messages.

The MSMQ transport requires explicit configuration to enable delayed message delivery using an external store. For example:

snippet: delayed-delivery

The SQL Server delayed message store (`SqlServerDelayedMessageStore`) is the only delayed message store that ships with the MSMQ transport.

> [!WARNING]
> When enabling native delayed delivery, the timeout manager should be disabled so that the two features do not compete for delayed messages. See the [MSMQ Transport version 1 to 2 upgrade guide](/transports/upgrades/msmq-1to2.md) for guidance on migrating away from the timeout manager.

### How it works

A delayed message store implements the `IDelayedMessageStore` interface. Delayed message delivery has two parts:

- Storing delayed messages via the `Store` method
- Polling and dispatching the delayed messages

### Polling and dispatching delayed messages

The message store is polled for due delayed messages in a background task which periodically calls `FetchNextDueTimeout`. If the method returns a message, the message is sent, and the method is immediately called again. If the method returns `null`, `Next` is called, which returns either a `DateTimeOffset` indicating when the next message will be due, or `null` if there are no delayed messages.

When a due delayed message is returned by `FetchNextDueTimeout`, the message is sent to the destination queue and then removed from the store using the `Remove` method. If an exception occurs when forwarding the message, the failure is registered using `IncrementFailureCount`. If the configured number of retries is exhausted the message is forwarded to the configured `error` queue.

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

To create a custom storage, implement the `IDelayedMessageStore` interface and pass an instance to the `DelayedDeliverySettings` constructor.

The built-in SQL Server delayed message store takes a pessimistic lock on the delayed message row in the `FetchNextDueTimeout` operation to prevent other physical instances of the same logical endpoint from delivering the same delayed message. A custom delayed message store must also take some kind of lock to prevent this from happening. For example, a delayed message store using Azure Blog Storage may take a lease lock.

### Consistency

In `TransactionScope` [transaction mode](/transports/transactions.md), the delayed message store is expected to enlist in the `TransactionScope` to ensure **exactly once** behavior. `FetchNextDueTimeout`, `Remove`, and sending messages to their destination queues are all executed in a single distributed transaction. The built-in SQL Server store supports this mode of operation.

In lower transaction modes the dispatch behavior is **at least once**. `FetchNextDueTimeout` and `Remove` are executed in the same `TransactionScope` but sending messages to their destination queues is executed in a separate (inner) transport scope. If `Remove` fails, the message will be sent to the destination queue multiple times and the destination endpoint must handle the duplicates, using either the [outbox feature](/nservicebus/outbox/) or a custom de-duplication mechanism.
