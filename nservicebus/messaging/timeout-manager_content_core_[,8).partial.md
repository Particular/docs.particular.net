{{NOTE:
Duplicate timeouts can be dispatched if the transport and persistence do not support [TransactionScope mode](/transports/transactions.md#transactions-transaction-scope-distributed-transaction), or are configured not to use it. Scaled-out environments, in particular, are vulnerable to duplicate timeouts. The receiving endpoint must account for this, for example with the [outbox](/nservicebus/outbox/) or another idempotent mechanism.

Exactly-once timeouts are possible with [MSMQ](/transports/msmq/) or [SQL](/transports/sql/) transports with [NHibernate](/persistence/nhibernate/) or [SQL](/persistence/sql/) persistence **and** if they are configured to share the transactional context (either through distributed transactions or connection sharing).
}}

NServiceBus provides a delayed-delivery implementation for transports that don't support it natively. All transports except MSMQ support delayed delivery natively.

The delayed-delivery feature uses a built-in persistent store and requires using NServiceBus persistence. The timeout data is stored in three different locations at various stages of processing: `[endpoint_queue_name].Timeouts` queue, timeouts storage location specific for the chosen persistence (e.g. dedicated table or document type) and `[endpoint_queue_name].TimeoutsDipatcher` queue. The queues are automatically created by [NServiceBus installers](/nservicebus/operations/installers.md) when setting up the endpoint.

### Storing timeout messages

When NServiceBus detects that an outgoing message should be delayed, the message is routed to the `[endpoint_queue_name].Timeouts` queue instead of directly to the destination queue. The ultimate destination address is preserved in a header.

The `[endpoint_queue_name].Timeouts` queue is monitored by an [internal receiver](/nservicebus/satellites). The receiver picks up timeout messages and stores them using the selected NServiceBus persistence.

Delayed messages are stored for the specified delay time, using persistance implementation specified in the configuration:

snippet: configure-persistence-timeout

### Retrieving expired timeouts

NServiceBus periodically retrieves expiring timeouts from persistence. When a timeout expires, then a message with that timeout ID is sent to the `[endpoint_queue_name].TimeoutsDipatcher` queue. That queue is monitored by NServiceBus internal receiver. When the receiver picks up a message, it looks up the corresponding timeout in the storage. If it finds it, it dispatches the timeout message to the destination queue.

### Handling of persistence errors

If there are any connection problems with the timeout storage then NServiceBus waits for 2 minutes (by default) to allow the storage to come back online. If the problem is not resolved within that time frame, then a [critical error](/nservicebus/hosting/critical-errors.md) is raised.

The default wait time can be changed:

snippet: TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages

NOTE: The timeout manager polls every minute. This means that it could take more time then the configured *TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages* value before an issue is detected.

When this happens the following critical error message will be raised:

> Repeated failures when fetching timeouts from storage, endpoint will be terminated.

If the NServiceBus.Host is used, the host will execute a fail-fast as documented in the [default critical error behavior for the NServiceBus.Host](/nservicebus/hosting/nservicebus-host/#endpoint-configuration-default-critical-error-action).

### Migrating timeouts to native delayed delivery

If a persistence that supports native delayed delivery is used, and the system still contains timeouts that were scheduled using the timeout manager, use the [timeout migration tool](/nservicebus/tools/migrate-to-native-delivery.md) to migrate these timeouts to the native-delay delivery implementation.
