The default limits of an endpoint can be changed in both code and via app.config.


### Via a IConfigurationProvider

snippet: TuningFromConfigurationProvider


### Via app.config

By using raw xml.

snippet: TuningFromAppConfig


## Run time settings

Version 5 and below allowed both concurrency and throughput throttling to be changed and read at run time using the code below.


## Optimizing at run time

snippet: ChangeTuning


## Reading current values at run time

snippet: ReadTuning