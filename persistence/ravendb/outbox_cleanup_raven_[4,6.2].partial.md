The cleanup task can be disabled by specifying a value of `Timeout.InfiniteTimeSpan` for `SetFrequencyToRunDeduplicationDataCleanup`. This can be useful when an endpoint is scaled out and instances are competing to run the cleanup task.

WARN: When running in [multi-tenant mode](/persistence/ravendb/#multi-tenant-support), cleanup must be handled manually since NServiceBus does not know what databases are in use.

NOTE: It is advised to run the cleanup task on only one NServiceBus endpoint instance per RavenDB database and disable the cleanup task on all other NServiceBus endpoint instances for the most efficient cleanup execution.
