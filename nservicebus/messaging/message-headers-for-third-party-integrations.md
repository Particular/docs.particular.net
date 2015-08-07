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

# Integrating with external systems? How to get more out of your monitoring?

When an NServiceBus endpoint receives messages from external systems such as BizTalk, TIBCO, etc directly, the message itself will not contain regular NServiceBus headers. Without these headers, ServiceControl cannot provide sufficient information to ensure effective monitoring of these endpoints. 

To ensure that your Ops team is able to monitor your endpoints and have valuable meta data from ServiceControl, it becomes important as to how you integrate these messages. 

When NServiceBus endpoints send messages, it includes a certain set of headers such as the name of the endpoint sending the message, the machine name etc, which is quite useful when analyzing this information from an Ops point of view in ServicePulse or when debugging situations using ServiceInsight. Therefore to get the best use of your monitoring and debugging tools, please ensure that the following headers are being added to the actual message by the third party endpoints when sending a message. 

### Mandatory Headers

Key  | Value
------------- | -------------
NServiceBus.EnclosedMessageTypes  | Fully Qualified Message type including the assembly name, For e.g. IntegrationSample.Messages.Commands.ProcessOrder, IntegrationSample.Messages


### Additional Headers 

Adding the following headers will make the diagrams in ServiceInsight much more meaningful and for Ops when these messages fail and show up in FailedMessages in ServicePulse.

Key  | Value
------------- | -------------
NServiceBus.ConversationId  | Valid Guid, useful to tie in the whole message flow to get a much accurate view in ServiceInsight, for example in message flow diagram
NServiceBus.OriginatingEndpoint  | Name of the third party endpoint sending the message, for e.g. BizTalk.ProcessOrder
NServiceBus.OriginatingMachine  | Server where the third party endpoint is located for e.g. BizTalkServer
