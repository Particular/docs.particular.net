If there are any problems with timeout storage by default a wait of 2 minutes is done to allow the storage to come back online. If the problem is not resolved within that time frame, a [Critical Error](/nservicebus/hosting/critical-errors.md) is raised.

To change the default wait time:

snippet:TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages
