---
title:  Editing failed messages
summary: Describes the concept of editing and retrying messages in ServicePulse
component: ServicePulse
reviewed: 2019-07-04
---
If a message cannot be successfully retried it is possible to modify that message prior to retrying it.


Enabling
- talk about the config? Say you must contact us?

Editable content
- headers on all messages
- locked headers
- sensitive headers (can be edited but it's better not to as they might lead to unwanted behaviors)
- non-locked headers can be removed (and the removal can be undone)
- message bodies json/xml
- Message editor will show a warning is the edited message is an event (NServiceBus.MessageIntent == Publish)
- As soon as an edited message is retried the original message is marked as resolved
- if the edited message fails it'll be marked as edited, and a link to the original message is provided

Limitations
- The original failed message must be available when the edit+retry is processed
- no encrypted properties support
