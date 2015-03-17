using NServiceBus;

class Program
{
    public static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseDataBus<FileShareDataBus>()
            .BasePath(@"..\..\..\DataBusShare\");
        busConfiguration.RijndaelEncryptionService();

        busConfiguration.ApplyCustomConventions();

        IStartableBus bus = Bus.Create(busConfiguration);
        bus.Start();
        CommandSender.Start(bus);
    }
}

