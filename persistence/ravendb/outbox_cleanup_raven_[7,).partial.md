The RavenDB implementation by default keeps deduplication records for 7 days.

These default settings can be changed by specifying new defaults in the settings dictionary:

snippet: OutboxRavendBTimeToKeep

Starting with NServiceBus.RavenDB version 7.0, cleanup is disabled by default and it is recommended to rely on [document expiration](https://ravendb.net/docs/article-page/latest/csharp/server/extensions/expiration) instead.

If document expiration cannot be used it is possible to enable the outbox purging by specifying:

snippet: OutboxRavendBEnableCleaner

If document expiration cannot be used, to improve efficiency it is advised to run cleanup on only one endpoint instance per RavenDB database, by explicitely disabling clean up on all other endpoint instances:

snippet: OutboxRavendBDisableCleanup

WARN: If document expiration is not being used when running in [multi-tenant mode](/persistence/ravendb/#multi-tenant-support), cleanup must be handled manually, since NServiceBus does not know what databases are in use.
