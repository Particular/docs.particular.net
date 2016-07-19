namespace Core4.UpgradeGuides._4to5
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

            public ScheduleMyTasks(IBus bus)
            {
                this.bus = bus;
            }

            public void Start()
            {
                // To send a message every 5 minutes
                Schedule.Every(TimeSpan.FromMinutes(5))
                    .Action(() =>
                    {
                        var myMessage = new MyMessage();
                        bus.SendLocal(myMessage);
                    });

                // Name a schedule task and invoke it every 5 minutes
                Schedule.Every(TimeSpan.FromMinutes(5))
                    .Action("Task name", () =>
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