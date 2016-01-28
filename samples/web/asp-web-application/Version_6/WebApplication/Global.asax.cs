using System;
using System.Web;
using NServiceBus;

public class Global : HttpApplication
{
    public static IEndpointInstance Endpoint;

    public override void Dispose()
    {
        Endpoint?.Stop().GetAwaiter().GetResult();
        base.Dispose();
    }

    protected void Application_Start(object sender, EventArgs e)
    {
        #region ApplicationStart

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.AsyncPages.WebApplication");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        Endpoint = NServiceBus.Endpoint.Start(busConfiguration).GetAwaiter().GetResult();

        #endregion
    }
}