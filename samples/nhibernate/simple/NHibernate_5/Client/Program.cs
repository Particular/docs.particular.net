using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.NHibernate.Client";

        Configure.Serialization.Json();

        var busConfiguration = Configure.With();
        busConfiguration.DefaultBuilder()
            .DefineEndpointName("Samples.NHibernate.Client")
            .UseNHibernateSagaPersister()
            .UseNHibernateTimeoutPersister()
            .UseNHibernateSubscriptionPersister()
            .UnicastBus();

        var startableBus = busConfiguration.CreateBus();
        var bus = startableBus.Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());

        Console.WriteLine("Press 'enter' to send a StartOrder messages");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }

            var orderId = Guid.NewGuid();
            var startOrder = new StartOrder
            {
                OrderId = orderId
            };

            bus.Send(startOrder);
            Console.WriteLine($"StartOrder Message sent with OrderId {orderId}");
        }
    }
}