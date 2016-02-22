---
title: Batched message dispatch
summary: Describes how NServiceBus collects outgoing operations when processing message in order to dispatch them more efficiently.
tags: []
redirects:
---


### Behavior in Version 6 and above

As of Version 6 NServiceBus will by default collect all outgoing operations, Send|Reply|Publish etc, that happens as part of processing a message and pass them on to the transport after message handling pipeline has completed. This has two main benefits:

 * Business data will always be committed to storage before any outgoing operations happen. This makes sure that there is no "ghost" messages pushed out even for transports with no support for transactions that spans receives and sends no matter in what order you issue storage or bus operations.
 * Allows transports to improve performance by batching outgoing operations. Since transports now gets access to all outgoing messages in one go they can now optimize communication with the underlying queuing infrastructure to minimize round trips.


### Behavior in Version 5 and below

Since batched dispatch isn't available for Version 5 and below you need to pay more attention to the ordering of outgoing operations when using transports other than MSMQ and SQLServer since they lack support for cross queue transactions. For those transports messages will be dispatched immediately to the transport as soon as the call to `.Send` or `.Publish` completes. This means that there is a risk for "ghost" message to be emitted if you don't make sure to make all your database calls before performing the mentioned operations. One example would to do a `.Publish<OrderPlaced>()` event before calling `DB.Store(new Order())` since that would cause the `OrderPlaced` event to sent even if the order could not be stored in the database.

To avoid ghost messages you have the following options:

 * Always make sure to make send/publish messages after all storage operations have completed. This would have to be enforced in code reviews and can be hard to detect when you have multiple message handlers for the same message. See our documentation on [message handler ordering](/nservicebus/handlers/handler-ordering.md) for more details on how to control make sure handlers are called in a deterministic way.
 * Turn on the [Outbox](/nservicebus/outbox) feature on since that essentially will make sure that outgoing operations are not dispatched until all handlers have completed successfully. Even using the [InMemory](/nservicebus/persistence/in-memory.md) storage for the outbox will provide this type of delayed dispatch.
 * Switch to [MSMQ](/nservicebus/msmq/) or [SqlServer](/nservicebus/sqlserver/) as your transport.
