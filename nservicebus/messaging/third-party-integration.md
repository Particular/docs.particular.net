---
title: Third Party Integration
summary: Receiving messages from external systems
component: Core
related:
- nservicebus/messaging/headers
reviewed: 2017-02-06
---

Endpoints can receive messages from external systems (such as BizTalk, TIBCO, etc). To ensure those messages can be handled correctly by NServiceBus, additional information might be required which are otherwise provided by NServiceBus automatically.


## Required information

In order to [deserialize](/nservicebus/serialization/) a message coming from a third party system, the message needs to contain a [NServiceBus.EnclosedMessageTypes header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes) to allow mapping the message to a message type.

Depending on the serializer used by the receiving endpoint, that information might also be provided in other ways. 

Embedding type info in the message body is currently supported by the following serializers

 * [Xml](/nservicebus/serialization/xml.md) 
 * [Json](/nservicebus/serialization/json.md)
 * [Newtonsoft](/nservicebus/serialization/newtonsoft.md)

The [RabbitMQ](/samples/rabbitmq/native-integration/), [SQL](/samples/sqltransport/native-integration/), and [Azure Service Bus](/samples/azure/native-integration-asb/) native integration samples demonstrates this.


## Additional information

To visualize messages from third party systems correctly within ServiceInsight, additional headers are necessary

 * [NServiceBus.ConversationId](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-conversationid) If not provided, then the external message won't be included in the conversation it started. ConversationId will be generated though for all the following messages sent using NServiceBus.
 * [NServiceBus.OriginatingEndpoint](/nservicebus/messaging/headers.md#diagnostic-and-informational-headers-nservicebus-originatingendpoint) Name of the third party endpoint sending the message, for e.g. *BizTalk.ProcessOrder*
 * [NServiceBus.OriginatingMachine](/nservicebus/messaging/headers.md#diagnostic-and-informational-headers-nservicebus-originatingmachine) Server where the third party endpoint is located, for e.g. *BizTalkServer*
