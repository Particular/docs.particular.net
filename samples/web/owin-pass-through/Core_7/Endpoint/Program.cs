using System;
using System.Threading.Tasks;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using NServiceBus;
using NServiceBus.Logging;
using Owin;

static class Program
{

    static async Task Main()
    {
        Console.Title = "Samples.OwinPassThrough";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Info);

        #region startbus

        var endpointConfiguration = new EndpointConfiguration("Samples.OwinPassThrough");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        using (StartOwinHost(endpointInstance))
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);

        #endregion
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    #region startowin

    static IDisposable StartOwinHost(IEndpointInstance endpointInstance)
    {
        var baseUrl = "http://localhost:12345/";
        var startOptions = new StartOptions(baseUrl)
        {
            ServerFactory = "Microsoft.Owin.Host.HttpListener",
        };

        return WebApp.Start(startOptions, builder =>
        {
            builder.UseCors(CorsOptions.AllowAll);
            MapToBus(builder, endpointInstance);
            MapToMsmq(builder);
        });
    }

    static void MapToBus(IAppBuilder builder, IEndpointInstance endpointInstance)
    {
        var owinToBus = new OwinToBus(endpointInstance);
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