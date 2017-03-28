---
title: MSMQ Dead Letter Queues
summary: Controlling MSMQ Dead Letter Queue behavior
reviewed: 2017-02-13
tags:
 - Transport
---

[Dead Letter Queues](https://msdn.microsoft.com/en-us/library/ms706227.aspx) is a feature of MSMQ that tracks messages that are undeliverable, deleted, expired etc. NServiceBus endpoints will by default enable DLQ for all outgoing messages except messages that have a [Time To Be Received(TTBR)](/nservicebus/messaging/discard-old-messages.md) set. This avoids expired messages ending up either in the TDLQ or DLQ (if [non-transactional queues are used](/nservicebus/msmq/connection-strings.md)) wasting disk space on the machine when they time out.

DLQ can be disabled for the entire endpoint using the [MSMQ connection string](/nservicebus/msmq/connection-strings.md).

NOTE: If DLQ is enabled messages will remain in the senders outgoing queue until processed and count towards disk space quota on the senders machine.


partial: config