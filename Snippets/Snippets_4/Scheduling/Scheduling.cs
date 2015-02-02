﻿using System;
using NServiceBus;

class Scheduling
{
    public void ScheduleTask()
    {
        IBus bus = null;

        #region ScheduleTask
        // 'Schedule' is a static class that can be accessed anywhere. 
        // To send a message every 5 minutes
        Schedule.Every(TimeSpan.FromMinutes(5))
            .Action(() => bus.Send(new CallLegacySystem()));

        // Name a schedule task and invoke it every 5 minutes
        Schedule.Every(TimeSpan.FromMinutes(5))
            .Action("Task name", SomeCustomMethod);

        #endregion
    }

    public void ScheduleUniqueTask()
    {
        #region ScheduleUniqueTask

        Schedule.Every(TimeSpan.FromMinutes(5))
            .UniqueAction("Unique task name", () => { });

        #endregion
    }


    void SomeCustomMethod()
    {
    }
}

class CallLegacySystem
{
}