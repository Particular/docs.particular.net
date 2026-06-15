---
title: MSMQ Dead Letter Queues
summary: Controlling MSMQ Dead Letter Queue behavior
reviewed: 2026-06-15
component: MsmqTransport
redirects:
 - nservicebus/msmq/dead-letter-queues
---

[Dead Letter Queues](https://learn.microsoft.com/en-us/previous-versions/windows/desktop/legacy/ms706227(v=vs.85)) is a feature of MSMQ that tracks messages that are undeliverable, deleted, expired etc. NServiceBus endpoints will by default enable DLQ for all outgoing messages except messages that have a [Time To Be Received (TTBR)](/nservicebus/messaging/discard-old-messages.md) set. This avoids expired messages ending up either in the TDLQ or DLQ (if [non-transactional queues are used](/transports/msmq/connection-strings.md)) wasting disk space on the machine when they time out.

DLQ can be disabled for the entire endpoint using the [MSMQ connection string](/transports/msmq/connection-strings.md).

> [!NOTE]
> If DLQ is enabled, messages will remain in the sender's outgoing queue until processed and count towards disk space quota on the sender's machine.


partial: config


### Monitoring DLQ

MSMQ moves messages that cannot be delivered to their destination to the DLQ. This may be due to misconfiguration of [routing](/nservicebus/messaging/routing.md) or queues being purged. See [Dead-Letter Queues](https://learn.microsoft.com/en-us/previous-versions/windows/desktop/legacy/ms706227(v=vs.85)) for a comprehensive list of conditions under which messages may be moved to the DLQ.

It is very important to monitor the DLQ in order to detect potential routing configuration errors or other situations that may lead to messages being moved to the dead-letter queue.

> [!NOTE]
> While there is usually a central error queue managed by NServiceBus, each machine has a separate dead-letter queue. This means that the DLQ on each machine has to be monitored individually.


#### Reading messages from DLQ

The following addresses can be used to read messages from DLQ on a given machine:

```text
DIRECT=OS:{MACHINE-NAME}\SYSTEM$;DEADLETTER
DIRECT=OS:{MACHINE-NAME}\SYSTEM$;DEADXACT
```


#### Performance counters

The [**MSMQ Queue**](https://learn.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2008-R2-and-2008/cc771098(v=ws.10)) performance object contains counters that can be used to monitor the number of messages in various queues. Value of the **Messages in Queue** counter of the **Computer Queues** instance tracks the number of messages in the DLQ on a given machine.
