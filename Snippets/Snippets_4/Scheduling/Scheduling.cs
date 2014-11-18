using System;
using NServiceBus;

class Scheduling
{
    public void ScheduleTask()
    {
        IBus bus = null;
        #region ScheduleTaskV4

        // To send a message every 5 minutes
        Schedule.Every(TimeSpan.FromMinutes(5))
            .Action(() => bus.Send(new CallLegacySystem()));

        // Name a schedule task and invoke it every 5 minutes
        Schedule.Every(TimeSpan.FromMinutes(5))
            .Action("Task name", SomeCustomMethod);

        #endregion
    }

    void SomeCustomMethod()
    {
    }
}

class CallLegacySystem
{
}