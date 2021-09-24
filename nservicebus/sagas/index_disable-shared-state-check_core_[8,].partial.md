
NServiceBus will perform a check at startup to ensure that saga data types are not shared across sagas. An exception will be thrown at startup if any shared types are found.

The startup check can be disabled by turning off the best practice validation:

snippet: disable-shared-state-validation