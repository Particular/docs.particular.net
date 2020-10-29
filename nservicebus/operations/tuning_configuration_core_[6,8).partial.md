The default concurrency settings of an endpoint can be changed via code:

snippet: TuningFromCode

### TimeoutManager satellite queues

Depending on the chosen transport, additional queues (satellite queues) may be used to handle deferred messages like delayed retries or timeouts. Satellite queues use the default concurrency configuration. This concurrency setting can be configured using:

snippet: TuningTimeoutManagerConcurrency