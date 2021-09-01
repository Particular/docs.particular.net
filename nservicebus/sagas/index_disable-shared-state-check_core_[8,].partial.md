
NServiceBus will perform a check at startup to ensure that saga data types are not shared across by sagas. An exception will be thrown at startup if any shared types are found.

The startup check can be disabled by disabling the best practice validation:

snippet: disable-shared-state-validation