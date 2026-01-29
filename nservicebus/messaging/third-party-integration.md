---
title: Third-Party Integration
summary: Receiving messages from external systems
component: Core
related:
- nservicebus/messaging/headers
reviewed: 2026-01-29
---

Endpoints can receive messages from external systems (such as BizTalk, TIBCO, etc). To ensure those messages can be handled correctly by NServiceBus, additional information might be required which is otherwise provided by NServiceBus automatically.

## Deserialization

To [deserialize](/nservicebus/serialization/) a message coming from a third-party system, NServiceBus needs to know the .NET type to use.

See [Message type detection](/nservicebus/messaging/message-type-detection.md) for details.

The [RabbitMQ](/samples/rabbitmq/native-integration/), [SQL](/samples/sqltransport/native-integration/), and [Azure Service Bus](/samples/azure-service-bus-netstandard/native-integration/) native integration samples demonstrate inferring message type from the message body.

## Visualization

To visualize messages from third-party systems correctly within ServicePulse, additional headers are necessary.

- [`NServiceBus.ConversationId`](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-conversationid) - used to show related messages in the Flow Diagram and Sequence Diagram. If this header is not provided, the message won't be included in the conversation it started, but a `ConversationId` will be generated for subsequent messages sent using NServiceBus.
- [`NServiceBus.OriginatingEndpoint`](/nservicebus/messaging/headers.md#diagnostic-and-informational-headers-nservicebus-originatingendpoint) - used in all views, to show the logical endpoint that sent the message. This should be the name of the third-party endpoint sending the message. For example, _BizTalk.ProcessOrder_.
