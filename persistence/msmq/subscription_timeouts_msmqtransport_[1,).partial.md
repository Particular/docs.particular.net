## Timeouts persistence

`MsmqPersistence` provides persistence only for storing event subscriptions. By default, NServiceBus also requires a timeout persistence, which is used by [delayed retries](/nservicebus/recoverability/#delayed-retries), [saga timeouts](/nservicebus/sagas/timeouts.md) and for [delayed delivery](/nservicebus/messaging/delayed-delivery.md).

If none of these features are used, timeouts can be disabled:

snippet: DisablingTimeoutManagerForMsmqPersistence

NOTE: If timeouts are disabled, features such as delayed retries and saga timeouts cannot be used.

Another approach is to use a different persistence storage types for features other than subscriptions like shown below:

snippet: MsmqPersistenceWithOtherPersisters
