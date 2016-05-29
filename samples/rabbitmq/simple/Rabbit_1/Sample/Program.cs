using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.RabbitMQ.Simple";
        Configure.Serialization.Json();
        #region ConfigureRabbit
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.RabbitMQ.Simple");
        configure.DefaultBuilder();
        configure.UseTransport<NServiceBus.RabbitMQ>(() => "host=localhost");
        #endregion
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            var myMessage = new MyMessage();
            bus.SendLocal(myMessage);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

    }

}