If there are any connection problems with the timeout storage then by default NServiceBus waits for 2 minutes to allow the storage to come back online. If the problem is not resolved within that time frame, then a [Critical Error](/nservicebus/hosting/critical-errors.md) is raised.

The default wait time can be changed:

snippet: TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages

NOTE: The timeout manager polls every minute. This means it could take more time then the configured *TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages* value before an issue is detected.
