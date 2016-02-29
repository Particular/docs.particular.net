using NServiceBus;

class Program
{
    public static void Main()
    {
        Console.Title = "Samples.Unobtrusive.Server";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Unobtrusive.Server");
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

