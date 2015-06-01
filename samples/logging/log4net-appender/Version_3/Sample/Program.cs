using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Configure configure = Configure.With();
        #region ConfigureAppender
        configure.Log4Net<MyConsoleAppender>(appender => { appender.Color = ConsoleColor.Green; });
        #endregion
        configure.DefineEndpointName("Samples.Log4Net.Appender");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            bus.SendLocal(new MyMessage());

            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }

    }
}

