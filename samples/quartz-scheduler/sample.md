---
title: Quartz.NET Usage
summary: Using of Quartz.NET to send messages from within an NServiceBus endpoint.
reviewed: 2017-09-20
component: Core
related:
- nservicebus/messaging/timeout-manager
---

This sample illustrates the use of [Quartz.NET](https://www.quartz-scheduler.net/) to send messages from within an NServiceBus endpoint.

> Quartz.NET is a full-featured, open source job scheduling system that can be used from smallest apps to large scale enterprise systems.


Note: The approach used in this sample can mitigate some of the architectural drawback of the [Scheduler](/nservicebus/scheduling/). The scheduler is build on top of the [Timeout Manager](/nservicebus/messaging/timeout-manager.md) which leverages the queuing system to trigger scheduled actions. Under heavy load, and associated delay in processing of messages, there may be some disparity between the expected time of a scheduled action and actual execution time. 


## Running the project

 1. Start both the Scheduler and Receiver projects.
 1. At startup Scheduler will schedule a message send to Receiver every 3 seconds.
 1. Receiver will handle the message.


## Code Walk-through

INFO: This sample uses a pre-release package of Quartz version 3 to reduce the friction with the NServiceBus async messaging api.


### Logging

Quartz.NET uses [LibLog](https://github.com/damianh/LibLog). Since LibLog support the detection and utilization of [Serilog](https://serilog.net/), this sample use the [NServiceBus Serilog integration](/nservicebus/logging/serilog.md)

snippet: serilog

LibLog [supports many other common logging libraries](https://github.com/damianh/LibLog/wiki#transparent-logging-support). Or Quartz can be configured to use a custom logger. See [Adding logging in Quartz.NET](https://www.quartz-scheduler.net/documentation/quartz-3.x/quick-start.html#adding-logging).


### Context Helper

A helper to inject and extract the `IEndpointInstance` from the Quartz scheduler context.

snippet: QuartzContextExtensions

Quartz also support Dependency Injection (DI) via the [JobFactory API](https://www.quartz-scheduler.net/documentation/quartz-2.x/tutorial/miscellaneous-features.html).


### Configure and start the scheduler

The endpoint is started and the `IEndpointInstance` is injected into the Quartz scheduler context

snippet: Configuration


### Job definition

A Quartz `IJob` that sends a message to Receiver.

snippet: SendMessageJob

Note `QuartzContextExtensions` is used to get access to the `IEndpointInstance` .


### Schedule a job

snippet: scheduleJob


### Cleanup

The Quartz scheduler should be shutdown when the endpoint is shutdown.

snippet: shutdown

As such for cleanup purpose either a static variable may need to be kept or the shutdown done as part of the container cleanup.


## Further information on Quartz

 * [Quartz.NET Quick Start Guide](https://www.quartz-scheduler.net/documentation/quartz-3.x/quick-start.html)
 * [Quartz.NET Tutorial](https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/index.html)