

## Suppress scanning exceptions

NOTE: This configuration option is only available in NServiceBus 6.2 and above.

By default, exceptions occurred during assembly scanning will be re-thrown. Those exceptions can be ignored using the following:

snippet: SwallowScanningExceptions

WARNING: Ignoring assembly scanning exceptions can cause the endpoint to not load some features, behaviors, messages or message handlers and cause incorrect behavior.