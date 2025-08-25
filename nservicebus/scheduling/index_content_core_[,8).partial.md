> [!CAUTION]
> The NServiceBus Scheduler is not recommended for most scenarios due to its [limitations](#current-limitations).

The NServiceBus Scheduler is a lightweight, non-durable API for scheduling tasks to execute at specified intervals. To leverage NServiceBus features such as built-in retries and error queue forwarding, scheduled tasks should only `Send` or `SendLocal` a single message to perform the actual work. For example, you might query a database for orders requiring action and emit individual messages for each order found.

> [!WARNING]
> The scheduler relies on [delayed delivery](/nservicebus/messaging/delayed-delivery.md). If delayed delivery fails, the scheduler is interrupted and a `ScheduledTask` message is sent to the error queue. In this case, the scheduled task will stop executing unless the message is retried or the endpoint is restarted.
>
> For requirements such as execution history, precise timing, or fully reliable scheduling, use dedicated scheduling technologies instead, such as:
>
> - **[System.Threading.Timer](https://msdn.microsoft.com/en-us/library/system.threading.timer.aspx):** Built-in .NET timer for recurring tasks.
> - **[NServiceBus Sagas](/nservicebus/sagas/):** For workflows and business logic with timeouts.
> - **[Quartz.NET](https://www.quartz-scheduler.net/):** Advanced job scheduling. See the [Quartz.NET Sample](/samples/scheduling/quartz/).
> - **Operating System Schedulers:** Windows Task Scheduler or Linux cron jobs.
> - **[Hangfire](https://www.hangfire.io/):** Background job processing for .NET. See the [Hangfire Sample](/samples/scheduling/hangfire/).

## How the scheduler works

The scheduler maintains a non-durable, in-memory dictionary of scheduled tasks per endpoint instance.

When a new task is scheduled:
- It receives a unique ID and is stored in the endpoint's in-memory dictionary.
- The ID is sent in a message to the timeout manager, which defers the message for the specified interval.
- When the interval elapses, the dispatcher returns the message to the endpoint, which uses the ID to fetch and execute the task.

## Example usage

The following examples show how to schedule a task, with and without a name for logging purposes:

snippet: ScheduleTask

## When not to use the Scheduler

Consider alternatives if:
- The task includes branching or business logic (use a [saga](/nservicebus/sagas) and [saga timeouts](/nservicebus/sagas/timeouts.md)).
- Polling can be replaced by publishing an event when a state transition occurs.
- You need features not supported by the Scheduler API, such as scaling out, canceling, or deleting scheduled tasks, or side-by-side deployments.

## Current limitations

- The scheduler is non-durable. After a process restart, all scheduled tasks are recreated with new IDs. Tasks scheduled before the restart that arrive at the queue will not be found, though a log entry is written.
- Scheduled tasks cannot be canceled or modified after creation.
- The scheduler supports specifying a repeat interval, but not a specific execution time.
- Tasks are executed as part of the incoming message pipeline, with a maximum duration limited by [the receive transaction timeout](/transports/transactions.md).
- If a task's execution time exceeds the configured interval, the same action may run concurrently. Non-thread-safe actions must handle synchronization, for example, using a [semaphore](https://docs.microsoft.com/en-us/dotnet/api/system.threading.semaphore).
- The scheduler's reliance on the queuing mechanism means there can be slight delays in task execution, especially in high-load systems or when using transports without native delayed delivery.
- The Scheduler API does not support scaling out the endpoint or side-by-side deployments. In scenarios with multiple endpoint instances (e.g., on the same machine with MSMQ or RabbitMQ), all instances share the same input queue. Since each endpoint maintains its own tasks in memory, a message dequeued by an endpoint instance that did not create the task will result in the task not being found.

> [!WARNING]
> This will result in the task not being executed but also not being rescheduled.

## Exception handling

When an exception is thrown inside a schedule callback, the exception will be logged as an error and the endpoint will **not** shutdown.
