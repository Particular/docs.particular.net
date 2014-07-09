using System;
using System.Diagnostics;
using NServiceBus;

public class SchedulerSample
{
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
                .Action("Task name", () => Debug.WriteLine("Task executed"));
        }

        public void Stop()
        {
        }
    }

    public class MyMessage : IMessage
    {
    }
}