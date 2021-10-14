using NServiceBus;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ASB.NativeIntegration";

        #region EndpointName

        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.NativeIntegration");

        #endregion

        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.Conventions().DefiningMessagesAs(type => type.Name == "NativeMessage");


        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
        }

        endpointConfiguration.UseTransport<AzureServiceBusTransport>().ConnectionString(connectionString);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}