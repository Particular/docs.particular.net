---
title: Timeout Manager
summary: NServiceBus persistent delayed message store
component: core
reviewed: 2016-12-05
tags:
 - Defer
 - Timeout Manager
related:
 - samples/delayed-delivery
 - nservicebus/messaging/delayed-delivery
---

NServiceBus has a built-in persistent store for delayed messages. The timeout data is stored in three different locations at various stages of processing: `[endpoint_queue_name].Timeouts` queue, timeouts storage location specific for the chosen persistence (e.g. dedicated table or document type) and `[endpoint_queue_name].TimeoutsDipatcher` queue. The queues are automatically created by [NServiceBus installers](/nservicebus/operations/installers.md) when setting up the endpoint.

### Storing timeout messages

Whne NServiceBus detects an outgoing message should be delayed, it routes it to to the `[endpoint_queue_name].Timeouts` queue instead of the destination queue. The ultimate destination address is preserved in a header. That queue is monitored by NServiceBus [internal receiver](/nservicebus/satellites.md). It picks up timeout messages and stores them using the selected NServiceBus persistence. NHibernate persistence stores timeout messages in a table called `TimeoutEntity`, RavenDB persistence stores them as documents of a type `TimeoutData`. 

If the transport is configured to use [TransactionScope mode](/nservicebus/transports/transactions.md#transactions-transaction-scope-distributed-transaction) and the selected persistence supports `TransactionScope` transactions (both NHibernate and RavenDB do) NServiceBus guarantees *exactly-once* semantics of the store operation, meaning that timeouts in the store will not get duplicated.

Delayed messages are persisted (stored in a durable storage) in a location specified for `Timeouts`. The messages will be stored for the specified delay time. Sample persistence configuration for Timeouts can be seen below.

snippet:configure-persistence-timeout

### Retrieving exipired timeouts

NServiceBus periodically retrieves expiring timeouts from persistence. When a timeout expires, then a message with that timeout ID is sent to `[endpoint_queue_name].TimeoutsDipatcher` queue. That queue is monitored by NServiceBus internal receiver. When it picks up a message, it looks up the corresponding timeout in the storage. If it finds it, it dispatches the timeout message to the destination queue.

If the transport is configured to use [TransactionScope mode](/nservicebus/transports/transactions.md#transactions-transaction-scope-distributed-transaction) and the selected persistence supports `TransactionScope` transactions NServiceBus guarantees *exactly-once* semantics of the dispatch operations, meaning that outgoing expired delayed messages will not get duplicated. If these conditions are not met, the timeout messages might get duplicated and the receiving endpoint has to account for that.

### Handling of persistence errors

If there are any problems with timeout storage by default a wait of 2 minutes is done to allow the storage to come back online. If the problem is not resolved within that time frame, a [Critical Error](/nservicebus/hosting/critical-errors.md) is raised.

To change the default wait time:

snippet:TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages


