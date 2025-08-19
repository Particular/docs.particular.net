This behavior may be disabled, to force the persistance to create its own connection, as it does when the outbox is enabled:

snippet: PostgreSqlDoNotShareConnection

> [!WARNING]
> In this mode NServiceBus does not guarantee *exactly-once* message processing behavior. A single incoming message could result in multiple calls of the saga message handling logic, if the previous processing attempt failed just before consuming the message.
