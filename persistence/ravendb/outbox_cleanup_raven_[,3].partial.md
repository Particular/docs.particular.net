The maximum value allowed for both properties is `int.MaxValue * 2` milliseconds, which is between 49 and 50 days.

The cleanup task can be disabled by specifying a value of `Timeout.InfiniteTimeSpan` for `SetFrequencyToRunDeduplicationDataCleanup`. This can be useful when an endpoint is scaled out and instances are competing to run the cleanup task.

NOTE: Regardless of the setting for `SetFrequencyToRunDeduplicationDataCleanup`, the cleanup process will always execute once a minute after the endpoint starts up. After that, the cleanup will run (or not run) according to the value of this property.

WARN: When running in [multi-tenant mode](/persistence/ravendb/#multi-tenant-support), cleanup must be handled maually since NServiceBus does not know what databases are in use.

NOTE: It is advised to run the cleanup task on only one NServiceBus endpoint instance per RavenDB database and disable the cleanup task on all other NServiceBus endpoint instances for the most efficient cleanup execution.