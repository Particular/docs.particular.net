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
        var endpointConfiguration = new EndpointConfiguration("Samples.WcfCallbacks.Endpoint");
        var scaleOut = endpointConfiguration.ScaleOut();
        scaleOut.InstanceDiscriminator("1");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region startbus

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            using (StartWcfHost(endpointInstance))
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        #endregion
    }

    #region startwcf

    static IDisposable StartWcfHost(IEndpointInstance endpointInstance)
    {
        var wcfMapper = new WcfMapper(endpointInstance, "http://localhost:8080");
        wcfMapper.StartListening<EnumMessage, Status>();
        wcfMapper.StartListening<ObjectMessage, ReplyMessage>();
        wcfMapper.StartListening<IntMessage, int>();
        return wcfMapper;
    }

    #endregion

}