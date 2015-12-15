---
title: Data not available
summary: Why ServiceInsight cannot visualize a conversation.
tags:
- ServiceControl
- ServiceInsight
- Expiration
---

Sometimes, when you select a message in ServiceInsight, the visualization windows will show the message "No data available".

![No data available](./images/no-conversation-data-available.png)

ServiceInsight visualizations are based on collections of messages called conversations. Each of these visualizations will query ServiceControl to get all of the messages that belong to a specific conversation in order to display them on the screen.

There are two reasons why ServiceControl may not be able to find any messages that belong to a given conversation.

Firstly, ServiceControl does not keep audit data forever. When messages are past a certain age they will be deleted from ServiceControl. If a message appears in a ServiceInsight list but the conversation cannot be found then it is likely because all of the messages have expired within ServiceControl and been removed. The messages still appearing in the list are being read from an in-memory ServiceInsight cache. See [Automatic Expiration of ServiceControl Data](/servicecontrol/how-purge-expired-data.md) for instructions on how to increase the time that message is available before it expires. 

The other reason that data might not be available is when messages in ServiceControl are not linked to a conversation. ServiceControl identifies which conversation a message belongs to by looking at the `NServiceBus.ConversationId` header on each message. If this header is not present for any reason then the message will not appear in any ServiceInsight visualizations.

NOTE: The `NServiceBus.ConversationId` header was introduced in NServiceBus Version 4. Messages sent or published by earlier versions of NServiceBus will be missing this header and will not appear in ServiceInsight visualizations.