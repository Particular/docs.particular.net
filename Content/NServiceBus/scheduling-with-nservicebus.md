---
title: Scheduling with NServiceBus
summary: Schedule a task or an action/lambda, to be executed repeatedly at a given interval.
tags:
- Scheduler
---

With the NServiceBus scheduling capability you can schedule a task or an action/lambda, to be executed repeatedly in a given interval.

## Use cases

You could use the scheduler to repeatedly poll some folder on your HDD for a file, schedule a reoccurring re-index of a database in a multi-tenant environment, and so on.

Do not add any business logic in your tasks; instead, use [sagas](sagas-in-nservicebus.md) .

## Example usage

The difference between these examples is that in the latter a name is given for the task. The name is used for logging.

### Version 5

`Schedule` is an instance class that can be resolved from the container.

<!-- import ScheduleTaskV5 -->

### Versions 3 and 4

`Schedule` is a static class that can access anywhere. 

<!-- import ScheduleTaskV4 -->

## Implementation

The scheduler holds a all list of tasks scheduled withing the current AppDomain.

When the task is created it is given an unique identifier. The identifier for the task is sent in a message to the Timeout Manager. When it times out and the Timeout Manager returns the message containing the identifier to the endpoint with the scheduled task, the endpoint uses that identifier to fetch and invoke the task from its internal list of tasks.

## Assumptions

- If the process restarts, all scheduled tasks are recreated and given new identifiers. Tasks scheduled before the restart will not be found and a message is written to the log. This is expected behavior.
- Each task executes on a new thread using the `Task.Factory.StartNew(Action)` method, which means that there will be no transaction scope by default and it is up to you to create one if needed.
- You will probably only do a `Bus.Send()` or `Bus.SendLocal()` in the task. The handler of that message will have the transaction as usual. It will run forever.

## When not to use it

You can look at a scheduled task as a simple never-ending saga. As soon as your task starts to get some branching logic (`if` or `switch` statements) you should consider moving to a [saga](sagas-in-nservicebus.md) .

