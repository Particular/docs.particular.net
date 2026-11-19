## Testing logging behavior

> [!WARNING]
> In NServiceBus version 10.2.0 and above, NServiceBus logging should be replaced by logging using Microsoft.Extensions.Logging `ILogger`.

To test that logging is performed correctly, use the `TestingLoggerFactory`. The factory writes to a `StringWriter` to allow unit tests to assert on log statements.

### Example

> [!NOTE]
> Using `WriteTo` or `Level` set the provided parameters to the statically cached factory for the lifetime of the application domain. For isolation of logging in concurrent scenarios it is recommended to use `BeginScope` that was introduced in Version 7.2.

The following code show how to verify that logging is performed by the message handler.

snippet: LoggerTestingSetup

The setup fixture above sets the testing logging factory once per assembly because the factory is statically cached during the lifetime of the application domain. Subsequent test executions then clear the logged statements before every test run as shown below.

snippet: LoggerTesting

Starting from Version 7.2 and above a scope can be defined for a user defined scope. Within that scope all the log statements will be collected on the text writer available within that scope.

snippet: LoggerTestingAmbient