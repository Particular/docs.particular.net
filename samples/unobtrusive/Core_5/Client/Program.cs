using System;
using System.Text;
using NServiceBus;

class Program
{
    public static void Main()
    {
        Console.Title = "Samples.Unobtrusive.Client";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Unobtrusive.Client");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        var dataBus = busConfiguration.UseDataBus<FileShareDataBus>();
        dataBus.BasePath(@"..\..\..\DataBusShare\");
        var encryptionKey = Encoding.ASCII.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        busConfiguration.RijndaelEncryptionService("2015-10", encryptionKey);

        busConfiguration.ApplyCustomConventions();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            CommandSender.Start(bus);
        }
    }
}

