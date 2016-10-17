namespace Core3.Scheduling
{
    using System;
    using log4net;
    using NServiceBus;

    class Scheduling
    {
        static ILog log = LogManager.GetLogger(typeof(Scheduling));

        Scheduling(IBus bus)
        {
            #region ScheduleTask

            // 'Schedule' is a static class that can be accessed anywhere.
            // To send a message every 5 minutes
            Schedule.Every(TimeSpan.FromMinutes(5))
                .Action(
                    task: () =>
                    {
                        var messages = new CallLegacySystem();
                        bus.Send(messages);
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