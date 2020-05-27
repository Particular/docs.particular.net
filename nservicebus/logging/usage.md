---
title: Logging from the user code
reviewed: 2019-07-29
component: Core
redirects:
- nservicebus/logging-writing
---

The NServiceBus logging abstractions can be used for writing log messages from the user code. This approach ensures that both NServiceBus and user code log messages are written to the same destinations.

Set up a single static field to an `ILog` in the classes, and then use it in all methods:

snippet: UsingLogging

WARNING: Make sure that logging is correctly initialized before resolving the `ILog` instance. Not doing so can result in a logger using an incorrect configuration

NOTE: To avoid unnecessary processing, especially when logging more verbose messages, such as `Debug`, make sure to first check if logging at that level is enabled.

NOTE: Since `LogManager.GetLogger(..);` is an expensive call, it is important that the field is `static` so that the call happens only once per class and has the best possible performance.