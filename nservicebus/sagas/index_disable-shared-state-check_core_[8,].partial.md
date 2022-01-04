
NServiceBus will perform a check at startup to ensure that saga data types are not shared across sagas. An exception will be thrown at startup if any shared root types are found.

NOTE: Types used in properties that are shared between sagas are not included. Depending on the persister sharing types between saga root types can result in shared storage schema which should be avoided.

The startup check can be disabled by turning off the best practice validation:

snippet: disable-shared-state-validation
