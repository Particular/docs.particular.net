## Receiving algorithm

Because of the way MSMQ API has been designed the receive algorithm is more complex than for other polling-driven transports (such as [SQLServer](/nservicebus/sqlserver/)).

The main loop starts uses the `GetMessageEnumerator2` to iterate over message available in the queue and creates a separate receiving task for each message.