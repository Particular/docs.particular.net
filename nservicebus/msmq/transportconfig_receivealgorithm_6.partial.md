## Receiving algorithm

Because of the way MSMQ API has been designed the receive algorithm is more complex than for other polling-driven transports (such as [SQLServer](/nservicebus/sqlserver/)).

The main loop uses [`GetMessageEnumerator2`](https://msdn.microsoft.com/en-us/library/system.messaging.messagequeue.getmessageenumerator2) to iterate over message available in the queue and creates a separate receiving task for each message.