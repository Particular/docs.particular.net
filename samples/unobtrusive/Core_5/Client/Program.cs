using System;
using System.Text;
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
        byte[] encryptionKey = Encoding.ASCII.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        busConfiguration.RijndaelEncryptionService("2015-10", encryptionKey);

        busConfiguration.ApplyCustomConventions();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            CommandSender.Start(bus);
        }
    }
}

