The supported Levels are defined by the [Log4Net.Level](https://logging.apache.org/log4net/release/sdk/index.html) class.


### Changing the Logging Level

Note: When logging level is defined in both app.config and code the app.config wins.


#### Via app.config

snippet: OverrideLoggingDefaultsInAppConfig


#### Via IProvideConfiguration

snippet: LoggingThresholdFromIProvideConfiguration


#### Via config API

This can be achieved by taking full control over the [Log4Net integration](log4net.md).