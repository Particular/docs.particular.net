The RavenDB persistence retains deduplication records for 7 days by default and runs the cleanup operation every minute.

These settings can be modified by specifying the desired values in the settings dictionary:

snippet: OutboxRavendBTimeToKeep

Starting with NServiceBus.RavenDB version 6.3, it is recommended to disable cleanup and rely on [document expiration](https://ravendb.net/docs/article-page/latest/csharp/server/extensions/expiration) instead.

Cleanup may be disabled by specifying `Timeout.InfiniteTimeSpan` for `SetFrequencyToRunDeduplicationDataCleanup`:

snippet: OutboxRavendBDisableCleanup

If document expiration cannot be used, to improve efficiency it is advised to run cleanup on only one endpoint instance per RavenDB database, by disabling clean up on all other endpoint instances.

WARN: If document expiration is not used when operating in [multi-tenant mode](/persistence/ravendb/#multi-tenant-support), cleanup must be handled manually, since NServiceBus is unaware of the databases in use.
