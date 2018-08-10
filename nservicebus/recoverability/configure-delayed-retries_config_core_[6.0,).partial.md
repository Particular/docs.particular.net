## Configuring delayed retries

 * `TimeIncrease`: Specified as a TimeSpan. Specifies the delay interval for each retry attempt. This delay increases by the same timespan with each delayed delivery. For example, if the specified value is ten seconds, i.e. 00:00:10, then the first delayed retry will be at ten seconds, the subsequent delayed retry will be twenty seconds, and so on.
 * `NumberOfRetries`: Number of times delayed retries are performed. Default: 3.

snippet: DelayedRetriesConfiguration
