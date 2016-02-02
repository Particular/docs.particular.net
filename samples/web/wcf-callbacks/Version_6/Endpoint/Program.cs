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
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.WcfCallbacks");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error");

        #region startbus


        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
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

    static IDisposable StartWcfHost(IBusSession busSession)
    {
        WcfMapper wcfMapper = new WcfMapper(busSession, "http://localhost:8080");
        wcfMapper.StartListening<EnumMessage, Status>();
        wcfMapper.StartListening<ObjectMessage, ReplyMessage>();
        wcfMapper.StartListening<IntMessage, int>();
        return wcfMapper;
    }

    #endregion

}