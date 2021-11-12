The default limits of an endpoint can be changed in both code and via app.config.


### Via a IConfigurationProvider

snippet: TuningFromConfigurationProvider


### Via app.config

By using raw xml.

NOTE: It is not required to specificy both `MaximumConcurrencyLevel` and `MaximumMessageThroughputPerSecond`. Set `MaximumMessageThroughputPerSecond` only if the system needs to run slower.

snippet: TuningFromAppConfig
