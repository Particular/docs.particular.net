using System;
using NServiceBus;

class Scheduling
{
    public void ScheduleTask()
    {
        Schedule schedule = null;
        IBus bus = null;

        #region ScheduleTask
        // 'Schedule' is an instance class that can be resolved from the container.
        // To send a message every 5 minutes
        schedule.Every(TimeSpan.FromMinutes(5), () => bus.Send(new CallLegacySystem()));

        // Name a schedule task and invoke it every 5 minutes
        schedule.Every(TimeSpan.FromMinutes(5), "Task name", SomeCustomMethod);

        #endregion
    }

    public void ScheduleUniqueTask()
    {
        Schedule schedule = null;

        #region ScheduleUniqueTask

        schedule.UniqueAction(TimeSpan.FromMinutes(5), "Unique task name", () => { });

        #endregion
    }

    void SomeCustomMethod()
    {
    }
}

class CallLegacySystem
{
}