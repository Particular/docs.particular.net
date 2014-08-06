using System;
using NServiceBus;

public class Scheduler
{
    #region SchedulerV4
    public class ScheduleMyTasks : IWantToRunWhenBusStartsAndStops
    {
        IBus bus;

        public ScheduleMyTasks(IBus bus)
        {
            this.bus = bus;
        }

        public void Start()
        {
            // To send a message every 5 minutes
            Schedule.Every(TimeSpan.FromMinutes(5))
                .Action(() => bus.SendLocal(new MyMessage()));

            // Name a schedule task and invoke it every 5 minutes
            Schedule.Every(TimeSpan.FromMinutes(5))
                .Action("Task name", () => bus.SendLocal(new MyMessage()));
        }

        public void Stop()
        {
        }
    }

    #endregion SchedulerV4

    public class MyMessage : IMessage
    {
    }
}