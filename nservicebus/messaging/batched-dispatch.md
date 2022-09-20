---
title: Batched message dispatch
summary: Describes how NServiceBus can collect outgoing operations in order to dispatch them more efficiently.
component: Core
reviewed: 2022-09-20
---

In Versions 6 and above, all outgoing operations that happen as part of processing a message (e.g. commands, responses, events) will be bundled together and passed to the transport after the message handling pipeline has been completed. With the introduction of the [transactional session](/nservicebus/transactional-session), outgoing message operations can also be bundled together and passed to the transport when the session commits. This has two benefits:

 * Business data will always be committed to storage before any outgoing operations are dispatched. This ensures that there are no outgoing messages in case an exception occurs.
 * Allows transports to improve performance by batching outgoing operations. Since transports get access to all outgoing messages as a group they can optimize communication with the underlying queuing infrastructure to minimize round trips.

There are cases in which the transport might decide, due to underlying restrictions, to bundle the batched operations into multiple individual calls to the broker. 
This is the case for:
- [Azure Service Bus](/transports/azure-service-bus)
- [SQS](/transports/sqs)
- [RabbitMQ](/transports/rabbitmq)