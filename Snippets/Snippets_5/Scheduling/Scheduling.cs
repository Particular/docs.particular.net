using System;
using NServiceBus;

class Scheduling
{
    public void ScheduleTask()
    {
        Schedule schedule = null;
        IBus bus = null;
        #region ScheduleTaskV5

        // To send a message every 5 minutes
        schedule.Every(TimeSpan.FromMinutes(5), () => bus.Send(new CallLegacySystem()));

        // Name a schedule task and invoke it every 5 minutes
        schedule.Every(TimeSpan.FromMinutes(5), "Task name", SomeCustomMethod);

        #endregion
    }

    void SomeCustomMethod()
    {
    }

}
class CallLegacySystem
{
}