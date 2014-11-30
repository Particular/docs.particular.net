---
title: Scheduling with NServiceBus
summary: Schedule a task or an action/lambda, to be executed repeatedly at a given interval.
tags:
- Scheduler
---

With the NServiceBus scheduling capability you can schedule a task to be executed repeatedly in a given interval.

## Use cases

You could use the scheduler to repeatedly poll some folder on your HDD for a file, schedule a reoccurring re-index of a database in a multi-tenant environment, and so on.

Do not add any business logic in your tasks; instead, use [sagas](sagas-in-nservicebus.md).

### Push versus Poll

In the scenario where you are "polling" for some change in information state you should instead consider a "push" approach.

The scheduling API is often used to poll state, for example the file system or an external webservice. In these scenarios it is much more efficient to adopt a push based model. In the case of the file system you should instead consider a [FileSystemWatcher](http://msdn.microsoft.com/en-us/library/system.io.filesystemwatcher.aspx). In the case a webservice should instead consider a [WebHook](http://en.wikipedia.org/wiki/Webhook).

## Example usage

The difference between these examples is that in the latter a name is given for the task. The name is used for logging.

### Version 5

`Schedule` is an instance class that can be resolved from the container.

<!-- import ScheduleTaskV5 -->

### Versions 3 and 4

`Schedule` is a static class that can be accessed anywhere. 

<!-- import ScheduleTaskV4 -->

## Accuracy 

The scheduling infrastructure leverages the reliable messaging approach at the core of NServiceBus. This allows scheduling to include features such as the error queue and retries. This however does have some side effects on the timeliness of scheduled tasks. When a task is scheduled to be run at a given time it is actual not "executed at that time", it is instead "queued at that time". In most cases this distinction will have no noticeable effect on the behavior of the the scheduling API. However in high load systems the fact that a scheduled task is added to the back of the queue can result in a noticeable delay between the "time the task has been request to be run" and the "time the task is actually executed".

## Implementation

The scheduler holds a all list of tasks scheduled. In version 3 and 4 tasks are scoped per `AppDomain`. In version 5 they are scoped per Bus instance.

When the task is created it is given an unique identifier. The identifier for the task is sent in a message to the Timeout Manager. When it times out and the Timeout Manager returns the message containing the identifier to the endpoint with the scheduled task, the endpoint uses that identifier to fetch and invoke the task from its internal list of tasks.

## Assumptions

- If the process restarts, all scheduled tasks are recreated and given new identifiers. Tasks scheduled before the restart will not be found and a message is written to the log. This is expected behavior.
- Each task executes on a new thread using the `Task.Factory.StartNew(Action)` method, which means that there will be no transaction scope by default and it is up to you to create one if needed.
- You will probably only do a `Bus.Send()` or `Bus.SendLocal()` in the task. The handler of that message will have the transaction as usual. It will run forever.

## When not to use it

You can look at a scheduled task as a simple never-ending saga. As soon as your task starts to get some branching logic (`if` or `switch` statements) you should consider moving to a [saga](sagas-in-nservicebus.md) .

