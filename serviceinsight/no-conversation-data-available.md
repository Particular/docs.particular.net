---
title: No conversation data available
summary: Why ServiceInsight cannot visualize a conversation.
tags: 
- ServiceControl
- ServiceInsight
- Expiration
---

Sometimes, when you select a message in ServiceInsight, the visualization windows will show the message "No conversation data available". 

![No conversation data available](./images/no-conversation-data-available.PNG)

ServiceInsight visualizations are based on collections of messages called conversations. Each of these visualizations will query Service Control to get all of the messages that belong to a specific conversation in order to display them on the screen. 

There are two reasons why Service Control may not be able to find any messages that belong to a given conversation.

Firstly, Service Control does not keep Audit data forever. When messages are past a certain age they will be deleted from Service Control. If a message appears in a Service Insight list but the conversation cannot be found then it is likely because all of the messages have expired within Service Control and been removed. The messages still appearing in the list are being read from an in-memory ServiceInsight cache. See [Automatic Expiration of ServiceControl Data](/servicecontrol/how-purge-expired-data.md) for instructions on how to increase the time that message is available before it expires.  

Secondly, if the messages are present in Service Control, then it's possible that only the conversation data is missing. This data comes from headers that NServiceBus applies to messages as they are sent or published. One of these headers is `NServiceBus.ConversationId`. When the processing of one message results in a new message being sent or published, both of these messages will have the same Conversation ID. This header was added in version 4 of NServiceBus. Messages sent by older versions of NServiceBus are missing the header and will not be available in ServiceInsight visualizations.