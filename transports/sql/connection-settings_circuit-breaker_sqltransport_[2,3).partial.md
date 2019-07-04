### Wait time

The circuit breakers default time to wait before triggering is two minutes. Use the `TimeToWaitBeforeTriggeringCircuitBreaker` method to change it.

snippet: sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker


### Pause Time

The circuit breakers default time to pause after a failure to receive a message is ten seconds. Use the `PauseAfterReceiveFailure` method to change it.

snippet: sqlserver-PauseAfterReceiveFailure
