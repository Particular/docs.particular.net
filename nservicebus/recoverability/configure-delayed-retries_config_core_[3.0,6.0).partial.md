## Configuring delayed retries


### Common settings

 * `Enabled`: Turns the feature on and off. Default: true.
 * `TimeIncrease`: A time span after which the time between retries increases. Default: 10 seconds (`00:00:10`).
 * `NumberOfRetries`: Number of times delayed retries are performed. Default: 3.


### Using app.config

To configure delayed retries, enable its configuration section:

snippet: DelayedRetriesAppConfig


### Through IProvideConfiguration

snippet: DelayedRetriesProvideConfiguration


### Through ConfigurationSource

snippet: DelayedRetriesConfigurationSource

snippet: DelayedRetriesConfigurationSourceUsage