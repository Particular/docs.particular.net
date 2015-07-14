---
title: Routing to CommonLogging
summary: Route all NServiceBus log entries to CommonLogging
tags: 
- Common Logging
related:
- samples/logging/commonlogging
---

Support for routing log entries to CommonLogging is compatible with NServiceBus 5 and higher.

There is a [nuget](https://www.nuget.org/packages/NServiceBus.CommonLogging/) package available that allows for simple integration of NServiceBus and [CommonLogging](http://netcommon.sourceforge.net/).


## Usage


### Pull in the nugets

    Install-Package NServiceBus.CommonLogging

This has a dependency on the `Common.Logging` nuget so that will automatically be pulled in.


### The code

<!-- import CommonLoggingInCode -->