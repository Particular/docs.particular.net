The RavenDB persistence retains deduplication records for 7 days by default and runs the cleanup operation every minute.

These settings can be modified by specifying the desired values in the settings dictionary:

snippet: OutboxRavendBTimeToKeep

Starting with NServiceBus.RavenDB version 7.0, cleanup is disabled by default and it is recommended to rely on [document expiration](https://ravendb.net/docs/article-page/latest/csharp/server/extensions/expiration) instead.

If document expiration cannot be used it is possible to enable the outbox purging by specifying:

snippet: OutboxRavendBEnableCleaner

If document expiration cannot be used, to improve efficiency it is advised to run cleanup on only one endpoint instance per RavenDB database, by explicitely disabling clean up on all other endpoint instances:

snippet: OutboxRavendBDisableCleanup

WARN: If document expiration is not used when operating in [multi-tenant mode](/persistence/ravendb/#multi-tenant-support), cleanup must be handled manually, since NServiceBus is unaware of the databases in use.
