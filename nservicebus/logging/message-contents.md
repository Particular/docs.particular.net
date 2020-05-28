---
title: Logging message contents
summary: How to output message contents to the log
reviewed: 2019-07-22
component: Core
redirects:
- nservicebus/logging-message-contents
---

When NServiceBus sends a message, it writes the result of the `ToString()` method of the message class to the log. By default, this writes the name of the message type only. To write the full message contents to the log, override the `ToString()` method of the relevant message class. Here's an example:

snippet: MessageWithToStringLogged

NOTE: NServiceBus only makes these calls at a log threshold of DEBUG or lower.