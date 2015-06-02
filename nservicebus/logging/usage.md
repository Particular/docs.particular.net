---
title: Writing a log entry
summary: How to write to logging
tags: 
- Logging
redirects:
- nservicebus/logging-writing
---

Writing to logging from your code is straightforward. Set up a single static field to a `ILog` in your classes, and then use it in all your methods, like this:

<!-- import UsingLogging -->


NOTE: Make sure that the log level is set to DEBUG when calling `.Debug(..)` or this log statement will not be added to the log file. See [Change settings via configuration](logging#changing-settings-via-configuration).

NOTE: Since `LogManager.GetLogger(..);` is an expensive call it is important that the field is `readonly static` so that the call only happens once per class and have the best possible performance.
