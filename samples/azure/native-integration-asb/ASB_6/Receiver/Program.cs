using System;
using System.IO;
using NServiceBus;
using NServiceBus.Azure.Transports.WindowsAzureServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.ASB.NativeIntegration";
        BusConfiguration busConfiguration = new BusConfiguration();

        #region EndpointAndSingleQueue

        busConfiguration.EndpointName("Samples.ASB.NativeIntegration");
        var scaleOut = busConfiguration.ScaleOut();
        scaleOut.UseSingleBrokerQueue();

        #endregion

        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseSerialization<JsonSerializer>();
        var transport = busConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));

        #region BrokeredMessageConvention

        BrokeredMessageBodyConversion.ExtractBody = brokeredMessage =>
        {
            using (MemoryStream stream = new MemoryStream())
            using (Stream body = brokeredMessage.GetBody<Stream>())
            {
                body.CopyTo(stream);
                return stream.ToArray();
            }
        };

        #endregion

        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
