The maximum value allowed for both properties is `int.MaxValue * 2` milliseconds, which is between 49 and 50 days.

The cleanup task can be disabled by specifying a value of `Timeout.InfiniteTimeSpan` for `SetFrequencyToRunDeduplicationDataCleanup`. This can be useful when an endpoint is scaled out and instances are competing to run the cleanup task.

NOTE: Regardless of the setting for `SetFrequencyToRunDeduplicationDataCleanup`, the cleanup process will always execute once a minute after the endpoint starts up. After that, the cleanup will run (or not run) according to the value of this property.