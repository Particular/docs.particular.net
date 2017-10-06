---
title: Writing a log entry
reviewed: 2017-10-06
component: Core
tags:
- Logging
redirects:
- nservicebus/logging-writing
---

Set up a single static field to an `ILog` in the classes, and then use it in all methods:

snippet: UsingLogging

NOTE: When writing to a logger, ensure the log level is set to a value that will result in that log entry being written. For example, when calling `.Debug(..)` ensure that the log level is set to DEBUG. See [Change settings via configuration](/nservicebus/logging/#changing-the-defaults).

NOTE: Since `LogManager.GetLogger(..);` is an expensive call, it is important that the field is `static` so that the call happens only once per class and has the best possible performance.
