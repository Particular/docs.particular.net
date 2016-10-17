using System;
using NServiceBus;
using NServiceBus.Installation.Environments;
using NServiceBus.Logging;

class Program
{
    static ILog log = LogManager.GetLogger(typeof(Program));

    static void Main()
    {
        Console.Title = "Samples.Scheduling";
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Scheduling");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());

            #region Schedule

            // Send a message every 5 seconds
            Schedule.Every(TimeSpan.FromSeconds(5))
                .Action(
                    task: () =>
                    {
                        var message = new MyMessage();
                        bus.SendLocal(message);
                    });

            // Name a schedule task and invoke it every 5 seconds
            Schedule.Every(TimeSpan.FromSeconds(5))
                .Action(
                    name: "MyCustomTask",
                    task: () =>
                    {
                        log.Info("Custom Task executed");
                    });

            #endregion

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

    }
}