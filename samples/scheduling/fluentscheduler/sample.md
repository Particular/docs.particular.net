---
title: FluentScheduler Usage
summary: Using FluentScheduler to send messages from within an NServiceBus endpoint.
reviewed: 2017-09-20
component: Core
related:
- nservicebus/messaging/timeout-manager
- nservicebus/scheduling
---

This sample illustrates the use of [FluentScheduler](https://github.com/fluentscheduler/FluentScheduler) to send messages from within an NServiceBus endpoint.


include: scheduler-drawbacks


## Running the project

 1. Start both the Scheduler and Receiver projects.
 1. At startup Scheduler will schedule sending a message to Receiver every 3 seconds.
 1. Receiver will handle the message.


## Code Walk-through


### Configure and start the scheduler

The endpoint is started and the `IEndpointInstance` is stored in the static endpoint helper.

snippet: Configuration


### Job definition

snippet: SendMessageJob

FluentScheduler does not currently have native support for async. As such the job execution is blocked with `.GetAwaiter().GetResult()`.

[Dependency Injection is supported](https://github.com/fluentscheduler/FluentScheduler#dependency-injection) for more advanced scenarios. For example injecting `IEndpointInstance` into the `IJob` constructor.


### Cleanup

The `JobManager` should be cleaned up when the endpoint is shut down.

snippet: shutdown


### Logging

FluentScheduler exposes events that can be helpful to log.

snippet: logging


## Scale Out

Note that in this sample an instance of the FluentScheduler is configured to run in every endpoint's instance. If an endpoint is [scaled out](/transports/scale-out.md) then the configured jobs will be executed by each of the running instances. This behavior needs to be considered when architecting a solution that requires scale out. For example message de-duplication may be required, or only running the scheduler on a single endpoint instance.


## Further information on FluentScheduler

 * [FluentScheduler Usage](https://github.com/fluentscheduler/FluentScheduler#usage)