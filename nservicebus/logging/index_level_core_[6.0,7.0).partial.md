The supported logging levels are

 * Debug
 * Info
 * Warn
 * Error
 * Fatal


### Changing the Logging Level

Note: When logging level is defined in both `app.config` and code, the code configuration will be applied.

include: configurationWarning


#### Via config API

snippet: OverrideLoggingLevelInCode


#### Via app.config

snippet: OverrideLoggingDefaultsInAppConfig


#### Via IProvideConfiguration

snippet: LoggingThresholdFromIProvideConfiguration

