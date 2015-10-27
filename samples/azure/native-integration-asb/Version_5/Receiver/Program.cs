using System;
using System.IO;
using NServiceBus;
using NServiceBus.Azure.Transports.WindowsAzureServiceBus;

namespace Receiver
{
    class Program
    {
        private static void Main()
        {
            var configuration = new BusConfiguration();

            #region EndpointAndSingleQueue

            configuration.EndpointName("Samples.ASB.NativeIntegration");
            configuration.ScaleOut().UseSingleBrokerQueue();

            #endregion

            configuration.EnableInstallers();
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.UseSerialization<JsonSerializer>();
            configuration.UseTransport<AzureServiceBusTransport>()
                .ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));

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

            using (IBus bus = Bus.Create(configuration).Start())
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}