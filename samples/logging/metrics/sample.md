---
title: Write metrics to the trace log
component: Metrics
reviewed: 2017-10-06
related:
 - monitoring/metrics
redirects:
 - samples/metrics/tracing
---


Illustrates how to write [metrics](/monitoring/metrics) information to the trace log.

There are many ways to look at the trace log for example via the [DbgView utility provided by Sysinternals](https://docs.microsoft.com/en-us/sysinternals/downloads/debugview)

![DbgView by Sysinternals](dbgview.png)

Note: This shows the output of NServiceBus.Metrics 3.x variant.

## Running the sample

 1. Run the solution in the Visual Studio debugger. A console application will start.
 1. Press the 'enter' key to send a message.
 1. Check the debug output window for metric information being written to the trace log.


## Sending metric data to Trace Log

snippet: EnableMetricTracing