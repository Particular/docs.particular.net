---
title: How logging works in ServiceInsight
summary: How logging works and how to log more verbosely if you need it.
tags: 
- Logging
---

One thing you can spot when launching ServiceInsight, is the Log Window. This window is more like Output window of your IDE and you can see some of the most important logs in the system, without going through log file parsing and hassles you usually would.

To keep the amount of logs minimal and relavant, it is related mostly to HTTP operations and calls to [ServiceControl](http://docs.particular.net/Search?q=ServiceControl), as due to nature of the HTTP operations (timeouts, network issues, etc.) it can cause the most confusion. 

You can see that we're logging all the HTTP communications with ServiceControl, the request being sent out, the parameters and the request/response headers are all logged. Also if a request to ServiceControl fails, you will be able to see that in red color in the Logs window.

![Log Window](008_logwindow.png)

The logging options, such as Log Level and source of the log, is currently not configurable from the ServiceInsight directly. So if need more details to be logged, or need log entries of some other ServiceInsight components, such as licensing, application startup, etc. you can still do this easily.

Advanced logging 
----------------

Under the hood, ServiceInsight uses [Log4net library](http://logging.apache.org/log4net/) and upon startup will look for a file named "log4net.config" to be there in the same folder as the application executable file. This is a regular [log4net configuration](http://logging.apache.org/log4net/release/manual/configuration.html) file and you can create one yourself or edit the one that comes with the application (but it is commented out for the most part).

A configuration file that would log everything (with log level = 'ALL') to a file named "Particular.ServiceInsight.txt" in "Logs" folder would look something like this:

 
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

NOTE: the use of Log4net library is subject to change in future versions. Its usage is described here for advanced logging and debugging scenarios and should not be relied on for any other purposes.
