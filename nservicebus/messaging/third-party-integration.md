---
title: Third Party Integration
summary: Receiving messages from external systems
component: Core
tags:
- Monitoring
- Third Party Integration
- ServiceInsight
related:
- nservicebus/messaging/headers
reviewed: 2017-01-18
---

NServiceBus endpoints can receive messages from external systems (such as BizTalk, TIBCO, etc). To ensure those messages can be handled correctly by NServiceBus, additional information might be required which are otherwise provided by NServiceBus automatically.


## Required information

In order for NServiceBus to deserialize a message coming from a third party system, the message needs to contain information to allow NServiceBus to map the message to a message type.

Header key  | Value
------------- | -------------
NServiceBus.EnclosedMessageTypes  | [FullName](https://msdn.microsoft.com/en-us/library/system.type.fullname) of the message type, e.g. `IntegrationSample.ProcessOrder`


Depending on the serializer used by the receiving endpoint, that information might also be provided in other ways. Embedding type info in the message body is currently supported by the default XmlSerializer and also the Json.NET serializer. The [RabbitMQ](/samples/rabbitmq/native-integration/) and [SQL](/samples/sqltransport/native-integration/) native integration samples demonstrates this.


## Additional information

To visualize messages from third party systems correctly within ServiceInsight, additional headers are necessary:

Header key  | Value
------------- | -------------
NServiceBus.ConversationId  | Arbitrary string value (e.g. a `Guid` string), used to tie in the whole message flow to get an accurate view in message flow or sequence diagrams. If not provided, then the external message won't be included in the conversation it started. ConversationId will be generated though for all the following messages sent using NServiceBus.
NServiceBus.OriginatingEndpoint  | Name of the third party endpoint sending the message, for e.g. *BizTalk.ProcessOrder*
NServiceBus.OriginatingMachine  | Server where the third party endpoint is located, for e.g. *BizTalkServer*
