using System;
using NServiceBus;

class Program
{
    public static void Main()
    {
        Console.Title = "Samples.Unobtrusive.Client";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Unobtrusive.Client");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseDataBus<FileShareDataBus>()
            .BasePath(@"..\..\..\DataBusShare\");
        busConfiguration.RijndaelEncryptionService("gdDbqRpqdRbTs3mhdZh8qCaDaxJXl+e7");

        busConfiguration.ApplyCustomConventions();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            CommandSender.Start(bus);
        }
    }
}

