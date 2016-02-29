using System;
using System.Threading.Tasks;
using NServiceBus;
using System.Configuration;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SenderSideScaleOut.Server1";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Server");

        #region Server-Set-InstanceId

        string discriminator = ConfigurationManager.AppSettings["InstanceId"];
        endpointConfiguration.ScaleOut().InstanceDiscriminator(discriminator);

        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press enter to exit.");
        Console.ReadLine();
        await endpoint.Stop();
    }
}