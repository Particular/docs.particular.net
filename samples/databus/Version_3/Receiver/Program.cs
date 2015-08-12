using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static string BasePath = "..\\..\\..\\storage";

    static void Main()
    {
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Sample.DataBus.Receiver");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        configure.FileShareDataBus(BasePath);
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}