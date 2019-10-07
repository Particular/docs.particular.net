## Additional exception data

Starting from NServiceBus version 7.2, exceptions from failing message handlers might contain additional error information in the `Exception.Data` property. Log4Net does not log this information by default, but can be configured to do so using a custom `PatternLayoutConverter`:

snippet: ExceptionDataConverter

The custom converter can then be registered and incorporated into the log layout:

snippet: RegisterConverter