If there are any connection problems with the timeout storage then by default NServiceBus waits for 2 minutes to allow the storage to come back online. If the problem is not resolved within that time frame, then a [Critical Error](/nservicebus/hosting/critical-errors.md) is raised. 

The default wait time can be changed:

snippet: TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages

NOTE: The timeout manager polls every minute which means that in the worst case scenario it can take almost 3 minutes (1 minute poll interval + 2 minute for circuit breaker) before the endpoint will raise a critical error. However, on average for any configured *TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages* value it will be actually about 30 seconds more.
