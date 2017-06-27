## Timeouts persistence

`MsmqPersistence` provides persistence only for storing event subscriptions. By default NServiceBus also requires a timeout persistence, which is used by [Delayed Retries](/nservicebus/recoverability/#delayed-retries), [Saga timeouts](/nservicebus/sagas/timeouts.md) and for [Delayed Delivery](/nservicebus/messaging/delayed-delivery.md).

If none of these features are used then timeouts can be disabled:

snippet: DisablingTimeoutManagerForMsmqPersistence

NOTE: If timeouts are disabled then features such as Delayed Retries and Saga timeouts cannot be used.

Another approach is to use a different persistence for features different than event subscriptions:

snippet: MsmqPersistenceWithOtherPersisters
