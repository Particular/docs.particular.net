using System;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using NServiceBus;
using NServiceBus.Logging;
using Owin;
using OwinPassThrough;

static class Program
{
    
    static void Main()
    {
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Info);
#region startbus
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.OwinPassThrough");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration).Start())
        using (StartGateway(bus))
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
#endregion
    }
#region startowin
    static IDisposable StartGateway(IBus bus)
    {
        var baseUrl = "http://localhost:12345/";
        var startOptions = new StartOptions(baseUrl)
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
        var owinToBus = new OwinToBus(bus);
        builder.Map("/to-bus", app => { app.Use(owinToBus.Middleware()); });
    }

    static void MapToMsmq(IAppBuilder builder)
    {
        var queue = Environment.MachineName + @"\private$\Samples.OwinPassThrough";
        var owinToMsmq = new OwinToMsmq(queue);
        builder.Map("/to-msmq", app => { app.Use(owinToMsmq.Middleware()); });
    }
#endregion

}