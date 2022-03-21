## Timeouts persistence

`MsmqPersistence` provides persistence only for storing event subscriptions. By default, NServiceBus also requires a timeout persistence, which is used by [delayed retries](/nservicebus/recoverability/#delayed-retries), [saga timeouts](/nservicebus/sagas/timeouts.md) and for [delayed delivery](/nservicebus/messaging/delayed-delivery.md).

Version 2 of MSMQ Transport supports delayed delivery of messages by persisting them in a delayed message store. There is a built-in SQL Server-based store and an extention point to use a custom storage.

snippet: delayed-delivery
