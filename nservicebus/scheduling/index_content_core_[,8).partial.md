DANGER: It is not recommended to use the NServiceBus Scheduler due to its [limitations](#current-limitations).

The scheduler is a lightweight/non-durable API that helps schedule a task that needs to be executed repeatedly at a specified interval. In order to benefit from NServiceBus features such as built-in retries and forwarding to the error queue, scheduled tasks should only `Send` or `SendLocal` a single message in order to perform the actual work. One example is to query the database for orders that need some action to be taken and emit individual messages for each order that is found.

{{WARNING:

Scheduling a task depends on [delayed delivery](/nservicebus/messaging/delayed-delivery.md). If the delayed delivery operation fails, the scheduler will be interrupted and a `ScheduledTask` message will be forwarded to the error queue. When this happens the scheduled task will stop executing unless the `ScheduledTask` message is retried or the endpoint instance is restarted.

Whenever execution history, or timely or fully-reliable scheduling is needed, it is recommended to use dedicated scheduling technology. For example:

* A [.NET Timer](https://msdn.microsoft.com/en-us/library/system.threading.timer.aspx).
* [NServiceBus Sagas](/nservicebus/sagas/)
* [Quartz.NET](https://www.quartz-scheduler.net/). See [Quartz.NET Sample](/samples/scheduling/quartz/).
* OS task scheduler, like the Windows task scheduler or Linux cron jobs.
* [Hangfire](https://www.hangfire.io/). See [Hangfire Sample](/samples/scheduling/hangfire/).
* [FluentScheduler](https://github.com/fluentscheduler/FluentScheduler). See [FluentScheduler Sample](/samples/scheduling/fluentscheduler/).

}}

## How the scheduler works

The scheduler holds a list of tasks scheduled in a non-durable in-memory dictionary that is scoped per endpoint instance.

When a new scheduled task is created it is given a unique ID and stored in the endpoint's in-memory dictionary. The ID for the task is sent in a message to the timeout manager, setting the message to be deferred with the specified time interval. When the specified interval has elapsed, the timeouts dispatcher returns the message containing the ID to the endpoint with the scheduled task ID. The endpoint then uses that ID to fetch and invoke the task from its internal list of tasks and executes it.

## Example usage

The difference between the following two examples is that in the latter a name is given for the task. The name can be used for logging.

snippet: ScheduleTask

## When not to use it

* As soon as the task starts to get some branching logic (`if` or `switch` statements) or business logic is added, consider moving to a [saga](/nservicebus/sagas) and using [saga timeouts](/nservicebus/sagas/timeouts.md) instead of a simple message send.
* Often, instead of polling for a certain state using the Scheduler API, it is more appropriate to publish an event when the expected state transition occurs, and the necessary action is then taken by a message handler which is subscribed to it.
* When there are requirements that are not currently supported by the Scheduler API. For example, scaling out the  tasks, canceling or deleting a scheduled task, doing a side-by-side deployment of a scheduled task as outlined in the following section.

## Current limitations

* Since the scheduler is non-durable, if the process restarts, all scheduled tasks (that are created during the endpoint's startup) are recreated and given new identifiers. Tasks scheduled before the restart that arrive at the endpoint queue will not be found although a message will be written to the log as information.
* Scheduled tasks, once created, cannot be canceled or changed.
* While the task repeat interval can be specified, setting a specific time for the task to run is not supported by the scheduler.
* The task is executed as part of the incoming message pipeline and the max duration limited by [the receive transaction timeout](/transports/transactions.md).
* If the time for a task is longer than the configured interval then the same action may be executed concurrently. If an action is not thread safe then the action needs to handle any synchronization logic, for example using a [semaphore](https://docs.microsoft.com/en-us/dotnet/api/system.threading.semaphore).
* Since the scheduler uses the queuing mechanism, it does have some side effects on the timelines of scheduled tasks. When a task is scheduled to be run at a given time it may not be executed at exactly that time, instead it becomes visible at that time and will be executed when it shows up in the queue. In most cases, this distinction will have no noticeable effect on the behavior of the scheduling API. However, in high load systems where the transport does not support native delayed delivery, the fact that a scheduled task is added to the back of the queue can result in a noticeable delay between the "time the task has been request to be run" and the "time the task is actually executed".
* The Scheduler API does not support scaling out the endpoint or doing a side-by-side deployment of an endpoint. When there are multiple instances of the endpoint that are running on the same machine, while using a non-broker transport such as MSMQ, or when there are scaling out the endpoint instances while using a broker transport such as RabbitMQ, these endpoint instances share the same input queue. Since each endpoint maintains its own created tasks in memory, when the specified time is up and the task is queued at the endpoint, any of the endpoint instances that are currently running can dequeue that message. If an endpoint that did not originally create this task happened to dequeue this message in order to execute it, it will not find the task in its list.

WARNING: This will result in the task not being executed but also not being rescheduled.

## Exception handling

When an exception is thrown inside a schedule callback, the exception will be logged as an error and the endpoint will **not** shutdown.
