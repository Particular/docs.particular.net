## Configuring Delayed Retries

 * `TimeIncrease`: Specified as a TimeSpan. Specifies the delay interval for each retry attempt. This delay increases by the same timespan with each delayed delivery. For example, if the specified value is 10 seconds, i.e. 00:00:10, then the first delayed retry will be at 10 seconds, the subsequent delayed retry will be 20 seconds and so on.
 * `NumberOfRetries`: Number of times Delayed Retries are performed. Default: 3.

snippet: DelayedRetriesConfiguration
