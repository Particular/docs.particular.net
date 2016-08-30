---
title: RabbitMQ Transport Message Id strategy
summary: Controlling the message Id strategy.
reviewed: 2016-08-30
component: rabbit
related:
 - nservicebus/messaging/message-identity
---


By default, the `message-id` property of the AMQP standard is used to relay the [message identity](nservicebus/messaging/message-identity.md). If this header isn't set, the transport will throw an exception because NServiceBus needs a message identity to perform retries, de-duplication, etc., in a safe way. For integration scenarios where the sender is not controlled, consider using a custom scheme that extracts a message identity from a custom header or some data contained in the actual message body. This custom strategy can be configured by calling:

snippet:rabbitmq-config-custom-id-strategy

WARNING: It is extremely important to use a uniquely identifying property of the message in a custom message identity strategy. If the value for a message were to change (for example, if attempting to use `Guid.NewGuid().ToString()`) then message retries would break, as the infrastructure would be unable to determine that it was processing the same message repeatedly.
