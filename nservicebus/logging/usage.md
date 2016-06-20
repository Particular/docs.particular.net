---
title: Writing a log entry
reviewed: 2016-03-17
tags:
- Logging
redirects:
- nservicebus/logging-writing
---

Writing to logging from code is straightforward. Set up a single static field to a `ILog` in the classes, and then use it in all methods, like this:

snippet:UsingLogging

NOTE: When writing to a logger ensure the log level is of an equivalent value that will result in that log entry being written. So for example when calling `.Debug(..)` ensure that the log level is set to DEBUG. See [Change settings via configuration](./#changing-settings-via-configuration).

NOTE: Since `LogManager.GetLogger(..);` is an expensive call it is important that the field is `static` so that the call only happens once per class and have the best possible performance.