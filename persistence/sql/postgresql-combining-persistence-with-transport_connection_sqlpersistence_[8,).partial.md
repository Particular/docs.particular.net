This behaviour may be disabled, to force the persistance to create its own connection, as it does when the outbox is enabled:

snippet: PostgreSqlDoNotShareConnection

> [!WARNING]
> In this mode NServiceBus does not guarantee *exactly-once* message processing behavior which means that saga message handling logic might be called multiple times for a single incoming message in case the previous processing attempts failed just before consuming the message.
