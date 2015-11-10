using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MessageBodyEncryption.Endpoint2");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.RegisterMessageEncryptor();
        busConfiguration.SendFailedMessagesTo("error");
        IStartableBus startableBus = Bus.Create(busConfiguration);
        using (await startableBus.StartAsync())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}