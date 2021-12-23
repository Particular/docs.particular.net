## Configuring delayed retries

 * `TimeIncrease`: Specified as a TimeSpan, defaults to 10 seconds. Specifies the delay interval for each retry attempt. This delay increases by the same timespan with each delayed delivery. For example, if the specified value is the default 10 seconds, i.e. 00:00:10, then the first delayed retry will be at ten seconds, the subsequent delayed retry will be 20 seconds, and so on.
 * `NumberOfRetries`: Number of times delayed retries are performed. Default is 3.

snippet: DelayedRetriesConfiguration
