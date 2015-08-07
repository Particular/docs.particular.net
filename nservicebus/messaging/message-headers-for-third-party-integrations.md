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

When an NServiceBus endpoint receives messages from external systems such as BizTalk, TIBCO, etc directly, the message itself will not contain regular NServiceBus headers. Some headers are required by NServiceBus to handle those messages (i.e. enable deserialization of the message). Others are essential to provide effective monitoring and troubleshooting experience. 

When NServiceBus endpoints send messages, they include a certain set of headers such as the type of message, the name of the sending endpoint, the machine name etc. To get the best use of your monitoring and debugging tools, please ensure that the following headers are being added to the actual message by the third party endpoints when sending a message. 

### Mandatory Headers

Required for deserializing the message:

Key  | Value
------------- | -------------
NServiceBus.EnclosedMessageTypes  | Fully Qualified Message type including the assembly name, for e.g. IntegrationSample.Messages.Commands.ProcessOrder, IntegrationSample.Messages


### Additional Headers 

Enable better monitoring in ServicePulse and debugging in ServiceInsight by providing additional data:

Key  | Value
------------- | -------------
NServiceBus.ConversationId  | Valid Guid, useful to tie in the whole message flow to get a much accurate view in ServiceInsight, for example in message flow diagram
NServiceBus.OriginatingEndpoint  | Name of the third party endpoint sending the message, for e.g. BizTalk.ProcessOrder
NServiceBus.OriginatingMachine  | Server where the third party endpoint is located, for e.g. BizTalkServer
