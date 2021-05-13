---
title: Third Party Integration
summary: Receiving messages from external systems
component: Core
related:
- nservicebus/messaging/headers
reviewed: 2021-05-12
---

Endpoints can receive messages from external systems (such as BizTalk, TIBCO, etc). To ensure those messages can be handled correctly by NServiceBus, additional information might be required which are otherwise provided by NServiceBus automatically.


## Deserialization

In order to [deserialize](/nservicebus/serialization/) a message coming from a third party system, NServiceBus needs to know the .NET type to use.

Starting from NServiceBus version 7.4, the [NServiceBus.EnclosedMessageTypes header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes) is automatically populated when missing. When using NServiceBus version 7.3 and below, the sender should set that header.

Some serializers can infer the message type from information embedded in the message body. 

 * [XML](/nservicebus/serialization/xml.md) 
 * [JSON](/nservicebus/serialization/json.md)
 * [Newtonsoft](/nservicebus/serialization/newtonsoft.md)

The [RabbitMQ](/samples/rabbitmq/native-integration/), [SQL](/samples/sqltransport/native-integration/), and [Azure Service Bus](/samples/azure-service-bus-netstandard/native-integration/) native integration samples demonstrate inferring message type from the message body.


## Visualization

To visualize messages from third party systems correctly within ServiceInsight, additional headers are necessary

- [`NServiceBus.ConversationId`](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-conversationid) - used to show related messages in the Flow Diagram and Sequence Diagram. If this header is not provided, the message won't be included in the conversation it started, but a `ConversationId` will be generated for subsequent messages sent using NServiceBus.
- [`NServiceBus.OriginatingEndpoint`](/nservicebus/messaging/headers.md#diagnostic-and-informational-headers-nservicebus-originatingendpoint) - used to in all views, to show the logical endpoint that sent the message. This should be the name of the third party endpoint sending the message. For example, _BizTalk.ProcessOrder_.
