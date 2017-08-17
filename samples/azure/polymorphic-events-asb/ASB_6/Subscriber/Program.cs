using System;
using System.Configuration;
using Events;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Features;

class Program
{
    static void Main()
    {
		AppContext.SetSwitch("Switch.System.IdentityModel.DisableMultipleDNSEntriesInSANCertificate", true);
		
        Console.Title = "Samples.ASB.Polymorphic.Subscriber";
        var busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.ASB.Polymorphic.Subscriber");
        var scaleOut = busConfiguration.ScaleOut();
        scaleOut.UseSingleBrokerQueue();
        var transport = busConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.ConnectionString(connectionString);

        #region DisableAutoSubscripton

        busConfiguration.DisableFeature<AutoSubscribe>();

        #endregion

        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.DisableFeature<SecondLevelRetries>();


        using (var bus = Bus.Create(busConfiguration).Start())
        {
            #region ControledSubscriptions

            bus.Subscribe<BaseEvent>();

            #endregion

            Console.WriteLine("Subscriber is ready to receive events");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }

    public class ProvideConfiguration : IProvideConfiguration<UnicastBusConfig>
    {
        public UnicastBusConfig GetConfiguration()
        {
            var config = new UnicastBusConfig
            {
                MessageEndpointMappings = new MessageEndpointMappingCollection
                {
                    new MessageEndpointMapping
                    {
                        AssemblyName = "Shared",
                        Endpoint = "Samples.ASB.Polymorphic.Publisher"
                    }
                }
            };
            
            return config;
        }
    }
}