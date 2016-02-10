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

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.AsyncPages.WebApplication");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        Endpoint = NServiceBus.Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        #endregion
    }
}