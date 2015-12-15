---
title: How logging works in ServiceInsight
summary: How logging works and how to log more verbosely if necessary.
tags:
- Logging
---

When launching ServiceInsight, you can see the Log Window. This window is like the Output window of your IDE and you can see some of the most important logs in the system without parsing log files and going through the usual hassles.

To keep the number of logs minimal and relevant, the Log Window relates mostly to HTTP operations and calls to ServiceControl, as due to the nature of the HTTP operations (timeouts, network issues, etc.) they can cause the most confusion.

You can see that all the HTTP communications with ServiceControl are logged; the request being sent, the parameters, and the request/response headers. Also if a request to ServiceControl fails, you can see it in red in the Logs window.

![Log Window](images/008-log-window.png)

The logging options such as Log Level and source of the log are currently not directly configurable from ServiceInsight. However, you can easily log more details, or log entries of other ServiceInsight components such as licensing, application startup, etc.

## Advanced logging

Under the hood, ServiceInsight uses [Log4net library](http://logging.apache.org/log4net/) and at startup looks for a file named "log4net.config" in the same folder as the application executable file. This is a regular [log4net configuration](http://logging.apache.org/log4net/release/manual/configuration.html) file and you can create one yourself or edit the one that comes with the application (but it is commented out for the most part).

A configuration file that logs everything (with log level = 'ALL') to a file named "Particular.ServiceInsight.txt" in the "Logs" folder would look like this:


```XML
<?xml version="1.0" encoding="utf-8" ?>
<log4net>
  <appender name="LogFileAppender" type="log4net.Appender.FileAppender">
    <file value="Logs\Particular.ServiceInsight.txt"/>
    <appendToFile value="true"/>
    <layout type="log4net.Layout.PatternLayout">
      <conversionPattern value="%date - [%-5level] - %logger - %message%newline"/>
    </layout>
  </appender>
  <root>
    <level value="ALL"/>
    <appender-ref ref="LogFileAppender" />
  </root>
</log4net>
```

NOTE: The use of the Log4net library is subject to change in future versions. Its usage is described here for advanced logging and debugging scenarios and should not be relied upon for any other purposes.
