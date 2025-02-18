---
title: Batched message dispatch
summary: Describes how NServiceBus collects outgoing operations in order to dispatch them more efficiently.
component: Core
reviewed: 2025-01-24
---

Starting from Versions 6, all outgoing operations executed during message processing (e.g. commands, responses, events) are bundled together and handed to the transport only after the message handling pipeline has completed (the [transactional session](/nservicebus/transactional-session) offers similar behavior for outgoing message operations when the session is committed). This has two benefits:

 * Business data operations will be committed to storage before outgoing operations are dispatched. This ensures no outgoing messages are sent in case an exception occurs - during or after the business data operations are committed.
 * Allows transports to improve performance by batching outgoing operations. Since transports get access to all outgoing messages as a group they can optimize communication with the underlying queuing infrastructure to minimize round trips.

There are cases in which the transport might decide, due to underlying restrictions, to bundle the batched operations into multiple individual calls to the broker. 
This is the case for:
- [Azure Service Bus](/transports/azure-service-bus)
- [SQS](/transports/sqs)
- [RabbitMQ](/transports/rabbitmq)
