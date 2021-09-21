---
title: Logging from the user code
reviewed: 2021-09-17
component: Core
redirects:
- nservicebus/logging-writing
---

While the use of [`Microsoft.Extensions.Logging`](/nservicebus/logging/extensions-logging.md) is recommended for new endpoints the NServiceBus logging abstraction can also be used for writing log messages from user code.

Set up a single static field to an `ILog` in the classes, and then use it in all methods:

snippet: UsingLogging

WARNING: Make sure that logging is correctly initialized before resolving the `ILog` instance. Not doing so can result in a logger using an incorrect configuration

NOTE: To avoid unnecessary processing, especially when logging more verbose messages, such as `Debug`, make sure to first check if logging at that level is enabled.

NOTE: Since `LogManager.GetLogger(..);` is an expensive call, it is important that the field is `static` so that the call happens only once per class and has the best possible performance.
