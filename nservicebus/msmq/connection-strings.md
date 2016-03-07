---
title: MSMQ connection strings
summary: Detailed connection string information for MSMQ.
tags:
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

 * `deadLetter`: The `deadLetter` configuration parameter enables or disables [dead letter queue](https://msdn.microsoft.com/en-us/library/ms706227.aspx) support, dead letter queues tracks messages that cannot be delivered. The default value is `true`.
 * `journal`: MSMQ supports the concept of journaling, when the journaling is active a copy of each sent message is stored in the [journal queue](https://msdn.microsoft.com/en-us/library/ms702011.aspx). The default value is `false`.
 * `useTransactionalQueues`: determines if the generated queues, or the existing ones, must be transactional queues or not. The default value is `true`. Setting this to `false` results in queues being created as non-transactional and messages being sent with [MessageQueueTransactionType.None](https://msdn.microsoft.com/en-us/library/system.messaging.messagequeuetransactiontype).
 * `cacheSendConnection`: instructs the underlying infrastructure to cache the connection to a remote queue and re-use it as needed instead of creating each time a new connection to send messages. The default value is `true`. This value is passed to the `enableCache` parameter of the [MessageQueue constructor](https://msdn.microsoft.com/en-us/library/ms143856) when sending a message.
 * `timeToReachQueue`: The time limit for the message to reach the destination queue, beginning from the time the message is sent. This sets the underlying [Message.TimeToReachQueue](https://msdn.microsoft.com/en-us/library/system.messaging.message.timetoreachqueue). Format must be compatible with [TimeSpan.Parse](https://msdn.microsoft.com/en-us/library/se73z7b9). **This setting is only available in Version 5.1+**.

NOTE: If the `deadletter` configuration is enabled and the receiving endpoint does not remove the messages from the endpoint queue, the messages will remain in the sender's queue as they are deemed "unprocessed".

NOTE: The MSMQ connection string and any of its settings are optional. It is not required to set all values when specifying the connection string.
