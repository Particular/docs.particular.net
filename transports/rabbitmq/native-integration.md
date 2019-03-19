---
title: Native integration
summary: Considerations when integrating NServiceBus endpoints with native RabbitMQ publishers and consumers.
reviewed: 2019-03-19
component: rabbit
versions: '[2,]'
related:
 - nservicebus/messaging/message-identity
redirects:
 - nservicebus/rabbitmq/message-id-strategy
 - transports/rabbitmq/message-id-strategy
---

This document describes how to consume messages from and sending messages to non NServiceBus endpoints via RabbitMQ in integration scenarios.

TODO: The below section needs a partial for 5.1.0

### Access to the native RabbitMQ message details

It can sometimes be useful to access the native RabbitMQ message from behaviors and handlers. When a message is received the transport add the native RabbitMQ client [`BasicDeliveryEventArgs`](TODO) to the message processing context. Use the code below to access the message details:

snippet: rabbitmq-config-access-to-event-args

TODO: Create the snippet code

### Custom message id strategy

By default, the `message-id` property of the AMQP standard is used to relay the [message identity](/nservicebus/messaging/message-identity.md). If this property isn't set, the transport will throw an exception because NServiceBus requires a message identity to perform retries, de-duplication, etc., in a safe way.

For integration scenarios where the sender is not controlled, the receiver might need to employ a custom strategy that extracts a message identity from a custom header or part of the message body. This custom strategy can be configured by calling:

snippet: rabbitmq-config-custom-id-strategy

WARNING: It is extremely important that the custom strategy is deterministic (it returns the same value when invoked multiple times for the same message), and that no two messages are assigned the same value.
