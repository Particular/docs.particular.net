## Disabling delayed delivery

From version 9.1 and higher it is possible to disable the creation of the delayed delivery infrastructure. This can be useful in integration scenarios where the creation of the delayed delivery infrastructure is undesirable.

snippet: rabbitmq-disable-delayed-delivery

> [!WARNING]
> [Saga Timeouts](/nservicebus/sagas/timeouts.md) and [Delayed Retries](/nservicebus/recoverability/#delayed-retries) features are dependent on delayed delivery.
