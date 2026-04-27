Configuring the global threshold to one of the levels described above means that all messages below that level are discarded. For example setting the threshold value to `Warn` means that only `Warn`, `Error` and `Fatal` messages are written.

The `LogManager` class is the entry point for the logging configuration. If needed, it allows using custom logging integrations (see below). It also allows customization of the default built-in logging. The `Use` generic method returns the `LoggingFactoryDefinition`-derived object that provides the customization APIs.

snippet: OverrideLoggingLevelInCode
