using System;
using System.Diagnostics;
using NServiceBus;
using NServiceBus.ObjectBuilder;

public class SchedulerSample
{
    public class ScheduleMyTasks : IWantToRunWhenBusStartsAndStops
    {
        IBus bus;
        Schedule schedule;

        public ScheduleMyTasks(IBus bus, Schedule schedule)
        {
            this.bus = bus;
            this.schedule = schedule;
        }

        public void Start()
        {      
            // To send a message every 5 minutes
            schedule.Every(TimeSpan.FromMinutes(5), () => bus.SendLocal(new MyMessage()));

            // Name a schedule task and invoke it every 5 minutes
            schedule.Every(TimeSpan.FromMinutes(5), "Task name", () => Debug.WriteLine("Task executed"));

        }

        public void Stop()
        {
        }
    }
    public class MyMessage : IMessage
    {
    }
}
