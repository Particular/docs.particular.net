using System;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Logging;

#region Schedule

public class ScheduleStarter : IWantToRunWhenConfigurationIsComplete
{
    static ILog log = LogManager.GetLogger<ScheduleStarter>();
    Schedule schedule;
    IBus bus;

    public ScheduleStarter(Schedule schedule, IBus bus)
    {
        this.schedule = schedule;
        this.bus = bus;
    }

    public void Run(Configure config)
    {
        // Send a message every 5 seconds
        schedule.Every(
            timeSpan: TimeSpan.FromSeconds(5),
            task: () =>
            {
                var message = new MyMessage();
                bus.SendLocal(message);
            });

        // Name a schedule task and invoke it every 5 seconds
        schedule.Every(
            timeSpan: TimeSpan.FromSeconds(5),
            name: "MyCustomTask",
            task: () =>
            {
                log.Info("Custom Task executed");
            });
    }
}

#endregion