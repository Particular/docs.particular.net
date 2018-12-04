---
title: Third Party Integration
summary: Receiving messages from external systems
component: Core
related:
- nservicebus/messaging/headers
reviewed: 2018-12-04
---

Endpoints can receive messages from external systems (such as BizTalk, TIBCO, etc). To ensure those messages can be handled correctly by NServiceBus, additional information might be required which are otherwise provided by NServiceBus automatically.


## Deserialization

In order to [deserialize](/nservicebus/serialization/) a message coming from a third party system, NServiceBus needs to know the .NET type to use.

The sender can specify a message type with the [NServiceBus.EnclosedMessageTypes header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes).

Some serialiazers can infer the message type from information embedded in the message body. 

 * [Xml](/nservicebus/serialization/xml.md) 
 * [Json](/nservicebus/serialization/json.md)
 * [Newtonsoft](/nservicebus/serialization/newtonsoft.md)

The [RabbitMQ](/samples/rabbitmq/native-integration/), [SQL](/samples/sqltransport/native-integration/), and [Azure Service Bus](/samples/azure/native-integration-asb/) native integration samples demonstrate inferring message type from .


## Visualization

To visualize messages from third party systems correctly within ServiceInsight, additional headers are necessary

 * [NServiceBus.ConversationId](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-conversationid) - is used to show related messages in the message flow and sequence views. If not provided, then the external message won't be included in the conversation it started. ConversationId will be generated though for all the following messages sent using NServiceBus.
 * [NServiceBus.OriginatingEndpoint](/nservicebus/messaging/headers.md#diagnostic-and-informational-headers-nservicebus-originatingendpoint) - is used to show the logical endpoint that sent the message in all views. This should be the name of the third party endpoint sending the message. e.g. *BizTalk.ProcessOrder*
 * [NServiceBus.OriginatingMachine](/nservicebus/messaging/headers.md#diagnostic-and-informational-headers-nservicebus-originatingmachine) - Server where the third party endpoint is located. e.g. *BizTalkServer*
