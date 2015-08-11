---
title: Required Headers For Third Party Integration
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

When an NServiceBus endpoint receives messages from external systems such as BizTalk, TIBCO, etc directly, the message itself might not contain regular NServiceBus headers. Headers provide additional information necessary for proper serialization of the message, better monitoring and troubleshooting experience.

To get the best use of your monitoring and debugging tools, please ensure that the following information is included in the message sent by the third party endpoints. 

### Required metadata

In order to enable NServiceBus to deserialize a message coming from a third party system, the message must contain its **full type name**. It can either be provided in the message body by ensuring appropriate serialization or by adding an additional header (in NServiceBus v3 and above).

Header key  | Value
------------- | -------------
NServiceBus.EnclosedMessageTypes  | Fully Qualified Message type including the assembly name, for e.g. IntegrationSample.Messages.Commands.ProcessOrder, IntegrationSample.Messages

For more information refer to integration samples for [RabbitMQ](/samples/rabbitmq/native-integration/) and [SQL](/samples/sqltransport/native-integration/) transports.


### Additional metadata 

For NServericeBus v4 and above by providing the following additional headers you will gain a better debugging experience in ServiceInsight. This information is required for including the external message in a flow diagram.

Header key  | Value
------------- | -------------
NServiceBus.ConversationId  | Valid Guid, useful to tie in the whole message flow to get a much accurate view in ServiceInsight, for example in message flow diagram
NServiceBus.OriginatingEndpoint  | Name of the third party endpoint sending the message, for e.g. BizTalk.ProcessOrder
NServiceBus.OriginatingMachine  | Server where the third party endpoint is located, for e.g. BizTalkServer
