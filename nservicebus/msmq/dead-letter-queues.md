---
title: MSMQ Dead Letter Queues
summary: Controlling MSMQ Dead Letter Queue behavior
reviewed: 2017-02-13
tags:
 - Transport
---

[Dead Letter Queues](https://msdn.microsoft.com/en-us/library/ms706227.aspx) is a feature of MSMQ that tracks messages that are undeliverable, deleted, expired etc. NServiceBus endpoints will by default enable DLQ for all outgoing messages except messages that have a [Time To Be Received(TTBR)](/nservicebus/messaging/discard-old-messages.md) set. This avoids expired messages ending up in the DLQ and wasting disk space on the machine where they timed out.

DLQ can be disabled for the entire endpoint using the [MSMQ connection string](/nservicebus/msmq/connection-strings.md).

NOTE: If DLQ is enabled messages will remain in the senders outgoing queue until processed and count towards disk space quota on the senders machine.

### Enabling DLQ for messages with TTBR

In Versions below 6.2 DLQ will be enabled by default for TTBR messages as well.

To opt-in to this behavior for Version 6.2 and higher use

snippet: msmq-dlq-for-ttbr-optin

