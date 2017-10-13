The default behavior is to stop the endpoint by lowering the number of worker threads to `0` using:

snippet: DefaultCriticalErrorAction

WARNING: This will cause the endpoint to stop processing messages until recreated and started again. The host process is not affected.