---
title: Message forwarding
summary: Describes how to set up message forwarding
component: Core
reviewed: 2017-03-14
related: 
 - nservicebus/operations/auditing
---


Use this feature to forward successfully processed messages from an endpoint to a specified destination endpoint. Forwarding messages is particularly useful in complex upgrade scenarios, when the old version and new version of a particular endpoint are running side-by-side.


## Auditing vs Fowarding

[Auditing](/nservicebus/operations/auditing.md) and Forwarding are very similar, both send a copy of the processed message to another queue. The main difference are intended usage scenarios.

Auditing is used for collecting information on what is happening in the system, therefore the audited message is enriched with additional [information regarding processing it](/nservicebus/operations/auditing.md#message-headers). The forwarded message is a copy of the processed message, without the additional auditing information.


partial: headers


## Configuring Message Forwarding

partial: config