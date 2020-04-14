## Receiving algorithm

Because of how the MSMQ API has been designed, i.e. polling receive that throws an exception when MSMQ timeout is reached, the receive algorithm is more complex than other polling-driven transports (such as [SQLServer](/transports/sql/)).

The main loop starts by subscribing to the `PeekCompleted` event and calling the `BeginPeek` method. When a message arrives, the event is raised by the MSMQ client API. The handler for this event starts a new receiving task and waits until this new task has completed its `Receive` call, after which it calls `BeginPeek` again to wait for more messages.
