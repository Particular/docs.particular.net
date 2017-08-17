using System;
using System.IO;
using NServiceBus;
using NServiceBus.Azure.Transports.WindowsAzureServiceBus;

class Program
{
    static void Main()
    {
		AppContext.SetSwitch("Switch.System.IdentityModel.DisableMultipleDNSEntriesInSANCertificate", true);
		
        Console.Title = "Samples.ASB.NativeIntegration";
        var busConfiguration = new BusConfiguration();

        #region EndpointAndSingleQueue

        busConfiguration.EndpointName("Samples.ASB.NativeIntegration");
        var scaleOut = busConfiguration.ScaleOut();
        scaleOut.UseSingleBrokerQueue();

        #endregion

        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseSerialization<JsonSerializer>();
        var transport = busConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);

        #region BrokeredMessageConvention

        BrokeredMessageBodyConversion.ExtractBody = brokeredMessage =>
        {
            using (var stream = new MemoryStream())
            using (var body = brokeredMessage.GetBody<Stream>())
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
