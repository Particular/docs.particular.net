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
                schedule.Every(TimeSpan.FromMinutes(5), () =>
                {
                    var myMessage = new MyMessage();
                    bus.SendLocal(myMessage);
                });

                // Name a schedule task and invoke it every 5 minutes
                schedule.Every(TimeSpan.FromMinutes(5), "Task name", () =>
                {
                    var myMessage = new MyMessage();
                    bus.SendLocal(myMessage);
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