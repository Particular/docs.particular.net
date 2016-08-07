namespace Core5.Scheduling
{
    using System;
    using NServiceBus;

    class Scheduling
    {
        Scheduling(Schedule schedule, IBus bus)
        {
            #region ScheduleTask

            // 'Schedule' is an instance class that can be resolved from the container.
            // To send a message every 5 minutes
            schedule.Every(
                timeSpan: TimeSpan.FromMinutes(5),
                task: () =>
                {
                    var message = new CallLegacySystem();
                    bus.Send(message);
                });

            // Name a schedule task and invoke it every 5 minutes
            schedule.Every(
                timeSpan: TimeSpan.FromMinutes(5),
                name: "MyCustomTask",
                task: SomeCustomMethod);

            #endregion
        }

        void SomeCustomMethod()
        {
        }

    }

    class CallLegacySystem
    {
    }
}