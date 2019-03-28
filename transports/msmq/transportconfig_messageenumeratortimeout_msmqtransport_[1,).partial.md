
### MessageEnumeratorTimeout

The amount of time to wait to peek the queues. Format must be compatible with [TimeSpan.Parse](https://msdn.microsoft.com/en-us/library/se73z7b9). The default is 1 second. Increasing the value can result in delays in shutting down of the endpoint. For example, a setting of 5 seconds for this value might delay the endpoint from shutting down for up to 5 seconds. **Version 1.0.1 and above** 
 
snippet: message-enumerator-timeout

