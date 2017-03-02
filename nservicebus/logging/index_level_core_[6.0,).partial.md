The supported logging levels are

 * Debug
 * Info
 * Warn
 * Error
 * Fatal


### Changing the Logging Level

Note: When logging level is defined in both app.config and code the code wins.

Warning: Configuration of logging via `app.config` is not recommended in Version 6, use the [code configuration API](#logging-levels-changing-the-logging-level-via-config-api) instead. The configuration section will be removed in Version 7.

#### Via config API

snippet:OverrideLoggingLevelInCode


#### Via app.config

snippet:OverrideLoggingDefaultsInAppConfig


#### Via IProvideConfiguration

snippet:LoggingThresholdFromIProvideConfiguration

