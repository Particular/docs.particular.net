---
title: Hangfire Usage
summary: Using of Hangfire to send messages from within an NServiceBus endpoint.
reviewed: 2017-09-20
component: Core
related:
- nservicebus/messaging/timeout-manager
- nservicebus/scheduling
---

This sample illustrates the use of [Hangfire](https://www.hangfire.io/) to send messages from within an NServiceBus endpoint.

> Hangfire - An easy way to perform background processing in .NET and .NET Core applications. Hangfire is an open-source framework that helps you to create, process and manage your background jobs.



include: scheduler-drawbacks


## Running the project

 1. Start both the Scheduler and Receiver projects.
 1. At startup Scheduler will schedule sending a message to Receiver every minute.
 1. Receiver will handle the message.


## Code Walk-through

### Endpoint Helper

This is a helper class used to make the NServiceBus `IEndpointInstance` available inside Hangfire jobs. In this sample it is implemented as a static property.

snippet: EndpointInstance

Hangfire also supports Dependency Injection (DI) via the [JobActivator API](http://docs.hangfire.io/en/latest/background-methods/using-ioc-containers.html) for more advanced scenarios.


### Configure and start the scheduler

The endpoint is started and the `IEndpointInstance` is stored in the static endpoint helper.

This sample uses in-memory storage for the jobs. Production scenarios should use more robust alternatives: SqlServer, Msmq or Redis.

Hangfire calls their scheduler a [BackgroundJobServer](http://docs.hangfire.io/en/latest/background-processing/processing-background-jobs.html). It is started automatically when an instance of the `BackgroundJobServer` class is instantiated.

snippet: Configuration


### Job definition

This sample passes in an expression pointing to the static `Run` method in `SendMessageJob`.

snippet: SendMessageJob

Note that the `EndpointHelper` is used by the job to get access to the `IEndpointInstance` .


### Schedule a job

Hangfire will accept any lambda expression as a job definition. 

The expression is serialized, stored and scheduled for execution by the `BackgroundJobServer` in Hangfire.

The schedule is set using [Cron](https://en.wikipedia.org/wiki/Cron) syntax through the `Cron` class. In this sample the job gets scheduled to run every minute.

snippet: scheduleJob


### Cleanup

The Hangfire server should be disposed when the endpoint is shut down.

snippet: shutdown

The Hangfire scheduler implements the `IDisposable` interface. For cleanup purposes, keep a reference to the scheduler instance and call `Dispose()` at shutdown. Alternatively, let a container do this as part of the container cleanup procedure.


### Logging

Hangfire uses [LibLog](https://github.com/damianh/LibLog). Since LibLog supports the detection and utilization of [Serilog](https://serilog.net/), this sample uses the [NServiceBus Serilog integration](/nservicebus/logging/serilog.md)

snippet: serilog

LibLog [supports many other common logging libraries](https://github.com/damianh/LibLog/wiki#transparent-logging-support). Hangfire can also be configured to use a custom logger. See [Adding logging in Hangfire](http://docs.hangfire.io/en/latest/configuration/configuring-logging.html) article for more details.


## Scale Out

Note that in this sample an instance of the Hangfire scheduler is configured to run in every endpoint's instance. If an endpoint is [scaled out](/transports/scale-out.md) then the configured jobs will be executed by each of the running instances. A persistent [job storage](http://docs.hangfire.io/en/latest/configuration/index.html) can help  to manage the Hangfire scheduler shared state, including jobs, triggers, calendars, etc.


## Further information on Hangfire

 * [Hangfire Quick Start Guide](http://docs.hangfire.io/en/latest/quick-start.html)
 * [Hangfire Tutorials](http://docs.hangfire.io/en/latest/tutorials/index.html)
