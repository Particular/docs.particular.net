using NServiceBus;

class Program
{
    public static void Main()
    {
        var busConfiguration = new BusConfiguration();

        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseDataBus<FileShareDataBus>()
            .BasePath(@"..\..\..\DataBusShare\");
        busConfiguration.RijndaelEncryptionService();

        busConfiguration.ApplyCustomConventions();

        var bus = Bus.Create(busConfiguration);
        bus.Start();
        CommandSender.Start(bus);
    }
}

