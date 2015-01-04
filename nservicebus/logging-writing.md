---
title: Writing a log entry
summary: How to write to logging
tags: 
- Logging
---

Writing to logging from your code is straightforward. Set up a single static field to a `ILog` in your classes, and then use it in all your methods, like this:

<!-- import UsingLogging -->
 

Note: Since `LogManager.GetLogger("Name");` is an expensive call it is important that the field is static so that the call only happens once per class.
