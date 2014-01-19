---
title: Scheduling with NServiceBus
summary: Schedule a task or an action/lambda, to be executed repeatedly at a given interval.
originalUrl: http://www.particular.net/articles/scheduling-with-nservicebus
tags:
- Scheduler
createdDate: 2013-05-21T19:11:11Z
modifiedDate: 2013-07-28T23:43:01Z
authors: []
reviewers: []
contributors: []
---

With the NServiceBus scheduling capability you can schedule a task or an action/lambda, to be executed repeatedly in a given interval.

Use cases
---------

You could use the scheduler to repeatedly poll some folder on your HDD for a file, schedule a reoccurring re-index of a database in a multi-tenant environment, and so on.

Do not add any business logic in your tasks; instead, use
[sagas](sagas-in-nservicebus.md) .

Example usage
-------------



```C#
// To send a message every 5 minutes
Schedule.Every(TimeSpan.FromMinutes(5)).Action(() => 
{ 
  Bus.Send<CallLegacySystem>(); 
});

// Name a schedule task and invoke it every 5 minutes
Schedule.Every(TimeSpan.FromMinutes(5)).Action("Task name", () => {});
```



The difference between these examples is that in the latter a name is given for the task. The name is used for logging.

To schedule tasks when your hosts starts, implement the IWantToRunWhenTheBusStarts interface in version 3.0 and implement the IWantToRunWhenBusStartsAndStops in version 4.0.


```C#
public class ScheduleStartUpTasks : IWantToRunWhenBusStartsAndStops
{
  public void Start()
  {
    Schedule.Every(TimeSpan.FromMinutes(5)).Action(() =>
    Console.WriteLine("Another 5 minutes have elapsed."));
    Schedule.Every(TimeSpan.FromMinutes(3)).Action(
      "MyTaskName",() =>
      { 
        Console.WriteLine("This will be logged under MyTaskName’.");
      });
  }
  
  public void Stop()
  {
  }
}
```

 Implementation
--------------

The scheduler holds a list of tasks created with the scheduler using:


```C#
Schedule.Every(TimeSpan.FromMinutes(5)).Action(() => { < task to be executed > })
```

 When the task is created it is given an unique identifier. The identifier for the task is sent in a message to the Timeout Manager. When it times out and the Timeout Manager returns the message containing the identifier to the endpoint with the scheduled task, the endpoint uses that identifier to fetch and invoke the task from its internal list of tasks.

Assumptions
-----------

-   If the process restarts, all scheduled tasks are recreated and given
    new identifiers. Tasks scheduled before the restart will not be
    found and a message is written to the log. This is expected
    behavior.
-   Each task executes on a new thread using the
    Task.Factory.StartNew(Action) method, which means that there will be
    no transaction scope by default and it is up to you to create one if
    needed.
-   You will probably only do a Bus.Send()/Bus.SendLocal() in the task.
    The handler of that message will have the transaction as usual. It
    will run for ever.

When not to use it
------------------

You can look at a scheduled task as a simple never-ending saga. As soon as your task starts to get some logic (if-/switch-statements) you should consider moving to a [saga](sagas-in-nservicebus.md) .

