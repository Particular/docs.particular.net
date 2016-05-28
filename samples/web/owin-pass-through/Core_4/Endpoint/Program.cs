using System;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using NServiceBus;
using NServiceBus.Installation.Environments;
using Owin;

static class Program
{

    static void Main()
    {
        Console.Title = "Samples.OwinPassThrough";
        #region startbus
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.OwinPassThrough");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            using (StartOwinHost(bus))
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        #endregion
    }

    #region startowin
    static IDisposable StartOwinHost(IBus bus)
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