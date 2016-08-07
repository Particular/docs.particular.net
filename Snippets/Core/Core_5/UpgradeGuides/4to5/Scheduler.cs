namespace Core5.UpgradeGuides._4to5
{
    using System;
    using NServiceBus;

    public class Scheduler
    {
        #region 4to5Scheduler

        public class ScheduleMyTasks :
            IWantToRunWhenBusStartsAndStops
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
                schedule.Every(
                    timeSpan: TimeSpan.FromMinutes(5),
                    task: () =>
                    {
                        var message = new MyMessage();
                        bus.SendLocal(message);
                    });

                // Name a schedule task and invoke it every 5 minutes
                schedule.Every(
                    timeSpan: TimeSpan.FromMinutes(5),
                    name: "Task name",
                    task: () =>
                    {
                        var message = new MyMessage();
                        bus.SendLocal(message);
                    });
            }

            public void Stop()
            {
            }
        }

        #endregion

        public class MyMessage :
            IMessage
        {
        }
    }
}