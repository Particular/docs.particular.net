## Receiving algorithm

Because of how the MSMQ API has been designed, the receive algorithm is more complex than other polling-driven transports (such as [SQLServer](/transports/sql/)).

The main loop uses [`GetMessageEnumerator2`](https://msdn.microsoft.com/en-us/library/system.messaging.messagequeue.getmessageenumerator2) to iterate over all messages available in the queue and creates a separate receiving task for each message.
