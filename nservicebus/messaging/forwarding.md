---
title: Message forwarding
summary: Describes how to set up message forwarding
component: Core
tags:
- Forwarding Messages
---


Use this feature to forward successfully processed messages from an endpoint to a specified destination endpoint. Forwarding messages is particularly useful in complex upgrade scenarios, when the old version and new version of a particular endpoint are running side-by-side.


## Auditing vs Fowarding

[Auditing](/nservicebus/operations/auditing.md) and Forwarding are very similar, both send a copy of the processed message to another queue. The main difference are intended usage scenarios.

Auditing is used for collecting information on what is happening in the system, therefore the audited message is enriched with additional [information regarding processing it](/nservicebus/operations/auditing.md#message-headers). The forwarded message is a copy of the processed message, without the additional auditing information.

Note: In Versions 5 and below some of the audit headers were available for the forwarded messages. In Versions 6 and above the forwarded messages will no longer contain the audit message headers.


## Configuring Message Forwarding

partial:config