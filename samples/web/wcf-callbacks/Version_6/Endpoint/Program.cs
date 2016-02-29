using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.WcfCallbacks.Endpoint";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.WcfCallbacks.Endpoint");
        endpointConfiguration.ScaleOut()
            .InstanceDiscriminator("1");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region startbus

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            using (StartWcfHost(endpoint))
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
        finally
        {
            await endpoint.Stop();
        }

        #endregion
    }

    #region startwcf

    static IDisposable StartWcfHost(IEndpointInstance endpointInstance)
    {
        WcfMapper wcfMapper = new WcfMapper(endpointInstance, "http://localhost:8080");
        wcfMapper.StartListening<EnumMessage, Status>();
        wcfMapper.StartListening<ObjectMessage, ReplyMessage>();
        wcfMapper.StartListening<IntMessage, int>();
        return wcfMapper;
    }

    #endregion

}