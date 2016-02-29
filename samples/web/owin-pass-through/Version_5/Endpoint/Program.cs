using System;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using NServiceBus;
using NServiceBus.Logging;
using Owin;

static class Program
{
    
    static void Main()
    {
        Console.Title = "Samples.OwinPassThrough";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Info);
#region startbus
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.OwinPassThrough");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (IBus bus = Bus.Create(busConfiguration).Start())
        using (StartOwinHost(bus))
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
#endregion
    }
#region startowin
    static IDisposable StartOwinHost(IBus bus)
    {
        string baseUrl = "http://localhost:12345/";
        StartOptions startOptions = new StartOptions(baseUrl)
        {
            ServerFactory = "Microsoft.Owin.Host.HttpListener",
        };

        return WebApp.Start(startOptions, builder =>
        {
            builder.UseCors(CorsOptions.AllowAll);
            MapToBus(builder, bus);
            MapToMsmq(builder);
        });
    }

    static void MapToBus(IAppBuilder builder, IBus bus)
    {
        OwinToBus owinToBus = new OwinToBus(bus);
        builder.Map("/to-bus", app => { app.Use(owinToBus.Middleware()); });
    }

    static void MapToMsmq(IAppBuilder builder)
    {
        string queue = Environment.MachineName + @"\private$\Samples.OwinPassThrough";
        OwinToMsmq owinToMsmq = new OwinToMsmq(queue);
        builder.Map("/to-msmq", app => { app.Use(owinToMsmq.Middleware()); });
    }
#endregion

}