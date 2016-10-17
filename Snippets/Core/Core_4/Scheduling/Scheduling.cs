namespace Core4.Scheduling
{
    using System;
    using NServiceBus;
    using NServiceBus.Logging;

    class Scheduling
    {
        static ILog log = LogManager.GetLogger(typeof(Scheduling));

        void ScheduleTask(IBus bus)
        {
            #region ScheduleTask

            // 'Schedule' is a static class that can be accessed anywhere.
            // To send a message every 5 minutes
            Schedule.Every(TimeSpan.FromMinutes(5))
                .Action(
                    task: () =>
                    {
                        var message = new CallLegacySystem();
                        bus.Send(message);
                    });

            // Name a schedule task and invoke it every 5 minutes
            Schedule.Every(TimeSpan.FromMinutes(5))
                .Action(
                    name: "MyCustomTask",
                    task: () =>
                    {
                        log.Info("Custom Task executed");
                    });

            #endregion
        }

    }

    class CallLegacySystem
    {
    }
}