---
title: Message Id strategy
summary: Controlling the message Id strategy.
reviewed: 2018-11-22
component: rabbit
versions: '[2,]'
related:
 - nservicebus/messaging/message-identity
redirects:
 - nservicebus/rabbitmq/message-id-strategy
---


By default, the `message-id` property of the AMQP standard is used to relay the [message identity](/nservicebus/messaging/message-identity.md). If this property isn't set, the transport will throw an exception because NServiceBus requires a message identity to perform retries, de-duplication, etc., in a safe way. 

For integration scenarios where the sender is not controlled, the receiver might need to employ a custom strategy that extracts a message identity from a custom header or part of the message body. This custom strategy can be configured by calling:

snippet: rabbitmq-config-custom-id-strategy

WARNING: It is extremely important that the custom strategy is deterministic (it returns the same value when invoked multiple times for the same message), and that no two messages are assigned the same value. 
