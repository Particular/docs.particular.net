---
title: MSMQ connection strings
summary: Detailed connection string information for MSMQ.
tags:
- NServiceBus
- Connection strings
- Transports
---

```xml
<connectionStrings>
   <!-- MSMQ -->
   <add name="NServiceBus/Transport"
        connectionString="deadLetter=true;
                          journal=true;
                          useTransactionalQueues=true;
                          cacheSendConnection=true"/>
</connectionStrings>
```

* `deadLetter`: The `deadLetter` configuration parameter enables or disables dead letter queue support, dead letter queues tracks messages that cannot be delivered (more information: https://msdn.microsoft.com/en-us/library/ms706227.aspx). The default value is `true`;
* `journal`: MSMQ supports the concept of journaling, when the journaling is active a copy of each sent message is stored in the journal queue (more information: https://msdn.microsoft.com/en-us/library/ms702011.aspx). The default value is `false`;
* `useTransactionalQueues`: determines if the generated queue, or the existing ones, must be transactional queues or not. The default value is `true`;
* `cacheSendConnection`: instructs the underlying infrastructure to cache the connection to a remote queue and re-use it as needed instead of creating each time a new connection to send messages. The default value is `true`;
* `timeToReachQueue`: The time limit for the message to reach the destination queue, beginning from the time the message is sent. **This setting is only available in v5.1+**;

NOTE: The MSMQ connection string is optional.
