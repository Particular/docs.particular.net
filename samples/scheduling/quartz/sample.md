---
title: Quartz.NET Usage
summary: Using of Quartz.NET to send messages from within an NServiceBus endpoint.
reviewed: 2017-09-20
component: Core
related:
- nservicebus/messaging/timeout-manager
- nservicebus/scheduling
---

This sample illustrates the use of [Quartz.NET](https://www.quartz-scheduler.net/) to send messages from within an NServiceBus endpoint.

> Quartz.NET is a full-featured, open source job scheduling system that can be used from smallest apps to large scale enterprise systems.


include: scheduler-drawbacks


## Running the project

 1. Start both the Scheduler and Receiver projects.
 1. At startup Scheduler will schedule a message send to Receiver every 3 seconds.
 1. Receiver will handle the message.


## Code Walk-through

INFO: This sample uses a pre-release package of Quartz version 3 to reduce the friction with the NServiceBus async messaging api.


### Context Helper

A helper to inject and extract the `IEndpointInstance` from the Quartz scheduler context.

snippet: QuartzContextExtensions

Quartz also support Dependency Injection (DI) via the [JobFactory API](https://www.quartz-scheduler.net/documentation/quartz-2.x/tutorial/miscellaneous-features.html).


### Configure and start the scheduler

The endpoint is started and the `IEndpointInstance` is injected into the Quartz scheduler context.

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

For cleanup purpose either a static variable may need to be kept or the shutdown done as part of the container cleanup.


### Logging

Quartz.NET uses [LibLog](https://github.com/damianh/LibLog). Since LibLog support the detection and utilization of [Serilog](https://serilog.net/), this sample use the [NServiceBus Serilog integration](/nservicebus/logging/serilog.md)

snippet: serilog

LibLog [supports many other common logging libraries](https://github.com/damianh/LibLog/wiki#transparent-logging-support). Or Quartz can be configured to use a custom logger. See [Adding logging in Quartz.NET](https://www.quartz-scheduler.net/documentation/quartz-3.x/quick-start.html#adding-logging).


### Exception Handling

Quartz recommendations for [Handling Exceptions](https://www.quartz-scheduler.net/documentation/best-practices.html#throwing-exceptions):

> Every listener method should contain a try-catch block that handles all possible exceptions. If a listener throws an exception, it may cause other listeners not to be notified and/or prevent the execution of the job, etc.

In the catch of a job consider either implementing a [circuit breaker](https://en.wikipedia.org/wiki/Circuit_breaker_design_pattern) or delegating to [critical-errors](/nservicebus/hosting/critical-errors.md).


## Scale Out

When using the approach in the sample, it is important to note that there is an instance of the Quartz scheduler running in every endpoint instance. So if an endpoint is [scaled out](/transports/scale-out.md) the configured jobs will be executed in each of those running instances. A persistent [Quartz JobStore](https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/job-stores.html) can help manage the the Quartz scheduler shared state including jobs, triggers, calendars, etc.


## Further information on Quartz

 * [Quartz.NET Quick Start Guide](https://www.quartz-scheduler.net/documentation/quartz-3.x/quick-start.html)
 * [Quartz.NET Tutorial](https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/index.html)