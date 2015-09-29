---
title: Batched message dispatch
summary: Describes how NServiceBus collects outgoing operations when processing message in order to dispatch them more efficiently.
tags: []
redirects:
---
When processing messages NServiceBus will by default collect all outgoing operations, Send|Reply|Publish etc, and pass them on to the transport after message handling pipeline has completed. This has two main benefits:

* Business data will always be committed to storage before any outgoing operations happen. This makes sure that there is no "ghost" messages pushed out even for transports with no support for transactions that spans receives and sends no matter in what order you issue storage or bus operations.
* Allows transports to improve performance by batching outgoing operations. Since transports now gets access to all outgoing messages in one go they can now optimize communication with the underlying queuing infrastructure to minimize round trips.

NOTE: Versions prior to Version 6 didn't do batched dispatch by default instead you would have to turn the [Outbox](/nservicebus/outbox) feature on.
