## Receiving algorithm

Because of the way MSMQ API has been designed i.e. polling receive that throws an exception when timeout is reached the receive algorithm is more complex than for other polling-driven transports (such as [SQLServer](/nservicebus/sqlserver/)).

The main loop starts by subscribing to `PeekCompleted` event and calling the `BeginPeek` method. When a message arrives the event is raised by the MSMQ client API. The handler for this event starts a new receiving task and waits till this new task has completed its `Receive` call. After that is calls `BeginPeek` again to wait for more messages.
