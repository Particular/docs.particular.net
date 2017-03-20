### Wait time

Overrides the default time to wait before triggering a circuit breaker that initiates the endpoint shutdown procedure in case of [repeated critical errors](/nservicebus/hosting/critical-errors.md).

The default value is 2 minutes.

snippet: sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker


### Pause Time

Overrides the default time to pause after a failure while trying to receive a message. The default value is 10 seconds.

snippet: sqlserver-PauseAfterReceiveFailure
