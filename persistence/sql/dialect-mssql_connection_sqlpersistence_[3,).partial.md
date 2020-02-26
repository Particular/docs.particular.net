When using the [outbox](/nservicebus/outbox/), SQL Persistence always opens its own connection. In order to force using a separate connection even when the [outbox](/nservicebus/outbox/) is disabled, use the following API:

snippet: MsSqlDoNotShareConnection

WARN: In this mode NServiceBus does not guarantee *exactly-once* message processing behavior which means that saga message handling logic might be called multiple times for a single incoming message in case the previous processing attempts failed just before consuming the message.
