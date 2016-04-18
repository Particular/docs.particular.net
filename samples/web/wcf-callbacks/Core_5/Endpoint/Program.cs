using System;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{

    static void Main()
    {
        Console.Title = "Samples.WcfCallbacks.Endpoint";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Info);

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.WcfCallbacks.Endpoint");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        #region startbus

        using (IBus bus = Bus.Create(busConfiguration).Start())
        using (StartWcfHost(bus))
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        #endregion
    }

    #region startwcf

    static IDisposable StartWcfHost(IBus bus)
    {
        WcfMapper wcfMapper = new WcfMapper(bus, "http://localhost:8080");
        wcfMapper.StartListening<EnumMessage, Status>();
        wcfMapper.StartListening<ObjectMessage, ReplyMessage>();
        wcfMapper.StartListening<IntMessage, int>();
        return wcfMapper;
    }

    #endregion

}