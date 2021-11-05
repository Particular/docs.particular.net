The default limits of an endpoint can be changed in both code and via app.config.


### Via a IConfigurationProvider

snippet: TuningFromConfigurationProvider


### Via app.config

By using raw xml.

NOTE: It is not required to specificy both `MaximumConcurrencyLevel` and `MaximumMessageThroughputPerSecond`. Only add `MaximumMessageThroughputPerSecond` if the system actually needs to run slower.

snippet: TuningFromAppConfig
