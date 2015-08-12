---
title: Third Party Integration
summary: Lists the necessary headers when receiving messages from non-NSB endpoints for better monitoring.
tags:
- Monitoring
- Third Party Integration
- ServiceInsight
- ServicePulse
related:
- nservicebus/messaging/message-headers
---

# How to integrate external systems with NServiceBus?

When an NServiceBus endpoint receives messages from external systems such as BizTalk, TIBCO, etc. directly, the message itself might not contain regular all information that NServiceBus provides. That information is necessary for proper serialization of the message, as well as better monitoring and troubleshooting experience.

To get the best use of your tools ensure that the following information is included in the message sent by the third party endpoints. 

### Required information

In order for NServiceBus to deserialize a message coming from a third party system, the message needs to contain information to allow NServiceBus to map the message to a message type. Depending on the serializer used by the receiving endpoint that information might be either provided in the message body or in a NServiceBus specific header.

Embedding type info in the message body is currently supported by the default XmlSerializer and also the Json.NET seriliazer. The [RabbitMQ](/samples/rabbitmq/native-integration/) and [SQL](/samples/sqltransport/native-integration/) integration samples demonstrates this.

If using a different serilizer or if you want to avoid embedding type info in your message bodies you can include the following header in your message:

Header key  | Value
------------- | -------------
NServiceBus.EnclosedMessageTypes  | [FullName](https://msdn.microsoft.com/en-us/library/system.type.fullname) of your message type, e.g. `IntegrationSample.Messages.Commands.ProcessOrder`

If set NServiceBus will instruct the serializer to deserialize the payload into the type specified.

### Additional information 

For NServericeBus v4 and above by providing the following additional headers you will gain a better debugging experience in ServiceInsight. This information is required for including the external message in a flow diagram.

Header key  | Value
------------- | -------------
NServiceBus.ConversationId  | Valid Guid, useful to tie in the whole message flow to get an accurate view in ServiceInsight, for example in a message flow or sequence diagrams. If not provided, then the external message won't be included in the conversation it started. ConversationId will be generated though for all the following messages sent using NServiceBus.
NServiceBus.OriginatingEndpoint  | Name of the third party endpoint sending the message, for e.g. BizTalk.ProcessOrder
NServiceBus.OriginatingMachine  | Server where the third party endpoint is located, for e.g. BizTalkServer
