
## Saga correlation

NOTE: As of Version 6 of NServiceBus, all correlated properties are unique by default so there is no longer a configuration setting.

One of the limitations of the RavenDB persistence is support for only one `[Unique]` property (a saga property which value is guaranteed to be unique across all sagas of this type). Because of that limitation advanced user can turn off the validation that ensures sagas are only being found by unique properties:

snippet: ravendb-persistence-stale-sagas

DANGER: This is a potentially dangerous feature that can result in multiple instances of saga being created instead of one in cases of high contention.