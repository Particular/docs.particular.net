using System;
using System.Threading.Tasks;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using NServiceBus;
using NServiceBus.Logging;
using Owin;

static class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Info);

        #region startbus

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.OwinPassThrough");
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            using (StartOwinHost(endpoint))
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            await endpoint.Stop();

            #endregion
        }
        finally
        {
            await endpoint.Stop();
        }
    }

    #region startowin

    static IDisposable StartOwinHost(IBusSession busSession)
    {
        string baseUrl = "http://localhost:12345/";
        StartOptions startOptions = new StartOptions(baseUrl)
        {
            ServerFactory = "Microsoft.Owin.Host.HttpListener",
        };

        return WebApp.Start(startOptions, builder =>
        {
            builder.UseCors(CorsOptions.AllowAll);
            MapToBus(builder, busSession);
            MapToMsmq(builder);
        });
    }

    static void MapToBus(IAppBuilder builder, IBusSession busSession)
    {
        OwinToBus owinToBus = new OwinToBus(busSession);
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