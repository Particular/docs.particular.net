---
title: RabbitMQ native integration
summary: Considerations when integrating NServiceBus endpoints with native RabbitMQ publishers and consumers.
reviewed: 2025-10-16
component: rabbit
versions: '[2,]'
related:
 - nservicebus/messaging/message-identity
 - samples/rabbitmq/native-integration
redirects:
 - nservicebus/rabbitmq/message-id-strategy
 - transports/rabbitmq/message-id-strategy
---

This document describes how to consume messages from and send messages to non-NServiceBus endpoints via RabbitMQ in integration scenarios.

### Access to the received native RabbitMQ message details

It can sometimes be useful to access the native RabbitMQ message from behaviors and handlers. When a message is received, the transport adds the native RabbitMQ client [`BasicDeliverEventArgs`](https://rabbitmq.github.io/rabbitmq-dotnet-client/api/RabbitMQ.Client.Events.BasicDeliverEventArgs.html) to the message processing context. Use the code below to access the message details from a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md):

snippet: rabbitmq-access-to-event-args

partial: outgoing-customization

### Custom message ID strategy

By default, the `message-id` property of the AMQP standard is used to relay the [message identity](/nservicebus/messaging/message-identity.md). If this property isn't set, the transport will throw an exception because NServiceBus requires a message identity to perform retries, de-duplication, etc., in a safe way.

For integration scenarios where the sender is not controlled, the receiver might need to employ a custom strategy that extracts a message identity from a custom header or part of the message body. This custom strategy can be configured by calling:

snippet: rabbitmq-config-custom-id-strategy

> [!WARNING]
> It is extremely important that the custom strategy is deterministic (it returns the same value when invoked multiple times for the same message), and that no two messages are assigned the same value.

### Message type detection

The native message must allow NServiceBus to [detect message type either via headers or message payload](/nservicebus/messaging/message-type-detection.md).

> [!NOTE]
> Starting with versions 10.1.4, 9.2.2, and 8.0.9, if the incoming AMQP message specifies a value for the [`Content type` property]([https://learn.microsoft.com/en-us/rest/api/servicebus/message-headers-and-properties](https://www.rabbitmq.com/docs/publishers#message-properties)), the value is used to populate the `NServiceBus.ContentType` header.
