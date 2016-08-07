namespace Core3.Scheduling
{
    using System;
    using NServiceBus;

    class Scheduling
    {
        Scheduling(IBus bus)
        {
            #region ScheduleTask

            // 'Schedule' is a static class that can be accessed anywhere.
            // To send a message every 5 minutes
            Schedule.Every(TimeSpan.FromMinutes(5))
                .Action(() =>
                {
                    var messages = new CallLegacySystem();
                    bus.Send(messages);
                });

            // Name a schedule task and invoke it every 5 minutes
            Schedule.Every(TimeSpan.FromMinutes(5))
                .Action("MyCustomTask", SomeCustomMethod);

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