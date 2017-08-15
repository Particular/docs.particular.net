using System;
using NServiceBus;
using NServiceBus.Logging;

#region Schedule

public class ScheduleStarter :
    IWantToRunWhenBusStartsAndStops
{
    static ILog log = LogManager.GetLogger<ScheduleStarter>();

    IBus bus;
    Schedule schedule;

    public ScheduleStarter(Schedule schedule, IBus bus)
    {
        this.schedule = schedule;
        this.bus = bus;
    }

    public void Start()
    {
        // Send a message every 5 seconds
        schedule.Every(
            TimeSpan.FromSeconds(5),
            () =>
            {
                var message = new MyMessage();
                bus.SendLocal(message);
            });

        // Name a schedule task and invoke it every 5 seconds
        schedule.Every(
            TimeSpan.FromSeconds(5),
            "MyCustomTask",
            () => { log.Info("Custom Task executed"); });
    }

    public void Stop()
    {
        //no-op
    }
}

#endregion