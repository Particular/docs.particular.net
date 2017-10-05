The default behavior is to stop the endpoint by lowering the number of worker threads to `0` using:

snippet: DefaultCriticalErrorAction

WARN: This will cause the enpoint to stop processing messages until recreated and started again. The host process is not affected.