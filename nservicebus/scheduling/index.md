---
title: Scheduling
summary: Schedule a task or an action/lambda, to be executed repeatedly at a given interval.
component: Core
related:
 - samples/scheduling
redirects:
- nservicebus/scheduling-with-nservicebus
reviewed: 2020-08-24
---

The Scheduler is a lightweight/non-durable API that helps schedule a task that needs to be executed repeatedly based on a specified interval. In order to benefit from NServiceBus features such as built in retries and forwarding to the error queue it's recommended that scheduled tasks only sends messages in order to perform the actual work. One example would be to query the database for orders that needs some action to be taken and emit individual messages for each order that is found.

{{WARNING:

Scheduling a task depends on [delayed delivery](/nservicebus/messaging/delayed-delivery.md). If the delayed delivery operation fails, the Scheduler will be interrupted and a `ScheduledTask` message will be forwarded to the error queue. When this happens the scheduled task will stop executing unless the `ScheduledTask` message is retried.

The Scheduler leverages the queuing system to trigger scheduled actions. Under heavy load there may be some disparity between the expected time of a scheduled action and execution time due to the delay between timeout messages being generated and processed. On approach to mitigating this behavior is to run the Scheduler in a dedicated endpoint so that appropriate resource allocation can put in place to ensure timely execution of scheduled actions. Or, alternatively, use a difference scheduling technology, for example:

 * A [.NET Timer](https://msdn.microsoft.com/en-us/library/system.threading.timer.aspx).
 * [NServiceBus Sagas](/nservicebus/sagas/)
 * [Quartz.NET](https://www.quartz-scheduler.net/). See [Quartz.NET Sample](/samples/scheduling/quartz/).
 * [Hangfire](https://www.hangfire.io/). See [Hangfire Sample](/samples/scheduling/hangfire/).
 * [FluentScheduler](https://github.com/fluentscheduler/FluentScheduler). See [FluentScheduler Sample](/samples/scheduling/fluentscheduler/).

}}


## How the Scheduler works

The scheduler holds a list of tasks scheduled in a non-durable in-memory dictionary that is scoped per endpoint instance.

When a new scheduled task is created it is given a unique ID and stored in the endpoint's in-memory dictionary. The ID for the task is sent in a message to the Timeout Manager, setting the message to be deferred with the specified time interval. When the specified time has elapsed, the Timeouts dispatcher returns the message containing the ID to the endpoint with the scheduled task ID. The endpoint then uses that ID to fetch and invoke the task from its internal list of tasks and executes it.


## Example usage

The difference between the following two examples is that in the latter a name is given for the task. The name can be used for logging.

snippet: ScheduleTask


## When not to use it

 * As soon as the task starts to get some branching logic (`if` or `switch` statements) or business logic is added, instead of a simple message send, consider moving to a [saga](/nservicebus/sagas) and using [saga timeouts](/nservicebus/sagas/timeouts.md).
 * Often, instead of polling for a certain state using the Scheduler API, it is more appropriate to publish an event when the expected state transition occurs, and the necessary action is then taken by a message handler which is subscribed to it.
 * When there are requirements that are not currently supported by the Scheduler API. For example, scaling out the  tasks, canceling or deleting a scheduled task, doing a side-by-side deployment of a scheduled task as outlined in the following section.

partial: limitations

## Exception Handling

When an exception is thrown inside a schedule callback, the exception will be logged as an error and the endpoint will **not** shutdown.
