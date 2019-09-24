snippet: OverrideLoggingLevelInCode

The same goal can be achieved by adding a configuration section:

snippet: OverrideLoggingDefaultsInAppConfig

or by adding a custom configuration provider:

snippet: LoggingThresholdFromIProvideConfiguration

Note: When logging threshold is defined in both configuration section and through `Use` method, the `Use` configuration takes precedence.

include: configurationWarning
