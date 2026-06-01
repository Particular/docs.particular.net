
## TimeoutManager satellite queues

Depending on the chosen transport, additional queues (satellite queues) might handle deferred messages such as delayed retries or timeouts. Satellite queues use the default concurrency configuration. You can configure this setting using:

snippet: TuningTimeoutManagerConcurrency
